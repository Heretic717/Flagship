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
	public float speedX = 0f; // speed on the x axis
	public float speedY = 0f; // speed on the y axis



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		//put a max and min on acceleration to prevent extreme speed or rubberbanding on deceleration
		if (acceleration > accel * 10)
			acceleration = accel * 10;
		else if (acceleration < 0)
			acceleration = 0;

		//set velocity	
		velocity += acceleration * (float)delta;



		if (Input.IsActionPressed("MovementUp") || Input.IsActionPressed("MovementDown") || Input.IsActionPressed("MovementLeft") || Input.IsActionPressed("MovementRight"))
		{
			ddp += accel; // accelerate if any movement key is pressed
		}
		
		// calculate directional speed based on which key was pressed
		if (Input.IsActionPressed("MovementUp"))
		{
			acceleration += accel;
			speedY -= velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsPhysicalKeyPressed(Key.A) || Input.IsPhysicalKeyPressed(Key.D)) {
				speedY *= .975f;
				acceleration -= accel;
			}
		}
		if (Input.IsActionPressed("MovementDown"))
		{
			acceleration += accel;
			speedY += velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsPhysicalKeyPressed(Key.A) || Input.IsPhysicalKeyPressed(Key.D))
			{
				speedY *= .975f;
				acceleration -= accel;
			}
		}
		if (Input.IsActionPressed("MovementLeft"))
		{
			acceleration += accel;
			speedX -= velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsPhysicalKeyPressed(Key.W) || Input.IsPhysicalKeyPressed(Key.S))
			{
				speedX *= .975f;
				acceleration -= accel;
			}
		}
		if (Input.IsActionPressed("MovementRight"))
		{
			acceleration += accel;
			speedX += velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsPhysicalKeyPressed(Key.W) || Input.IsPhysicalKeyPressed(Key.S))
			{
				speedX *= .975f;
				acceleration -= accel;
			}
		}

		// set new position based on current directional speed
		this.Position += new Vector2(speedX, speedY);


		// friction for smooth accel/decel
		if (speedY != 0)
		{
			speedY *= .95f;
		} 
		if (speedX != 0)
		{
			speedX *= .95f;
		}
		acceleration -= velocity * 5f;
	}
}
