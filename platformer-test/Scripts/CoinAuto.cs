using Godot;
using System;

public partial class CoinAuto : Node2D
{
	public override void _Ready()
	{
		ScoreManager.Instance.AddScore(200);
	}
	
	public void OnTimerTimeout()
	{
		GD.Print("time");
		QueueFree();
	}
}
