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
            if(other.tag == "Player")
            {
                if (SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex + 1) == null)
                {
                    Application.Quit();
                }
                    if (SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex + 1) != null)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }
    }
}
