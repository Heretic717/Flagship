using Godot;
using System;

public partial class turret : Node2D
{
	PackedScene projectile = (PackedScene)GD.Load("res://Scenes/projectile.tscn");
	private float startRot = 0f;
	Timer timer;
	private float rOF = .25f;
	private bool canFire = false;

	private bool barrel1 = true;
	private bool barrel2 = false;
	Vector2 CursorPos;
	Vector2 miss;
	float minAcc = .95f;
	float maxAcc = 1.05f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startRot = this.Rotation;
		timer = this.GetChild<Timer>(0, false);
		timer.WaitTime = rOF;
		timer.Start();
		timer.Timeout += () => canFire = true;
	}



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CursorPos = GetGlobalMousePosition();
		GetChild<Sprite2D>(1).LookAt(CursorPos);
		GetChild<Sprite2D>(1).GetChild<Node2D>(0).LookAt(CursorPos);

		miss = new((float)GD.RandRange(minAcc, maxAcc), (float)GD.RandRange(minAcc, maxAcc));

		if (Input.IsMouseButtonPressed(MouseButton.Left) && canFire) {
			Fire(miss);
		}
	}
	private void Fire(Vector2 miss)
	{
		projectile_logic proj = projectile.Instantiate<projectile_logic>();
		GetTree().Root.AddChild(proj);
		if (barrel1)
		{
			proj.GlobalPosition = GetChild<Sprite2D>(1).GetChild<Node2D>(0).GlobalPosition;
			timer.Stop();
			timer.WaitTime = rOF;
			timer.Start();
		}
		else if (barrel2)
		{
			proj.GlobalPosition = GetChild<Sprite2D>(1).GetChild<Node2D>(1).GlobalPosition;
			timer.Stop();
			timer.WaitTime = rOF;
			timer.Start();
		}
		barrel1 = !barrel1;
		barrel2 = !barrel2;
		canFire = false;

		proj.LookAt(CursorPos * miss);
		proj.velocity = (CursorPos * miss - GlobalPosition).Normalized();
	}
}


