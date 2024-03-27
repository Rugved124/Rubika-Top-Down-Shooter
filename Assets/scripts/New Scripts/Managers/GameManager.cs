using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentScene;

    public Vector3 respawnPoint;

    public int playerHealth;
    public string firstAmmo, secondAmmo;

    public int ammoCount;

    public Transform pcLocation;
    public Transform pcLocationInHell;

    private int canLoad;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this.gameObject);
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            FindObjectOfType<PC>().gameObject.transform.position = pcLocation.position;
            respawnPoint = pcLocation.position;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            FindObjectOfType<PC>().gameObject.transform.position = pcLocation.position;
            respawnPoint = pcLocationInHell.position;
        }
    }
    public enum GameStates
    {
        INMENU,
        RUNNING,
        PAUSED
    }

    public GameStates currentState;

    private void OnEnable()
    {
        ManagerEvents.switchState += ChangeState;
        ManagerEvents.currentScene += CurrentScene;
        ManagerEvents.respawnPoint += SetRespawn;
        ManagerEvents.playerData += SetPlayerData;
    }
    private void OnDisable()
    {
        ManagerEvents.switchState -= ChangeState;
        ManagerEvents.currentScene -= CurrentScene;
        ManagerEvents.respawnPoint -= SetRespawn;
        ManagerEvents.playerData -= SetPlayerData;

    }
    void Start()
    {
        currentScene = 1;
        //currentState = GameStates.INMENU;
        //DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            respawnPoint = pcLocation.position;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            respawnPoint = pcLocationInHell.position;
        }
        switch (currentState)
        {
            case GameStates.INMENU:
                Cursor.visible = true;
                break;
            case GameStates.RUNNING:
                Cursor.visible = false; 
                //InputManager.instance.gameObject.SetActive(true);
                break;
            case GameStates.PAUSED:
                Cursor.visible = true;
                //InputManager.instance.gameObject.SetActive(false);
                break;
        }
    }

    public void ChangeState(GameStates state) 
    {
        if(currentState != state)
        {
            currentState = state;
        }
    }

    private void CurrentScene(int sceneIndex)
    {
        currentScene =  sceneIndex;
    }

    private void SetRespawn(Vector3 location)
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            pcLocation.position = location;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            pcLocationInHell.position = location;
        }
    }

    private void SetPlayerData(int health, int ammo, string first, string second)
    {
        playerHealth = health;
        ammoCount = ammo;
        firstAmmo = first;
        secondAmmo = second;
    }

    public void ReloadScene()
    {
        ChangeState(GameStates.RUNNING);
        SetLoadData(2);
        SceneManager.LoadScene(SaveManager.LoadLevelIndex());

    }
    public void ChangeSpawnPoint()
    {
        respawnPoint = pcLocationInHell.position;
    }

    public bool CanLoadData()
    {
        return canLoad > 0;
    }

    public void SetLoadData(int yes)
    {
        canLoad = yes;
    }

    public void ReduceLoadCount()
    {
        canLoad--;
    }
}
