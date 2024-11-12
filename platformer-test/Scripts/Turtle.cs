using Godot;
using System;

public partial class Turtle : Enemy
{
	public const float Speed = 20.0f;
	public const float ShellSpeed = 120.0f;
	private bool _direction = true;
	private bool _awake = true;
	private bool _shellHit = false;
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
		if (_awake || _shellHit)
		{
			ProcessWalk(delta);
		}
		// TODO die if block underneath is hit
	}

	public void ProcessWalk(double delta)
	{
		Vector2 velocity = Velocity;
		float curSpeed = (_shellHit ? ShellSpeed : Speed);

		// gravity
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// reverse when hitting wall
		if (velocity.X == 0)
		{
			_direction = !_direction;
		}

		if (_direction)
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
		if (_awake){
			_knockoutTimer.Start();
			_animatedSprite.Play("die");
			_awake = false;
		}
		else
		{
			// starting and stopping the shell
			if (_shellHit)
			{
				_shellHit = false;
				_knockoutTimer.Start();
				_animatedSprite.Play("die");
			}
			else
			{
				_shellHit = true;
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
		_awake = true;
		_animatedSprite.Play("default");
	}
}
