using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class turret : Sprite2D
{

	private float StartRot = 0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StartRot = this.Rotation;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Vector2 CursorPos = GetGlobalMousePosition();


		LookAt(CursorPos);

	}
}
