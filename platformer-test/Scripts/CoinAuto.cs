using Godot;
using System;

public partial class CoinAuto : Node2D
{
	public override void _Ready()
	{
		GameManager.Instance.AddScore(200);
		GameManager.Instance.AddCoin(1);
	}

	public void OnTimerTimeout()
	{
		QueueFree();
	}
}
