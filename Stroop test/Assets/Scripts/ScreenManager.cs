using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
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
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WonGame(int mistakes, string time)
    {
        screens[1].SetActive(false);
        screens[2].SetActive(true);
        finalScoreText.text = "Time taken " + time;
        
        if(mistakes > 1)
            mistakesTest.text = "But you made " + mistakes + " mistakes!";
        else if(mistakes == 1)
            mistakesTest.text = "But you made " + mistakes + " mistake";
        else
            mistakesTest.text = "And you got all of them right!";
        
        EventSystem.current.SetSelectedGameObject(retryButton);
    }
}
