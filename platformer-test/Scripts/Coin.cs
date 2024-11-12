using Godot;
using System;

public partial class Coin : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.BodyEntered += OnArea2DBodyEntered;
	}

	public void OnArea2DBodyEntered(Node2D node)
	{
		if (node is Player)
		{
			GameManager.Instance.AddScore(200);
			GameManager.Instance.AddCoin(1);
			QueueFree();
		}
	}
}
