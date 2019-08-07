using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;    

// script attached to volume sliders to set the volume levels in the mixer
public class MixLevels : MonoBehaviour
{
    // the mixer for audio in the game
    public AudioMixer masterMixer;

    // floats to hold volume levels for master, sfx, and music
    private float masterVolume;
    private float sfxVolume;
    private float musicVolume;

    /**
     * called on first frame update, checks if PlayerPrefs for volume levels have been set, if not defaults to 1
     * sets the volume taken from these values
     */
    private void Start()
    {
        
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            masterVolume = PlayerPrefs.GetFloat("MasterVol", 1.0f);
        }
        else
        {
            masterVolume = 1.0f;
        }

        if (PlayerPrefs.HasKey("SFXVol"))
        {
            sfxVolume = PlayerPrefs.GetFloat("SFXVol", 1.0f);
        }
        else
        {
            sfxVolume = 1.0f;
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVol", 1.0f);
        }
        else
        {
            musicVolume = 1.0f;
        }

        masterMixer.SetFloat("MasterVol", masterVolume);
        masterMixer.SetFloat("SFXVol", sfxVolume);
        masterMixer.SetFloat("MusicVol", musicVolume);
        
    }
    
    // takes input from master slider, converts to logaritmic value and sets as master volume 
    public void SetMasterLevel(float masterLevel)
    {
        masterVolume = Mathf.Log10(masterLevel) * 20;
        masterMixer.SetFloat("MasterVol", masterVolume);
        PlayerPrefs.SetFloat("MasterVol", masterLevel);
    }

    // takes input from effects slider, converts to logaritmic value and sets as effects volume 
    public void SetSfxLevel(float sfxLevel)
    {
        sfxVolume = Mathf.Log10(sfxLevel) * 20;
        masterMixer.SetFloat("SFXVol", sfxVolume);
        PlayerPrefs.SetFloat("SFXVol", sfxLevel);
    }

    // takes input from music slider, converts to logaritmic value and sets as music volume 
    public void SetMusicLevel(float musicLevel)
    {
        musicVolume = Mathf.Log10(musicLevel) * 20;
        masterMixer.SetFloat("MusicVol", musicVolume);
        PlayerPrefs.SetFloat("MusicVol", musicLevel);
    }
}
