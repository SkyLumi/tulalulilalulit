using Godot;

public partial class Cahaya : Node2D
{
	[Export] public Area2D triggerArea;
	[Export] public Node2D bunga;
	
	public override void _Ready()
	{
		triggerArea.BodyEntered += OnBodyEntered;
	}
	
	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("player"))
		{
			var dialogicBridge = GetNode("/root/Bridgedialogic");
			dialogicBridge.Call("_isCahayaPuzzle");
			dialogicBridge.Connect("ngirim_dialog", new Callable(this, nameof(OnDialogFinished)));
		}
		
	}
	
	private void OnDialogFinished(string result)
	{
		if (result == "bunga")
		{
			bunga.Show();
			var tween = GetTree().CreateTween();
			tween.TweenProperty(bunga, "position:y", -100, 1.0);
		}
		
		else if (result == "selesai")
		{
			GetTree().ChangeSceneToFile("res://maps/map_2.tscn");
		}
	}
}
