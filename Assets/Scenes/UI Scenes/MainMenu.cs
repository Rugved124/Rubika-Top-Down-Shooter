using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void PlayGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		ManagerEvents.switchState.Invoke(GameManager.GameStates.RUNNING);
	}
	public void LoadGame()
	{
		//GameManager.Instance.LoadSaveFile();
	}
	public void QuitGame()
	{
		print("QUIT!!");
		Application.Quit();
	}
}
