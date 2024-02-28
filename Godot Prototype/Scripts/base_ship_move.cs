using Godot;
using System;
using System.Reflection;

public partial class base_ship_move : Area2D
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
	Area2D Attack_Orbit;

	PackedScene death = GD.Load<PackedScene>("res://Effects/Explosion_dead.tscn");
	AudioStreamPlayer2D explode;

	AudioStreamPlayer2D thrust1;
	AudioStreamPlayer2D thrust2;
	AudioStreamPlayer2D thrust3;


	public float health;

	public float maxHealth = 200;

	public override void _Ready()
	{

		thrusterMain = GetChild<Sprite2D>(2).GetChild<Node2D>(0).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster1 = GetChild<Sprite2D>(2).GetChild<Node2D>(1).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster2 = GetChild<Sprite2D>(2).GetChild<Node2D>(2).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster3 = GetChild<Sprite2D>(2).GetChild<Node2D>(3).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster4 = GetChild<Sprite2D>(2).GetChild<Node2D>(4).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);

		explode = GetNode("/root/Sfx").GetChild<AudioStreamPlayer2D>(0);
		thrust1 = GetNode("/root/Sfx").GetChild<AudioStreamPlayer2D>(4);
		thrust2 = GetNode("/root/Sfx").GetChild<AudioStreamPlayer2D>(4);
		thrust3 = GetNode("/root/Sfx").GetChild<AudioStreamPlayer2D>(4);

		Attack_Orbit = GetChild<Area2D>(3);

		AreaEntered += (Area2D body) => _on_Hit(body);
		health = maxHealth;

	}

	public override void _PhysicsProcess(double delta)
	{

		if (health <= 0) 
		{
			_on_Death();
		}
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
			thrust1.GlobalPosition = thrusterMain.GlobalPosition;
			if (!thrust1.Playing)
			{
				thrust1.Play();
			}
		}
		if (Input.IsActionPressed("MovementDown"))
		{
			speed.X *= .8f;
			thrusterMain.Emitting = false;
			thruster1.Emitting = false;
			thruster2.Emitting = false;
			if (thrust1.Playing)
			{
				thrust1.Stop();
			}
		}
		if (Input.IsActionPressed("MovementLeft"))
		{
			radialAcceleration += accel;
			rotationSpeed += (-radialVelocity * (float)delta + radialAcceleration * (float)delta * (float)delta * .5f) / 180 * 3.14f;
			rotationSpeed *= .8f;
			thruster4.Emitting = true;
			thrust2.GlobalPosition = thruster4.GlobalPosition;
			if (!thrust3.Playing)
			{
				thrust2.Play();
			}

		}
		if (Input.IsActionPressed("MovementRight"))
		{
			radialAcceleration += accel;
			rotationSpeed += (radialVelocity * (float)delta + radialAcceleration * (float)delta * (float)delta * .5f) / 180 * 3.14f;
			rotationSpeed *= .8f;
			thruster3.Emitting = true;
			thrust3.GlobalPosition = thruster3.GlobalPosition;
			if (!thrust3.Playing)
			{
				thrust3.Play();
			}
		}

		if (Input.IsActionJustReleased("MovementUp")) {
			thrusterMain.Emitting = false;
			thruster1.Emitting = false;
			thruster2.Emitting = false;
			thrust1.Stop();
		}
		if (Input.IsActionJustReleased("MovementLeft")) {
			thruster4.Emitting = false;
			thrust3.Stop();
		}
		if (Input.IsActionJustReleased("MovementRight")) {
			thruster3.Emitting = false;
			thrust2.Stop();
		}


		// set new position and rotationSpeed based on current speed
		Rotate(rotationSpeed);
		Position += new Vector2(speed.X, speed.Y).Rotated(Rotation);


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

	private void hurt()
	{
		health -= 10;
	}

	private void _on_Hit(Area2D body)
	{

		if (body.IsInGroup("enemyprojectilesmall"))
		{
			hurt();
			body.QueueFree();
		}
	}

	private async void _on_Death()
	{
		// spawn explosion particles here

		explode.Play();
		explode.GlobalPosition = GlobalPosition;
		GpuParticles2D deathExplosion = death.Instantiate<GpuParticles2D>();
		GetTree().Root.AddChild(deathExplosion);
		deathExplosion.GlobalPosition = GlobalPosition;

		// stop the game loop and display the game over screen
		await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);

		GetTree().ChangeSceneToFile("res://Scenes/GameOver.tscn");

	}
}
