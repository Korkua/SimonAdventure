using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject gameButtonPrefab;
    public List<ButtonSetting> buttonSettings;
    public Transform gameFieldPanelTransform;
    public AudioSource bleepAudio;
    public AudioSource gameOverAudio;
    List<GameObject> gameButtons;
    static int minBleepCount = 3;
    int bleepCount = minBleepCount;
    List<int> bleeps;
    List<int> playerBleeps;
    System.Random rg;
    bool inputEnabled = false;
    bool gameOver = false;
    double temp;
    void Start()
    {
        gameButtons = new List<GameObject>();
        CreateGameButton(0, new Vector3(-250, 250));
        CreateGameButton(1, new Vector3(250, 250));
        CreateGameButton(2, new Vector3(250, -250));
        CreateGameButton(3, new Vector3(-250, -250));

        bleeps = new List<int>();

        StartCoroutine(SimonSays());
    }
    void update() {
    }
    IEnumerator SimonSays()
    {
        inputEnabled = false;
        rg = new System.Random((new System.Random().Next()).GetHashCode());
        SetBleeps();
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < bleeps.Count; i++)
        {
            Bleep(bleeps[i]);
            PlayBleepSound();
            yield return new WaitForSeconds(0.6f);
        }
        inputEnabled = true;
        yield return null;
    }
    void SetBleeps()
    {
        playerBleeps = new List<int>();
        if(minBleepCount == bleepCount){
            for (int i = 0; i < bleepCount; i++)
            {
                bleeps.Add(rg.Next(0, gameButtons.Count));
            }
        }
        else{
            bleeps.Add(rg.Next(0, gameButtons.Count));
        }

        bleepCount++;
    }
    void Bleep(int index)
    {
        LeanTween.value(gameButtons[index], buttonSettings[index].normalColor, buttonSettings[index].highlightColor, 0.25f).setOnUpdate((Color color) => {
            gameButtons[index].GetComponent<Image>().color = color;
        });

        LeanTween.value(gameButtons[index], buttonSettings[index].highlightColor, buttonSettings[index].normalColor, 0.25f)
            .setDelay(0.5f)
            .setOnUpdate((Color color) => {
                gameButtons[index].GetComponent<Image>().color = color;
        });

    }
    void WrongBleep(int index)
    {
        LeanTween.value(gameButtons[index], buttonSettings[index].normalColor, buttonSettings[index].failColor, 0.25f).setOnUpdate((Color color) => {
            gameButtons[index].GetComponent<Image>().color = color;
        });
        LeanTween.value(gameButtons[index], buttonSettings[index].failColor, buttonSettings[index].normalColor, 0.25f)
            .setDelay(0.5f)
            .setOnUpdate((Color color) => {
                gameButtons[index].GetComponent<Image>().color = color;
        });
        LeanTween.value(gameButtons[index], buttonSettings[index].normalColor, buttonSettings[index].failColor, 0.25f).setDelay(1f).setOnUpdate((Color color) => {
            gameButtons[index].GetComponent<Image>().color = color;
        });
        LeanTween.value(gameButtons[index], buttonSettings[index].failColor, buttonSettings[index].normalColor, 0.25f)
            .setDelay(0.5f)
            .setOnUpdate((Color color) => {
                gameButtons[index].GetComponent<Image>().color = color;
        });
    }
    private void CreateGameButton(int index, Vector3 pos)
    {
        GameObject gameButton = Instantiate(gameButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        gameButton.transform.SetParent(gameFieldPanelTransform, false);
        gameButton.transform.localPosition = pos;

        gameButton.GetComponent<Image>().color = buttonSettings[index].normalColor;
        gameButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnGameButtonClick(index);
        });
        gameButtons.Add(gameButton);

    }
    void OnGameButtonClick(int index)
    {
        if (!inputEnabled)
        {
            return;
        }
        playerBleeps.Add(index);
        if (bleeps[playerBleeps.Count - 1] != index)
        {
            WrongBleep(index);
            PlayGameOverSound();
            GameOver(bleeps[playerBleeps.Count - 1]);
            return;
        }
        Bleep(index);
        PlayBleepSound();
        if (bleeps.Count == playerBleeps.Count)
        {
            StartCoroutine(SimonSays());
        }
    }
    void GameOver(int index)
    {
        StartCoroutine(GameOverAnimation(index));
    }
    IEnumerator GameOverAnimation(int index){
        Bleep(index);
        yield return new WaitForSeconds(1f);
        Bleep(index);
        yield return new WaitForSeconds(1f);
        Bleep(index);

        gameOver = true;
        inputEnabled = false;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }
    void PlayBleepSound()
    {
        bleepAudio.Play();
    }
    void PlayGameOverSound(){
        gameOverAudio.Play();
    }
}
