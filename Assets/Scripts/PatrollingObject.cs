/*
*
*   file:      PatrollingObject.cs
*   author:    Thomas Coffey
*   date:      05/09/2024
*
* Script to be attached to obstacles and enemy designed to follow a path
* Takes a start and end point and patrols between them facing the direction of the next targeted point
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingObject : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public float speed = .5f;
    public AudioClip flipSound;

    int motionPath = -1;

    // If this game object is a platform, please set this boolean to true... 
    // this will prevent the platform form flipping when it reaches it destination location.
    public bool platform;
    AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update() {
        Vector2 targetPosition = pointTarget();

        transform.position = Vector2.Lerp(transform.position, targetPosition, speed * Time.deltaTime);


        if ((targetPosition - (Vector2)transform.position).magnitude <= 1f) 
        {
            motionPath *= -1;
            if (platform == false)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                PlayFlipSound();
            }
        }
    }

    Vector2 pointTarget()
    {
        if (motionPath == -1) {
            return end.position;
        } 
        else 
        {
            return start.position;
        }
    }
    void PlayFlipSound()
    {
        if (audioSource != null && flipSound != null)
        {
            audioSource.PlayOneShot(flipSound);
        }
    }
}
