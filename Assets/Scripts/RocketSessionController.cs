using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script attached to rocket model on tracker which controls behaviour in exercise session scene
public class RocketSessionController : MonoBehaviour
{
    // flag to check if end point has been reached
    private bool endPointReached;

    // sound effects for scene
    public AudioSource redAsteroidSound;
    public AudioSource greenAsteroidSound;
    public AudioSource successSound;
    public AudioSource victorySound;

    // particle systems to add effects to pick ups
    public GameObject redPickUpEffect;
    public GameObject greenPickUpEffect;
    public GameObject victoryPickUpEffect;

    // session controller
    private SessionController sessionController;

    // Start is called before the first frame update, initialises variables
    void Start()
    {
        endPointReached = false;

        sessionController = FindObjectOfType<SessionController>();
    }

    // called when tracker collides with certain points that act as triggers
    private void OnTriggerEnter(Collider other)
    {
        // if end point is reached from start point, resets mid points, plays sound, increases player score and instantiates particle effect
        if(other.CompareTag("End Point") && !endPointReached)
        {
            endPointReached = true;
            sessionController.ActivateMidPoints(true);
            sessionController.AddPlayerScore(20);
            redAsteroidSound.Play();
            Instantiate(redPickUpEffect, transform.position, transform.rotation);
        }
        if(other.CompareTag("Mid Point")) // when mid point is collided with, adds player score, plays sound and instantiates particle effect
        {
            other.gameObject.SetActive(false);
            sessionController.AddPlayerScore(10);
            greenAsteroidSound.Play();
            Instantiate(greenPickUpEffect, transform.position, transform.rotation);
        }
        if(other.CompareTag("Start Point") && endPointReached) // if start point is reached from end point, resets mid points, plays sound, increases player score, instantiates particle effect and increases rep number
        {
            endPointReached = false;
            sessionController.ActivateMidPoints(true);
            sessionController.AddPlayerScore(20);
            sessionController.AddRepetition();
            successSound.Play();
            Instantiate(redPickUpEffect, transform.position, transform.rotation);
        }
    }

    // plays victory sound and instantiates particle system
    public void playVictory()
    {
        victorySound.Play();
        Instantiate(victoryPickUpEffect, transform.position, transform.rotation);
    }
}
