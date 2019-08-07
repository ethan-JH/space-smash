using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// script to control game logic and mechanics of main menu scene
public class MenuController : MonoBehaviour
{

    // UI panels present in main menu
    public GameObject mainMenuButtons;
    public GameObject volumePanel;
    public GameObject profileSelectionPanel;
    public GameObject verifyProfileCreation;
    public GameObject profilePanel;
    public GameObject clearSaveDataVerificationPanel;

    // array to hold the profile buttons on profile selection screen
    public Button[] profileButtons;

    // color of buttons with saved profile 
    public Color profileButtonNormalColor;
    public Color profileButtonHighlightedColor;
    public Color profileButtonTextColor;

    // color of buttons with no saved profile
    public Color createNewButtonNormalColor;
    public Color createNewButtonHighlightedColor;
    public Color createNewButtonTextColor;

    // title text
    public Text titleText;

    // input field for new profile names
    public InputField profileNameInput;

    // int to hold number of profiles created
    private int profilesCreated;

    // int to hold current profile number that has been selected
    private int currentProfileNum;

    // Start is called before the first frame update, initialises variables
    void Start()
    {
        mainMenuButtons.SetActive(true);
        volumePanel.SetActive(false);
        profileSelectionPanel.SetActive(false);
        verifyProfileCreation.SetActive(false);
        profilePanel.SetActive(false);
        clearSaveDataVerificationPanel.SetActive(false);

        titleText.text = "SPACE SMASH";

        if (PlayerPrefs.HasKey("ProfilesCreated"))
        {
            profilesCreated = PlayerPrefs.GetInt("ProfilesCreated");
        }
        else
        {
            profilesCreated = 0;
        }

        SetProfileButtons();
    }

    // sets whether volume settings is active
    public void ChangeVolumeSettings()
    {
        mainMenuButtons.SetActive(false);
        volumePanel.SetActive(true);
    }

    // activates the initial main menu screen
    public void ActivateMainMenu()
    {
        volumePanel.SetActive(false);
        profileSelectionPanel.SetActive(false);
        mainMenuButtons.SetActive(true);
        titleText.text = "SPACE SMASH";
    }

    // activates profile selection screen
    public void ActivateProfilePanel()
    {
        mainMenuButtons.SetActive(false);
        profilePanel.SetActive(false);
        profileSelectionPanel.SetActive(true);
        titleText.text = "PROFILES";
    }

    /**
     * activates profile creation verification screen if no profile created
     * otherwise if profile has been created activates profile screen
     */
    public void VerifyProfile()
    {
        if(currentProfileNum <= 0)
        {
            if (verifyProfileCreation.activeSelf)
            {
                profileSelectionPanel.SetActive(true);
                verifyProfileCreation.SetActive(false);
            }
            else
            {
                profileSelectionPanel.SetActive(false);
                verifyProfileCreation.SetActive(true);
            }
        }
        else if (currentProfileNum > 0)
        {
            titleText.text = PlayerPrefs.GetString("ProfileName" + currentProfileNum.ToString());
            profileSelectionPanel.SetActive(false);
            profilePanel.SetActive(true);
            profilePanel.GetComponent<ProfileController>().SetProfile();
        }
    }

    /**
     * creates profile with unique id based on number of profiles created
     * saves profile name, exercises completed to PlayerPrefs using the unique id to create a unique key
     * increments profiles created
     */
    public void CreateProfile()
    {
        string profileName = profileNameInput.text;
        if(profileName.Length > 0 && !profileName.Equals("...") && profilesCreated < 6)
        {
            profilesCreated++;
            PlayerPrefs.SetString("ProfileName" + profilesCreated.ToString(), profileName);
            PlayerPrefs.SetInt("ProfilesCreated", profilesCreated);
            PlayerPrefs.SetInt("ExercisesCompleted" + profilesCreated.ToString(), 0);
            SetProfileButtons();
            VerifyProfile();
        }
    }

    /**
     * iterates through the profile button array and sets the profile button colour block based on whether a profile has been created
     * sets the profile number for each button, with a default value of -1
     */
    private void SetProfileButtons()
    {
        if (profilesCreated > 0)
        {
            for (int loop = 0; loop < profilesCreated; loop++)
            {
                ColorBlock cb;
                cb = profileButtons[loop].colors;
                cb.normalColor = profileButtonNormalColor;
                cb.highlightedColor = profileButtonHighlightedColor;
                profileButtons[loop].colors = cb;

                profileButtons[loop].GetComponentInChildren<Text>().color = profileButtonTextColor;
                profileButtons[loop].GetComponentInChildren<Text>().text = PlayerPrefs.GetString("ProfileName" + (loop + 1).ToString());

                profileButtons[loop].GetComponent<Profile>().SetProfileNumber(loop + 1);
            }

        }
        else if (profilesCreated <= 0)
        {
            for (int loop = 0; loop < profileButtons.Length; loop++)
            {
                ColorBlock cb;
                cb = profileButtons[loop].colors;
                cb.normalColor = createNewButtonNormalColor;
                cb.highlightedColor = createNewButtonHighlightedColor;
                profileButtons[loop].colors = cb;

                profileButtons[loop].GetComponentInChildren<Text>().color = createNewButtonTextColor;
                profileButtons[loop].GetComponentInChildren<Text>().text = "CREATE NEW";

                profileButtons[loop].GetComponent<Profile>().SetProfileNumber(-1);
            }
        }
    }
    
    // sets the current activated profile to the selected button and sets the current profile number to PlayerPrefs
    public void SetCurrentProfile(Button profileButton)
    {
        SetProfileButtons();
        currentProfileNum = profileButton.GetComponent<Profile>().GetProfileNumber();
        //Debug.Log(currentProfileNum);
        PlayerPrefs.SetInt("CurrentProfileNumber", currentProfileNum);
    }

    // sets whether the clear save data verification panel is active
    public void VerifyClearSaveData()
    {
        if (clearSaveDataVerificationPanel.activeSelf)
        {
            clearSaveDataVerificationPanel.SetActive(false);
            profileSelectionPanel.SetActive(true);
        }
        else
        {
            clearSaveDataVerificationPanel.SetActive(true);
            profileSelectionPanel.SetActive(false);
        }
    }

    // deletes all saved PlayerPrefs data and resets profiles created
    public void ClearAllSaveData()
    {
        PlayerPrefs.DeleteAll();
        profilesCreated = 0;
        SetProfileButtons();
    }
}
