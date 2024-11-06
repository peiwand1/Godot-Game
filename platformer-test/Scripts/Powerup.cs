using Godot;
using System;

public partial class PowerUp : CharacterBody2D
{
	protected PowerState powerUpLevel = PowerState.SMALL;

	public void OnArea2DBodyEntered(Node2D body)
	{
		// Check if the body is the player (replace "Player" with the player's node name)
		if (body is Player player)
		{
			player.PowerUpTo(powerUpLevel);
			QueueFree();  // Remove the collectible after collection
		}
	}
}
