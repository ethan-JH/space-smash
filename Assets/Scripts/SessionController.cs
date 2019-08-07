using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// controls the game logic and mechanics for the exercise session scene
public class SessionController : MonoBehaviour
{
    // holds the spaceship tracker 
    public GameObject spaceship;

    // UI elements for scene
    public GameObject scorePanel;
    public Text scoreText;
    public Text repetitionText;
    public Text timerText;

    public GameObject vrScorePanel;
    public Text vrPlayerNameText;
    public Text vrInstructionText;
    public Text vrScoreText;
    public Text vrRepetitionText;
    public Text vrTimerText;

    public GameObject dropdownPanel;
    public GameObject saveVerificationPanel;
    public GameObject volumeSettings;
    public GameObject sceneVerificationPanel;

    // exercise information
    private string exerciseName;
    private int plannedRepetitionNumber;
    private int actualRepetitionNumber;
    private int playerScore;

    // current profile information
    private int currentProfileNumber;
    private string currentProfileName;
    private int exercisesCompleted;

    // flag to check if exercise is completed
    private bool completedExercise;

    // float to hold game timer
    private float timer;

    // game object holding start and end point
    private GameObject startPoint;
    private GameObject endPoint;

    // array to hold mid points and all asteroids
    private GameObject[] midPoints;
    private GameObject[] asteroids;

    // dynamically sized list to hold asteroid points
    private List<GameObject> asteroidList = new List<GameObject>();

    // Start is called before the first frame update, initialises variables and game objects
    void Start()
    {
        dropdownPanel.SetActive(false);
        saveVerificationPanel.SetActive(false);
        volumeSettings.SetActive(false);
        sceneVerificationPanel.SetActive(false);
        
        // exercise info from PlayerPrefs
        exerciseName = PlayerPrefs.GetString("ExerciseName");
        plannedRepetitionNumber = PlayerPrefs.GetInt("RepetitionNumber");

        actualRepetitionNumber = 0;
        playerScore = 0;
        timer = 0.0f;

        completedExercise = false;

        // profile info from PlayerPrefs
        currentProfileNumber = PlayerPrefs.GetInt("CurrentProfileNumber");
        currentProfileName = PlayerPrefs.GetString("ProfileName" + currentProfileNumber.ToString());
        exercisesCompleted = PlayerPrefs.GetInt("ExercisesCompleted" + currentProfileNumber.ToString());

        vrPlayerNameText.text = "Player: "  + currentProfileName;

        // adds points to asteroid list
        startPoint = GameObject.FindGameObjectWithTag("Start Point");
        asteroidList.Add(startPoint);
        endPoint = GameObject.FindGameObjectWithTag("End Point");
        asteroidList.Add(endPoint);

        midPoints = GameObject.FindGameObjectsWithTag("Mid Point");

        for (int loop = 0; loop < midPoints.Length; loop++)
        {
            asteroidList.Add(midPoints[loop]);
        }

        // converts asteroid list to array
        asteroids = asteroidList.ToArray();

        vrInstructionText.text = "GET THE ASTEROIDS!";
    }

    // Update is called once per frame
    void Update()
    {   
        // continues timer if repetition number hasn't been reached
        if(actualRepetitionNumber < plannedRepetitionNumber && !completedExercise)
        {
            timer += Time.deltaTime;
        }
        else if(actualRepetitionNumber >= plannedRepetitionNumber && !completedExercise) // ends game if rep number is reached, plays sound, and verifies the save
        {
            completedExercise = true;
            vrInstructionText.text = "YOU WIN!";
            spaceship.GetComponent<RocketSessionController>().playVictory();
            VerifySaveData();
        }
        // updates UI elements
        timerText.text = "Time: " + SessionTimer();
        vrTimerText.text = timerText.text;
        scoreText.text = "Score: " + playerScore;
        vrScoreText.text = scoreText.text;
        repetitionText.text = "Repetitions: " + actualRepetitionNumber + "/" + plannedRepetitionNumber;
        vrRepetitionText.text = repetitionText.text;
    }

