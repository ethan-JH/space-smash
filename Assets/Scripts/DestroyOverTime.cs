using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * script attached to particle effects which destroys them after a certain time
 */
public class DestroyOverTime : MonoBehaviour
{

    // lifetime of particle effect
    public float lifetime;

    // Update is called once per frame, destroys attached object after certain time
    void Update()
    {
        Destroy(gameObject, lifetime);
    }
}
