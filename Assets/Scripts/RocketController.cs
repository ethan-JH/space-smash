using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script attached to rocket model on tracker which controls behaviour in exercise creation scene
public class RocketController : MonoBehaviour
{

    // number of points instantiated
    private int pointNumber;

    // game controller object
    private GameController gameController;

    // audio sources for game
    public AudioSource startSound;
    public AudioSource successSound;

    // boolean flags to check whether actions can be or have been performed
    private bool canStartTimer;
    private bool timerEnded;

    private bool canStartCalibration;
    private bool calibrationEnded;

    private bool playedSuccessSound;
    private bool finishedCalibration;

    // called on first frame update, initialises values
    private void Start()
    {
        pointNumber = 0;
        gameController = FindObjectOfType<GameController>();
        canStartTimer = false;
        timerEnded = false;
        canStartCalibration = false;
        calibrationEnded = false;
        playedSuccessSound = false;
        finishedCalibration = false;
    }

    // called on every frame update
    void Update()
    {   
        // when spacebar is pressed and less than two points have been instantiated, and cannot start calibration, verifies point creation
        if (Input.GetButtonDown("Jump") && pointNumber < 2 && !canStartCalibration)
        {
            gameController.checkPoints(pointNumber);
            pointNumber++;
        }
        else if (Input.GetButtonDown("Jump") && pointNumber == 2 && canStartTimer && !canStartCalibration) // when spacebar is pressed and start and end point created and cannot start calibration, timer is started
        {
            gameController.startTimingExercise(canStartTimer);
            startSound.Play();
        }
        else if (timerEnded && !canStartCalibration) // ends timer and plays success sound
        {
            gameController.endTimingExercise(timerEnded);
            if (!playedSuccessSound)
            {
                successSound.Play();
                playedSuccessSound = true;
            }
        }
        else if (Input.GetButtonDown("Jump") && pointNumber == 2 && canStartCalibration && !finishedCalibration) // when spacebar is pressed and can start calibration, starts calibration
        {
            // Debug.Log("calling calibration method");
            gameController.startCalibration(canStartCalibration);
            startSound.Play();
            playedSuccessSound = false;
            finishedCalibration = true;
        }
        else if (timerEnded && canStartCalibration && calibrationEnded) // when calibration is ended, ends calibration
        {
            gameController.endCalibration(calibrationEnded);
            if (!playedSuccessSound)
            {
                successSound.Play();
                playedSuccessSound = true;
            }
        }
    }

    // called when tracker collides with certain points that act as triggers 
    private void OnTriggerEnter(Collider other)
    {   
        // allows user to start timer when colliding with start point and points instantiated, changes instruction text
        if (other.gameObject.CompareTag("Start Point") && pointNumber == 2 && !timerEnded)
        {
            canStartTimer = true;
            gameController.changeInstructionText("The patient is in the starting position. Upon pressing the SPACE BAR, move the patient in as slow and controlled a manner " +
                "as is possible to the end point. A timer will measure the exercise time and automatically stop at the end point." +
                " If the exercise time is greater than 30 seconds calibration will be restarted.");
        }
        else if (other.gameObject.CompareTag("Start Point") && pointNumber == 2 && timerEnded) // allows user to start calibration when colliding with start point and timer has been ended, changes instruction text
        {
            gameController.changeInstructionText("The patient has returned to the starting position. To accurately calibrate the exercise, repeat the same movement as previously," +
                " as closely replicating your speed and movement as possible. Press the SPACE BAR to begin." +
                " If the exercise time is greater than 30 seconds calibration will be restarted.");
            canStartCalibration = true;
            // Debug.Log(canStartCalibration.ToString());
        }

        if (other.gameObject.CompareTag("End Point") && canStartTimer && !timerEnded) // ends timing and changes instruction text when colliding with end point
        {
            timerEnded = true;
            gameController.changeInstructionText("Exercise recorded. Return to the start position.");
        }
        else if (other.gameObject.CompareTag("End Point") && canStartTimer && timerEnded && canStartCalibration) // ends calibration and changes instruction text when colliding with end point 
        {
            calibrationEnded = true;
            // Debug.Log(calibrationEnded.ToString());
        }
    }
}
