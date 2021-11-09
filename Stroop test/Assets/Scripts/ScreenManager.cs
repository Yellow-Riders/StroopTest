using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    [Header("AudioClips")] 
    public AudioClip screenChangeSound;
    
    [Header("Screen Change Effects")]
    public GameObject[] screens;
    public GameObject leftButton;
    public GameObject retryButton;
    public Text finalScoreText;
    public Text mistakesTest;

    public void Play()
    {
        screens[0].SetActive(false);
        screens[1].SetActive(true);
        EventSystem.current.SetSelectedGameObject(leftButton);
        GetComponent<AudioSource>().PlayOneShot(screenChangeSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GetComponent<AudioSource>().PlayOneShot(screenChangeSound);
    }

    public void WonGame(int mistakes, string time)
    {
        GetComponent<AudioSource>().PlayOneShot(screenChangeSound);
        screens[1].SetActive(false);
        screens[2].SetActive(true);
        finalScoreText.text = "Tijd " + time;
        
        if(mistakes > 1)
            mistakesTest.text = "Je maakte " + mistakes + " fouten!";
        else if(mistakes == 1)
            mistakesTest.text = "Je hebt een fout"; //But you made " + mistakes + " mistake
        else
            mistakesTest.text = "Alle vragen zijn goed beantwoord"; //And you got all of them right!
        
        EventSystem.current.SetSelectedGameObject(retryButton);
    }
}
