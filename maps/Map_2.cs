using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Map_2 : Node2D
{
    private enum ItemTypes
    {
        Axe,
        Hammer,
        Ribbon
    }
    private enum ItemConditions
    {
        NotTaken, // Item still exist in the world, therefore it's visible
        Taken,    // Item is taken, therefore not visible and is held by the player. It's not visible
        Placed    // Item is taken and placed in the box. It's not visible
    }
    private struct Item
    {
        public Node2D itemObject { get; set; }
        public ItemTypes itemType { get; set; }
        public ItemConditions itemCondition { get; set; }

    }
    // Start Region of Export
    [Export] public NodePath PlayerPath;
    [Export] public NodePath LabelPath;
    [Export] public NodePath Item1Path; // Change to object name later
    [Export] public NodePath Item2Path; // Change to object name later
    [Export] public NodePath Item3Path; // Change to object name later
    // End Region of Export
    private Node2D _player;
    private Label _nearestLabel;
    private Item[] items = new Item[3];

    public override void _Ready()
    {
        GD.Print($"Path: {PlayerPath}, {LabelPath}");
        _player = GetNode<Node2D>(PlayerPath);
        Camera2D camera = _player.GetNode<Camera2D>("Camera2D");
        _nearestLabel = GetNode<Label>(LabelPath);
        items = [
            new Item{
                itemObject = GetNode<Node2D>(Item1Path),
                itemType = ItemTypes.Axe,
                itemCondition = ItemConditions.NotTaken
            },
            new Item{
                itemObject = GetNode<Node2D>(Item2Path),
                itemType = ItemTypes.Hammer,
                itemCondition = ItemConditions.NotTaken
            },
            new Item{
                itemObject = GetNode<Node2D>(Item3Path),
                itemType = ItemTypes.Hammer,
                itemCondition = ItemConditions.NotTaken
            }
        ];
    }

    public override void _PhysicsProcess(double delta)
    {
        Node2D nearest = GetNearestBucket();
        CollideItem();
    }

    private Node2D GetNearestBucket()
    {
        float nearestDist = float.MaxValue;
        Godot.Collections.Array<Node> Buckets = GetTree().GetNodesInGroup("Buckets");
        Godot.Collections.Array<Node> Labels = GetTree().GetNodesInGroup("Labels");
        int? indexAt = null;
        for (int i = 0; i < Buckets.Count; i++)
        {
            if (Buckets[i] is Node2D bucket)
            {
                float dist = _player.GlobalPosition.DistanceTo(bucket.GlobalPosition);
                if (dist < nearestDist && dist < 50)
                {
                    nearestDist = dist;
                    indexAt = i;
                }
                ((Label)Labels[i]).Visible = false;
            }
        }
        if (indexAt != null)
        {
            ((Label)Labels[(int)indexAt]).Visible = true;
            return (Node2D)Buckets[(int)indexAt];
        }
        else
        {
            return null;
        }

    }
    private void CollideItem()
    {
        float detectionRadius = 40.0f;

        for (int i =0; i < items.Count(); i++)
        {
            Item itemStruct = items[i];
            if (itemStruct.itemCondition == ItemConditions.NotTaken)
            {
                Node node = itemStruct.itemObject;
                if (node is Node2D item)
                {
                    float distance = _player.GlobalPosition.DistanceTo(item.GlobalPosition);

                    if (distance <= detectionRadius)
                    {
                        GD.Print($"Item dekat terdeteksi: {item.Name} (jarak: {distance})");
                        items[i].itemCondition = ItemConditions.Taken;
                        item.Visible = false;
                        item.GetNode<StaticBody2D>("StaticBody2D").GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
                        // Contoh aksi: tampilkan info item, sorot, dll
                    }
                }
            }
        }
    }
}
