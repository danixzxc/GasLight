using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DisplayClue : MonoBehaviour, IObserver
{
    private GameObject _cluePrefab;
    private GameObject _inventorySlot;
    [SerializeField] private string _cluePrefabName = "CluePrefab";
    [SerializeField] private string _inventorySlotName = "InventorySlot";
    [SerializeField] private ItemDescription _infoMenu;
    [SerializeField] private List<ClueData> _cluesList;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private GameObject _itemsParent;
    [SerializeField] private GameObject _notifyObject;

    private class Value
    {
        public Value(ClueData clue, bool Value2)
        {
            Value1 = clue;
            this.Value2 = Value2;
        }
        public ClueData Value1 { get; set; }
        public bool Value2 { get; set; }
    }


    private Dictionary<int, Value> _clues = new Dictionary<int, Value>();
    private Dictionary<int, InventorySlot> _inventorySlots = new Dictionary<int, InventorySlot>();

    private GameObject _clueObject;
    private void Start()
    {
        InstantiateClues();
        SubscribeOnGameManager();
    }

    private void InstantiateClues()
    {
        _cluePrefab = Resources.Load(_cluePrefabName) as GameObject;
        _inventorySlot = Resources.Load(_inventorySlotName) as GameObject;
        GameObject gameObject;
        foreach (var clue in _cluesList)
        {
            Value value = new Value(clue, clue.scoreValue <= PlayerPrefs.GetInt("HighScore"));
            _clues.Add(clue.scoreValue, value); 
            gameObject = Instantiate(_inventorySlot);
            InventorySlot inventorySlot = gameObject.GetComponent<InventorySlot>();
            inventorySlot.clue = clue;
            inventorySlot.infoMenu = _infoMenu;
            inventorySlot.isFound = inventorySlot.clue.scoreValue <= PlayerPrefs.GetInt("HighScore");
            inventorySlot.UpdateImage();
            _inventorySlots.Add(clue.scoreValue, inventorySlot);
            gameObject.transform.SetParent(_itemsParent.transform, false);
        }
    }

    private void SubscribeOnGameManager()
    {
        var subject = FindObjectOfType<GameManager>();
        subject.AddObserver(this);
    }

    public void OnNotify(int currentScore, int lampNumber)
    {
        if (_clueObject != null)
            Destroy(_clueObject);
        if (_clues.ContainsKey(currentScore) && _clues[currentScore].Value2 == false)
        {
            ClueData clue = _clues[currentScore].Value1;
            _clueObject = Instantiate(_cluePrefab);
            SpriteRenderer spriteRenderer = _clueObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = clue.sprite;
            spriteRenderer.color = Color.black ;
            _clueObject.transform.position = _spawnPoints[lampNumber].position;
        }
    }

    public void LastLampNotify(int currentScore)
    {
        if (_clueObject == null)
            return;
        else
        {
            if (_clueObject != null)
            {
                SpriteRenderer spriteRenderer = _clueObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.white;
                _clues[currentScore].Value2 = true;
                _inventorySlots[currentScore].isFound = true;
            _inventorySlots[currentScore].UpdateImage();
                _notifyObject.SetActive(true);
            }
        }
    }

    public void NotifyIncorrect()
    {
        if (_clueObject != null)
        {
            Destroy(_clueObject);
        }

    }


}
