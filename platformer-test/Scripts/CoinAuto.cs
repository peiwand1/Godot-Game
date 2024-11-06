using Godot;
using System;

public partial class CoinAuto : Node2D
{
	public void OnTimerTimeout()
	{
		GD.Print("time");
		QueueFree();
	}
}
