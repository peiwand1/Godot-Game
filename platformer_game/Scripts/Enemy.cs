using Godot;
using System;

public abstract partial class Enemy : CharacterBody2D
{
	public abstract void OnHit();
	protected abstract void OnDeathTimerTimeout();
}
