using Godot;
using System;

public partial class base_ship_move : CharacterBody2D
{
	float velocity = 0f; //velocity
	float acceleration = 0f; //real acceleration
	float radialAcceleration = 0f;
	float radialVelocity = 0f;
	float rotationSpeed;
	const float accel = 50f; // base value for acceleration
	public static Vector2 speed = new(0, 0); // speed on x and y axis


	public override void _PhysicsProcess(double delta)
	{
		//put a max and min on acceleration to prevent extreme speed or rubberbanding on deceleration
		if (acceleration > accel * 10)
			acceleration = accel * 10;
		else if (acceleration < 0)
			acceleration = 0;
		if (radialAcceleration > accel * 5)
			radialAcceleration = accel * 5;
		else if (radialAcceleration < 0)
			radialAcceleration = 0;

		//set velocity	
		velocity += acceleration * (float)delta;
		radialVelocity += radialAcceleration * (float)delta;
		

		// calculate directional speed based on which key was pressed
		if (Input.IsActionPressed("MovementUp"))
		{
			acceleration += accel;
			speed.X += velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
		}
		if (Input.IsActionPressed("MovementDown"))
		{
			speed.X *= .8f;
		}
		if (Input.IsActionPressed("MovementLeft"))
		{
			radialAcceleration += accel;
			rotationSpeed += (-radialVelocity * (float)delta + radialAcceleration * (float)delta * (float)delta * .5f) / 180 * 3.14f;
			rotationSpeed *= .7f;
		}
		if (Input.IsActionPressed("MovementRight"))
		{
			radialAcceleration += accel;
			rotationSpeed += (radialVelocity * (float)delta + radialAcceleration * (float)delta * (float)delta * .5f) / 180 * 3.14f;
			rotationSpeed *= .7f;
		}


		// set new position and rotationSpeed based on current speed
		Rotate(rotationSpeed);
		MoveAndCollide(new Vector2(speed.X, speed.Y).Rotated(Rotation));


		// friction for smooth accel/decel
		if (speed.Y != 0)
		{
			speed.Y *= .95f;
		}
		if (speed.X != 0)
		{
			speed.X *= .95f;
		}
		if (rotationSpeed != 0)
		{
			rotationSpeed *= .95f;
		}

		acceleration -= velocity * 5f;
		radialAcceleration -= radialVelocity * 5f;
	}
}
