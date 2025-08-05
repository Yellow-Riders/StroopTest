using System;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphInfo : MonoBehaviour
{
    [SerializeField] private BarInfo[] bars;
    [SerializeField] private TextMeshProUGUI[] timeLabels;
    [SerializeField] private TextMeshProUGUI averageTimeText;
    [SerializeField] private TextMeshProUGUI mistakesText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private RectTransform _verticalLineTransform;
    
    private List<BarData> barsInfo;
    private List<float> questionTimes;
    private List<float> heightValues;
    private float verticalLineHeight;

    private void Start()
    {
        verticalLineHeight = _verticalLineTransform.rect.height;

        UpdateBarVisuals();
    }

    public void UpdateBars(List<BarData> infoBar, int mistakes, string difficulty)
    {
        barsInfo = infoBar;
        UpdateText(mistakes, difficulty);
    }
    
    private void UpdateBarVisuals()
    {
        CalculateHeights();
        
        float average = Mathf.Round(questionTimes.Average()*100)/100;
        averageTimeText.SetText($"{average}s");
        
        UpdateBarNumbers();
    }

    void CalculateHeights()
    {
        questionTimes = new List<float>();
        heightValues = new List<float>();
        
        foreach (var bar in barsInfo)
            questionTimes.Add(bar.GetTime());
        
        float highestNumber = UpdateTimeLabels();
        float normalizedHeight = verticalLineHeight - 25f;
        
        foreach (var time in questionTimes)
            heightValues.Add(time / highestNumber * normalizedHeight);
    }
    void UpdateText(int mistakes, string mode)
    {
        difficultyText.SetText(mode);
        mistakesText.SetText($"{mistakes}");
    }

    float UpdateTimeLabels()
    {
        float[] times = questionTimes.ToArray();
        float highestInt = Mathf.Ceil(Mathf.Max(times));
        for (int i = 0; i < timeLabels.Length; i++)
            timeLabels[i].text = "" + highestInt/10 * i;

        return highestInt;
    }

    private void UpdateBarNumbers()
    {
        for (int i = 0; i < barsInfo.Count; i++)
        {
            bars[i].UpdateValues(questionTimes[i], heightValues[i], barsInfo[i].GetCorrect());
        }
    }
}
