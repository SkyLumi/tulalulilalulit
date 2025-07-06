using Godot;
using System;

public partial class Map3C4 : Node2D
{
	[Export]
	public string NextCorner = "res://maps/map3/map_3_finish.tscn";

	[Export]
	public string PresentCorner = "res://maps/map3/map_3_corner4.tscn";

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
		GetTree().ChangeSceneToFile(NextCorner);
	}

	private void NowStage()
	{
		GetTree().ChangeSceneToFile(PresentCorner);
	}
}
