using Godot;
using System;

public partial class Goomba : Enemy
{
	public const float Speed = 30.0f;
	private bool _direction = true;
	private bool _alive = true;
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
		if (_alive)
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
			_direction = !_direction;
		}

		if (_direction)
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
		GameManager.Instance.AddScore(100);
		_deathTimer.Start();
		_collisionShape.CallDeferred("set_disabled", true);
		_animatedSprite.Play("die");
		_alive = false;
	}

	protected override void OnDeathTimerTimeout()
	{
		QueueFree();
	}
}
