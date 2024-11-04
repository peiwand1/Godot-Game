using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// create a new signal that triggers when the character is hit
	[Signal]
	public delegate void HitEventHandler();

	[Export]
	public int Speed { get; set; } = 14;

	[Export]
	public int FallAcceleration { get; set; } = 75;

	[Export]
	public int JumpImpulse { get; set; } = 20;

	[Export]
	public int BounceImpulse { get; set; } = 16;

	private Vector3 _targetVelocity = Vector3.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		// create a local variable to store the input direction
		var direction = Vector3.Zero;

		// check for each move input and update the direction accordingly
		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1.0f;
		}
		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1.0f;
		}
		if (Input.IsActionPressed("move_back"))
		{
			direction.Z += 1.0f;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction.Z -= 1.0f;
		}

		// if any direction is pressed
		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized(); // normalize vector
			// setting the basis property will affect the rotation of the node.
			GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
			GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = 4;
		}
		else
		{
			GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = 1;
		}

		// ground velocity
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		// vertical velocity
		if (!IsOnFloor()) // if in the air, fall towards the floor
		{
			_targetVelocity.Y -= FallAcceleration * (float)delta;
		}

		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			_targetVelocity.Y = JumpImpulse;
		}

		for (int index = 0; index < GetSlideCollisionCount(); index++)
		{
			// get first collision with the player
			KinematicCollision3D collision = GetSlideCollision(index);

			// if the collision is with a mob and collision is from above
			if (collision.GetCollider() is Mob mob && Vector3.Up.Dot(collision.GetNormal()) > 0.1f)
			{
				// if so, we squash it and bounce
				mob.Squash();
				_targetVelocity.Y = BounceImpulse;
				// prevent further duplicate calls
				break;
			}
		}

		// moving the character
		var pivot = GetNode<Node3D>("Pivot");
		pivot.Rotation = new Vector3(Mathf.Pi / 6.0f * Velocity.Y / JumpImpulse, pivot.Rotation.Y, pivot.Rotation.Z);
		Velocity = _targetVelocity;
		MoveAndSlide();
	}

	private void OnMobDetectorBodyEntered(Node3D body)
	{
		Die();
	}

	private void Die()
	{
		EmitSignal(SignalName.Hit);
		QueueFree();
	}
}
