using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;  // Singleton instance
    private bool isGamePaused = false;   // Flag to check if the game is paused
    private bool isGameOver = false;     // Flag to check if the game is over
    private int playerScore = 0;         // Player's score

    public GameObject gameOverUI;        // Reference to the Game Over UI
    public GameObject pauseMenuUI;       // Reference to the Pause Menu UI

    void Awake()
    {
        // Ensure there's only one GameManager instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Prevent destruction between scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy any duplicate GameManagers
        }
    }

    void Start()
    {
        // Initialize game state
        isGamePaused = false;
        isGameOver = false;

        // Ensure UI elements are disabled at the start
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // When a new scene is loaded, find the UI elements again
        FindUIElements();
    }


    void Update()
    {
        // Check UI References if becomes null
        if (gameOverUI == null || pauseMenuUI == null) {

            // Find the UI Objects
            FindUIElements();

        }


        // Handle Pause/Unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGameOver)
            {
                if (isGamePaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
        // Restart the Game 
        else if (Input.GetKeyUp(KeyCode.R)) {

            RestartGame();
        
        }
    }

    // Function to restart the game
    public void RestartGame()
    {
        // Reload the active scene to restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Reset game over flag and score
        isGameOver = false;
        playerScore = 0;

        // Disable Game Over UI
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    // Function to pause the game
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);     // Enable pause menu UI
        Time.timeScale = 0f;             // Stop the time in-game
        isGamePaused = true;
    }

    // Function to resume the game from pause
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);    // Disable pause menu UI
        Time.timeScale = 1f;             // Resume the time in-game
        isGamePaused = false;
    }

    // Function to handle Game Over
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Time.timeScale = 0f;         // Stop the time
            gameOverUI.SetActive(true);  // Show Game Over UI
            Debug.Log("Game Over!");
        }
    }

    // Function to quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    // Function to add score
    public void AddScore(int points)
    {
        playerScore += points;
        Debug.Log("Score: " + playerScore);
    }

    // Getter for checking if the game is paused
    public bool IsGamePaused()
    {
        return isGamePaused;
    }

    // Getter for checking if the game is over
    public bool IsGameOver()
    {
        return isGameOver;
    }

    // Getter for getting the player's score
    public int GetPlayerScore()
    {
        return playerScore;
    }

    void FindUIElements() {
        pauseMenuUI = GameObject.Find("ScreenCanvas").transform.Find("PauseMenuPanel").gameObject;
        gameOverUI  = GameObject.Find("ScreenCanvas").transform.Find("GameOverPanel").gameObject;

    }
}
