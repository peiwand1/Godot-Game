using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Export]
	public Label ScoreLabel;
	[Export]
	public Label CoinLabel;
	[Export]
	public Label TimerLabel;

	public override void _Ready()
	{
		GameManager.Instance.Connect("ScoreUpdated", new Callable(this, nameof(OnScoreUpdated)));
		GameManager.Instance.Connect("CoinsUpdated", new Callable(this, nameof(OnCoinsUpdated)));
		GameManager.Instance.Connect("TimerUpdated", new Callable(this, nameof(OnTimerUpdated)));
	}

	private void OnScoreUpdated(int newScore)
	{
		ScoreLabel.Text = "MARIO\n" + newScore.ToString("D6");
	}

	private void OnCoinsUpdated(int newCount)
	{
		CoinLabel.Text = newCount.ToString("D2");
	}

	private void OnTimerUpdated( int newTimeLeft)
	{
		TimerLabel.Text = "TIME\n " + newTimeLeft.ToString("D3");;
	}
}
