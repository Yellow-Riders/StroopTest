using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetWordLanguage : MonoBehaviour
{
    [SerializeField] 
    private int _lineNumber;

    private LanguageManager _languageManager;
    private TMP_Text _text;

    private void Awake()
    {
        _languageManager = FindObjectOfType<LanguageManager>();
        _text = GetComponent<TMP_Text>();
        _languageManager.OnLanguageChange += UpdateText;
    }

    private void UpdateText(List<string> words)
    {
        _text.text = words[_lineNumber-1];
    }
}
