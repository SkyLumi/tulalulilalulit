using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;

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
	[Export] public NodePath AxePath; // Change to object name later
	[Export] public NodePath CarvePath; // Change to object name later
	[Export] public NodePath SawPath; // Change to object name later
									  // End Region of Export
	// Start Region of Attribute
	private Node2D _player;
	private Label _nearestLabel;
	private Item[] items = new Item[3]; // Order 0 => kapak, 1=> pisau ukir, dan 2 => gergaji
	private Godot.Collections.Array<Node> Buckets { get; set; } 
	private Godot.Collections.Array<Node> Labels { get; set; }
	private bool[] isStored = [false, false, false];
	private AnimatedSprite2D Bucket { get; set;}
	private int IndexBucket { get; set; }
	private Node2D bridgeNode { get; set; }
	private bool waitForText = false;
	// Use this Dictionary to point if the order of item in the boxes are correct
	// Key or the correct order
	private int[] CorrectOrderIndexItems = [2,1,0]; // SO the correct order are gergaji, pisau ukir, dan kapak
	// Data
	private Dictionary<int, int> kIBucketvIItem = new Dictionary<int, int>(){ // Key is index of bucket and value is index of items
																		{0, -1},
																		{1, -1},
																		{2, -1}
																		};
	private bool IsFilled { get; set; }
	// End Region of Attribute

	public override void _Ready()
	{
		GD.Print($"Path: {PlayerPath}, {LabelPath}");
		_player = GetNode<Node2D>(PlayerPath);
		_nearestLabel = GetNode<Label>(LabelPath);
		Buckets = GetTree().GetNodesInGroup("Buckets");
		Labels = GetTree().GetNodesInGroup("Labels");
		items = [
			new Item{
				itemObject = GetNode<Node2D>(AxePath),
				itemType = ItemTypes.Axe,
				itemCondition = ItemConditions.NotTaken
			},
			new Item{
				itemObject = GetNode<Node2D>(CarvePath),
				itemType = ItemTypes.Hammer,
				itemCondition = ItemConditions.NotTaken
			},
			new Item{
				itemObject = GetNode<Node2D>(SawPath),
				itemType = ItemTypes.Hammer,
				itemCondition = ItemConditions.NotTaken
			}
		];
		bridgeNode = this.GetNode<Node2D>("TextChoice");
		bridgeNode.Connect("dialog_finished", new Callable(this, nameof(OnDialogFinished)));
	}

	public override void _PhysicsProcess(double delta)
	{
		Node2D nearest = GetNearestBucket();
		CollideItem();
		// foreach (Item item in items)
		// {
		// 	GD.Print($"{item.itemObject.Name}, {item.itemCondition}");
		// }
	}

	private Node2D GetNearestBucket()
	{
		float nearestDist = float.MaxValue;

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
			if (Input.IsActionPressed("ui_accept"))
			{
				// GD.Print($"Pressed at {Buckets[(int)indexAt].Name} with label {((Label)Labels[(int)indexAt]).Name}");
				if (!waitForText){
					IndexBucket = (int)indexAt;
					Bucket = Buckets[(int)indexAt].GetNode<AnimatedSprite2D>("AnimatedSprite2D");
					if (!Bucket.IsPlaying())
					{
						// for (int i=0; i<items.Count();i++)
						// {
						// 	if (items[i].itemCondition == ItemConditions.Taken)
						// 	{
						IsFilled = false;
						string begginingText = "Some Clue";
						string choiceText = "Pilih barang yang ingin dimasukkan";
						bridgeNode.Call("ShowDialog", begginingText, choiceText, isStored[0], isStored[1], isStored[2]);
						waitForText = true;
						// 	}
						// }
					}
					else
					{
						IsFilled = true;
						string begginingText = "Kotak sudah terisi";
						string choiceText = "Pilih tindakan yang ingin dilakukan";
						bool[] isItemInBucket = [false, false, false];
						// Rubah untuk deteksi item pada indeks mana yang ada pada bucket
						isItemInBucket[kIBucketvIItem[IndexBucket]] = true;
						bridgeNode.Call("ShowDialog", begginingText, choiceText, isItemInBucket[0], isItemInBucket[1], isItemInBucket[2]);
						waitForText = true;
					}
				}
			}
			return (Node2D)Buckets[(int)indexAt];
		}
		else
		{
			return null;
		}

	}
	private void OnDialogFinished(string result)
	{
		switch (result)
		{
			case "kapak":
				if (IsFilled)
				{
					isStored[0] = true;
					items[0].itemCondition = ItemConditions.Taken;
					kIBucketvIItem[IndexBucket] = -1;
					Bucket.Stop();
				}
				else
				{
					isStored[0] = false;
					items[0].itemCondition = ItemConditions.Placed;
					kIBucketvIItem[IndexBucket] = 0;
					Bucket.Play();
				}
				break;
			case "pisau ukir":
				if (IsFilled)
				{
					isStored[1] = true;
					items[1].itemCondition = ItemConditions.Taken;
					kIBucketvIItem[IndexBucket] = -1;
					Bucket.Stop();
				}
				else
				{
					isStored[1] = false;
					items[1].itemCondition = ItemConditions.Placed;
					kIBucketvIItem[IndexBucket] = 1;
					Bucket.Play();
				}
				break;
			case "gergaji":
				if (IsFilled)
				{
					isStored[2] = true;
					items[2].itemCondition = ItemConditions.Taken;
					kIBucketvIItem[IndexBucket] = -1;
					Bucket.Stop();
				}
				else
				{
					isStored[2] = false;
					items[2].itemCondition = ItemConditions.Placed;
					kIBucketvIItem[IndexBucket] = 2;
					Bucket.Play();
				}
				break;
			default:
				break;
		}
		waitForText = false;
		if (!IsFilled)
		{
			int index = 0;
			foreach (KeyValuePair<int, int> keyVal in kIBucketvIItem)
			{
				GD.Print($"Bucket: {keyVal.Key}, item: {keyVal.Value}");
				GD.Print($"Buckets Region");
				GD.Print($"Bucket: {keyVal.Key}, item: {keyVal.Value}");
				if (keyVal.Value != CorrectOrderIndexItems[index])
				{
					return;
				}
				index++;
			}
			GD.Print("Kamu Menang");
			GetTree().Quit(); // Rubah buat logic ke scene selanjutnya.
		}
	}
	private void CollideItem()
	{
		float detectionRadius = 40.0f;

		for (int i = 0; i < items.Count(); i++)
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
						isStored[i] = true;
						item.Visible = false;
						item.GetNode<StaticBody2D>("StaticBody2D").GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
						// Contoh aksi: tampilkan info item, sorot, dll
					}
				}
			}
		}
	}
}
