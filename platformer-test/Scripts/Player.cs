using Godot;
using System;

public partial class Player : CharacterBody2D
{
	const float MIN_SPEED = 4.453125f;
	const float MAX_SPEED = 153.75f;
	const float MAX_WALK_SPEED = 93.75f;
	const float MAX_FALL_SPEED = 270.0f;
	const float MAX_FALL_SPEED_CAP = 240.0f;
	const float MIN_SLOW_DOWN_SPEED = 33.75f;

	const float WALK_ACCELERATION = 133.59375f;
	const float RUN_ACCELERATION = 200.390625f;
	const float WALK_FRICTION = 182.8125f;
	const float SKID_FRICTION = 365.625f;

	// Jump physics vary based on horizontal speed thresholds;
	readonly float[] JUMP_SPEED = { -240.0f, -240.0f, -300.0f };
	readonly float[] LONG_JUMP_GRAVITY = { 450.0f, 421.875f, 562.5f };
	readonly float[] GRAVITY = { 1575.0f, 1350.0f, 2025.0f };

	readonly float[] SPEED_THRESHOLDS = { 60.0f, 138.75f };

	const float STOMP_SPEED = 240.0f;
	const float STOMP_SPEED_CAP = -60.0f;

	const float COOLDOWN_TIME_SEC = 3.0f;

	///////////////////////////////////////////////////////////
	Vector2 inputAxis = Vector2.Zero;
	float speedScale = 0.0f;

	bool isFacingLeft = false;
	bool isRunning = false;
	bool isJumping = false;
	bool isFalling = false;
	bool isCrouching = false;
	bool isSkidding = false;
	bool hitboxNeedsUpdate = false;
	bool hasPoweredUp = false;

	float minSpeed = MIN_SPEED;
	float maxSpeed = MAX_WALK_SPEED;
	float acceleration = WALK_ACCELERATION;

	int speedThreshold = 0;

	PowerState curState = PowerState.SMALL;

	private AnimatedSprite2D _animatedSprite;
	private CollisionShape2D _collisionShape;
	private RectangleShape2D _rectangleShape;
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		_rectangleShape = _collisionShape.Shape as RectangleShape2D;
		if (_rectangleShape == null)
		{
			GD.PrintErr("The shape is not a RectangleShape2D!");
		}

