using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarInfo : MonoBehaviour
{
    [SerializeField] private Image _barImage;
    [SerializeField] private Color32 _normalColor;
    [SerializeField] private Color32 _wrongColor;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TMP_Text _timeText;

    public void UpdateValues(float time, float height, bool isCorrect)
    {
        _barImage.color = isCorrect ? _normalColor : _wrongColor;
        
        DOTween.To(() => 0, x => time = x, time, time);
        
        _timeText.SetText( isCorrect ? $"{time}" : "X");
        
        _rectTransform.DOSizeDelta(new Vector2(_rectTransform.rect.width, height), time);
    }
}

public class BarData
{
    private float barTime;
    private bool barCorrect;
    
    public BarData(float time, bool isCorrect)
    {
        barTime = time;
        barCorrect = isCorrect;
    }

    public float GetTime()
    {
        return barTime;
    }
    
    public bool GetCorrect()
    {
        return barCorrect;
    }
}
