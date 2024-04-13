using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class VfxObserver : MonoBehaviour, IObserver
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private GameObject _visualEffects;
    private GameObject _visualEffectObject;

    [SerializeField] private List<ClueData> _cluesList;
    private Dictionary<int, ClueData> _clues = new Dictionary<int, ClueData>();
    private void Start()
    {
        foreach (var clue in _cluesList)
        {
            _clues.Add(clue.scoreValue, clue);
        }
        SubscribeOnGameManager();
    }

    private void SubscribeOnGameManager()
    {
        var subject = FindObjectOfType<GameManager>();
        subject.AddObserver(this);
    }

    public void OnNotify(int currentScore, int lampNumber)
    {
        if (_visualEffectObject != null)
            Destroy(_visualEffectObject);
        if (_clues.ContainsKey(currentScore) && currentScore > PlayerPrefs.GetInt("HighScore"))
        {
            _visualEffectObject = Instantiate(_visualEffects);
            _visualEffectObject.transform.position = _spawnPoints[lampNumber].position;
        }
    }

    public void LastLampNotify()
    {
        //
    }

}
