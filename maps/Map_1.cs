using Godot;
using System;

public partial class Map_1 : Node2D
{
	public override void _Ready()
	{
		var dialogicBridge = GetNode("/root/Bridgedialogic");
		dialogicBridge.Call("_isOpening");
	}
}
