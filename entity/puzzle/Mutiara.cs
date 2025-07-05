using Godot;

public partial class Mutiara : CharacterBody2D
{
	[Export] public float PushForce = 300.0f; // Kekuatan dorongan
	[Export] public float Friction = 0.85f; // Gesekan (0.9 = lambat berhenti, 0.1 = cepat berhenti)
	[Export] public float MinPushDistance = 60.0f; // Jarak minimum untuk mulai dorong
	
	private CharacterBody2D player;
	private Vector2 pushVelocity = Vector2.Zero;
	
	public override void _Ready()
	{
		GD.Print("Mutiara ready! Looking for player...");
		FindPlayer();
	}
	
	private void FindPlayer()
	{
		// Cari di group dulu
		player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		
		if (player == null)
		{
			// Fallback: cari manual di scene tree
			player = FindPlayerInScene(GetTree().CurrentScene);
		}
		
		if (player != null)
		{
			GD.Print($"Player found: {player.Name}");
		}
		else
		{
			GD.Print("ERROR: Player not found!");
		}
	}
	
	private CharacterBody2D FindPlayerInScene(Node node)
	{
		if (node is CharacterBody2D body)
		{
			return body;
		}
		
		foreach (Node child in node.GetChildren())
		{
			var result = FindPlayerInScene(child);
			if (result != null)
				return result;
		}
		
		return null;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (player == null) return;
		
		float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
		
		// Jika player cukup dekat untuk mendorong
		if (distanceToPlayer <= MinPushDistance && distanceToPlayer > 0)
		{
			// Hitung arah dorongan (dari player ke mutiara)
			Vector2 pushDirection = (GlobalPosition - player.GlobalPosition).Normalized();
			
			// Tambahkan gaya dorong
			pushVelocity += pushDirection * PushForce * (float)delta;
			
		}
		
		// Apply friction (perlambatan)
		pushVelocity *= Friction;
		
		// Set velocity dan move
		Velocity = pushVelocity;
		MoveAndSlide();
		
	}
}
