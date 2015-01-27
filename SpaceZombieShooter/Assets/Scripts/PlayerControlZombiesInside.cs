﻿using UnityEngine;
using System.Collections;

public class PlayerControlZombiesInside : MonoBehaviour {


	public GUIText ZombiesInPlayerGUI;
	private int zombiesNr;
	public GUIText ZombieWarningGUI;
	public int zombiesToEndWarning = 3;
	public int zombiesToEnd = 6;

	// Use this for initialization
	void Start () 
	{
		zombiesNr = 0;
		ZombiesInPlayerGUI.text = "Zombies: " + zombiesNr + "/" + zombiesToEnd;
	}


	void addZombiesInPlayer()
	{
		if (ZombiesInPlayerGUI != null || ZombieWarningGUI != null) 
		{
			zombiesNr++;
			ZombiesInPlayerGUI.text = "Zombies: " + zombiesNr + "/" + zombiesToEnd;
			
			if(zombiesNr >= zombiesToEndWarning)
			{
				ZombieWarningGUI.text = "Uwaga, masz trupy na statku! Naciśnij G żeby obronić statek!";
			}
			else ZombieWarningGUI.text = " ";
			
			
			if(zombiesNr >= zombiesToEnd)
			{
				Application.LoadLevel("GameOver");
			}
			
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if(!gameObject)
			return;




		if(ZombieWarningGUI.text == "Uwaga, masz trupy na statku! Naciśnij G żeby obronić statek!" )
		{
			//ZombieWarningGUI.text = "Uwaga, masz trupy na statku! Naciśnij G żeby obronić bazę!";
				
				if (Input.GetKeyDown ("g")) 
				{
					Application.LoadLevel("2DShooter");
				}
				
		}
		else ZombieWarningGUI.text = " ";

		if(zombiesNr >= zombiesToEndWarning)
			ZombieWarningGUI.text = "Uwaga, masz trupy na statku! Naciśnij G żeby obronić statek!";
		else ZombieWarningGUI.text = " ";


	}
}