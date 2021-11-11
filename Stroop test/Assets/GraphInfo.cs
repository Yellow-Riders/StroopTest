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
    private List<BarInfo> barsInfo;
    private List<float> questionTimes;
    private List<float> heightValues;

    public void UpdateBars(List<BarInfo> infoBar, int mistakes, string difficulty) // Could Animate with Lerp or move towards
    {
        questionTimes = new List<float>();
        barsInfo = infoBar;
        foreach (var bar in infoBar)
            questionTimes.Add(bar.GetTime());
        UpdateText(mistakes, difficulty);
        UpdateBarsHeight();
    }

    void UpdateBarsHeight()
    {
        heightValues = new List<float>();
        float highestNumber = UpdateTimeLabels();
        float oneHeight = 380/highestNumber;
        
        for (int i = 0; i < bars.Length; i++)
        {
            //Set the sze and calculate height
            Transform barImage = bars[i].GetChild(0);
            Transform barText = barImage.GetChild(0);
            
            float seconds = questionTimes[i];
            float oneDecimal = Mathf.Round(seconds * 10) / 10;
            float value = seconds * oneHeight;
            heightValues.Add(value);
            
            if (barsInfo[i].GetCorrect()) // Set the colors and numbers in the Text
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

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            for (int i = 0; i < bars.Length; i++)
            {
                RectTransform barImage = bars[i].GetChild(0).GetComponent<RectTransform>();
                Vector2 targetSize = new Vector2(20, heightValues[i]);
                barImage.sizeDelta = Vector2.MoveTowards(barImage.sizeDelta, new Vector2(20, heightValues[i]), .5f);
                //move towards moves in a certain amount of time
                //Lerp uses Time delta
            }
        }
    }
}
