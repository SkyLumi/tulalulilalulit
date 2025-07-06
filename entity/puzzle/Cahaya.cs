using Godot;
using System;

public partial class Cahaya : Node2D
{
	[Export] public Area2D triggerArea;
	
	public override void _Ready()
	{
		triggerArea.BodyEntered += OnBodyEntered;
	}
	
	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("mutiara"))
		{
			var dialogicBridge = GetNode("/root/Bridgedialogic");
			dialogicBridge.Call("_isCahayaPuzzle", true);
			
	   		body.QueueFree();
		}
	}
}
