using Godot;

public partial class Kerang : Node2D
{
	// Drag & drop reference ke Sprite2D di Inspector, atau pakai GetNode di _Ready
	[Export] public Sprite2D buka;
	[Export] public Sprite2D tutup;
	[Export] public Area2D triggerArea;
	
	public override void _Ready()
	{
		triggerArea.BodyEntered += OnBodyEntered;
	}
	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("mutiara")) // atau body is Mutiara
		{
			var cahaya = GetNode<Node2D>("../Cahaya");
			cahaya.Position = new Vector2(-622,-62);
			
			var dialogicBridge = GetNode("/root/Bridgedialogic");
			dialogicBridge.Call("_isMutiaraDimakan", true);
			buka.Hide();
			tutup.Show();
			
	   		body.QueueFree();
		}
	}
}
