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
    private List<float> avgTimes;

    private void Start()
    {
        avgTimes = new List<float>();
    }


    public void UpdateBars(List<BarInfo> infoBar, int mistakes) // Could Animate with Lerp or move towards
    {
        for (int i = 0; i < infoBar.Count; i++)
        {
            //Set the sze and calculate height
            RectTransform barImage = bars[i].GetChild(0).GetComponent<RectTransform>();
            RectTransform barText = bars[i].GetChild(1).GetComponent<RectTransform>();
            float seconds = infoBar[i].GetTime();
            float value = seconds * 190f;
            float oneDecimal = Mathf.Round(seconds * 10) / 10;
            barImage.sizeDelta = new Vector2(20, value);
            barText.sizeDelta = new Vector2(value, 20);
            avgTimes.Add(seconds);
            
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

        UpdateText(mistakes);
    }

    void UpdateText(int mistakes)
    {
        float average = Mathf.Round(avgTimes.Average()*100)/100;
        averageTimeText.text = "" + average + "s";
        mistakesText.text = "" + mistakes;
    }

    void UpdateTimeLabels() //will have to do this before setting the bar heights
    {
        // float[] times = avgTimes.ToArray();
        // Mathf.Max(times);
        float highestTime = 3.45f;
        float nearestInt = Mathf.Ceil(highestTime);
        float oneHeight = 380/nearestInt;
            //380
        
        for (int i = 0; i < timeLabels.Length; i++)
        {
            timeLabels[i].text = "" + nearestInt/10 * i;
        }
    }

}
