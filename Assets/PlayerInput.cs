﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	// Variable变量区
	[Header("===== Key settings ====")]
 	public string keyUp = "w";
	public string keyDown = "s";
	public string keyLeft = "a";
	public string keyRight = "d";

	public string keyA;
	public string keyB;
	public string keyC;
	public string keyD;

	[Header("==== Output signal ====")]
	public float Dup;
	public float Dright;
	public float Dmag;
	public Vector3 Dvec;

	//1. pressing signal 按压式
	public bool run;

	//2. trigger once signal 按一次触发
	public bool jump;
	public bool lastJump;
	//3. double trigger

	[Header("==== others ====")]

	public bool inputEnabled = true;  // Flag

	private float targetDup;
	private float targetDright;
	private float velocityDup;   //速度
	private float velocityDright;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		targetDup = (Input.GetKey (keyUp)? 1.0f:0) - (Input.GetKey (keyDown)?1.0f:0);
		targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

		if(inputEnabled == false)
        {
			targetDup = 0;
			targetDright = 0;
        }

		Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
		Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

		Vector2 tempDAxis = SquareToCircle(new Vector2(Dup, Dright));
		float Dright2 = tempDAxis.x;
		float Dup2 = tempDAxis.y;

		Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
		Dvec = Dright2 * transform.right + Dup2 * transform.forward;

		run = Input.GetKey(keyA);

		bool newJump = Input.GetKey(keyB);
		jump = newJump;
        //jump = tempjump
        if (newJump != lastJump && newJump == true)
        {
			jump = true;
			print("jump trigger!!!!");

        }
        else
        {
			jump = false;
        }
		lastJump = newJump;
	}

	//椭圆映射法解决斜向走路时动画为奔跑的问题
	private Vector2 SquareToCircle(Vector2 input)
    {
		Vector2 output = Vector2.zero;

		output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
		output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);


		return output;
    }
}
