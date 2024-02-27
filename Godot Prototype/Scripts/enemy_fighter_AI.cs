using Godot;
using System;

public partial class enemy_fighter_AI : RigidBody2D
{

	public float health = 20f;
	float speed = 3f;
	Vector2 velocity = Vector2.Zero;
	public RigidBody2D player;
	public Vector2 targetSpeed = new(0, 0);
	float randomnum;
	float radiusMove = 100f;
	float radiusFire = 20f;
	private Timer timer;
	private bool canFire = false;

	PackedScene projectile = (PackedScene)GD.Load("res://Scenes/projectile.tscn");

	GpuParticles2D thrusterMain;
	GpuParticles2D thruster1;
	GpuParticles2D thruster2;
	GpuParticles2D thruster3;
	GpuParticles2D thruster4;

	int collidingBodies;

	public enum State 
	{
		APPROACH,
		ORBIT,
		STATEMAX,
	}

	public State state = State.APPROACH;

	private void move(Vector2 target, double delta, Vector2 targetSpeed) 
	{
		Vector2 direction = (target - GlobalPosition).Normalized();
		Vector2 desired_velocity = direction * speed;
		Vector2 steering = (desired_velocity - velocity) * (float)delta * 2.5f;
		velocity += steering;
		if (state == State.ORBIT)
			MoveAndCollide(velocity + targetSpeed.Rotated(player.GlobalRotation) * .1f);
		else
			MoveAndCollide(velocity);
	}
	private Vector2 get_circle_position(float random, float radius) {
		Vector2 kill_cirlce_center = player.GlobalPosition;
		float angle = random * (float)Math.PI * 2f;
		float x = kill_cirlce_center.X + (float)Math.Cos(angle) * radius;
		float y = kill_cirlce_center.Y + (float)Math.Sin(angle) * radius;

		return new Vector2(x, y);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ContactMonitor = true;

		player = GetTree().Root.GetChild<Node2D>(0).GetChild<Node2D>(1).GetChild<RigidBody2D>(0);

		thrusterMain = GetChild<Sprite2D>(3).GetChild<Node2D>(0).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster1 = GetChild<Sprite2D>(3).GetChild<Node2D>(1).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster2 = GetChild<Sprite2D>(3).GetChild<Node2D>(2).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster3 = GetChild<Sprite2D>(3).GetChild<Node2D>(3).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
		thruster4 = GetChild<Sprite2D>(3).GetChild<Node2D>(4).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);

		RandomNumberGenerator rng = new();
		rng.Randomize();
		randomnum = rng.Randf();

		timer = GetChild<Timer>(1);
		timer.WaitTime = 2.5f;
		timer.Timeout += () => canFire = true;
		timer.Start();
		MaxContactsReported = 100;
		BodyEntered += (RigidBody2D) => _on_Hit((RigidBody2D)GetCollidingBodies()[collidingBodies]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		collidingBodies = GetCollidingBodies().Count;
		if (health <= 0) {
			QueueFree();
		}
		LookAt(player.GlobalPosition);
		switch (state){
			case State.APPROACH: 
				{

					move(get_circle_position(randomnum, radiusMove), delta, targetSpeed);
					thrusterMain.Emitting = true;
					thruster2.Emitting = true;
					thruster3.Emitting = true;
					thruster4.Emitting = false;
					thruster1.Emitting = false;

				} break;
			case State.ORBIT:
				{

					move(get_circle_position(randomnum, radiusMove + 20f), delta, targetSpeed);
					thrusterMain.Emitting = false;
					thruster2.Emitting = false;
					thruster3.Emitting = false;
					thruster4.Emitting = true;
					thruster1.Emitting = true;
					if (canFire)
					{
						AIFire(get_circle_position(GD.Randf(), radiusFire));
						canFire = false;
						timer.WaitTime = 2.5f;
						timer.Stop();
						timer.Start();
						randomnum = GD.Randf();
					} else 
					{
						state = State.ORBIT;
					}

				} break;
				default : 
				{
					state = State.APPROACH;
				} break;
		}
	}

	private void hurt() {
		health -= 1;
		GD.Print(health);
	}

	private void _on_Hit(RigidBody2D body) 
	{
		GD.Print("hit");
		if (body.IsInGroup("projectile"))
		{
			hurt();
			body.QueueFree();
		}
	}

	private void AIFire(Vector2 miss)
	{
		projectile_logic proj = projectile.Instantiate<projectile_logic>();
		GetTree().Root.AddChild(proj);
			proj.GlobalPosition = GetChild<Node2D>(2).GlobalPosition;
			timer.Stop();
			timer.WaitTime = 0.4;
			timer.Start();

		canFire = false;

		proj.LookAt(miss);
		proj.velocity = (miss - GlobalPosition).Normalized() + velocity * new Vector2(.1f, .1f);
	}
}
