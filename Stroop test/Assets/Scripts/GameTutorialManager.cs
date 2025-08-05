using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameTutorialManager : MonoBehaviour
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
    public Text colorInfoText;
    public Text leftColor;
    public Text rightColor;

    private static int MAXSCORE = 3;
    private int score;
    private int questionNumber;
    private List<ColorList> colorList;
    private List<ColorList> tempColorList;
    private ColorList correctColor;
    private int randomNumb;
    private bool gameWon;
    private ScreenManager SM;

    private void Start()
    {
        SM = FindFirstObjectByType<ScreenManager>();
        colorList = new List<ColorList>()
            {
                new ColorList(new Color32(191, 76, 76,255), "Rood"), new ColorList(new Color32(26,128,254,255), "Blauw"), new ColorList(new Color32(84, 191, 76,255), "Groen"),
                new ColorList(Color.yellow, "Geel"), new ColorList(Color.black, "Zwart"),
                new ColorList(Color.magenta, "Roze"), new ColorList(Color.cyan, "Cyan")
            };
        GenerateNewPrompt();
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
            colorInfoText.text = "Kies de kleur van het woord:";
            colorInfoText.color = Color.white;
            GenerateNewPrompt();
        }
        else
        {
            bgAnim.SetBool("isRed", true);
            SM.GetComponent<AudioSource>().PlayOneShot(wrongSound);
            wrongIcon.transform.position = button.position;
            wrongIcon.SetActive(true);
            colorInfoText.text = "Selecteer de kleur van het woord niet de betekenis";
            colorInfoText.color = new Color32(191, 76, 76, 255);
        }
        Invoke("ResetAnim", 0.2f);    
        
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

        if (questionNumber < 1)
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
        QuestNumbText.text = "Vraag " + questionNumber+"/3";
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

    private void GameWon()
    {
        SM.TutorialWon();
    }
}