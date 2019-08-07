using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// class attached to list item prefab to hold exercise information
public class Exercise : MonoBehaviour
{
    // exercise information
    private string ExerciseName, ExerciseReps, ExerciseScore, ExerciseTimeTaken;

    // text that is outputted in the list object
    public Text exerciseNameText, exerciseRepsText, exerciseScoreText, exerciseTimeTakenText;

    // sets the exercise information and the outputted text of the list item
    public void SetExercise(string exerciseName, string exerciseReps, string exerciseScore, string exerciseTimeTaken)
    {
        ExerciseName = exerciseName;
        ExerciseReps = exerciseReps;
        ExerciseScore = exerciseScore;
        ExerciseTimeTaken = exerciseTimeTaken;

        exerciseNameText.text = " " + exerciseName;
        exerciseRepsText.text = exerciseReps;
        exerciseScoreText.text = exerciseScore;
        exerciseTimeTakenText.text = exerciseTimeTaken; 
    }
}
