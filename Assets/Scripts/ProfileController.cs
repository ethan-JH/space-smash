using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// controls the mechanics and objects of profile page
public class ProfileController : MonoBehaviour
{
    // game objects for the exercise history scroll view
    public ScrollRect exerciseScrollView;
    public GameObject scrollContent;
    public GameObject listItemPrefab;
    public GameObject exerciseHistory;

    // dynamic list to hold each list items
    private List<GameObject> listItems = new List<GameObject>();

    // current profile number which acts as unique id 
    private int currentProfileNumber;

    // number of exercises completed on this profile
    private int exercisesCompleted;

    // sets the current profile based on saved PlayerPrefs values
    public void SetProfile()
    {
        currentProfileNumber = PlayerPrefs.GetInt("CurrentProfileNumber");
        exercisesCompleted = PlayerPrefs.GetInt("ExercisesCompleted" + currentProfileNumber.ToString());
        //Debug.Log("Current Profile Number" + currentProfileNumber);
        //Debug.Log("Exercises Completed " + exercisesCompleted);

        // checks if any exercises have been completed, if not deactivates the exercise history
        if(exercisesCompleted == 0)
        {
            exerciseHistory.SetActive(false);
        }
        else
        {
            exerciseHistory.SetActive(true);

            // generates list items for the number of exercises completed
            for (int loop = 1; loop <= exercisesCompleted; loop++)
            {
                GenerateItem(loop);
            }

            // normalises scroll position to the top
            exerciseScrollView.verticalNormalizedPosition = 1;
        }
    }

    // generates list item prefab for exercise based on unique PlayerPrefs keys
    private void GenerateItem(int exerciseNumber)
    {
        string exerciseName = PlayerPrefs.GetString(currentProfileNumber.ToString() + "ExerciseName" + exerciseNumber.ToString());
        string exerciseReps = (PlayerPrefs.GetInt(currentProfileNumber.ToString() + "RepsCompleted" + exerciseNumber.ToString())).ToString();
        string exerciseScore = (PlayerPrefs.GetInt(currentProfileNumber.ToString() + "Score" + exerciseNumber.ToString())).ToString();
        string exerciseTimeTaken = PlayerPrefs.GetString(currentProfileNumber.ToString() + "TimeTaken" + exerciseNumber.ToString());

        GameObject scrollItemObject = Instantiate(listItemPrefab);
        scrollItemObject.transform.SetParent(scrollContent.transform, false);
        scrollItemObject.GetComponent<Exercise>().SetExercise(exerciseName, exerciseReps, exerciseScore, exerciseTimeTaken);
        listItems.Add(scrollItemObject);
    }

    // destroys all instantiated items in list
    public void DestroyListItems()
    {
        GameObject[] listItemsArray = listItems.ToArray();

        for (int loop = 0; loop < listItemsArray.Length; loop++)
        {
            GameObject.Destroy(listItemsArray[loop]);
        }
    }

    // destroys list items and loads exercise creation scene
    public void CreateNewExercise()
    {
        DestroyListItems();
        SceneManager.LoadScene("ExerciseCreation");
    }
}
