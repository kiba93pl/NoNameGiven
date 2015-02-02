﻿using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Screen.showCursor = true; 
	}
	


	void OnMouseDown()
	{
		if(gameObject.tag == "Player")
			Application.LoadLevel ("HighScores");

		//else if(gameObject.tag == "Start")
		//	Application.LoadLevel ("Story");

		else if(gameObject.tag == "SimpleTower")
			Application.LoadLevel ("License");

		else if(gameObject.name == "Boss")
			Application.LoadLevel ("Authors");

		else if(gameObject.name == "Enemy1")
			Application.Quit();

		else if(gameObject.name == "Small explosion")
			Application.LoadLevel("Story");

		else 
			Application.LoadLevel ("GameOver");
	}
}
