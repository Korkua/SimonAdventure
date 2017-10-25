using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayMainMenu : MonoBehaviour{

	public Button playButton;
	public AudioSource playAudio;
	public void Start(){
		Button btn = playButton.GetComponent<Button>();
		btn.onClick.AddListener(playMenu);
	}
	public void playMenu(){
		PlayPlayMenuSound();
		SceneManager.LoadScene("Game");
	}
	public void PlayPlayMenuSound(){
        playAudio.Play();
	}
}