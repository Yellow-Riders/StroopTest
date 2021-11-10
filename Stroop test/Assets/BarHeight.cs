using TMPro;
using UnityEngine;

public class BarHeight : MonoBehaviour
{
    public float seconds;
    private Transform barImage;
    private Transform barText;

    private void Start()
    {
        barImage = transform.GetChild(0);
        barText = transform.GetChild(1);
        //380 max
    }

    public void Update()
    {
        float value = seconds * 190f;
        float oneDecimal = Mathf.Round(seconds * 10) / 10;
        
        barImage.GetComponent<RectTransform>().sizeDelta = new Vector2(20, value);
        barText.GetComponent<RectTransform>().sizeDelta = new Vector2(value, 20);
        barText.GetComponent<TextMeshProUGUI>().text = "" + seconds;
        
        if(value >50)
            barText.GetComponent<TextMeshProUGUI>().text = "" + seconds;
        else
            barText.GetComponent<TextMeshProUGUI>().text = "" + oneDecimal;
    }
}
