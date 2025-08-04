using System;
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
    private List<float> tempNumbers;

    [SerializeField]
    private RectTransform _verticalLineTransform;
    private float highestHeight = 300f;

    private void Start()
    {
        highestHeight = _verticalLineTransform.rect.height - 70f;
        
        Debug.Log(highestHeight);
    }

    public void UpdateBars(List<BarInfo> infoBar, int mistakes, string difficulty)
    {
        questionTimes = new List<float>();
        barsInfo = infoBar;
        CalculateHeights();
        UpdateText(mistakes, difficulty);
        tempNumbers = new List<float>() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    }

    void CalculateHeights()
    {
        heightValues = new List<float>();
        
        foreach (var bar in barsInfo)
            questionTimes.Add(bar.GetTime());
        
        float highestNumber = UpdateTimeLabels();
        float normalizedHeight = highestHeight/highestNumber;
        
        foreach (var time in questionTimes)
        {
            float value = time * normalizedHeight;
            heightValues.Add(value);
        }
    }
    void UpdateText(int mistakes, string mode)
    {
        float average = Mathf.Round(questionTimes.Average()*100)/100;
        difficultyText.text = mode;
        averageTimeText.text = "" + average + "s";
        mistakesText.text = "" + mistakes;
    }

    float UpdateTimeLabels()
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
            UpdateBarNumbers();
            GrowBars();
        }
    }

    private void UpdateBarNumbers()
    {
        for (int i = 0; i < barsInfo.Count; i++)
        {
            float value = heightValues[i];
            float seconds = questionTimes[i];
            
            
            Transform barImage = bars[i].GetChild(0);
            Transform barText = barImage.GetChild(0);

            if (barsInfo[i].GetCorrect()) // Set the colors and numbers in the Text
            {
                barImage.GetComponent<Image>().color = new Color32(26,128,254,255);
                tempNumbers[i] = Mathf.MoveTowards(tempNumbers[i], seconds,
                    seconds * Time.deltaTime); //use value maybe
                if (value > 50)
                {
                    float second = Mathf.Round(tempNumbers[i] * 100) / 100;
                    barText.GetComponent<TextMeshProUGUI>().text = "" + second;
                }
                else
                {
                    float secondOneDecimal = Mathf.Round(tempNumbers[i] * 10) / 10;
                    barText.GetComponent<TextMeshProUGUI>().text = "" + secondOneDecimal;
                }

            }
            else
            {
                barImage.GetComponent<Image>().color = new Color32(191, 76, 76,255);
                barText.GetComponent<TextMeshProUGUI>().text = "X";
            }
        }
    }

    private void GrowBars()
    {
        for (int i = 0; i < bars.Length; i++)
        {
            RectTransform barImage = bars[i].GetComponent<RectTransform>();
            Vector2 targetSize = new Vector2(20, heightValues[i]);
            barImage.sizeDelta = Vector2.MoveTowards(barImage.sizeDelta, targetSize, Time.deltaTime * heightValues[i]);
        }
    }
}
