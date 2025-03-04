using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GM : MonoBehaviour
{
    // Singleton instance
    public static GM Inst { get; private set; }

    #region State machine for scene management
    public enum GameState
    {
        MainMenu,
        GameStart,
        Level_1,
        Level_2,
        Level_3,
        Paused,
        GameOver
    }
    public GameState currentState { get; private set; }
    #endregion

    private void Awake()
    {
        // Singleton pattern
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject); // Don't destroy on scene loads
        }
        else
        {
            Destroy(gameObject);
            return; // Stop further execution in Awake if destroyed
        }

        // Set initial game state 
        currentState = GameState.Level_1;
    }

    // Method to change the game state and load a new scene
    #region scene management methods
    public void ChangeState(GameState newState, string sceneName = "")
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.MainMenu:
                // Load the main menu scene if a name is provided
                if (!string.IsNullOrEmpty(sceneName))
                {
                    SceneManager.LoadScene(sceneName);
                }
                break;

            case GameState.GameStart:
                // You can have a separate loading scene or handle loading in the background
                break;

            case GameState.Level_1:
                // Load the scene if a name is provided
                if (!string.IsNullOrEmpty(sceneName))
                {
                    SceneManager.LoadScene(sceneName);
                }
                break;

            case GameState.Paused:
                // Handle pausing the game (e.g., Time.timeScale = 0;)
                Time.timeScale = 0;
                break;

            case GameState.GameOver:
                // Handle game over logic, potentially load a game over scene
                if (!string.IsNullOrEmpty(sceneName))
                {
                    SceneManager.LoadScene(sceneName);
                }
                break;

            default:
                Debug.LogError("Invalid game state specified!");
                break;
        }
    }

    // Example method to transition to the Gameplay state
    public void StartGame(string gameplaySceneName)
    {
        ChangeState(GameState.GameStart);
        StartCoroutine(LoadSceneAndSwitchState(gameplaySceneName, GameState.Level_1));
    }

    // Example method to pause the game
    public void PauseGame()
    {
        Time.timeScale = 0;
        // ChangeState(GameState.Paused);
    }

    // Example method to unpause the game
    public void UnpauseGame()
    {
        Time.timeScale = 1;
        // ChangeState(GameState.Level_1);
    }

    // Example method to go to main menu
    public void GoToMainMenu(string mainMenuSceneName)
    {
        ChangeState(GameState.MainMenu, mainMenuSceneName);
    }

    // Example method to handle game over
    public void GameOver(string gameOverSceneName)
    {
        ChangeState(GameState.GameOver, gameOverSceneName);
    }

    // Coroutine to load a scene asynchronously and then switch to a new state
    private IEnumerator LoadSceneAndSwitchState(string sceneName, GameState newState)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Switch to the new state
        ChangeState(newState);
    }
    #endregion
}