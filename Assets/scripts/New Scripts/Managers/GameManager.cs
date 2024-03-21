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
        if(FindObjectOfType<PC>() != null)
        {
            respawnPoint = FindObjectOfType<PC>().transform.position;
            playerHealth = FindObjectOfType<PC>().maxHP;
        }
        firstAmmo = "DEFAULTAMMO";
        secondAmmo = "DEFAULTAMMO";
        ammoCount = 20;
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
    }
    private void OnDisable()
    {
        ManagerEvents.switchState -= ChangeState;
        ManagerEvents.currentScene -= CurrentScene;
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
    public void SaveData()
    {
        SaveSystem.SaveAllData();
    }
    public void LoadSaveFile()
    {
        if(SaveSystem.LoadData() != null)
        {
            SaveData data = SaveSystem.LoadData();
            playerHealth = data.health;
            ammoCount = data.ammoCount;
            firstAmmo = data.firstAmmo;
            secondAmmo = data.secondAmmo;

            respawnPoint.x = data.position[0];
            respawnPoint.y = data.position[1];
            respawnPoint.z = data.position[2];
            currentScene = data.scene;

            ManagerEvents.loadSavedScene?.Invoke(currentScene);
            if (SceneManager.GetActiveScene().buildIndex == currentScene)
            {
                ManagerEvents.loadData?.Invoke();
            }
        }
        else
        {
            ManagerEvents.loadSavedScene?.Invoke(SceneManager.GetActiveScene().buildIndex);
        }

    }
}
