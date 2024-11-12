using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Export]
	public Label _scoreLabel;
	[Export]
	public Label _coinLabel;
	[Export]
	public Label _timerLabel;

	public override void _Ready()
	{
		GameManager.Instance.Connect("ScoreUpdated", new Callable(this, nameof(OnScoreUpdated)));
		GameManager.Instance.Connect("CoinsUpdated", new Callable(this, nameof(OnCoinsUpdated)));
		GameManager.Instance.Connect("TimerUpdated", new Callable(this, nameof(OnTimerUpdated)));
	}

	public override void _Process(double delta)
	{

	}

	private void OnScoreUpdated(int newScore)
	{
		_scoreLabel.Text = "MARIO\n" + newScore.ToString("D6");
	}

	private void OnCoinsUpdated(int newCount)
	{
		_coinLabel.Text = newCount.ToString("D2");
	}

	private void OnTimerUpdated( int newTimeLeft)
	{
		_timerLabel.Text = "TIME\n " + newTimeLeft.ToString("D3");;
	}
}