		SetAnimation("stand");
		UpdateHitbox();
	}

	public override void _Process(double delta)
	{
		ProcessInput();
		// if you fall in a hole
		if (Position.Y > 10)
		{
			Die();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		ProcessWalk(delta);
		ProcessJump(delta);

		if (hitboxNeedsUpdate)
		{
			UpdateHitbox();
		}

		UpdateAnimation();
		MoveAndSlide();
	}

	private void ProcessInput()
	{
		inputAxis.X = Input.GetAxis("move_left", "move_right");
		inputAxis.Y = Input.GetAxis("jump", "move_down");

		bool wasCrouching = isCrouching;

		if (IsOnFloor())
		{
			isRunning = Input.IsActionPressed("run");
			isCrouching = Input.IsActionPressed("move_down");

			if (isCrouching && inputAxis.X != 0)
			{
				isCrouching = false;
				inputAxis.X = 0;
			}
		}

		if (isCrouching != wasCrouching)
		{
			hitboxNeedsUpdate = true;
		}
	}

	private void ProcessJump(double delta)
	{
		if (IsOnFloor())
		{
			if (Input.IsActionJustPressed("jump"))
			{
				isJumping = true;

				float speed = Mathf.Abs(Velocity.X);
				int speedThreshold = SPEED_THRESHOLDS.Length;

				for (int i = 0; i < SPEED_THRESHOLDS.Length; i++)
				{
					if (speed < SPEED_THRESHOLDS[i])
					{
						speedThreshold = i;
						break;
					}
				}

				Vector2 vel = Velocity;
				vel.Y = JUMP_SPEED[speedThreshold];
				Velocity = vel;
			}
		}
		else
		{
			float gravity = GRAVITY[speedThreshold];

			if (Input.IsActionPressed("jump") && !isFalling)
			{
				gravity = LONG_JUMP_GRAVITY[speedThreshold];
			}

			// Apply gravity to the Y component
			Vector2 vel = Velocity;
			vel.Y += (float)(gravity * delta);

			// Cap fall speed if necessary
			if (vel.Y > MAX_FALL_SPEED)
			{
				vel.Y = MAX_FALL_SPEED_CAP;
			}

			Velocity = vel; // Re-assign to velocity
		}

		if (Velocity.Y > 0)
		{
			isJumping = false;
			isFalling = true;
		}
		else if (IsOnFloor())
		{
			isFalling = false;
		}
	}

	private void ProcessWalk(double delta)
	{
		Vector2 vel = Velocity;
		if (inputAxis.X != 0)
		{
			if (IsOnFloor())
			{
				if (Velocity.X != 0)
				{
					isFacingLeft = inputAxis.X < 0.0f;
					isSkidding = (Velocity.X < 0.0f) != isFacingLeft;
				}

				if (isSkidding)
				{
					minSpeed = MIN_SLOW_DOWN_SPEED;
					maxSpeed = MAX_WALK_SPEED;
					acceleration = SKID_FRICTION;
				}
				else if (isRunning)
				{
					minSpeed = MIN_SPEED;
					maxSpeed = MAX_SPEED;
					acceleration = RUN_ACCELERATION;
				}
				else
				{
					minSpeed = MIN_SPEED;
					maxSpeed = MAX_WALK_SPEED;
					acceleration = WALK_ACCELERATION;
				}
			}
			else if (isRunning && Mathf.Abs(Velocity.X) > MAX_WALK_SPEED)
			{
				maxSpeed = MAX_SPEED;
			}
			else
			{
				maxSpeed = MAX_WALK_SPEED;
			}

			float targetSpeed = inputAxis.X * maxSpeed;
			vel.X = (float)Mathf.MoveToward(Velocity.X, targetSpeed, acceleration * delta);
		}
		else if (IsOnFloor() && Velocity.X != 0)
		{
			if (!isSkidding)
			{
				acceleration = WALK_FRICTION;
			}

			if (inputAxis.Y != 0)
			{
				minSpeed = MIN_SLOW_DOWN_SPEED;
			}
			else
			{
				minSpeed = MIN_SPEED;
			}

			if (Mathf.Abs(Velocity.X) < minSpeed)
			{
				vel.X = 0.0f;
			}
			else
			{
				vel.X = (float)Mathf.MoveToward(Velocity.X, 0.0f, acceleration * delta);
			}
		}

		if (Mathf.Abs(Velocity.X) < MIN_SLOW_DOWN_SPEED)
		{
			isSkidding = false;
		}

		Velocity = vel;
		speedScale = Mathf.Abs(Velocity.X) / MAX_SPEED;
	}

	private void SetAnimation(string animationName)
	{
		switch (curState)
		{
			case PowerState.SMALL:
				animationName += "_small";
				break;
			case PowerState.BIG:
				animationName += "_big";
				break;
			case PowerState.FIRE:
				animationName += "_fire";
				break;
			default:
				animationName = "stand_small";
				break;
		}
		// GD.Print("Setting animation to: " + (isFacingLeft ? "mirrored " : "") + animationName);
		_animatedSprite.SpeedScale = Mathf.Lerp(1, 2.5f, Velocity.Length() / MAX_SPEED);
		_animatedSprite.FlipH = isFacingLeft;
		_animatedSprite.Play(animationName);
	}

	private void UpdateAnimation()
	{
		if (hasPoweredUp)
		{
			// TODO Animation gets skipped because it gets overwritten after 1 frame, find fix
			SetAnimation("power_up");
			hasPoweredUp = false;
		}
		else if (isFalling)
		{
			// freeze animation
			_animatedSprite.Pause();
		}
		else if (isCrouching)
		{
			SetAnimation("crouch");
		}
		else if (isJumping)
		{
			SetAnimation("jump");
		}
		else if (isSkidding)
		{
			SetAnimation("turn");
		}
		else if (Velocity.X != 0) // if moving horizontally
		{
			SetAnimation("walk");
		}
		else
		{
			SetAnimation("stand");
		}
	}

	private void UpdateHitbox()
	{
		switch (curState)
		{
			case PowerState.SMALL:
				_rectangleShape.Size = new Vector2(10, 12);
				_collisionShape.Position = new Vector2(8, 26);
				break;
			case PowerState.BIG:
			case PowerState.FIRE:
				if (!isCrouching)
				{
					_rectangleShape.Size = new Vector2(12, 24);
					_collisionShape.Position = new Vector2(8, 20);
				}
				else
				{
					_rectangleShape.Size = new Vector2(12, 12);
					_collisionShape.Position = new Vector2(8, 26);
				}
				break;
		}

		hitboxNeedsUpdate = false;
	}

	public void PowerUpTo(PowerState aState)
	{
		if (aState > curState)
		{
			curState = aState;
			hasPoweredUp = true;
		}
		hitboxNeedsUpdate = true;
	}

	public PowerState GetPowerState()
	{
		return curState;
	}

	public bool IsFalling()
	{
		return isFalling;
	}

	private void OnEnemyDetectorBodyEntered(Node2D body)
	{
		GD.Print("detector");

		// if the collision is with a mob and collision is from above
		if (body is Enemy mob && isFalling)
		{
			Vector2 vel = Velocity;

			// if so, we squash it and bounce
			mob.OnHit();
			vel.Y = -STOMP_SPEED;
			Velocity = vel;
		}
		else
		{
			Die();
		}
	}

	private void Die()
	{
		//TODO initiate death process (restart level, show lives screen, etc.)
		Position = new Vector2(48.0f, -64.0f);
	}
}
