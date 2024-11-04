using Godot;
using System;

public partial class Powerup : CharacterBody2D
{	
	protected PowerState powerUpLevel = PowerState.SMALL;
	
	protected void processCollision()
	{
		// check what the powerup is touching
		for (int index = 0; index < GetSlideCollisionCount(); index++)
		{
			KinematicCollision2D collision = GetSlideCollision(index);

			// if the collision is with a player
			if (collision.GetCollider() is Player player)
			{
				// if so, we pick it up
				GD.Print("Collided with: " + player.Name);
				GD.Print(Name + " has power state: " + powerUpLevel);
				player.PowerUpTo(powerUpLevel);

				QueueFree();
				break;
			}
		}
	}
}
