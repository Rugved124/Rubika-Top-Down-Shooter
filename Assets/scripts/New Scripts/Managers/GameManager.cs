using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
    }
    private void OnDisable()
    {
        ManagerEvents.switchState -= ChangeState;
    }
    void Start()
    {
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
}
