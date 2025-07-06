using Godot;
using System;

public partial class Map2 : Node2D
{
    [Export] public NodePath Player;
    [Export] public NodePath Label;
    private Node2D _player;
    private Label _nearestLabel;

    public override void _Ready()
    {
        GD.Print($"Path: {Player}, {Label}");
        // _player = GetNode<Node2D>("Player");
        // _nearestLabel = GetNode<Label>("NearestLabel");
    }

    public override void _PhysicsProcess(double delta)
    {
        GD.Print("Ayoyo");
        // Node2D nearest = GetNearestBucket();
        // if (nearest != null)
        //     _nearestLabel.Text = $"Terdekat: {nearest.Name}";
        // else
        //     _nearestLabel.Text = "Tidak ada bucket";
    }

    private Node2D GetNearestBucket()
    {
        Node2D nearest = null;
        float nearestDist = float.MaxValue;

        foreach (Node node in GetTree().GetNodesInGroup("buckets"))
        {
            if (node is Node2D bucket)
            {
                float dist = _player.GlobalPosition.DistanceTo(bucket.GlobalPosition);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearest = bucket;
                }
            }
        }

        return nearest;
    }
}
