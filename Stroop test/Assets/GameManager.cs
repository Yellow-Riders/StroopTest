using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Text colorPrompt;
    public Text leftColor;
    public Text rightColor;
    public Text QuestNumbText;
    public Text scoreText;
    public Text timerText;

    private static int MAXSCORE = 20;
    private int score;
    private int questionNumber;
    private List<ColorList> colorList;
    private List<ColorList> tempColorList;
    private ColorList correctColor;
    private int randomNumb;
    private float timeLeft;
    private bool gameWon;
    private ScreenManager SM;

    private void Start()
    {
        SM = FindObjectOfType<ScreenManager>();
        timeLeft = 60;
        colorList = new List<ColorList>()
            {
                new ColorList(Color.red, "Red"), new ColorList(Color.blue, "Blue"), new ColorList(Color.green, "Green"),
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
        if (colorText.text == correctColor.GetName())
            score++;
        scoreText.text = score.ToString();
        GenerateNewPrompt();
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
        QustionNumebrUpdate();
    }

    void UpdateOptions()
    {
        int sideNumber = Random.Range(0, 2);

        switch (sideNumber)
        {
            case 0: leftColor.text = correctColor.GetName();
                rightColor.text = ReturnRandomColor().GetName();
                break;
            case 1: rightColor.text = correctColor.GetName();
                leftColor.text = ReturnRandomColor().GetName();
                break;
        }
        leftColor.color = ReturnRandomColor().GetColor();
        rightColor.color = ReturnRandomColor().GetColor();
    }

    void QustionNumebrUpdate()
    {
        questionNumber++;
        QuestNumbText.text = "Q" + questionNumber;
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
        if(!gameWon && timeLeft > 0)
            timeLeft -= Time.deltaTime;
        else
            GameWon();
        
        timerText.text = "" + Mathf.FloorToInt(timeLeft);
    }

    private void GameWon()
    {
        SM.WonGame(score, MAXSCORE);
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
