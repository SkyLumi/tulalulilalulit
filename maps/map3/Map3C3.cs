using Godot;
using System;

public partial class Map3C3 : Node2D
{
	[Export]
	public string NextCorner = "res://maps/map3/map_3_corner4.tscn";

	[Export]
	public string PresentCorner = "res://maps/map3/map_3_corner3.tscn"; // Tetap di sini jika salah

	public override void _Ready()
	{
		var areaLeft = GetNode<Area2D>("AreaLeft");
		var areaUp = GetNode<Area2D>("AreaUp");
		var areaRight = GetNode<Area2D>("AreaRight");
		var areaDown = GetNode<Area2D>("AreaDown");

		areaLeft.BodyEntered += OnAreaCorrectEntered;
		areaUp.BodyEntered += OnAreaWrongEntered;
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
		GetTree().ChangeSceneToFile(NextCorner);
	}

	private void NowStage()
	{
		GetTree().ChangeSceneToFile(PresentCorner);
	}
}
