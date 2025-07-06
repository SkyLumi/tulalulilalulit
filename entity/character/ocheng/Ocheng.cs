using Godot;
using System;

public partial class Ocheng : CharacterBody2D
{
	[Export] public NodePath PlayerPath;
	[Export] public float Speed = 80f;
	[Export] public float MinDistance = 12f;

	private Node2D _player;
	private NavigationAgent2D _agent;
	private AnimatedSprite2D _anim;

	public override void _Ready()
	{
		_agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (PlayerPath != null)
			_player = GetNode<Node2D>(PlayerPath);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player == null || _agent == null)
			return;

		_agent.TargetPosition = _player.GlobalPosition;
		float distance = GlobalPosition.DistanceTo(_player.GlobalPosition);

		if (distance > MinDistance)
		{
			Vector2 nextPoint = _agent.GetNextPathPosition();
			Vector2 direction = (nextPoint - GlobalPosition).Normalized();
			Velocity = direction * Speed;
			MoveAndSlide();

			// Pilih animasi berdasarkan arah
			if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
			{
				// Gerak dominan kanan/kiri
				if (direction.X > 0)
					_anim.Play("walk_right");
				else
					_anim.Play("walk_left");
			}
			else
			{
				// Gerak dominan atas/bawah
				if (direction.Y > 0)
					_anim.Play("walk_down");
				else
					_anim.Play("walk_front");
			}
		}
		else
		{
			Velocity = Vector2.Zero;
			_anim.Play("default");
		}
	}
}
