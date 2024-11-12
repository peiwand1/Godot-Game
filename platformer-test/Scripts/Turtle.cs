using Godot;
using System;

public partial class Turtle : Enemy
{
	public const float Speed = 20.0f;
	public const float ShellSpeed = 120.0f;
	private bool Direction = true;
	private bool Awake = true;
	private bool ShellHit = false;
	protected Timer _knockoutTimer;
	protected Timer _wakeupTimer;
	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		base._Ready();
		_knockoutTimer = GetNode<Timer>("KnockoutTimer");
		_wakeupTimer = GetNode<Timer>("WakeupTimer");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Awake || ShellHit)
		{
			ProcessWalk(delta);
		}
		// TODO die if block underneath is hit
	}

	public void ProcessWalk(double delta)
	{
		Vector2 velocity = Velocity;
		float curSpeed = (ShellHit ? ShellSpeed : Speed);

		// gravity
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// reverse when hitting wall
		if (velocity.X == 0)
		{
			Direction = !Direction;
		}

		if (Direction)
		{
			velocity.X = curSpeed;
		}
		else
		{
			velocity.X = -curSpeed;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void OnHit()
	{
		if (Awake){
			_knockoutTimer.Start();
			_animatedSprite.Play("die");
			Awake = false;
		}
		else
		{
			// starting and stopping the shell
			if (ShellHit)
			{
				ShellHit = false;
				_knockoutTimer.Start();
				_animatedSprite.Play("die");
			}
			else
			{
				ShellHit = true;
				_knockoutTimer.Stop();
				_wakeupTimer.Stop();
				_animatedSprite.Play("die");
			}
		}
	}

	protected override void OnDeathTimerTimeout()
	{
		_wakeupTimer.Start();
		_animatedSprite.Play("waking");
	}

	protected void OnWakeupTimerTimeout()
	{
		Awake = true;
		_animatedSprite.Play("default");
	}
}
