using UnityEngine;

public class CheckIfAudible : MonoBehaviour
{
	AudioListener audioListener;
	AudioSource audioSource;
	float distanceFromPlayer;

	void Start()
	{
		audioListener = Camera.main.GetComponent<AudioListener>();
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	void Update()
	{
		distanceFromPlayer = Vector3.Distance(transform.position, audioListener.transform.position);
		if (distanceFromPlayer <= audioSource.maxDistance)
		{
			ToggleAudioSource(true);
		}
		else
		{
			ToggleAudioSource(false);
		}
	}

	void ToggleAudioSource(bool isAudible)
	{
		if (!isAudible && audioSource.isPlaying)
		{
			audioSource.Pause();
		}
		else if (isAudible && !audioSource.isPlaying)
		{
			audioSource.Play();
		}
	}
}


