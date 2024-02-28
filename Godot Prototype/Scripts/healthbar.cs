using Godot;
using System;

public partial class healthbar : TextureProgressBar
{

	Area2D player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent().GetChild<Node2D>(1).GetChild<Area2D>(0);
		MaxValue = player.health;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
