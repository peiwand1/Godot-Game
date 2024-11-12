using Godot;
using System;

public partial class Coin : Area2D
{
	public override void _Ready()
	{
		this.BodyEntered += OnArea2DBodyEntered;
	}

	public void OnArea2DBodyEntered(Node2D node)
	{
		if (node is Player)
		{
			// collect coin when the player enters it's area
			GameManager.Instance.AddScore(200);
			GameManager.Instance.AddCoin(1);
			QueueFree();
		}
	}
}
