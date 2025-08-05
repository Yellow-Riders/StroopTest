using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Serialization;

public class ScreenManager : MonoBehaviour
{
    [Header("AudioClips")] 
    public AudioClip screenChangeSound;
    
    [Header("Screen Change Effects")]
    public ScreenInfo[] screens;
    public GameObject retryButton;
    public TMP_Text finalScoreText;
    public TMP_Text mistakesText;
    public GameObject graphButton;
    public GraphInfo graph;

    // [Header("Save")] 
    // [SerializeField] private DataStore DS;

    private LanguageManager _languageManager;

    private void Awake()
    {
        _languageManager = FindFirstObjectByType<LanguageManager>();
    }

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
        EventSystem.current.SetSelectedGameObject(FindFirstObjectByType<Button>().gameObject);
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
        finalScoreText.text = _languageManager.currentLanguageWords[20] + time;

        string[] scoreDisplay = _languageManager.currentLanguageWords[22].Split('0');
        
        if(score < maxScore)
            mistakesText.text = scoreDisplay[0] + score + scoreDisplay[1] + maxScore + scoreDisplay[2]; //23 needs split by 0
        else
            mistakesText.text = _languageManager.currentLanguageWords[21]; //22
        
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
        finalScoreText.text = _languageManager.currentLanguageWords[17]; //18
        mistakesText.text = _languageManager.currentLanguageWords[18]; //9
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
