﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float maxTurnRate = 1200f;
	public Vector3 maxImpulse = new Vector3(10f, 10f, 700f);
	public Vector3 velocity = new Vector3(0,0,0);
	public float impulseSensitivity = 500f;
	public float turnSensitivity = 5000f; //12000f

	public ParticleSystem leftExhaustFX;
	public ParticleSystem rightExhaustFX;



	private Vector3 impulse = Vector3.zero;
	private float desiredImpulse = 0f;
	private Vector3 impulseActual = Vector3.zero;
	private float maxImpulseChange = 100f;
	private Vector3 turnRate = Vector3.zero;
	private float desiredImpulseInput = 0f;
	private float desiredTurnXInput = 0f;
	private float desiredTurnYInput = 0f;
	private float desiredInputX = 0f;
	private float desiredInputY = 0f;
	private float desiredTurnX = 0f;
	private float desiredTurnY = 0f;


	private Transform thisTransform;
    private float enginePowerValue = 0f;

    public float EnginePowerValue
    {
        get { return enginePowerValue; }
    }
	public GUIText enginePercentageText;
	public GUIText hpGUI;

	public Transform destroyObjectFX;



	public Vector3 Velocity
	{
		get { return velocity;}
	}

	public Vector3 Impulse
	{
		get {return impulse;}
		set 
		{
			impulse.x = Mathf.Clamp (value.x, 0, maxImpulse.x);
			impulse.y = Mathf.Clamp (value.y, 0, maxImpulse.y);
			impulse.z = Mathf.Clamp (value.z, 0, maxImpulse.z);
		}
	}





	// Use this for initialization
	void Start () 
	{
		thisTransform = transform;
	}


	void OnEnginePowerChange()
	{
		if (enginePercentageText != null) 
		{
			enginePowerValue = (desiredImpulse / maxImpulse.z * 100) / 100f; // calculate percentage


			if(enginePowerValue <= 0) 
				enginePercentageText.text = "Engines 0%";
			else if(enginePowerValue >= 1)
				enginePercentageText.text = "Engines 100%";
			else
				enginePercentageText.text = "Engines " + (enginePowerValue * 100).ToString("f0") + "%";
				
		}
	}

	void AdjustEngineFX()
	{
		if(leftExhaustFX != null && rightExhaustFX != null) 
		{
			leftExhaustFX.startSize = 0.2f + enginePowerValue * 1.2f;
			rightExhaustFX.startSize = 0.2f + enginePowerValue * 1.2f;

			leftExhaustFX.startSpeed = 1f + enginePowerValue * 4f;
			rightExhaustFX.startSpeed = 1f + enginePowerValue * 4f;
		}
	}

	void showCurrentHitPoints()
	{
		if (hpGUI != null) 
		{

			GameObject Player = GameObject.Find ("Player");
			Health hp = Player.GetComponent<Health> ();

			hpGUI.text = "Player HP: " + hp.currentHitPoints.ToString ();

		}
	}


	// Update is called once per frame
	void Update () 
	{
		showCurrentHitPoints ();

		OnEnginePowerChange();
		AdjustEngineFX();
		CheckInput();
														//object rotation
		thisTransform.Rotate (turnRate * Time.deltaTime, Space.Self);

		if(Vector3.Distance(impulse, impulseActual) < maxImpulseChange * Time.deltaTime)
		{
			impulseActual = impulse;
		}
		else
		{
			impulseActual += (impulse - impulseActual).normalized * maxImpulseChange * Time.deltaTime;
		}

		velocity = thisTransform.rotation * impulseActual / 20f; // spaceSHEEP speed :D
		thisTransform.Translate (velocity * Time.deltaTime, Space.World);

	}


	void CheckInput()
	{
		if (this == null) return; // this - our script

		desiredImpulseInput = 0f;
		desiredTurnXInput = 0f;
		desiredTurnYInput = 0f;
		desiredTurnX = 0f;
		desiredTurnY = 0f;

		// forward - engine power
		desiredImpulseInput += Input.GetAxis ("Thrust") * impulseSensitivity * Time.deltaTime; //left control speed up spaceship
        //desiredImpulseInput += (Input.GetKeyDown(KeyCode.LeftAlt) ) * impulseSensitivity * Time.deltaTime; //left alt speed down spaceship
		desiredImpulse = (Mathf.Clamp (desiredImpulseInput, GetImpulse2(), GetMaxImpulse2() ));
		desiredImpulse += desiredImpulseInput;

		// up and down
		desiredTurnXInput += Input.GetAxis ("Vertical") * turnSensitivity * Time.deltaTime;
		desiredTurnX = (Mathf.Clamp (desiredTurnXInput, -GetMaxTurnRate(), GetMaxTurnRate() ));
		desiredTurnX += desiredTurnXInput;

		// left and right
		desiredTurnYInput += Input.GetAxis ("Horizontal") * turnSensitivity * Time.deltaTime;
		desiredTurnY = (Mathf.Clamp (desiredTurnYInput, -GetMaxTurnRate(), GetMaxTurnRate() ));
		desiredTurnY += desiredTurnYInput;

		SetImpulse2 (desiredImpulse); // forward
		SetTurnRate (desiredTurnX, desiredTurnY, 0);
	}



	public void SetImpulse2(float z)
	{
		Impulse = new Vector3 (0, 0, z);
	}

	public float GetImpulse2()
	{
		return impulse.z;
	}

	public float GetMaxImpulse2()
	{
		return maxImpulse.z;
	}



	public void SetTurnRate (float x, float y, float z)
	{
		turnRate.x = Mathf.Clamp (x, -maxTurnRate, maxTurnRate);
		turnRate.y = Mathf.Clamp (y, -maxTurnRate, maxTurnRate);
		turnRate.z = Mathf.Clamp (z, -maxTurnRate, maxTurnRate);
	}

	public void SetTurnRate (Vector3 v)
	{
		SetTurnRate (v.x, v.y, v.z);
	}

	public Vector3 GetTurnRate()
	{
		return turnRate;
	}

	public float GetMaxTurnRate()
	{
		return maxTurnRate;
	}


	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Friend" )
		{
			ContactPoint contact = collision.contacts[0]; 	// point of collision
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			Instantiate (destroyObjectFX, pos, rot);
			Destroy(gameObject);


			Application.LoadLevel("MainMenuScene");
		}

		if (collision.gameObject.tag == "Enemy" )
		{
			collision.gameObject.SendMessageUpwards("TakeDamage", 999, SendMessageOptions.DontRequireReceiver);

			gameObject.SendMessageUpwards("TakeDamage", 300, SendMessageOptions.DontRequireReceiver);

		}
		/*
		if (collision.gameObject.tag == "SimpleTower" )
		{
			collision.gameObject.SendMessageUpwards("TakeDamage", 999, SendMessageOptions.DontRequireReceiver);
			
			gameObject.SendMessageUpwards("TakeDamage", 300, SendMessageOptions.DontRequireReceiver);
			
		}
		*/

	}

}