    // formats time into 00:00 format
    private string SessionTimer()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string timeFormat = string.Format("{0:00}:{1:00}", minutes, seconds);
        return timeFormat;
    }

    // activates mid point asteroids
    public void ActivateMidPoints(bool activated)
    {
        for(int loop = 0; loop < midPoints.Length; loop++)
        {
            midPoints[loop].SetActive(activated);
        }
    }

    // increments player score
    public void AddPlayerScore(int scoreAdded)
    {
        playerScore += scoreAdded;
    }

    // increments the rep number
    public void AddRepetition()
    {
        actualRepetitionNumber++;
    }
    
    // sets whether all asteroids are active
    public void SetAsteroidsActive(bool asteroidsActive)
    {
        for (int loop = 0; loop < asteroids.Length; loop++)
        {
            asteroids[loop].SetActive(asteroidsActive);
        }
    }
    
    // sets whether session is paused
    private void SetPauseSession(bool pauseSession)
    {
        if (pauseSession)
        {
            Time.timeScale = 0;
            SetAsteroidsActive(false);
            dropdownPanel.SetActive(false);
        }
        else if (!pauseSession)
        {
            Time.timeScale = 1;
            SetAsteroidsActive(true);
        }
    }

    // sets whether volume settings is active
    public void changeVolumeSettings()
    {
        if (volumeSettings.activeSelf)
        {
            volumeSettings.SetActive(false);
        }
        else
        {
            volumeSettings.SetActive(true);
        }
    }

    // destroys all asteroids
    private void DestroyAsteroids()
    {
        for (int loop = 0; loop < asteroids.Length; loop++)
        {
            Destroy(asteroids[loop]);
        }
    }

    // saves all exercise information to PlayerPrefs with unique key from current profile number and exercises completed
    public void SaveExerciseData()
    {
        exercisesCompleted++;
        //Debug.Log("Exercises Completed " + exercisesCompleted);
        PlayerPrefs.SetInt("ExercisesCompleted" + currentProfileNumber.ToString(), exercisesCompleted);
        //Debug.Log("Pref set to " + PlayerPrefs.GetInt("ExercisesCompleted" + currentProfileNumber.ToString()));
        PlayerPrefs.SetString(currentProfileNumber.ToString() + "ExerciseName" + exercisesCompleted.ToString(), exerciseName);
        PlayerPrefs.SetInt(currentProfileNumber.ToString() + "RepsCompleted" + exercisesCompleted.ToString(), actualRepetitionNumber);
        PlayerPrefs.SetInt(currentProfileNumber.ToString() + "Score" + exercisesCompleted.ToString(), playerScore);
        PlayerPrefs.SetString(currentProfileNumber.ToString() + "TimeTaken" + exercisesCompleted.ToString(), SessionTimer());
    }

    // deactivates asteroids and activates save verification panel
    public void VerifySaveData()
    {
        SetAsteroidsActive(false);
        saveVerificationPanel.SetActive(true);
    }

    // sets scene verification panel to active and save verification panel to false
    public void VerifySceneChange()
    {
        sceneVerificationPanel.SetActive(true);
        saveVerificationPanel.SetActive(false);
    }

    // destroys asteroids and loads main menu scene
    public void LoadMainMenu()
    {
        DestroyAsteroids();
        SceneManager.LoadScene("MainMenu");
    }

    // destroys asteroids and loads exercise creation scene
    public void LoadExerciseCreation()
    {
        DestroyAsteroids();
        SceneManager.LoadScene("ExerciseCreation");
    }

    // loads current scene
    public void ReloadExerciseSession()
    {
        SetAsteroidsActive(true);
        for (int loop = 0; loop < asteroids.Length; loop++)
        {
            DontDestroyOnLoad(asteroids[loop]);
        }
        Time.timeScale = 1;
        SceneManager.LoadScene("ExerciseSession");
    }
}
