using Godot;
using System;

public partial class LoadPlayerTemp : Node2D
{
	string[] shipPaths = new string[] { "res://Scenes/base_ship_assembly.tscn", "res://Scenes/star_ship_assembly.tscn" };
	
	PackedScene shipModel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shipModel = GD.Load<PackedScene>(shipPaths[0]);
		RigidBody2D ship = shipModel.Instantiate<RigidBody2D>();
		AddChild(ship);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
