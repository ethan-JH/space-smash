using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script attached to profile buttons which sets the profile number id for each created profile
public class Profile : MonoBehaviour
{
    private int profileNumber;

    // called on first frame update, defaults profile number to -1
    private void Start()
    {
        profileNumber = -1;
    }

    // sets the profile number
    public void SetProfileNumber(int num)
    {
        profileNumber = num;
    }

    // returns the profile number
    public int GetProfileNumber()
    {
        return profileNumber;
    }

}
