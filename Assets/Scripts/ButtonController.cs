using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ButtonController : MonoBehaviour, IPausable
{

    public int thisButtonNumber;

    private GameManager _gameManager;

    private AudioSource _sound;

    [SerializeField] private Light2D _beamLight;
    [SerializeField] private Light2D _centerLight;

    private int _pauseCounter = 0; // int because there are several ways to pause game
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _sound = GetComponent<AudioSource>();
    }

    public void turnOn()
    {
        _centerLight.enabled = true;
    }

    public void turnOff()
    {
        _centerLight.enabled = false;
    }

    public void lightOn()
    {
        _centerLight.enabled = true;
        _beamLight.enabled = true;
    }

    public void lightOff()
    {
        _centerLight.enabled = false;
        _beamLight.enabled = false;
    }

    private void OnMouseDown()
    {
        if (!isPaused())
        {
            lightOn();
            _sound.Play();
        _gameManager.ColorPressedDown(thisButtonNumber);
        }

    }

    private void OnMouseUp()
    {
        if (!isPaused())
        {
            lightOff();
            _sound.Stop();
            _gameManager.ColorPressed(thisButtonNumber);
        }
    }

    public void Pause()
    {
        _pauseCounter++;
    }


    public void Unpause()
    {
        _pauseCounter--;
    }

    private bool isPaused()
    {
        return _pauseCounter != 0;
    }
}
