using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float speed = 200f;
	private float speedMultiplier = 1.0f;
	
	// Dipanggil tiap frame
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Vector2.Zero;

		// Input gerakan (WASD atau Arrow keys)
		if (Input.IsActionPressed("ui_right"))
			velocity.X += 1;
		if (Input.IsActionPressed("ui_left"))
			velocity.X -= 1;
		if (Input.IsActionPressed("ui_down"))
			velocity.Y += 1;
		if (Input.IsActionPressed("ui_up"))
			velocity.Y -= 1;

		// Normalisasi agar diagonal gak lebih cepat
		if (velocity != Vector2.Zero)
			velocity = velocity.Normalized();

		// Set velocity untuk CharacterBody2D
		Velocity = velocity * speed * speedMultiplier;
		
		MoveAndSlide();
	}
	
	public void SetSpeedMultiplier(float multiplier)
	{
		speedMultiplier = multiplier;
	}
}
