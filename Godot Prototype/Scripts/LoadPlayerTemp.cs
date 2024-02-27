using Godot;
using System;


public partial class LoadPlayerTemp : Node2D
{
	string[] shipPaths = new string[] { "res://Scenes/base_ship_assembly.tscn", "res://Scenes/star_ship_assembly.tscn" };
	int selectedShip;
	PackedScene shipModel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		selectedShip = GetNode<Node>("/root/UserVariables").Get("loadedShip").As<int>();
		shipModel = GD.Load<PackedScene>(shipPaths[selectedShip]);
		Area2D ship = shipModel.Instantiate<Area2D>();
		AddChild(ship);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
