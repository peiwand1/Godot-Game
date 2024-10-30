using Godot;
using System;

public partial class Camera : Camera2D
{
	Vector2 boundsOrigin;
	Vector2 boundsSize;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void setBounds(CollisionShape2D bounds)
	{
		boundsOrigin = bounds.GlobalPosition;
		if (bounds.Shape is RectangleShape2D rectangleShape)
		{
			boundsSize = rectangleShape.Size * 2;
		}
		else
		{
			GD.PrintErr("Unsupported shape type for bounds. Only RectangleShape2D is supported.");
			return;
		}

		LimitLeft = (int)(boundsOrigin.X - boundsSize.X / 2);
		LimitRight = (int)(boundsOrigin.X + boundsSize.X / 2);
		LimitTop = (int)(boundsOrigin.Y - boundsSize.Y / 2);
		LimitBottom = (int)(boundsOrigin.Y + boundsSize.Y / 2);
	}
}
