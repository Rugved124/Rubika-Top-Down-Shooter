using UnityEngine;
using static GameManager;

public class PauseMenu : MonoBehaviour
{
	public static bool GameIsPaused = false;
	public GameObject pausePanel;
	public SceneLoader sceneLoader;
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameIsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}
	void Pause()
	{
		pausePanel.SetActive(true);
		Time.timeScale = 0f;
		GameManager.Instance.ChangeState(GameManager.GameStates.PAUSED);
		GameIsPaused = true;
	}
	public void ReloadCheckPoint()
	{
		Time.timeScale = 1f;
		GameManager.Instance.ChangeState(GameStates.RUNNING);
        GameManager.Instance.ReloadScene();
	}
	public void Resume()
	{
		GameManager.Instance.ChangeState(GameManager.GameStates.RUNNING);
		pausePanel.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}
	public void MainMenu()
	{
        GameManager.Instance.ChangeState(GameStates.INMENU);
        Time.timeScale = 1;
		sceneLoader.LoadScene(0);
	}
}
