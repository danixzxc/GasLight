using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;

public class DialogController : MonoBehaviour
{
    [SerializeField] private string prefix = "DIALOGUE_";
    [SerializeField] private int count = 5;
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private GameObject _officer;
    [SerializeField] private GameObject _suspects;

    private string[] _dialogeMessages;
    private int _currentMessage = 0;
    private void Start()
    {
        GenerateDialogueArray();
        _text.text = _dialogeMessages[0];
    }

    private void GenerateDialogueArray()
    {
        _dialogeMessages = new string[count];

        var table = LocalizationSettings.StringDatabase.GetTable("Test Table");

        for (int i = 0; i < count; i++) 
        {
            _dialogeMessages[i] = table.GetEntry(prefix + i).Value;
            Debug.Log(table.GetEntry(prefix + i).Value);
        }
    }

    public void NextMessage()
    {
        _currentMessage++;
        if(_currentMessage == _dialogeMessages.Length) 
        {
            _officer.SetActive(false);
            _suspects.SetActive(true);
        }
        else if (_currentMessage > _dialogeMessages.Length)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            _text.text = _dialogeMessages[_currentMessage];
    }
}
