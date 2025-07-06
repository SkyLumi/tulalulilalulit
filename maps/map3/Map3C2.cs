using Godot;
using System;

public partial class Map3C2 : Node2D
{
	[Export]
	public string nextCorner = "res://maps/map3/map_3_corner3.tscn";

	[Export]
	public string presentCorner = "res://maps/map3/map_3_corner2.tscn"; // Tetap di scene ini jika salah

	public override void _Ready()
	{
		var areaUp = GetNode<Area2D>("AreaUp");
		var areaLeft = GetNode<Area2D>("AreaLeft");
		var areaRight = GetNode<Area2D>("AreaRight");
		var areaDown = GetNode<Area2D>("AreaDown");

		areaUp.BodyEntered += OnAreaCorrectEntered;
		areaLeft.BodyEntered += OnAreaWrongEntered;
		areaRight.BodyEntered += OnAreaWrongEntered;
		areaDown.BodyEntered += OnAreaWrongEntered;
	}

	private void OnAreaCorrectEntered(Node body)
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

	private void NextStage()
	{
		GetTree().ChangeSceneToFile(nextCorner);
	}

	private void NowStage()
	{
		GetTree().ChangeSceneToFile(presentCorner);
	}
}
