using Godot;
using System;

public partial class Goomba : Enemy
{
	public const float Speed = 30.0f;
	public const float JumpVelocity = -400.0f;
	private bool direction = true;
	private bool alive = true;
	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		base._Ready();
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (alive)
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
			direction = !direction;
		}

		if (direction)
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

	public override void StartDeath()
	{
		base.StartDeath();
		_animatedSprite.Play("die");
		alive = false;
	}
}
