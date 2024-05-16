/*
*
*   file:      PauseMenu.cs
*   author:    Dennis Fuentes
*   date:      
*
*   Escape is the key to pause the game
*
*   This program is to be be used to controll the pause menu.
*   The goal of this script is to monitor the escape key to watch 
*   for the pause signal. When the Escape Key is pressed...
*   The used will be allowed to navigate the pause menu.
*   This script will allow the button is the pause menu to 
*   function as well. The quit key will allow the user to
*   exit the game while the resume key will allow the user
*   to resume playing the game.
*  
*
*                       - SudoPowers.
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // public references to the pause menu in order to set it active when the pause key is pressed.
    public GameObject pauseMenu;
    
    // bool to 
    public static bool isPaused;
    // Audio
    AudioSource audioSource;
    public AudioClip pauseSound;
    public AudioClip resumeSound;
    public AudioClip buttonHoverSound;
    public AudioClip buttonClickSound;
    public AudioClip pauseMenuMusic;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // this will ensure the pause menu is inactive when the scene is loaded. 
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            // if the is pause variable is set to true 
            if(isPaused)
            {
                Game_Resume();
            }
            else
            {
                Game_Pause();
            }
        }
    }

    public void Game_Pause()
    {
        // bring forth the pause menu
        pauseMenu.SetActive(true);
        // set the global time scale to 0.0 to stop the game clock.
        Time.timeScale = 0.0f;
        // update the global variable as the game is now paused.
        isPaused = true;
        // Play the pause sound
        PlaySound(pauseSound);
        // Play background music
        PlayBackgroundMusic();
    }

    public void Game_Resume()
    {
        // remove the pause menu
        pauseMenu.SetActive(false);
        // set the global time scale to 1.0 to resume the game clock.
        Time.timeScale = 1.0f;
        // update the global variable as the game is no longer paused 
        isPaused = false;
        // Play the resume sound
        PlaySound(resumeSound);
        // Stop background music
        StopBackgroundMusic();
    }

    // When this method is called upon, the scene manager will quit us out of the Game
    public void Game_Exit()
   {
        PlaySound(buttonClickSound); 
        Application.Quit();
   }
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Play background music
    void PlayBackgroundMusic()
    {
        if (audioSource != null && pauseMenuMusic != null)
        {
            audioSource.clip = pauseMenuMusic;
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

    // Play button hover sound
    public void PlayHoverSound()
    {
        PlaySound(buttonHoverSound);
    }
}