/*
*
*   file:      MainMenuButtonScript.cs
*   author:    Dennis Fuentes
*   date:      
*
*   This program is to be be used to controll the buttons in the main menu
*   The goal of this script is to add functions to the main menu buttons to
*   allow a player to navigate the main menu.
*   By using global functions in this class we can allow the buttons to preform
*   a behavior when clicked/interacted with.
*
*
*   We would like the play button to load the first game scene.
*   We would like the settings button to bring forth some settings.
*   We would also like the quit button to exit the game when interacted with.
*
*                       - SudoPowers.
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuButtonScript : MonoBehaviour
{
     AudioSource audioSource;

    // clicks
    public AudioClip buttonClickSound;
    // hovering over selection
    public AudioClip buttonHoverSound;
    // music
    public AudioClip backgroundMusic;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBackgroundMusic();
    }

    // Play background music
    void PlayBackgroundMusic()
    {
        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    // Stop background music
    void StopBackgroundMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
    // When this method is called upon, the scene manager will load the next scene in the build. 
    // Assuming this scene is placed at index 0 this will progress us to scene at index 1.
   public void LoadGame()
   {
     StopBackgroundMusic();
     PlaySound(buttonClickSound);
     SceneManager.LoadScene("Level_1", LoadSceneMode.Single);
   }

    // When this method is called upon, the scene manager will quit us out of the Game
    public void ExitGame()
   {
     StopBackgroundMusic();
     PlaySound(buttonClickSound);
     Application.Quit();
   }
     // Play button click sound
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Play button hover sound
    public void PlayHoverSound()
    {
        PlaySound(buttonHoverSound);
    }
}
