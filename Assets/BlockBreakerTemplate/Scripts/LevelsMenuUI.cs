using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsMenuUI : MonoBehaviour
{

    public void PlayButton (int sceneNumber)
	{
		Application.LoadLevel(sceneNumber);	//Loads the 'Game' scene to begin the game
	}

	//Called when the quit button is pressed
	public void BackButton ()
	{
		Application.LoadLevel(0);
	}
}
