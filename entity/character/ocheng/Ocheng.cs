using Godot;
using System;

public partial class Ocheng : CharacterBody2D
{
	[Export] public NodePath PlayerPath;
	[Export] public float Speed = 50f;
	[Export] public float MinDistance = 15f;
	[Export] public NodePath AnimatedSpritePath = "AnimatedSprite2D";

	private CharacterBody2D _player;
	private AnimatedSprite2D _animSprite;

	public override void _Ready()
	{
		if (PlayerPath != null)
			_player = GetNode<CharacterBody2D>(PlayerPath);
		_animSprite = GetNode<AnimatedSprite2D>(AnimatedSpritePath);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player == null)
			return;

		Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
		float distance = GlobalPosition.DistanceTo(_player.GlobalPosition);

		if (distance > MinDistance)
		{
			float adjustedSpeed = Mathf.Lerp(0, Speed, Mathf.Clamp((distance - MinDistance) / 32f, 0f, 1f));
			Velocity = direction * adjustedSpeed;
			MoveAndSlide();

			// Pilih animasi berdasarkan arah dominan
			if (_animSprite != null)
			{
				string anim = "idle";
				if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
				{
					anim = direction.X > 0 ? "walk_right" : "walk_left";
				}
				else
				{
					anim = direction.Y < 0 ? "walk_front" : "walk_down";
				}

				if (_animSprite.Animation != anim)
					_animSprite.Play(anim);
			}
		}
		else
		{
			Velocity = Vector2.Zero;
			if (_animSprite != null && _animSprite.Animation != "default")
				_animSprite.Play("default");
		}
	}
}
