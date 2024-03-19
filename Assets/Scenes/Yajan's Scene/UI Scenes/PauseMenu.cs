using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public static bool GameIsPaused = false;
	public GameObject pausePanel;

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
		GameIsPaused = true;
	}


	public void Resume()
	{
		pausePanel.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

}
