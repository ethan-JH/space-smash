using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// class to manage game logic and mechanics of exercise creation scene
public class GameController : MonoBehaviour
{
    // transform of vive tracker
    public Transform trackerTransform;

    // asteroid prefabs to form start and end points and mid point
    public GameObject endPointPrefab;
    public GameObject midPointPrefab;

    // UI panels for instructions, verifications and inputs
    public GameObject volumeSettings;
    public GameObject instructionPanel;
    public GameObject verificationPanel1;
    public GameObject verificationPanel2;
    public GameObject inputPanel;

    //audio for sound effects
    public AudioSource redAsteroidSound;
    public AudioSource greenAsteroidSound;

    // UI text to show timer, instructions and errors
    public Text timerText;
    public Text instructionText;
    public Text errorText;

    // input fields for exercise name and number of repetitions
    public InputField exerciseNameInput;
    public InputField repetitionInput;

    // game objects created by calibration for path of exercise
    private GameObject startPoint;
    private GameObject endPoint;
    private GameObject midPoint;

    // floats used to determine time and calculate when to instantiate game objects
    private float startTime;
    private float timeOfExercise;
    private float timeDivider;
    private float exerciseTimeLimit;

    // boolean values to act as flags for certain conditional statements and methods
    private bool startTiming;
    private bool endTiming;
    private bool canStartCalibration;
    private bool canEndCalibration;
    private bool canResetTimer;
    private bool calibrationVerified;

    // number of points already instantiated
    private int pointsInstantiated;

    // list of points
    private List<GameObject> points = new List<GameObject>();

    // Start is called before the first frame update, sets initial values of variables in scene
    private void Start()
    {
        startTiming = false;
        endTiming = false;
        canStartCalibration = false;
        canEndCalibration = false;
        canResetTimer = false;
        calibrationVerified = false;

        pointsInstantiated = 0;

        startTime = 0.0f;
        timeOfExercise = 0.0f;
        timeDivider = 0.2f;
        exerciseTimeLimit = 30.0f;

        instructionText.text = "To begin creating your exercise, attach the tracker to the hand or foot of the limb being exercised, " +
            "then have the patient in the resting position/start point of the exercise. Press the SPACE BAR to record this start point.";
        timerText.text = "";
        errorText.text = "";

        volumeSettings.SetActive(false);
        verificationPanel1.SetActive(false);
        verificationPanel2.SetActive(false);
        inputPanel.SetActive(false);

    }

    /**
     * checks how many points have been instantiated and calls the create point method based on point number
     * plays sound and changes instruction text
     */
    public void checkPoints(int pointNumber)
    {
        if(pointNumber == 0)
        {
            createPoint(startPoint, "Start Point");
            redAsteroidSound.Play();
            changeInstructionText("Now move the patient into the end of the range of motion or their extended/tensed position of the exercise. Press the SPACE BAR to record this end point.");
        }
        else if (pointNumber == 1)
        {
            createPoint(endPoint, "End Point");
            redAsteroidSound.Play();
            changeInstructionText("The start and end points have been recorded. Move the patient back into the starting position");
            verifyStartAndEndPoint();
        }
    }

    // sets if timing exercise should start
    public void startTimingExercise(bool canStartTiming)
    {
        startTiming = canStartTiming;
    }

    // sets if timing exercise should end
    public void endTimingExercise(bool canEndTiming)
    {
        endTiming = canEndTiming;
    }

    // sets if calibrating exercise should start
    public void startCalibration(bool startingCalibration)
    {
        canStartCalibration = startingCalibration;
    }

    // sets if calibrating exercise should end
    public void endCalibration(bool endingCalibration)
    {
        canEndCalibration = endingCalibration;
    }

