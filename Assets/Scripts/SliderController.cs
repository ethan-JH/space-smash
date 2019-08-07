using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// controls the mechanics and logic of the volume sliders
public class SliderController : MonoBehaviour
{
    // UI panel for settings
    public GameObject volumePanel;

    // the volume slider objects
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    // the slider text objects
    public Text masterText;
    public Text sfxText;
    public Text musicText;

    // floats to hold volumes
    private float masterVolume;
    private float sfxVolume;
    private float musicVolume;

    // percentage values
    private int masterPercentage;
    private int sfxPercentage;
    private int musicPercentage;

    // Start is called before the first frame update, checks if PlayerPrefs have been set and initialises volume values (default of 1), slider values, and percentage text
    void Start()
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

        masterSlider.value = masterVolume;
        sfxSlider.value = sfxVolume;
        musicSlider.value = musicVolume;

        masterPercentage = Mathf.RoundToInt(masterVolume * 100);
        sfxPercentage = Mathf.RoundToInt(sfxVolume * 100);
        musicPercentage = Mathf.RoundToInt(musicVolume * 100);

        masterText.text = masterPercentage.ToString() + "%";
        sfxText.text = sfxPercentage.ToString() + "%";
        musicText.text = musicPercentage.ToString() + "%";
        
    }

}
