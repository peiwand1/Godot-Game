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
		GD.Print("OnArea2DBodyEntered");
		if (node is Player)
		{
			//TODO collect coin

			QueueFree();
		}
	}
}
