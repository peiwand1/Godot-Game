using Godot;
using System;

public partial class Brick : Node2D
{
	[Export] public PackedScene _itemScene; // The item to spawn when hit

	public void OnArea2DBodyEntered(Node2D node)
	{
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
		// Play bounce animation for block if player is small (can't break the block)
		Vector2 originalPosition = Position;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position:y", Position.Y - 8, 0.1f).SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(this, "position:y", originalPosition.Y, 0.1f).SetTrans(Tween.TransitionType.Quad).SetDelay(0.1f);
	}

	private void BreakBlock()
	{
		// Spawn the held item above the block if it has one
		if (_itemScene != null)
		{
			Node2D item = (Node2D)_itemScene.Instantiate();
			item.Position = Position + new Vector2(0, -16); // Adjust to spawn above the block
			GetParent().AddChild(item);
		}

		GameManager.Instance.AddScore(50);
		QueueFree();
	}
}
