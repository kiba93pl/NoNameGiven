﻿using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour {

	public GameObject bulletPrefab;
	public float fireDelay = 2f;
	float cooldownTimer = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		cooldownTimer -= Time.deltaTime;
		if (cooldownTimer <= 0) {
			cooldownTimer = fireDelay;
			Vector3 offset = transform.rotation * new Vector3(0.25f,0.7f,0);
			GameObject bullet = (GameObject) Instantiate(bulletPrefab, transform.position + offset, transform.rotation);
			bullet.layer = gameObject.layer;
		}
	}
}
