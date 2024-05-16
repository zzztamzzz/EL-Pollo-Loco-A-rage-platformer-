/*
*
*   file:      playerHealth.cs
*   author:    Dennis Fuentes (edit by Thomas Coffey for obstacle destruction)
*   date:      
*
*
*   This program is to be be used to controll the main charecters health.
*   The goal of this script is to monitor when a player makes contact with 
*   an obstical or enemy. If a collision is detected, one of the players lives
*   will be lost. In addition to this, I have implimented a health UI so the
*   number of lives is always visable to the player in the upper left corner.
* 
*   Next, I will impliment the effects of powerups
*   We have a couple types of power ups:
*   
*   Watermelon: Extra life is added to the players health bar
*   Strawberry: The player will be invincible /  unable to take damage for ** aamount of time
*   Blueberry: speed increase... this will be handles in the player movement script.
*                       - SudoPowers.
*
*/


using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PolloManController))] // certainty
public class Health : MonoBehaviour 
{   
    // The starting health value.
    [SerializeField] private static int startingHealth = 3;

    // the value of the current health
    public static int currentHealth;

    // array of sprites that contains the maximum number of lives
    public Image[] lives;
    public Sprite fullLife;
    public Sprite missingLife;

    // Controller reference
    PolloManController pManC;

    // Audio clips
    public AudioClip damageSound;
    public AudioClip powerUpSound;
    AudioSource audioSource;

    // boolean to track if a player is in an invincibility state
    private bool isInvincible = false; 
    // Duration of invincibility state in seconds
    private float invincibilityDuration = 4f;    
    // Timer to track remaining invincibility time
    private float invincibilityTimer = 0f;
    private void Awake()
    {
        pManC = GetComponent<PolloManController>();
        audioSource = GetComponent<AudioSource>();
        pManC.doubledMoveSpeed = 0f;

        // if the player passed a level with greater than 0 lives left
        if (startingHealth != 0) {
            // assign the strating health
            currentHealth = startingHealth;    
        }
        else 
        {
            // reset the health bar if we ran out of lives
            startingHealth = 3;
            // assign the current health
            currentHealth = startingHealth;
        }
        
    }
   
    // In every update, we should check the players health to see how many lives remain...
    // if we have no lives left we should retunr to the main menu
	void Update()
    {   
        // check if health has fallen below zero... if so we should return to the main menu
		if(currentHealth <= 0)
        {
		    ReloadSceneToStartingIndex();
        }
        // check if the health has exceeded the maximum number of health images in the UI
        // if this has happened we have exceed the max nunber of lives...
        if (currentHealth > lives.Length) 
        {
            // the current health should not exceed the max number of images we can present
            currentHealth =  lives.Length;
        }
        // repopulate the UI array accordingly
        for (int i = 0; i < lives.Length; i++) {
            // if current index is less than our current health
            if (i < currentHealth)
            {
                // set the image to the full life indicaator and enable it's presence
                lives[i].sprite = fullLife;
                //lives[i].enabled =  true;
            }
            else
            {
                // otherwise, the image to the missing life indicaator and disable it's presence
                lives[i].sprite = missingLife;
                //lives[i].enabled = false;
            }
        }

        // finally, update the invincibility timer if it is active
         if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
            }
        }

        startingHealth =  currentHealth;
    }


    // Method to restart game when the player runs out of health
	public void ReloadSceneToStartingIndex()
    {
        // reload the current scene that cause the players death. 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single); 
    }

    // This method will be used to detect collisions between the player and any obsticals / enemies
	 private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object the player hit is a damage dealer, also ensure player is not invincible
        if (collision.gameObject.CompareTag("DamageDealer")  && !isInvincible)
        {
            currentHealth = currentHealth - 1;
            // destroy obstacle after collision so as to not get caught.
            Destroy(collision.gameObject);
            PlaySound(damageSound);
        }
        else if (collision.gameObject.CompareTag("Watermelon"))
        {
            // add one to the players health
            currentHealth = currentHealth + 1;
            // destroy the water melon as it has been consumed.
            Destroy(collision.gameObject);
            PlaySound(powerUpSound);
        } 
        else if (collision.gameObject.CompareTag("Strawberry"))
        {
            // set the invincibility boolean to true as we should be invincible
            isInvincible = true;
            // set the invicnibility count down
            invincibilityTimer = invincibilityDuration;
            // destroy the water melon as it has been consumed.
            Destroy(collision.gameObject);
            PlaySound(powerUpSound);
        } 
        // Tamzid. Following along what Dennis and Thomas coded, upon consumption, the movement speed is doubled.
        else if (collision.gameObject.CompareTag("Blueberry")) 
        {
        pManC.doubledMoveSpeed = pManC.runSpeed * 2;
        // Set a timer to revert the movement speed back to normal after a certain duration
        StartCoroutine(ResetMovementSpeedAfterDelay(5f));
        Destroy(collision.gameObject);
        PlaySound(powerUpSound);
        }
    }
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    // Tamzid. Helper method for a timer. After *said* amount of time, the effect of powerup is gone
    // Default movement speed initially set in PolloManController.cs is activated.
    private IEnumerator ResetMovementSpeedAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        pManC.doubledMoveSpeed = 0f;
    }
}
