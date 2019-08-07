using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// script attached to pause button to pause and resume game
public class PauseScript : MonoBehaviour
{

    // UI for pause screen
    public Text pauseText;
    public GameObject pauseScreen;

    // UI colors for normal and paused color
    public Color normalColor;
    public Color pauseColor;

    // pause button image
    private Image pauseButton;

    // flag to check if game is paused
    private bool isPaused;

    // session controller variable
    private SessionController sessionController;

    // arrays to hold all points in asteroids, and just midpoints in midpoints
    private GameObject[] asteroids;
    private GameObject[] midPoints;

    // dynamically sized list to take asteroids
    private List<GameObject> asteroidList = new List<GameObject>();
    
    // called on first frame update, initialises values
    private void Start()
    {
        pauseButton = gameObject.GetComponent<Image>();
        isPaused = false;
        SetPauseScreenActive(false);

        sessionController = FindObjectOfType<SessionController>();

        asteroidList.Add(GameObject.FindGameObjectWithTag("Start Point"));
        asteroidList.Add(GameObject.FindGameObjectWithTag("End Point"));
        midPoints = GameObject.FindGameObjectsWithTag("Mid Point");

        // adds all points to asteroid list
        for (int loop = 0; loop < midPoints.Length; loop++)
        {
            asteroidList.Add(midPoints[loop]);
        }

        // converts asteroid list to array
        asteroids = asteroidList.ToArray();
    }

    // sets whether timescale is normal and asteroids are active, changes UI elements to show if game is paused
    public void PauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            sessionController.SetAsteroidsActive(isPaused);
            isPaused = false;
            SetPauseText("pause");
            SetPauseScreenActive(false);
            SetPauseButtonColor(normalColor);
            
        }
        else
        {
            Time.timeScale = 0;
            sessionController.SetAsteroidsActive(isPaused);
            isPaused = true;
            SetPauseText("resume");
            SetPauseScreenActive(true);
            SetPauseButtonColor(pauseColor);
        }
    }

    // sets the pause text
    private void SetPauseText(string text)
    {
        pauseText.text = text;
    }

    // sets the colour of the pause button
    private void SetPauseButtonColor(Color color)
    {
        pauseButton.color = color;
    }

    // sets whether the pause screen is active
    private void SetPauseScreenActive(bool isActive)
    {
        pauseScreen.SetActive(isActive);
    }

}
