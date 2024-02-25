using Godot;
using System;

public partial class projectile_logic : RigidBody2D
{

	public Vector2 velocity = new(0, 0);
	float speed = 500f;
	Vector2 startingPos;
	float range = .8f;
	float totalMoved;
	Timer timer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startingPos = Position;
		timer = GetChild<Timer>(2);
		timer.WaitTime = range + GD.RandRange(-.1, .1);
		timer.Start();
		timer.Timeout += () => QueueFree();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);;
		MoveAndCollide(new Vector2(velocity.X * (float)delta * speed, velocity.Y * (float)delta * speed));
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{

	}
}