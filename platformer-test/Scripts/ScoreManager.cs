using Godot;

public partial class ScoreManager : Node
{
	public static ScoreManager Instance { get; private set; }

	private int _score = 0;

	[Signal]
	public delegate void ScoreUpdatedEventHandler(int newScore);

	public override void _Ready()
	{
		Instance = this;
	}

	public void AddScore(int points)
	{
		_score += points;
		EmitSignal(nameof(ScoreUpdated), _score);
	}

	public int GetScore()
	{
		return _score;
	}
}
