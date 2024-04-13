using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    
    public ClueData clue
    {
        get => _clue;

        set
        {
            _clue = value;
            UpdateImage();
        }
    }
    [HideInInspector] public ItemDescription infoMenu;
    private ClueData _clue;
    [SerializeField] private Image _image;
    public bool isFound;
    public void UpdateImage()
    {
        _image.sprite = _clue.sprite;  
        if(isFound)
            _image.color = Color.white;
        if (!isFound)
            _image.color = Color.black;
    }
    public void OnClick()
    {
        if (isFound)
        {
            infoMenu.gameObject.SetActive(true);
            infoMenu.setInfo(_clue);
        }
    }
}
