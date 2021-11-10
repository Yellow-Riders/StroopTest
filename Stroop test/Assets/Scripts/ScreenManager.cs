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
    public GameObject leftButton;
    public GameObject retryButton;
    public Text finalScoreText;
    public Text mistakesTest;
    public GameObject graphButton;

    private void Start()
    {
        LoadScreen("Menu");
    }

    public void Play()
    {
        LoadScreen("Game");
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
    }
    
    public void PlayTutorial()
    {
        LoadScreen("Tutorial");
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
    }
    
    public void PlayHardMode()
    {
        LoadScreen("Hardmode");
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
    }
    
    public void GraphDisplay()
    {
        LoadScreen("Graph");
        EventSystem.current.SetSelectedGameObject(FindObjectOfType<Button>().gameObject);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GetComponent<AudioSource>().PlayOneShot(screenChangeSound);
    }

    public void WonGame(string difficulty, int score, int maxScore, string time)
    {
        LoadScreen("WinScreen");
        finalScoreText.text = "Tijd: " + time;
        
        if(score < maxScore)
            mistakesTest.text = "Je hebt "+ score +" van de "+ maxScore+" goed";
        else
            mistakesTest.text = "Alle vragen zijn goed beantwoord"; //And you got all of them right!
        
        EventSystem.current.SetSelectedGameObject(retryButton);
        graphButton.SetActive(true);
        Analytics.CustomEvent("GameFinished", new Dictionary<string, object>
        {
            { "Difficulty", difficulty },
            { "Time Taken", time },
            { "Mistakes made", maxScore-score }
        });
    }
    
    public void TutorialWon(int score, int maxScore)
    {
        graphButton.SetActive(false);
        LoadScreen("WinScreen");
        finalScoreText.text = "Goed gedaan, je hebt de uitleg afgerond";
        mistakesTest.text = "Klik op terug om het spel te starten"; 
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
