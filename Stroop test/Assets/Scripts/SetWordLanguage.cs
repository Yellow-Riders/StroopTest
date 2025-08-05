using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetWordLanguage : MonoBehaviour
{
    [SerializeField] 
    private int _lineNumber;

    private LanguageManager _languageManager;
    private TMP_Text _text;

    private List<string> _currentLanguageList;

    private void Awake()
    {
        _languageManager = FindFirstObjectByType<LanguageManager>();
        _text = GetComponent<TMP_Text>();
        _languageManager.OnLanguageChange += UpdateText;
    }

    private void OnEnable()
    {
        UpdateText();
        //Invoke(nameof(UpdateText), .1f);
    }

    private void UpdateText()
    {
        //Debug.Log(_lineNumber-1);
        if (_languageManager.currentLanguageWords.Count > 0)
            _text.text = _languageManager.currentLanguageWords[_lineNumber-1];
    }
}
