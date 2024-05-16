/*
*
*   file:      level_swap_door_controller.cs
*   author:    Dennis Fuentes
*   date:      
*
*   This program is to be attcahed to the doors in the scene that will allow the main character to 
*   progress to the next level or scene. The script is to be attached to all door objects.
*   the script wil continiously check for collisions between the door collider and the main
*   main charecter collider. 
*   If a collision is detected, the current scene/ level is destroyed and the next one is loaded in.
*                       - SudoPowers.
*
*/

// Imports
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Entrance
public class level_swap_door_controller : MonoBehaviour
{
    Animator animator;
    Scene activeScene;
    AudioSource audioSource;
    // Add from inspector
    public AudioClip doorOpenSound;
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        Scene activeScene = SceneManager.GetActiveScene();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // This function is called when another collider enters the trigger collider attached to this GameObject
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collider that entered is tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // animator trigger to animator to plaay the door animation
            animator.SetTrigger("endLevel");

            // Play the door open sound
            audioSource.PlayOneShot(doorOpenSound);
            
            // Invoke the method to load the next scene after a delay
            Invoke("LoadNextScene", 1.05f); // Change the delay time as needed
        }
    }

    // Method to load the next scene will allow us to use invoke to let the animation play before the level is swapped
    private void LoadNextScene()
    {
        // Scene manager will close out all other scenes and progress forward to the next level / scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
    

}
