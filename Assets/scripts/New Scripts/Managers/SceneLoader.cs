using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    PC pc;

    //public static SceneLoader instance;
    public Animator fadeToBlack;

    bool hasInvoked;

    private void Awake()
    {
        //if(instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(this.gameObject);
        //}
    }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pc.transform.position = GameManager.Instance.respawnPoint;
    }

    public void LoadNextScene()
    {
        hasInvoked = false;
        ManagerEvents.currentScene.Invoke(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
        pc.transform.position = GameManager.Instance.respawnPoint;
    }
}
