using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour
{
    [Header("AudioClips")] 
    public AudioClip screenChangeSound;
    
    [Header("Screen Change Effects")]
    public ScreenInfo[] screens;
    public GameObject retryButton;
    public Text finalScoreText;
    public Text mistakesTest;
    public GameObject graphButton;
    public GraphInfo graph;

    [Header("Save")] 
    [SerializeField] private DataStore DS;

    private void Start()
    {
        // Remote.allowExitToHome = false;
        LoadScreen("Menu");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Restart();
    }

    public void ChangeScreen(string screen)
    {
        LoadScreen(screen);
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GetComponent<AudioSource>().PlayOneShot(screenChangeSound);
    }
    
    public void WonGame(string difficulty, int score, int maxScore, string time, List<BarInfo> BI)
    {
        graph.UpdateBars(BI,maxScore-score, difficulty);
        LoadScreen("WinScreen");
        finalScoreText.text = "Temps: " + time;
        
        if(score < maxScore)
            mistakesTest.text = "Vous avez " + score + " sur " + maxScore + " bien";
        else
            mistakesTest.text = "Toutes les réponses étaient bonnes!"; //And you got all of them right!
        
        EventSystem.current.SetSelectedGameObject(retryButton);
        graphButton.SetActive(true);
        Analytics.CustomEvent("GameFinished", new Dictionary<string, object>
        {
            { "Difficulty", difficulty },
            { "Time Taken", time },
            { "Mistakes made", maxScore-score }
        });
        SaveGame(difficulty, time,maxScore-score);
    }

    void SaveGame(string difficulty, string time, int mistakes)
    {
        DataStore.data_Difficulty = difficulty;
        DataStore.data_Time = time;
        DataStore.data_Mistakes = mistakes;
        DataStore.SaveData();
    }
    
    public void TutorialWon()
    {
        graphButton.SetActive(false);
        LoadScreen("WinScreen");
        finalScoreText.text = "Bravo, vous avez terminé le tutoriel";
        mistakesTest.text = "Cliquez sur retour pour démarrer le jeu"; 
        EventSystem.current.SetSelectedGameObject(retryButton);
    }

    void LoadScreen(string screenName)
    {
        foreach (var screen in screens)
        {
            if (screen.screenName == screenName)
                screen.gameObject.SetActive(true);
            else
                screen.gameObject.SetActive(false);
        }
        GetComponent<AudioSource>().PlayOneShot(screenChangeSound);
    }
}
