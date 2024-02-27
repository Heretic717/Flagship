using Godot;
using System;

public partial class LoadPlayerTemp : Node2D
{
	string[] shipPaths = new string[2] { "res://Scenes/base_ship_assembly.tscn", "res://Scenes/star_ship_assembly.tscn" };
	
	PackedScene shipModel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shipModel = GD.Load<PackedScene>(shipPaths[0]);
		CharacterBody2D ship = shipModel.Instantiate<CharacterBody2D>();
		AddChild(ship);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
