using Godot;
using System;

public partial class Brick : Node2D
{
	[Export] private PackedScene itemScene; // The item to spawn, e.g., a coin

	public override void _Ready()
	{
		// Connect the Area2D signal to detect upward collisions
		// GetNode<Area2D>("Area2D").BodyEntered += OnArea2DBodyEntered;
	}

	public void OnArea2DBodyEntered(Node2D node)
	{
		GD.Print("OnArea2DBodyEntered");
		// Check if the body is the player and the block hasn't been activated
		if (node is Player player)
		{
			if (player.GetPowerState() > PowerState.SMALL)
			{
				BreakBlock();
			}
			else
			{
				BounceBlock();
			}
		}
	}

	private void BounceBlock()
	{
		// Play bounce animation for block if player is small
		Vector2 originalPosition = Position;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position:y", Position.Y - 8, 0.1f).SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(this, "position:y", originalPosition.Y, 0.1f).SetTrans(Tween.TransitionType.Quad).SetDelay(0.1f);
	}

	private void BreakBlock()
	{
		// Spawn the item above the block
		if (itemScene != null)
		{
			Node2D item = (Node2D)itemScene.Instantiate();
			item.Position = Position + new Vector2(0, -16); // Adjust to spawn above the block
			GetParent().AddChild(item);
		}

		QueueFree();
	}
}
