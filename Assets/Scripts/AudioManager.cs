using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _bakcgroundMusic;
    public void SwitchMusic(AudioClip clip)
    {
        if (_bakcgroundMusic.clip.name == clip.name)
            return;

        _bakcgroundMusic.Stop();
        _bakcgroundMusic.clip = clip;
        _bakcgroundMusic.Play();
    }
}
