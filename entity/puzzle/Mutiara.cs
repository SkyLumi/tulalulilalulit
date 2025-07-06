using Godot;

public partial class Mutiara : CharacterBody2D
{
	[Export] public float PushForce = 250.0f;
	[Export] public float Friction = 0.82f;
	[Export] public float MaxSpeed = 150.0f;
	[Export] public float PushDistance = 25.0f;
	
	private Vector2 pushVelocity = Vector2.Zero;
	private CharacterBody2D player;
	private bool isPushing = false;
	
	public override void _Ready()
	{
		FindPlayer();
		GD.Print("Strict push mechanic ready!");
	}
	
	private void FindPlayer()
	{
		player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		if (player == null)
		{
			player = FindPlayerInScene(GetTree().CurrentScene);
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
		
		CheckPushInput(delta);
		
		// Apply friction
		pushVelocity *= Friction;
		
		// Limit max speed
		if (pushVelocity.Length() > MaxSpeed)
		{
			pushVelocity = pushVelocity.Normalized() * MaxSpeed;
		}
		
		// Set velocity dan move
		Velocity = pushVelocity;
		MoveAndSlide();
	}
	
	private void CheckPushInput(double delta)
	{
		float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
		
		// Reset pushing status
		isPushing = false;
		
		// Cek jarak
		if (distanceToPlayer <= PushDistance)
		{
			Vector2 pushDirection = (GlobalPosition - player.GlobalPosition).Normalized();
			
			// Cek input player untuk setiap arah
			bool canPush = false;
			Vector2 inputDirection = Vector2.Zero;
			
			// Deteksi input dan arah dorongan
			if (Input.IsActionPressed("ui_right") && pushDirection.X > 0.5f)
			{
				canPush = true;
				inputDirection = Vector2.Right;
			}
			else if (Input.IsActionPressed("ui_left") && pushDirection.X < -0.5f)
			{
				canPush = true;
				inputDirection = Vector2.Left;
			}
			else if (Input.IsActionPressed("ui_up") && pushDirection.Y < -0.5f)
			{
				canPush = true;
				inputDirection = Vector2.Up;
			}
			else if (Input.IsActionPressed("ui_down") && pushDirection.Y > 0.5f)
			{
				canPush = true;
				inputDirection = Vector2.Down;
			}
			
			if (canPush)
			{
				isPushing = true;
				
				// Apply push
				pushVelocity += pushDirection * PushForce * (float)delta;
				
				// Visual feedback
				Modulate = Colors.Yellow;
				
			}
			else
			{
				// Reset visual
				Modulate = Colors.White;
			}
		}
		else
		{
			// Reset visual
			Modulate = Colors.White;
		}
	}
}
