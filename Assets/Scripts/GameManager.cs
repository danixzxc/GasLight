using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    [SerializeField] private ButtonController[] _lamps;
    [SerializeField] private AudioSource[] _sounds;

    private List<IObserver> _observers = new List<IObserver>();

    private int _colourSelect;
    
    [SerializeField] private float _stayLit;
    private float _stayLitCounter;

    [SerializeField] private float waitBetweenLights;
    private float waitBetweenCounter;

    private bool _shouldBeLit;
    private bool _shouldBeDark;

    [SerializeField] private List<int> _activeSequence;
    private int _positionInSequence;

    private bool _gameActive;
    private int _inputInSequence;

    [SerializeField] private AudioSource _correct;
    [SerializeField] private AudioSource _incorrect;

    [SerializeField] private TextMeshPro _scoreText;
    [SerializeField] private TextMeshProUGUI _highscoreText;

    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _endButton;

    private int _endScore = 16;
    
    private void Start()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.SetInt("HighScore", 0);

        _scoreText.text = "0";
        _highscoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        if (PlayerPrefs.GetInt("HighScore") >= _endScore)
            _endButton.SetActive(true);
    }

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }
    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }
    public void NotifyObservers(int currentScore, int lampNumber)
    {
        _observers.ForEach(observer => observer.OnNotify(currentScore, lampNumber));
    }
    public void NotifyLastLamp(int currentScore)
    {
        _observers.ForEach(observer => observer.LastLampNotify(currentScore));
    }

    public void NotifyObserversIncorrect()
    {
        _observers.ForEach(observer => observer.NotifyIncorrect());
    }

    void Update()
    {
        if (_shouldBeLit)
        {
            _stayLitCounter -= Time.deltaTime;
            if (_stayLitCounter < 0)
            {
                _lamps[_activeSequence[_positionInSequence]].turnOff();
                _sounds[_activeSequence[_positionInSequence]].Stop();

                _shouldBeLit = false;

                _shouldBeDark = true;
                waitBetweenCounter = waitBetweenLights;

                _positionInSequence++;
            }
        }
        if (_shouldBeDark)
        {
            waitBetweenCounter -= Time.deltaTime;

            if (_positionInSequence >= _activeSequence.Count)
            {
                _shouldBeDark = false;
                _gameActive = true;
                PauseManager.UnpauseGame();
            }
            else
            {
                if (waitBetweenCounter < 0)
                {

                    _lamps[_activeSequence[_positionInSequence]].turnOn();
                    _sounds[_activeSequence[_positionInSequence]].Play();

                    _stayLitCounter = _stayLit;

                    _shouldBeLit = true;
                    _shouldBeDark = false;
                }
            }
        }
    }



    public void StartGame()
    {

        PauseManager.PauseGame();

        _activeSequence.Clear();

        _positionInSequence = 0;
        _inputInSequence = 0;

        _colourSelect = UnityEngine.Random.Range(0, _lamps.Length);

        _activeSequence.Add(_colourSelect);


        _lamps[_activeSequence[_positionInSequence]].turnOn();
        _sounds[_activeSequence[_positionInSequence]].Play();

        _stayLitCounter = _stayLit;
        //  _shouldBeLit = true;
        _shouldBeDark = true;
        _shouldBeLit = false;


        _scoreText.text = "0";
        _highscoreText.text = PlayerPrefs.GetInt("HighScore").ToString();

        _startButton.active = false;
    }

    public void ColorPressedDown(int buttonNumber)
    {
        if (_gameActive && _activeSequence[_inputInSequence] == buttonNumber && _inputInSequence >= _activeSequence.Count - 1)
            NotifyLastLamp(_inputInSequence+1);

    }

    public void ColorPressed(int buttonNumber)
    {
        if (_gameActive)
        {
            if (_activeSequence[_inputInSequence] == buttonNumber)
            {
                _inputInSequence++;


                if (_inputInSequence >= _activeSequence.Count)
                {


                    if (_activeSequence.Count > PlayerPrefs.GetInt("HighScore"))
                    {
                        PlayerPrefs.SetInt("HighScore", _activeSequence.Count);
                        if (PlayerPrefs.GetInt("HighScore") >= _endScore)
                            _endButton.SetActive(true);
                       // NotifyObservers(PlayerPrefs.GetInt("HighScore"), _activeSequence.Last());
                    }

                    _scoreText.text = _activeSequence.Count.ToString();
                    _highscoreText.text = PlayerPrefs.GetInt("HighScore").ToString();

                    StartCoroutine(WaitCoroutine(buttonNumber));
                }
            }
            else
            {
                NotifyObserversIncorrect();
                _incorrect.Play();
                _gameActive = false;
                _scoreText.text = "0";
                _highscoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
                _startButton.active = true;
            }
        }
    }

    private IEnumerator WaitCoroutine(int buttonNumber)
    {

        yield return new WaitForSeconds(.25f);
        _positionInSequence = 0;
        _inputInSequence = 0;

        _colourSelect = UnityEngine.Random.Range(0, _lamps.Length);

        _activeSequence.Add(_colourSelect);

        NotifyObservers(_activeSequence.Count(), _activeSequence.Last());

        //used to notify observers here 

        _lamps[_activeSequence[_positionInSequence]].turnOn();
        _sounds[_activeSequence[_positionInSequence]].Play();

        _stayLitCounter = _stayLit;

        _shouldBeLit = true;


        _gameActive = false;
        PauseManager.PauseGame();

        _correct.Play();

    }
}
