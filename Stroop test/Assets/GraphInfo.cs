using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphInfo : MonoBehaviour
{
    public TextMeshProUGUI[] timeLabels;
    public Transform[] bars;
    public TextMeshProUGUI averageTimeText;
    public TextMeshProUGUI mistakesText;
    public TextMeshProUGUI difficultyText;
    private List<float> questionTimes;

    public void UpdateBars(List<BarInfo> infoBar, int mistakes, string difficulty) // Could Animate with Lerp or move towards
    {
        questionTimes = new List<float>();
        foreach (var bar in infoBar)
        {
            questionTimes.Add(bar.GetTime());
        }
        UpdateText(mistakes, difficulty);
        UpdateBarsHeight(infoBar);
    }

    void UpdateBarsHeight(List<BarInfo> infoBar)
    {
        float highestNumber = UpdateTimeLabels();
        float oneHeight = 380/highestNumber;
        
        for (int i = 0; i < infoBar.Count; i++)
        {
            //Set the sze and calculate height
            RectTransform barImage = bars[i].GetChild(0).GetComponent<RectTransform>();
            RectTransform barText = bars[i].GetChild(1).GetComponent<RectTransform>();
            
            float seconds = questionTimes[i];
            float oneDecimal = Mathf.Round(seconds * 10) / 10;
            float value = seconds * oneHeight;
            
            barImage.sizeDelta = new Vector2(20, value); // Animate this
            barText.sizeDelta = new Vector2(value, 20); //       Ani
            
            if (infoBar[i].GetCorrect()) // Set the colors and numbers in the Text
            {
                barImage.GetComponent<Image>().color = new Color32(26,128,254,255);
                if(value >50)
                    barText.GetComponent<TextMeshProUGUI>().text = "" + seconds;
                else
                    barText.GetComponent<TextMeshProUGUI>().text = "" + oneDecimal;
            }
            else
            {
                barImage.GetComponent<Image>().color = new Color32(191, 76, 76,255);
                barText.GetComponent<TextMeshProUGUI>().text = "X";
            }
        }
    }

    void UpdateText(int mistakes, string mode)
    {
        float average = Mathf.Round(questionTimes.Average()*100)/100;
        difficultyText.text = mode;
        averageTimeText.text = "" + average + "s";
        mistakesText.text = "" + mistakes;
    }

    float UpdateTimeLabels() //will have to do this before setting the bar heights
    {
        float[] times = questionTimes.ToArray();
        float highestInt = Mathf.Ceil(Mathf.Max(times));
        for (int i = 0; i < timeLabels.Length; i++)
            timeLabels[i].text = "" + highestInt/10 * i;

        return highestInt;
    }

}
