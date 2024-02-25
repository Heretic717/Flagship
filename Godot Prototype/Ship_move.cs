using Godot;
using System;

public partial class Ship_move : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	float dp = 0f; //velocity
	float ddp = 0f; //real acceleration
	float accel = 50f; // base value for acceleration
	float AMOUNTX = 0f; // speed on the x axis
	float AMOUNTY = 0f; // speed on the y axis



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		//put a max and min on DDP to prevent extreme speed or rubberbanding on deceleration
		if (ddp > accel * 10)
			ddp = accel * 10;
		else if (ddp < 0)
			ddp = 0;

		//set velocity	
		dp += ddp * (float)delta;

		if (Input.IsPhysicalKeyPressed(Key.W) || Input.IsPhysicalKeyPressed(Key.S) || Input.IsPhysicalKeyPressed(Key.A) || Input.IsPhysicalKeyPressed(Key.D))
		{
			ddp += accel; // accelerate if any movement key is pressed
		}
		
		// calculate directional speed based on which key was pressed
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


		// implement as nested if in previous section when I have the time.
		if ((Input.IsPhysicalKeyPressed(Key.W) && Input.IsPhysicalKeyPressed(Key.A)) || (Input.IsPhysicalKeyPressed(Key.W) && Input.IsPhysicalKeyPressed(Key.D)) || (Input.IsPhysicalKeyPressed(Key.S) && Input.IsPhysicalKeyPressed(Key.A)) || (Input.IsPhysicalKeyPressed(Key.S) && Input.IsPhysicalKeyPressed(Key.D))) 
		{
			AMOUNTX *= .975f;
			AMOUNTY *= .975f;
		}

		// set new position based on current directional speed
		this.Position += new Vector2(AMOUNTX, AMOUNTY);


		// friction for smooth accel/decel
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
