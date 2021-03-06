﻿using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

    public EnemyControl enemyControl;

	private float coolDown = 0f; // 
	public float fireRate = 0f; // time between shooting

	// checks to see if we're actually firing
	private bool isFiring = false;

	// firing point transforms for launching projectiles
	public Transform leftFirePoint;
	public Transform rightFirePoint;

	// our projectile object
	public GameObject laserPrefab;

	public AudioSource fireFXSound;

   
    public Transform PlayerTransform;

	private bool stopPlayer = false;


	// Use this for initialization
	void Start () 
	{
		isFiring = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (stopPlayer)
			return;


		if(this.gameObject.tag == "Player")
            CheckInput();
        if (this.gameObject.tag == "Enemy")
        {
            isFiring = enemyControl.Shooting;
            
        }
            

		coolDown -= Time.deltaTime;

		if (isFiring)
		{
			// player has initiated shooting laser
			Fire();
		}



	}


	void CheckInput()
	{
		if (Input.GetKeyDown ("space") || Input.GetKeyDown(KeyCode.Mouse0)) 
		{
			isFiring = true;
		}
		else 
			isFiring = false;
	}


	void Fire()
	{
		if(coolDown > 0)
		{
			return; // do not fire
		}

		// sound FX when firing
		if(fireFXSound != null)
		{
			fireFXSound.Play();
		}
		GameObject.Instantiate (laserPrefab, leftFirePoint.position, leftFirePoint.rotation);
		GameObject.Instantiate (laserPrefab, rightFirePoint.position, rightFirePoint.rotation);

		coolDown = fireRate;

	}


	void stopEngines()
	{
		//enginePowerValue = 0f;
		stopPlayer = true;
	}
	
	void startEngines()
	{
		stopPlayer = false;
	}



}
