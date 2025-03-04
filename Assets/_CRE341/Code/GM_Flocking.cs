using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class GM_Flocking : MonoBehaviour
{
    // Singleton instance
    public static GM_Flocking Instance { get; private set; }

    public List<GameObject> flockers = new List<GameObject>(); // List of all flocking agents
    public GameObject leader; // Reference to the leader

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    
}

