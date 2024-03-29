using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
    public void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
