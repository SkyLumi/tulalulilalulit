using Godot;
using System;

public partial class Map3C1 : Node2D
{
	[Export] public string nextCorner = "res://maps/map3/map_3_corner2.tscn";
	[Export] public string presentCorner = "res://maps/map3/map_3_corner1.tscn";

	public override void _Ready()
	{
		var areaRight = GetNode<Area2D>("AreaRight");
		var areaLeft = GetNode<Area2D>("AreaLeft");
		var areaUp = GetNode<Area2D>("AreaUp");
		var dialogue01 = GetNode<Area2D>("Dialogue01");
		var dialogicBridge = GetNode("/root/Bridgedialogic");

		dialogicBridge.Call("_map03Dial02");

		areaRight.BodyEntered += OnAreaRightEntered;
		areaLeft.BodyEntered += OnAreaWrongEntered;
		areaUp.BodyEntered += OnAreaWrongEntered;
		dialogue01.BodyEntered += OnAreaDialogue01Entered;

		var spawnManager = GetNode<SpawnManager>("/root/SpawnManager");

		if (spawnManager.ForcedSpawnPosition != null)
		{
			var player = GetNode<Node2D>("Player");
			player.GlobalPosition = (Vector2)spawnManager.ForcedSpawnPosition;
			spawnManager.ForcedSpawnPosition = null;
		}
	}

	private void OnAreaDialogue01Entered(Node body)
	{
		if (body.IsInGroup("player"))
		{
			CallDeferred(nameof(Dialogue01));
		}
	}

	private void OnAreaRightEntered(Node body)
	{
		if (body.IsInGroup("player"))
		{
			CallDeferred(nameof(NextStage));
		}
	}

	private void OnAreaWrongEntered(Node body)
	{
		if (body.IsInGroup("player"))
		{
			CallDeferred(nameof(NowStage));
		}
	}
	
	private void Dialogue01()
	{
		var dialogicBridge = GetNode("/root/Bridgedialogic");
		dialogicBridge.Call("_map03Dial03");
	}

	private void NextStage()
	{
		GetTree().ChangeSceneToFile(nextCorner);
	}

	private void NowStage()
	{
		var resetPoint = GetNode<Marker2D>("ResetSpawnPoint");

		var spawnManager = GetNode<SpawnManager>("/root/SpawnManager");
		spawnManager.ForcedSpawnPosition = resetPoint.GlobalPosition;

		GetTree().ChangeSceneToFile(presentCorner);
	}
}
