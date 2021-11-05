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

    public void WonGame(int score, int maxScore)
    {
        screens[1].SetActive(false);
        screens[2].SetActive(true);
        finalScoreText.text = "You got " + score + "/" + maxScore + " correct";
        EventSystem.current.SetSelectedGameObject(retryButton);
    }
}
