using Godot;
using System;

public partial class QuestionBlock : Node2D
{
	[Export] private PackedScene itemScene; // The item to spawn, e.g., a coin
	private bool activated = false;

	public void OnArea2DBodyEntered(Node2D node)
	{
		// Check if the body is the player and the block hasn't been activated
		if (node is Player && !activated)
		{
			ActivateBlock();
			GetNode<Area2D>("Area2D").Monitoring = false;
		}
	}

	private void ActivateBlock()
	{
		activated = true;

		// Change block sprite or animation to an "empty" block
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("empty");

		// Play bounce animation for block (moves up and down slightly)
		Vector2 originalPosition = Position;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position:y", Position.Y - 8, 0.1f).SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(this, "position:y", originalPosition.Y, 0.1f).SetTrans(Tween.TransitionType.Quad).SetDelay(0.1f);

		// Spawn the item above the block
		if (itemScene != null)
		{
			Node2D item = (Node2D)itemScene.Instantiate();
			item.Position = Position + new Vector2(0, -16); // Adjust to spawn above the block
			GetParent().AddChild(item);
		}
	}
}