using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentLanguage{ English, Dutch, German, French, Spanish} //could be a list ot get the index
public class LanguageManager : MonoBehaviour //need a player prefs to save current language
{
    public Action<List<string>> OnLanguageChange;

    [SerializeField] 
    private Image _languageButtonImage;
    
    [SerializeField] 
    private Sprite[] _languageIcons;
    
    [SerializeField] 
    private CurrentLanguage _currentLanguage;

    [SerializeField] 
    private TextAsset _textAsset;

    //private string[] _languages = new[] { "English", "Dutch", "German", "French", "Spanish" }; //could use to find the word index
    private int _currentLanguageIndex;
    
    private List<string> _lines = new ();

    //Language List
    //private List<string> _currentLanguageWords;
    private List<List<string>> _languageWordsList = new ();

    private List<string> _englishWords = new();
    private List<string> _dutchWords = new();
    private List<string> _germanWords = new();
    private List<string> _frenchWords = new();
    private List<string> _spanishWords = new();

    private void Awake()
    {
        InitializeLanguageList();
    }

    private void InitializeLanguageList()
    {
        foreach (string line in _textAsset.text.Split("\n"))
        {
            _lines.Add(line);
            
            string[] words = line.Split("Δ");
            _englishWords.Add(words[0]);
            _dutchWords.Add(words[1]);
            _germanWords.Add(words[2]);
            _frenchWords.Add(words[3]);
            _spanishWords.Add(words[4]);
        }

        _languageWordsList.Add(_englishWords);
        _languageWordsList.Add(_dutchWords);
        _languageWordsList.Add(_germanWords);
        _languageWordsList.Add(_frenchWords);
        _languageWordsList.Add(_spanishWords);
    }

    public void ChangeLanguage()
    {
        if (_currentLanguageIndex > _languageWordsList.Count - 2)
            _currentLanguageIndex = 0;
        else
            _currentLanguageIndex++;
        
        //update the icon
        _languageButtonImage.sprite = _languageIcons[_currentLanguageIndex];
        OnLanguageChange?.Invoke(_languageWordsList[_currentLanguageIndex]);
    }
}

