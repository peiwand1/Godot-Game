using Godot;
using System;

public partial class Mushroom : PowerUp
{
	public const float Speed = 50.0f;
	public const float JumpVelocity = -400.0f;
	private bool _direction = false;

	public override void _Ready()
	{
		_powerUpLevel = PowerState.BIG;
	}

	public override void _PhysicsProcess(double delta)
	{		
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

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

		// TODO jump and reverse direction if block underneath is hit

		Velocity = velocity;
		MoveAndSlide();
	}
}
