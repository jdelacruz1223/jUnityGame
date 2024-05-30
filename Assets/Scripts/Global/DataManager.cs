using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//A singleton class for storing persistent data between scenes.
public class DataManager : MonoBehaviour
{
    public static DataManager me;
    public GameObject lifeBar = null;
    public int maxLives = 5;
    public int lifeCount = 3;
    [SerializeField] public int BombsCollected = 0;
    public GameObject bossPrefab;
    public Vector3 bossSpawnPos;
    private bool isBossSpawned = false;
    public int destroyedDoors = 0;
    public bool isBossDead = false;


    void Start()
    {
    }
    void SpawnBoss()
    {
        Instantiate(bossPrefab, bossSpawnPos, Quaternion.identity);
        isBossSpawned = true;
        Debug.Log("Boss spawned!");
        
    }
    
    void Update()
    {
        if(destroyedDoors >= 3 && !isBossSpawned)
        {
            SpawnBoss();
        }

        if(lifeCount <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            lifeCount++;
        }

        if(isBossDead) Application.Quit();
        
    }


    void Awake()
    {
        if (me != null) //If an instance of this class already exists
        {
            Destroy(gameObject);  //Destroy this new instance
            return;// Exit the function
        }
        // end of new code

        me = this;  // Store this instance in a static variable
        DontDestroyOnLoad(gameObject);  //Do not destroy this object when the current scene ends and a new one begins
    }

}
