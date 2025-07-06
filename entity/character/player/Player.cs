using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 100f;
	
	private AnimatedSprite2D animatedSprite;
	private Vector2 lastDirection = Vector2.Zero;
	private bool isMoving = false;
	
	public override void _Ready()
	{
		// Get AnimatedSprite2D node
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if (animatedSprite != null)
		{
			// Set default animation
			animatedSprite.Play("idle");
			GD.Print("Player animation system ready!");
		}
		else
		{
			GD.Print("ERROR: AnimatedSprite2D not found!");
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDirection = GetInputDirection();
		
		// Set velocity
		Velocity = inputDirection * Speed;
		
		// Update movement state
		isMoving = inputDirection != Vector2.Zero;
		
		// Update animation
		UpdateAnimation(inputDirection);
		
		// Move player
		MoveAndSlide();
	}
	
	private Vector2 GetInputDirection()
	{
		Vector2 direction = Vector2.Zero;
		
		// Get input
		if (Input.IsActionPressed("ui_right"))
			direction.X += 1;
		if (Input.IsActionPressed("ui_left"))
			direction.X -= 1;
		if (Input.IsActionPressed("ui_down"))
			direction.Y += 1;
		if (Input.IsActionPressed("ui_up"))
			direction.Y -= 1;
		
		// Normalize diagonal movement
		return direction.Normalized();
	}
	
	private void UpdateAnimation(Vector2 direction)
	{
		if (animatedSprite == null) return;
		
		if (isMoving)
		{
			// Update last direction untuk idle animation
			lastDirection = direction;
			
			// Tentukan animation berdasarkan direction
			string animationName = GetAnimationName(direction);
			
			// Play animation kalau berbeda dari yang sekarang
			if (animatedSprite.Animation != animationName)
			{
				animatedSprite.Play(animationName);
			}
		}
		else
		{
			if (animatedSprite.Animation != "idle")
			{
				animatedSprite.Play("idle");
			}
		}
	}
	
	private string GetAnimationName(Vector2 direction)
	{
		// Priority: horizontal movement over vertical
		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
		{
			return direction.X > 0 ? "right" : "left";
		}
		else
		{
			return direction.Y > 0 ? "back" : "front";
		}
	}
}
