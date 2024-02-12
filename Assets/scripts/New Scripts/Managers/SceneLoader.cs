using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCount)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                Debug.Log("Game End");
                Application.Quit();
            }
        }
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
