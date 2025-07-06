using Godot;
using System;

public partial class Map3F : Node2D
{
	[Export]
	public string PresentCorner = "res://maps/map3/map_3_finish.tscn";

	private bool _canInteractWithRose = false;
	private bool _hasInteractedWithRose = false;

	public override void _Ready()
	{
		var areaDown = GetNode<Area2D>("AreaDown");
		areaDown.BodyEntered += OnAreaEntered;

		var areaRose = GetNode<Area2D>("AreaRose");
		areaRose.BodyEntered += OnRoseEntered;
		areaRose.BodyExited += OnRoseExited;
	}

	public override void _Process(double delta)
	{
		if (_canInteractWithRose && !_hasInteractedWithRose && Input.IsActionJustPressed("ui_accept"))
		{
			TriggerRoseDialog();
		}
	}

	private void OnAreaEntered(Node body)
	{
		if (body.IsInGroup("player"))
		{
			CallDeferred(nameof(ReloadScene));
		}
	}

	private void ReloadScene()
	{
		GetTree().ChangeSceneToFile(PresentCorner);
	}

	private void OnRoseEntered(Node body)
	{
		if (body.IsInGroup("player"))
		{
			_canInteractWithRose = true;

			var label = GetNode<Label>("Label");
			label.Visible = true;
		}
	}

	private void OnRoseExited(Node body)
	{
		if (body.IsInGroup("player"))
		{
			_canInteractWithRose = false;
			var label = GetNode<Label>("Label");
			label.Visible = false;
		}
	}

	private void TriggerRoseDialog()
	{
		_hasInteractedWithRose = true;

		var label = GetNode<Label>("Label");
		label.Visible = false;

		var dialogicBridge = GetNode("/root/Bridgedialogic");
		dialogicBridge?.Call("_map03Dial05");

		var boquet = GetNode<Sprite2D>("Boquet");
		boquet.Visible = true;
		var areaRose = GetNode<Area2D>("AreaRose");
		var collision = areaRose.GetNode<CollisionShape2D>("CollisionShape2D");

		areaRose.Monitoring = false;
		areaRose.Visible = false;
		collision.Disabled = true;
	}
}
