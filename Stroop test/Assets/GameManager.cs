using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    [Header("Button Images")] 
    public Image leftButtonImage;
    public Image rightButtonImage;
    
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

    private float initialTime;
    private List<BarInfo> barsInfo;

    private void Start()
    {
        SM = FindObjectOfType<ScreenManager>();
        barsInfo = new List<BarInfo>();
        initialTime = time;
        colorList = new List<ColorList>()
            {
                new ColorList(new Color32(191, 76, 76,255), "Red"), new ColorList(new Color32(26,128,254,255), "Red"), new ColorList(new Color32(84, 191, 76,255), "Green"),
                new ColorList(Color.yellow, "Yellow"), new ColorList(Color.black, "Black"),
                new ColorList(Color.magenta, "Pink"), new ColorList(Color.cyan, "Cyan")
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
            QuestionTimeAnalytics(true);
        }
        else
        {
            bgAnim.SetBool("isRed", true);
            SM.GetComponent<AudioSource>().PlayOneShot(wrongSound);
            wrongIcon.transform.position = button.position;
            wrongIcon.SetActive(true);
            QuestionTimeAnalytics(false);
        }
        Invoke("ResetAnim", 0.2f);
        GenerateNewPrompt();
    }

    float TimeTaken()
    {
        float timeTaken = time - initialTime;
        float seconds = Mathf.Round( timeTaken * 100)/100;
        initialTime = time;
        return seconds;
    }

    void QuestionTimeAnalytics(bool isCorrect)
    {
        float timeTaken = TimeTaken();
        barsInfo.Add(new BarInfo(timeTaken,isCorrect));
        Analytics.CustomEvent("QuestionTimes", new Dictionary<string, object>
        {
            { "Difficulty", "normal" },
            { "Question Number", questionNumber },
            { "Time Taken", timeTaken },
            { "Correct Answer", isCorrect }
        });
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
                leftButtonImage.color = correctColor.GetColor();
                rightColor.text = randomColor.GetName();
                rightButtonImage.color = randomColor.GetColor();
                break;
            case 1: rightColor.text = correctColor.GetName();
                rightButtonImage.color = correctColor.GetColor();
                leftColor.text = randomColor.GetName();
                leftButtonImage.color = randomColor.GetColor();
                break;
        }
    }

    void QuestionNumberUpdate()
    {
        questionNumber++;
        QuestNumbText.text = "Question " + questionNumber+"/20";
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
        SM.WonGame("normal", score, MAXSCORE, timerText.text,barsInfo);
    }
}

public class ColorList
{
    private Color color;
    private string name;

    public ColorList(Color color, string name)
    {
        this.color = color;
        this.name = name;
    }

    public Color GetColor()
    {
        return color;
    }

    public string GetName()
    {
        return name;
    }
}
