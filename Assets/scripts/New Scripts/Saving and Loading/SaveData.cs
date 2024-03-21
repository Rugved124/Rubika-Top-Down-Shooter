using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int health;

    public float[] position;

    public string firstAmmo;
    public string secondAmmo;
    public int ammoCount;

    public int scene;


    public SaveData() 
    {
        health = GameManager.Instance.playerHealth;

        position = new float[3];
        position[0] = GameManager.Instance.respawnPoint.x;
        position[1] = GameManager.Instance.respawnPoint.y;
        position[2] = GameManager.Instance.respawnPoint.z;

        firstAmmo = GameManager.Instance.firstAmmo;
        secondAmmo = GameManager.Instance.secondAmmo;

        scene = GameManager.Instance.currentScene;
    }
}
