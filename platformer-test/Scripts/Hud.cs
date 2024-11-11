using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Export]
	public Label _scoreLabel;

	public override void _Ready()
	{
		ScoreManager.Instance.Connect("ScoreUpdated", new Callable(this, nameof(OnScoreUpdated)));
	}

	public override void _Process(double delta)
	{

	}

	private void OnScoreUpdated(int newScore)
	{
		_scoreLabel.Text = $"{newScore}";
	}
}
