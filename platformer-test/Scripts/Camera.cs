using Godot;
using System;

public partial class Camera : Camera2D
{
	private Vector2 _boundsOrigin;
	private Vector2 _boundsSize;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void SetBounds(CollisionShape2D bounds)
	{
		_boundsOrigin = bounds.GlobalPosition;
		if (bounds.Shape is RectangleShape2D rectangleShape)
		{
			_boundsSize = rectangleShape.Size * 2;
		}
		else
		{
			GD.PrintErr("Unsupported shape type for bounds. Only RectangleShape2D is supported.");
			return;
		}

		LimitLeft = (int)(_boundsOrigin.X - _boundsSize.X / 2);
		LimitRight = (int)(_boundsOrigin.X + _boundsSize.X / 2);
		LimitTop = (int)(_boundsOrigin.Y - _boundsSize.Y / 2);
		LimitBottom = (int)(_boundsOrigin.Y + _boundsSize.Y / 2);
	}
}
