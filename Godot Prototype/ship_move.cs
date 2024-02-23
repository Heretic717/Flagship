using Godot;
using System;

public partial class ship_move : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	float dp = 0f;
	float ddp = 0f;
	float accel = 50f;
	float AMOUNTX = 0f;
	float AMOUNTY = 0f;



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (ddp > accel * 10)
			ddp = accel * 10;
		else if (ddp < 0)
			ddp = 0;
		dp += ddp * (float)delta;

		if (Input.IsPhysicalKeyPressed(Key.W) || Input.IsPhysicalKeyPressed(Key.S) || Input.IsPhysicalKeyPressed(Key.A) || Input.IsPhysicalKeyPressed(Key.D))
		{
			ddp += accel;
		}
		

		if (Input.IsPhysicalKeyPressed(Key.W))
		{
			AMOUNTY -= dp * (float)delta + ddp * (float)delta * (float)delta * .5f;
		}
		if (Input.IsPhysicalKeyPressed(Key.S))
		{
			AMOUNTY += dp * (float)delta + ddp * (float)delta * (float)delta * .5f;
		}
		if (Input.IsPhysicalKeyPressed(Key.A))
		{
			AMOUNTX -= dp * (float)delta + ddp * (float)delta * (float)delta * .5f;
		}
		if (Input.IsPhysicalKeyPressed(Key.D))
		{
			AMOUNTX += dp * (float)delta + ddp * (float)delta * (float)delta * .5f;
		}


		this.Position += new Vector2(AMOUNTX, AMOUNTY);

		if (AMOUNTY != 0)
		{
			AMOUNTY *= .95f;
		} 
		if (AMOUNTX != 0)
		{
			AMOUNTX *= .95f;
		}

		ddp -= dp * 5f;
	}
}
