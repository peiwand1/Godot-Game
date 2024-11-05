using Godot;
using System;

public partial class Mushroom : Powerup
{
	public const float Speed = 50.0f;
	public const float JumpVelocity = -400.0f;

	public override void _Ready()
	{
		powerUpLevel = PowerState.BIG;
	}

	public override void _PhysicsProcess(double delta)
	{
		// processCollision();
		
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// TODO reverse direction if hitting wall
		if (true)
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