    // Update is called once per frame
    private void Update()
    {
        // if only startTiming is true, timer is started and output to UI
        if(startTiming && !endTiming && !canStartCalibration && !canEndCalibration && !canResetTimer)
        {
            startTime += Time.deltaTime;
            timerText.text = "Time: " + startTime.ToString("F2") + "s";
            // checks if over time limit and restarts scene
            if(startTime >= exerciseTimeLimit)
            {
                restartScene();
            }
            instructionPanel.SetActive(false);
        }
        else if(startTiming && endTiming && !canStartCalibration && !canEndCalibration && !canResetTimer) // if startTiming and endTiming true only, then ends timer and resets
        {
            timeOfExercise = startTime;
            timerText.text = "Time: " + timeOfExercise.ToString("F2") + "s";
            instructionPanel.SetActive(true);
            setStartTime(0.0f);
            canResetTimer = true;
        }
        else if (startTiming && endTiming && canStartCalibration && !canEndCalibration && canResetTimer) // if canStartcalibration true, restarts timer and begins calibration
        {
            startTime += Time.deltaTime;
            timerText.text = "Time: " + startTime.ToString("F2") + "s";
            instructionPanel.SetActive(false);

            // if timer is at each interval created by the time divider, instantiates one of up to 4 midpoints and plays audio
            if (startTime >= (timeOfExercise * timeDivider) &&  pointsInstantiated < 4)
            {
                createPoint(midPoint, "Mid Point");
                greenAsteroidSound.Play();
                // Debug.Log(startTime.ToString());
                pointsInstantiated++;
                timeDivider += 0.2f;
            }
            // restarts if time limit is reached
            if (startTime >= exerciseTimeLimit)
            {
                restartScene();
            }
        } 
        else if(startTiming && endTiming && canStartCalibration && canEndCalibration && canResetTimer) // if all flags are true, ends calibration process and verifies with user
        {
            changeInstructionText("Exercise calibrated.");
            // flag conditional statement so that method is only called once
            if (!calibrationVerified)
            {
                verifyCalibration();
                calibrationVerified = true;
            }
            timerText.text = "";
            instructionPanel.SetActive(false);
        }
    }

    // sets the startTime
    private void setStartTime(float timeToSet)
    {
        startTime = timeToSet;
    }

    /**
     * instantiates asteroid point gameobject at position of tracker when called, given name and tag from parameters
     * instantiated points are saved between scenes via DontDestroyOnLoad method
     */
    private void createPoint(GameObject point, string nameAndTag)
    {
        if(nameAndTag.Equals("Mid Point"))
        {
            point = Instantiate(midPointPrefab, trackerTransform.position, trackerTransform.rotation);
            point.name = nameAndTag;
            point.tag = nameAndTag;
            points.Add(point);
            DontDestroyOnLoad(point);
        }
        else
        {
            point = Instantiate(endPointPrefab, trackerTransform.position, trackerTransform.rotation);
            point.name = nameAndTag;
            point.tag = nameAndTag;
            points.Add(point);
            DontDestroyOnLoad(point);
        }
        
    }

    // sets the instruction text
    public void changeInstructionText(string instruction)
    {
        instructionText.text = instruction;
    }

    // sets whether the start and end point verification panel is active
    public void verifyStartAndEndPoint()
    {
        if (instructionPanel.activeSelf)
        {
            instructionPanel.SetActive(false);
            verificationPanel1.SetActive(true);
        }
        else
        {
            instructionPanel.SetActive(true);
            verificationPanel1.SetActive(false);
        }
        
    }

    // // sets whether the finish calibration verification panel is active
    public void verifyCalibration()
    {
        if (!verificationPanel2.activeSelf)
        {
            instructionPanel.SetActive(false);
            verificationPanel2.SetActive(true);
        }
        else
        {
            instructionPanel.SetActive(true);
            verificationPanel2.SetActive(false);
            inputPanel.SetActive(true);
        }
    }


    // destroys all instantiated points and reloads current scene
    public void restartScene()
    {
        for(int loop = 0; loop < points.Count; loop++)
        {
            Destroy(points[loop]);
        }
        SceneManager.LoadScene("ExerciseCreation");
    }

    /**
     * handles input fields and validation for exercise name and rep number
     * if validation is passed values are saved as PlayerPrefs to be accessed in the next scene
     */
    public void startExerciseButton()
    {
        string exerciseName = exerciseNameInput.text;
        string repText = repetitionInput.text;

        if (exerciseName.Length > 0 && repText.Length > 0)
        {
            int repNumber = int.Parse(repetitionInput.text);

            if (repNumber > 0 && repNumber <= 500)
            {
                PlayerPrefs.SetString("ExerciseName", exerciseName);
                PlayerPrefs.SetInt("RepetitionNumber", repNumber);
                SceneManager.LoadScene("ExerciseSession");
            }
            else if (repNumber < 0 || repNumber > 500)
            {
                errorText.text = "Repetition number must be in range 1-500";
            }
        }
        else if (exerciseName.Length == 0 && repetitionInput.text.Length == 0)
        {
            errorText.text = "Exercise name and repetition number cannot be null";
        }
        else if (exerciseName.Length > 0 && repetitionInput.text.Length == 0)
        {
            errorText.text = "Repetition number cannot be null";
        }
        else if (exerciseName.Length == 0 && repetitionInput.text.Length > 0)
        {
            errorText.text = "Exercise name cannot be null";
        }
    }

    // sets whether volume settings menu is active
    public void ChangeVolumeSettings()
    {
        if(volumeSettings.activeSelf)
        {
            volumeSettings.SetActive(false);
        }
        else
        {
            volumeSettings.SetActive(true);
        }
    }

    // loads main menu scene
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
