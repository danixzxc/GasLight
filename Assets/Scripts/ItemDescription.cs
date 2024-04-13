using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;

    public void setInfo(ClueData clue)
    {
        _image.sprite = clue.sprite;

        var table = LocalizationSettings.StringDatabase.GetTable("Test Table");
        _description.text = table.GetEntry(clue.description).Value;
        _name.text = table.GetEntry(clue.name).Value;
    }
}
