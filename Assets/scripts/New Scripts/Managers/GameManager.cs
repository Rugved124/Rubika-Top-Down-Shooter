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
        respawnPoint = FindObjectOfType<PC>().transform.position;
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
        currentState = GameStates.INMENU;
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {

        if(SceneManager.GetActiveScene().ToString() != "Main-Menu")
        {
            ChangeState(GameStates.RUNNING);
        }
        switch (currentState)
        {
            case GameStates.INMENU:
                Cursor.visible = true;
                break;
            case GameStates.RUNNING:
                Cursor.visible = false; 
                break;
            case GameStates.PAUSED:
                Cursor.visible = true;
                break;
        }
    }

    private void ChangeState(GameStates state) 
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
        respawnPoint = location;
    }

    private void SetPlayerData(int health, int ammo, string first, string second)
    {
        playerHealth = health;
        ammoCount = ammo;
        firstAmmo = first;
        secondAmmo = second;
    }
}
