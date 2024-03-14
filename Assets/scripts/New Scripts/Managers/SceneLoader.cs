using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    PC pc;

    [SerializeField]
    bool resetScene;
    private void Start()
    {
        pc = FindObjectOfType<PC>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if(other.tag == "Player")
            {
                if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
                {
                    ManagerEvents.currentScene.Invoke(SceneManager.GetActiveScene().buildIndex + 1);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {
                    Debug.Log("Game End");
                    Application.Quit();
                }
            } 
        }
    }
    public void ReloadScene()
    {
        if (resetScene)
        {
            //pc.gameObject.SetActive(true);
            //pc.Respawn();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}
