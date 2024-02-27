using Godot;
using System;
using System.Reflection;

public partial class base_ship_move : RigidBody2D
{
	float velocity = 0f; //velocity
	float acceleration = 0f; //real acceleration
	float radialAcceleration = 0f;
	float radialVelocity = 0f;
	float rotationSpeed;
	const float accel = 20f; // base value for acceleration
	public static Vector2 speed = new(0, 0); // speed on x and y axis
	GpuParticles2D thrusterMain;
	GpuParticles2D thruster1;
	GpuParticles2D thruster2;
	GpuParticles2D thruster3;
	GpuParticles2D thruster4;
	int intersectingBodies;
	Area2D Attack_Orbit;

	public float health = 200;

	int collidingBodies;

	public override void _Ready()
	{

		thrusterMain = GetChild<Sprite2D>(2).GetChild<Node2D>(0).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster1 = GetChild<Sprite2D>(2).GetChild<Node2D>(1).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster2 = GetChild<Sprite2D>(2).GetChild<Node2D>(2).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster3 = GetChild<Sprite2D>(2).GetChild<Node2D>(3).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster4 = GetChild<Sprite2D>(2).GetChild<Node2D>(4).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);

		Attack_Orbit = GetChild<Area2D>(3);
		Attack_Orbit.BodyEntered += (RigidBody2D) => _on_Attack_Orbit_body_entered((enemy_fighter_AI)Attack_Orbit.GetOverlappingBodies()[intersectingBodies]);
		Attack_Orbit.BodyExited += (RigidBody2D) => _on_Attack_Orbit_body_exited((enemy_fighter_AI)Attack_Orbit.GetOverlappingBodies()[intersectingBodies]);

		ContactMonitor = true;
		MaxContactsReported = 100;
		BodyEntered += (RigidBody2D) => _on_Hit((RigidBody2D)GetCollidingBodies()[collidingBodies]);
	}

	public override void _PhysicsProcess(double delta)
	{
		intersectingBodies = Attack_Orbit.GetOverlappingBodies().Count;
		collidingBodies = GetCollidingBodies().Count;
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
			thrusterMain.Emitting = true;
			thruster1.Emitting = true;
			thruster2.Emitting = true;
		}
		if (Input.IsActionPressed("MovementDown"))
		{
			speed.X *= .8f;
			thrusterMain.Emitting = false;
			thruster1.Emitting = false;
			thruster2.Emitting = false;
		}
		if (Input.IsActionPressed("MovementLeft"))
		{
			radialAcceleration += accel;
			rotationSpeed += (-radialVelocity * (float)delta + radialAcceleration * (float)delta * (float)delta * .5f) / 180 * 3.14f;
			rotationSpeed *= .8f;
			thruster4.Emitting = true;
		}
		if (Input.IsActionPressed("MovementRight"))
		{
			radialAcceleration += accel;
			rotationSpeed += (radialVelocity * (float)delta + radialAcceleration * (float)delta * (float)delta * .5f) / 180 * 3.14f;
			rotationSpeed *= .8f;
			thruster3.Emitting = true;
		}

		if (Input.IsActionJustReleased("MovementUp")) {
			thrusterMain.Emitting = false;
			thruster1.Emitting = false;
			thruster2.Emitting = false;
		}
		if (Input.IsActionJustReleased("MovementLeft")) {
			thruster4.Emitting = false;
		}
		if (Input.IsActionJustReleased("MovementRight")) {
			thruster3.Emitting = false;
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

	private void _on_Attack_Orbit_body_entered(enemy_fighter_AI body) {

		if (body.IsInGroup("Enemy"))
			body.state = enemy_fighter_AI.State.ORBIT;
	}
	private void _on_Attack_Orbit_body_exited(enemy_fighter_AI body)
	{

		if (body.IsInGroup("Enemy"))
			body.state = enemy_fighter_AI.State.APPROACH;
	}

	private void hurt()
	{
		health -= 1;
		GD.Print(health);
	}

	private void _on_Hit(RigidBody2D body)
	{

		if (body.IsInGroup("enemyprojectile"))
		{
			GD.Print("hurt");
			hurt();
			body.QueueFree();
		}
	}
}
