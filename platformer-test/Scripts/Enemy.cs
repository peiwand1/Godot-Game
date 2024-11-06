using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	protected Timer _deathTimer;
	protected CollisionShape2D _collisionShape;

	public override void _Ready()
	{
		_deathTimer = GetNode<Timer>("DeathTimer");
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public virtual void StartDeath()
	{
		_deathTimer.Start();
		_collisionShape.Disabled = true;
	}

	protected void OnDeathTimerTimeout()
	{
		QueueFree();
	}
}
