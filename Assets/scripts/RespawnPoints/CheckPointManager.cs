using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
	private static Vector3 spawnLocation;

	// Set the spawn location when player dies
	public static void SetSpawnLocation(Vector3 location)
	{
		spawnLocation = location;
	}
	// Get the spawn location
	public static Vector3 GetSpawnLocation()
	{
		return spawnLocation;
	}

	// Call this method to respawn the player at the spawn point
	public static void RespawnPlayer(GameObject player)
	{
		player.transform.position = spawnLocation;
	}

	// Optional: Draw a gizmo to visualize the spawn point in the editor
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, 0.5f);
	}
}
