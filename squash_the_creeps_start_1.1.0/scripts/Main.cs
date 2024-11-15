using Godot;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }

	public override void _Ready()
	{
		GetNode<Control>("UserInterface/Retry").Hide();
	}

	private void OnMobTimerTimeout()
	{
		Mob mob = MobScene.Instantiate<Mob>();
		var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		Vector3 playerPosition = GetNode<Player>("Player").Position;
		mob.Initialize(mobSpawnLocation.Position, playerPosition);
		mob.Squashed += GetNode<ScoreLabel>("UserInterface/ScoreLabel").OnMobSquashed;
		AddChild(mob);
	}

	private void OnPlayerHit()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Control>("UserInterface/Retry").Show();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept") && GetNode<Control>("UserInterface/Retry").Visible)
		{
			// This restarts the current scene.
			GetTree().ReloadCurrentScene();
		}
	}
}
