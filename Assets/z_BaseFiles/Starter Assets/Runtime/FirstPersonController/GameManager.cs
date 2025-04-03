using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep it across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    public GameObject npcPrefab;
    private Vector3 lastSpawnPosition = Vector3.zero;

    private bool isGameActive = true; // To track if the game is active
    public float timer;
    public Text survivalTimeText;
    private float survivalTime;
    public int minutes;
    public int seconds;
    private int finalMinutes;
    private int finalSeconds;
    private IEnumerator SpawnNPCCoroutine()
    {
        while (true)
        {
            SpawnNPC();
            yield return new WaitForSeconds(120); // Wait for 120 seconds
        }
    }

    void Start()
    {
        StartCoroutine(SpawnNPCCoroutine());
        survivalTime = 0f;
      
        GameObject.Find("ReloadMenu").SetActive(false);
    }

    private void SpawnNPC()
    {
        // If this is the first spawn, just spawn at a random position
        if (lastSpawnPosition == Vector3.zero)
        {
            lastSpawnPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            Instantiate(npcPrefab, lastSpawnPosition, Quaternion.identity);
            return;
        }

        // Calculate a new spawn position near the last NPC
        Vector3 newSpawnPosition = lastSpawnPosition + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));

        // Instantiate the new NPC
        Instantiate(npcPrefab, newSpawnPosition, Quaternion.identity);

        // Update the last spawn position
        lastSpawnPosition = newSpawnPosition;
    }


    public void Update()
     
    {
       
        {
            survivalTime += Time.deltaTime; // Update survival time

            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(survivalTime / 60);
            int seconds = Mathf.FloorToInt(survivalTime % 60);

            // Display the formatted time
            survivalTimeText.text = string.Format("Survival Time: {0:00}:{1:00}", minutes, seconds);


        }

        if (!isGameActive)
        {
            // Activate the menu or reload scene logic here
            ShowReloadMenu();
            finalMinutes = minutes;
            finalSeconds = seconds;
        }
    }



   

    public void PlayerDestroyed()
    {
        isGameActive = false; 
    }
    public void OnPlayerDeath()
    {

        GameObject.Find("finalSurvivalTimeText").GetComponent<Text>().text = string.Format("Final Survival Time: {0:00}:{1:00}", finalMinutes, finalSeconds);
            }
    private void ShowReloadMenu()
    {
        
        Debug.Log("Player is destroyed! Show reload menu.");
        GameObject.Find("ReloadMenu").SetActive(true);
        UpdateFinalSurvivalTime(GetFinalSeconds());
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    private int GetFinalSeconds()
    {
        return finalSeconds;
    }

    private void UpdateFinalSurvivalTime(int finalSeconds)
    {
        // Calculate minutes from the total seconds
        int finalMinutes = finalSeconds / 60; // Divide by 60 to get minutes

        // Calculate remaining seconds after extracting minutes
        finalSeconds %= 60; 

        // Find the Text component and update its text
        GameObject finalSurvivalTimeText = GameObject.Find("FinalSurvivalTimeText");
        if (finalSurvivalTimeText != null)
        {
            Text textComponent = finalSurvivalTimeText.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = string.Format("Final Survival Time: {0:00}:{1:00}", finalMinutes, finalSeconds);
            }
        }
    }


}


