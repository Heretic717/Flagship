using Godot;
using System;

public partial class star_ship_move : Area2D
{
	// Called when the node enters the scene tree for the first time.
	

	float velocity = 0f; //velocity
	float acceleration = 0f; //real acceleration
	const float accel = 10f; // base value for acceleration
	public static Vector2 speed = new(0,0); // speed on x and y axis

	GpuParticles2D thruster1;
	GpuParticles2D thruster2;
	GpuParticles2D thruster3;
	GpuParticles2D thruster4;
	GpuParticles2D thruster5;
	GpuParticles2D thruster6;
	GpuParticles2D thruster7;
	GpuParticles2D thruster8;
	Area2D Attack_Orbit;

	PackedScene death;

	public float health;
	public float maxHealth = 200;

	AudioStreamPlayer2D explode;


	public override void _Ready()
	{
		thruster1 = GetChild<Sprite2D>(2).GetChild<Node2D>(0).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster2 = GetChild<Sprite2D>(2).GetChild<Node2D>(1).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster3 = GetChild<Sprite2D>(2).GetChild<Node2D>(2).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster4 = GetChild<Sprite2D>(2).GetChild<Node2D>(3).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster5 = GetChild<Sprite2D>(2).GetChild<Node2D>(4).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster6 = GetChild<Sprite2D>(2).GetChild<Node2D>(5).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster7 = GetChild<Sprite2D>(2).GetChild<Node2D>(6).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster8 = GetChild<Sprite2D>(2).GetChild<Node2D>(7).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);

		Attack_Orbit = GetChild<Area2D>(3);

		AreaEntered += (Area2D body) => _on_Hit(body);

		explode = GetNode("/root/Sfx").GetChild<AudioStreamPlayer2D>(0);
	}

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
			thruster7.Emitting = true;
			thruster6.Emitting = true;
			thruster8.Emitting = true;
			acceleration += accel;
			speed.Y -= velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementLeft")) {
				speed.Y *= .975f;
				acceleration -= accel;
				thruster8.Emitting = false;

			}
			if (Input.IsActionPressed("MovementRight"))
			{
				speed.Y *= .975f;
				acceleration -= accel;
				thruster6.Emitting = false;
			}
		}
		if (Input.IsActionPressed("MovementDown"))
		{
			thruster2.Emitting = true;
			thruster3.Emitting = true;
			thruster4.Emitting = true;
			acceleration += accel;
			speed.Y += velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementLeft"))
			{
				speed.Y *= .975f;
				acceleration -= accel;
				thruster2.Emitting = false;

			}
			if (Input.IsActionPressed("MovementRight"))
			{
				speed.Y *= .975f;
				acceleration -= accel;
				thruster4.Emitting = false;
			}
		}
		if (Input.IsActionPressed("MovementLeft"))
		{
			thruster5.Emitting = true;
			thruster6.Emitting = true;
			thruster4.Emitting = true;
			acceleration += accel;
			speed.X -= velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementUp"))
			{
				speed.X *= .975f;
				acceleration -= accel;
				thruster4.Emitting = false;

			}
			if (Input.IsActionPressed("MovementDown"))
			{
				speed.X *= .975f;
				acceleration -= accel;
				thruster6.Emitting = false;

			}
		}
		if (Input.IsActionPressed("MovementRight"))
		{
			thruster1.Emitting = true;
			thruster8.Emitting = true;
			thruster2.Emitting = true;
			acceleration += accel;
			speed.X += velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementUp"))
			{
				speed.X *= .975f;
				acceleration -= accel;
				thruster2.Emitting = false;

			}
			if (Input.IsActionPressed("MovementDown"))
			{
				speed.X *= .975f;
				acceleration -= accel;
				thruster8.Emitting = false;

			}
		}

		if (Input.IsActionJustReleased("MovementUp"))
		{
			thruster6.Emitting = false;
			thruster7.Emitting = false;
			thruster8.Emitting = false;
		}
		if (Input.IsActionJustReleased("MovementDown"))
		{
			thruster2.Emitting = false;
			thruster3.Emitting = false;
			thruster4.Emitting = false;
		}
		if (Input.IsActionJustReleased("MovementLeft"))
		{
			thruster4.Emitting = false;
			thruster5.Emitting = false;
			thruster6.Emitting = false;
		}
		if (Input.IsActionJustReleased("MovementRight"))
		{
			thruster8.Emitting = false;
			thruster1.Emitting = false;
			thruster2.Emitting = false;
		}

		// set new position based on current directional speed
		Position += new Vector2(speed.X, speed.Y);


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

	private void hurt()
	{
		health -= 1;

	}

	private void _on_Hit(Area2D body)
	{

		if (body.IsInGroup("enemyprojectile"))
		{
			hurt();
			body.QueueFree();
		}
	}

	private void _on_Death() 
	{
		// spawn explosion particles here
		explode.Play();
		explode.GlobalPosition = GlobalPosition;
		GpuParticles2D deathExplosion = death.Instantiate<GpuParticles2D>();
		GetTree().Root.AddChild(deathExplosion);
		deathExplosion.GlobalPosition = GlobalPosition;
		
		QueueFree();
		// stop the game loop and display the game over screen
	}
}
