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
    public float spawnInterval = 120f; // Time in seconds to spawn NPCs
    private bool isGameActive = true; // To track if the game is active
    public float timer;
    public Text survivalTimeText;
    private float survivalTime;
    public int minutes;
    public int seconds;
    private int finalMinutes;
    private int finalSeconds;

    void Start()
    {
        timer = spawnInterval;
        survivalTime = 0f;
        StartCoroutine(SpawnNPCs());
        GameObject.Find("ReloadMenu").SetActive(false);
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

    private IEnumerator SpawnNPCs()
    {
        while (isGameActive)
        {
            SpawnNPC();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnNPC()
    {
        // Get a random position on the NavMesh
        Vector3 randomPosition = GetRandomNavMeshPosition();
        if (randomPosition != Vector3.zero)
        {
            Instantiate(npcPrefab, randomPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        // Generate a random point within the bounds of the NavMesh
        Vector3 randomPoint = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)); // Adjust range as needed
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position; 
        }
        return Vector3.zero; 
    }

    public void PlayerDestroyed()
    {
        isGameActive = false; 
    }
    public void OnPlayerDeath()
    {

        GameObject.Find("finalSurvivalTimeText").GetComponent<Text>().text = string.Format("Final Survival Time: {0:00}:{1:00}", finalMinutes, finalSeconds);
        // ... other code ...
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


