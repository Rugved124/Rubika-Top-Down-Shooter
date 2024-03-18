using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    PC pc;

    [SerializeField]
    bool resetScene;

    public Animator fadeToBlack;

    bool hasInvoked;
    private void Start()
    {
        hasInvoked = false;
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
                    if (fadeToBlack != null)
                    {
                        fadeToBlack.SetTrigger("LoadScene");
                    }
                    if (!hasInvoked)
                    {
                        hasInvoked = true;
                        Invoke("LoadNextScene", 1f);
                    }
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

    public void LoadNextScene()
    {
        hasInvoked = false;
        ManagerEvents.currentScene.Invoke(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
