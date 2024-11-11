using Godot;
using System;

public partial class Goomba : Enemy
{
	public const float Speed = 30.0f;
	private bool Direction = true;
	private bool Alive = true;
	protected Timer _deathTimer;
	private AnimatedSprite2D _animatedSprite;
	private CollisionShape2D _collisionShape;

	public override void _Ready()
	{
		_deathTimer = GetNode<Timer>("DeathTimer");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Alive)
		{
			ProcessWalk(delta);
		}
		// TODO die if block underneath is hit
	}

	public void ProcessWalk(double delta)
	{
		Vector2 velocity = Velocity;

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
			velocity.X = Speed;
		}
		else
		{
			velocity.X = -Speed;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void OnHit()
	{
		_deathTimer.Start();
		_collisionShape.CallDeferred("set_disabled", true);
		_animatedSprite.Play("die");
		Alive = false;
	}

	protected override void OnDeathTimerTimeout()
	{
		ScoreManager.Instance.AddScore(100); 
		QueueFree();
	}
}
