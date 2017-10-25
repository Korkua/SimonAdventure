using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ExitMainMenu : MonoBehaviour{

	public Button exitButton;
	public AudioSource exitAudio;
	public void Start(){
		Button btn = exitButton.GetComponent<Button>();
		btn.onClick.AddListener(exitMenu);
	}
	public void exitMenu(){
		PlayExitMenuSound();
		Application.Quit();
	}
	public void PlayExitMenuSound(){
        exitAudio.Play();
	}
}
