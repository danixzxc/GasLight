using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("HighScore");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartNextScene()
    {
       StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }    

    IEnumerator LoadLevel(int index)
    {
        _animator.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(index);

        PlayerPrefs.SetInt("Scene", index);
    }

    private void Start()
    {


        if (PlayerPrefs.HasKey("Scene") && SceneManager.GetActiveScene().buildIndex != PlayerPrefs.GetInt("Scene"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("Scene"));
        }
        else
            PlayerPrefs.SetInt("Scene", SceneManager.GetActiveScene().buildIndex);
        if (!PlayerPrefs.HasKey("MusicSliderValue"))
            PlayerPrefs.SetFloat("MusicSliderValue", 1);

        if (!PlayerPrefs.HasKey("SFXSliderValue"))
            PlayerPrefs.SetFloat("SFXSliderValue", 1);

        _sfxSlider.value = PlayerPrefs.GetFloat("SFXSliderValue");
        _musicSlider.value = PlayerPrefs.GetFloat("MusicSliderValue");


        if (!PlayerPrefs.HasKey("Language"))
            PlayerPrefs.SetString("Language", "ru");
        bool isRussian = PlayerPrefs.GetString("Language") == "ru";
        int localeCount = isRussian ? 1 : 0;
        if (LocalizationSettings.AvailableLocales.Locales.Count != 0)
        {
            var localization = LocalizationSettings.AvailableLocales.Locales[localeCount];
            var locales = LocalizationSettings.AvailableLocales;
            LocalizationSettings.SelectedLocale = locales.Locales[PlayerPrefs.GetString("Language") == "ru" ? 1 : 0];
        }
    }

    public void setMusicVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat("MusicSliderValue", sliderValue);
        _mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicSliderValue")) * 20 );
    }
    public void setSFXVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat("SFXSliderValue", sliderValue);
        _mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXSliderValue")) * 20);
    }

    public void ChooseRussianLanguage()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        PlayerPrefs.SetString("Language", "ru");
    }
    public void ChooseEnglishLanguage()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        PlayerPrefs.SetString("Language", "en");
    }

    public void LoadTutorial()
    {
        StartCoroutine(LoadLevel(1));
    }    
}
