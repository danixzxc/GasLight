using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseImposterMenu : MonoBehaviour
{

    [SerializeField] private Animator _winAnimator;
    [SerializeField] private Animator _loseAnimator;
    [SerializeField] private CanvasGroup _winCanvasGroup;
    [SerializeField] private CanvasGroup _loseCanvasGroup;
    [SerializeField] private GameObject _buttonsToActivate;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _winMusic;
    [SerializeField] private AudioClip _loseMusic;
    [SerializeField] private AudioClip _defaultMusic;

    private AudioManager _backgroundMusic;

    private void Start()
    {
        _backgroundMusic = FindObjectOfType<AudioManager>();
        if(_backgroundMusic != null ) { Debug.Log("AUDIO MANAGER IS FOUND"); }
    }
    public void PlayDefaultMusic()
    {
        if (_defaultMusic != null)
            _backgroundMusic.SwitchMusic(_defaultMusic);
    }
    public void PlayLoseMusic()
    {
        if(_loseMusic != null) 
            _backgroundMusic.SwitchMusic(_loseMusic);
    }
    public void PlayWinMusic()
    {
        if (_winMusic != null)
            _backgroundMusic.SwitchMusic(_winMusic);
    }
    public void PlayWinAnimation()
    {
        _winAnimator.SetTrigger("Start");
        _winCanvasGroup.blocksRaycasts = true;
        _buttonsToActivate.SetActive(true);
    }
    public void PlayLoseAnimation()
    {
        _loseAnimator.SetTrigger("Start");
        _loseCanvasGroup.blocksRaycasts = true;
        _buttonsToActivate.SetActive(true);
    }



    public void ReturnToGameplay()
    {
        StartCoroutine(LoadLevel(2));
    }

    IEnumerator LoadLevel(int index)
    {
        _animator.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(index);

        PlayerPrefs.SetInt("Scene", index);
    }
    public void MakeReview()
    {
    Application.OpenURL("market://details?id=" + Application.productName);
    }

}
