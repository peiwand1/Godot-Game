using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	private int _score = 0;
	private int _coins = 0;
	private int _timeLeft = 400;
	private double _timeBetweenUpdates = 0.4; // 0.4 seconds per in game time unit
	private double _timeSinceUpdate = 0;

	[Signal]
	public delegate void ScoreUpdatedEventHandler(int newScore);
	[Signal]
	public delegate void CoinsUpdatedEventHandler(int newCoins);
	[Signal]
	public delegate void TimerUpdatedEventHandler(int newTimeLeft);

	public override void _Ready()
	{
		Instance = this;
	}

	public override void _Process(double delta)
	{
		_timeSinceUpdate += delta;
		if (_timeSinceUpdate > _timeBetweenUpdates)
		{
			_timeSinceUpdate -= _timeBetweenUpdates;
			_timeLeft -= 1;
			EmitSignal(nameof(TimerUpdated), _timeLeft);

			if (_timeLeft <= 0)
			{
				GetTree().ChangeSceneToFile("res://main.tscn");
				ResetLevel();
			}
		}
	}

	public void AddScore(int points)
	{
		_score += points;
		EmitSignal(nameof(ScoreUpdated), _score);
	}

	public void AddCoin(int count)
	{
		_coins += count;
		EmitSignal(nameof(CoinsUpdated), _coins);
	}

	private void ResetLevel()
	{
		_timeLeft = 400;
		_timeBetweenUpdates = 0.4; // 0.4 seconds per in game time unit
		_timeSinceUpdate = 0;

		EmitSignal(nameof(TimerUpdated), _timeLeft);
		EmitSignal(nameof(ScoreUpdated), _score);
		EmitSignal(nameof(CoinsUpdated), _coins);
	}
}
