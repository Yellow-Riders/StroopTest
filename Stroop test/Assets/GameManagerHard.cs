using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManagerHard : MonoBehaviour
{
    [Header("AudioClips")] 
    public AudioClip wrongSound;
    public AudioClip rightSound;
    
    [Header("Animators")]
    public Animator bgAnim;
    public GameObject wrongIcon;
    public GameObject rightIcon;
    
    [Header("Texts")]
    public Text colorPrompt;
    public Text QuestNumbText;
    public Text timerText;
    public Text leftColor;
    public Text rightColor;

    private static int MAXSCORE = 20;
    private int score;
    private int questionNumber;
    private List<ColorList> colorList;
    private List<ColorList> tempColorList;
    private ColorList correctColor;
    private int randomNumb;
    private float time;
    private bool gameWon;
    private ScreenManager SM;

    private void Start()
    {
        SM = FindObjectOfType<ScreenManager>();
        colorList = new List<ColorList>()
            {
                new ColorList(new Color32(191, 76, 76,255), "Rood"), new ColorList(new Color32(26,128,254,255), "Blauw"), new ColorList(new Color32(84, 191, 76,255), "Groen"),
                new ColorList(Color.yellow, "Geel"), new ColorList(Color.black, "Zwart"),
                new ColorList(Color.magenta, "Roze"), new ColorList(Color.cyan, "Cyan")
            };
        GenerateNewPrompt();
    }

    private void Update()
    {
        Timer();
    }

    public void CorrectAnswerCheck(Text colorText)
    {
        Transform button = colorText.transform.parent;
        if (colorText.text == correctColor.GetName())
        {
            score++;
            bgAnim.SetBool("isGreen", true);
            SM.GetComponent<AudioSource>().PlayOneShot(rightSound);
            rightIcon.transform.position = button.position;
            rightIcon.SetActive(true);
            
        }
        else
        {
            bgAnim.SetBool("isRed", true);
            SM.GetComponent<AudioSource>().PlayOneShot(wrongSound);
            wrongIcon.transform.position = button.position;
            wrongIcon.SetActive(true);
        }
        Invoke("ResetAnim", 0.2f);    
        GenerateNewPrompt();
    }

    void ResetAnim()
    {
        bgAnim.SetBool("isGreen", false);
        bgAnim.SetBool("isRed", false);
        rightIcon.SetActive(false);
        wrongIcon.SetActive(false);
    }

    private void GenerateNewPrompt()
    {
        tempColorList = new List<ColorList>();
        tempColorList.AddRange(colorList);
        correctColor = ReturnRandomColor();

        if (questionNumber < 5)
            colorPrompt.text = correctColor.GetName();
        else
            colorPrompt.text = ReturnRandomColor().GetName();
        
        colorPrompt.color = correctColor.GetColor();
        UpdateOptions();
        QuestionNumberUpdate();
    }

    void UpdateOptions()
    {
        int sideNumber = Random.Range(0, 2);
        ColorList randomColor = ReturnRandomColor();

        switch (sideNumber)
        {
            case 0: leftColor.text = correctColor.GetName();
                rightColor.text = randomColor.GetName();
                break;
            case 1: rightColor.text = correctColor.GetName();
                leftColor.text = randomColor.GetName();
                break;
        }
    }

    void QuestionNumberUpdate()
    {
        questionNumber++;
        QuestNumbText.text = "Vraag " + questionNumber+"/"+ MAXSCORE;
        if(questionNumber > MAXSCORE)
            GameWon();
    }

    private ColorList ReturnRandomColor()
    {
        randomNumb = Random.Range(0, tempColorList.Count);
        ColorList col = tempColorList[randomNumb];
        tempColorList.RemoveAt(randomNumb);
        return col;
    }

    private void Timer()
    {
        if(!gameWon)
            time += Time.deltaTime;
        else
            GameWon();

        int minute = Mathf.FloorToInt(time / 60);
        int second = Mathf.FloorToInt(time % 60);
        if(second < 10)
            timerText.text = minute + ":0" + second;
        else
            timerText.text = minute + ":" + second;
    }

    private void GameWon()
    {
        SM.WonGame(score, MAXSCORE, timerText.text);
    }
}
