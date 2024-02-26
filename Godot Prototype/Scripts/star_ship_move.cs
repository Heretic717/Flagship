using Godot;
using System;

public partial class Ship_move : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	float velocity = 0f; //velocity
	float acceleration = 0f; //real acceleration
	const float accel = 50f; // base value for acceleration
	public static Vector2 speed = new(0,0); // speed on x and y axis



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

		//put a max and min on acceleration to prevent extreme speed or rubberbanding on deceleration
		if (acceleration > accel * 10)
			acceleration = accel * 10;
		else if (acceleration < 0)
			acceleration = 0;

		//set velocity	
		velocity += acceleration * (float)delta;
		
		// calculate directional speed based on which key was pressed
		if (Input.IsActionPressed("MovementUp"))
		{
			acceleration += accel;
			speed.Y -= velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementLeft") || Input.IsActionPressed("MovementRight")) {
				speed.Y *= .975f;
				acceleration -= accel;
			}
		}
		if (Input.IsActionPressed("MovementDown"))
		{
			acceleration += accel;
			speed.Y += velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementLeft") || Input.IsActionPressed("MovementRight"))
			{
				speed.Y *= .975f;
				acceleration -= accel;
			}
		}
		if (Input.IsActionPressed("MovementLeft"))
		{
			acceleration += accel;
			speed.X -= velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementUp") || Input.IsActionPressed("MovementDown"))
			{
				speed.X *= .975f;
				acceleration -= accel;
			}
		}
		if (Input.IsActionPressed("MovementRight"))
		{
			acceleration += accel;
			speed.X += velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementUp") || Input.IsActionPressed("MovementDown"))
			{
				speed.X *= .975f;
				acceleration -= accel;
			}
		}

		// set new position based on current directional speed
		MoveAndCollide(new Vector2(speed.X, speed.Y));


		// friction for smooth accel/decel
		if (speed.Y != 0)
		{
			speed.Y *= .95f;
		} 
		if (speed.X != 0)
		{
			speed.X *= .95f;
		}
		acceleration -= velocity * 5f;
	}
}
