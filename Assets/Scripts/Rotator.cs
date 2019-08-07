using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attached to asteroids to rotate them
public class Rotator : MonoBehaviour
{
    // called every frame, rotates object at set rate
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
