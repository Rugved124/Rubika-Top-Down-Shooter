using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject continueButton;

	[SerializeField]
	bool hasPlayedBefore;

    private void Awake()
    {
    }
    private void Update()
    {
        if (SaveManager.LoadPlayerPosition() != Vector3.zero)
        {
            continueButton.SetActive(true);
        }
        if (SaveManager.LoadPlayerPosition() == Vector3.zero)
		{
            continueButton.SetActive(false);
        }
    }
    public void PlayGame()
	{
		SaveManager.DeleteSaveData();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		ManagerEvents.switchState.Invoke(GameManager.GameStates.RUNNING);
	}
	public void LoadGame()
	{
		GameManager.Instance.ReloadScene();
	}
	public void QuitGame()
	{
		print("QUIT!!");
		Application.Quit();
	}

	public void DeleteSaveData()
	{
		SaveManager.DeleteSaveData();
	}
}
