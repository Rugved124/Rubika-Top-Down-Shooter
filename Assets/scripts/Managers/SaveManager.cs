using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using static AmmoManager;

public class SaveManager : MonoBehaviour
{

    public static SaveManager instance;
    private const string playerPositionKey = "PlayerLastCheckPoint";
    private const string equippedAmmoKey1 = "EquippedAmmo1";
    private const string equippedAmmoKey2 = "EquippedAmmo2";
    private const string equippedAmmoCountKey = "EquippedAmmoCount";
    private const string playerHealthKey = "PlayerHealth";
    private const string currentLevelKey = "currentLevelIndex";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    public static void SavePlayerStats(Vector3 position, int health, EquippedAmmoType equippedAmmo1, EquippedAmmoType equippedAmmo2, int ammoCount, int levelIndex)
    {
        PlayerPrefs.SetFloat(playerPositionKey + "_x", position.x);
        PlayerPrefs.SetFloat(playerPositionKey + "_y", position.y);
        PlayerPrefs.SetFloat(playerPositionKey + "_z", position.z);

        // Save equipped ammo1
        string equippedAmmo1Json = JsonUtility.ToJson(equippedAmmo1);
        PlayerPrefs.SetString(equippedAmmoKey1, equippedAmmo1Json);

        // Save equipped ammo2
        string equippedAmmo2Json = JsonUtility.ToJson(equippedAmmo2);
        PlayerPrefs.SetString(equippedAmmoKey2, equippedAmmo2Json);

        // Save player ammo count
        PlayerPrefs.SetInt(equippedAmmoCountKey, ammoCount);

        // Save player health
        PlayerPrefs.SetInt(playerHealthKey, health);
        PlayerPrefs.SetInt(currentLevelKey, levelIndex);
        PlayerPrefs.Save();
    }

    public static int LoadLevelIndex()
    {
        return PlayerPrefs.GetInt(currentLevelKey, 1);
    }
    // Call this method to load the player's position
    public static Vector3 LoadPlayerPosition()
    {
        float x = PlayerPrefs.GetFloat(playerPositionKey + "_x");
        float y = PlayerPrefs.GetFloat(playerPositionKey + "_y");
        float z = PlayerPrefs.GetFloat(playerPositionKey + "_z");

        return new Vector3(x, y, z);
    }

    // Call this method to load the ammo count
    public static int GetAmmoCount() 
    {
        return PlayerPrefs.GetInt(equippedAmmoCountKey, 20);
    }
    // Call this method to load the equipped ammo
    public static EquippedAmmoType LoadEquippedAmmo1()
    {
        string equippedAmmo1Json = PlayerPrefs.GetString(equippedAmmoKey1);

        if (!string.IsNullOrEmpty(equippedAmmo1Json))
        {
            return JsonUtility.FromJson<EquippedAmmoType>(equippedAmmo1Json);
        }
        else
        {
            // Return a default value if no equipped ammo is found
            return EquippedAmmoType.DEFAULTAMMO;
        }
    }

    public static EquippedAmmoType LoadEquippedAmmo2()
    {
        string equippedAmmo2Json = PlayerPrefs.GetString(equippedAmmoKey2);

        if (!string.IsNullOrEmpty(equippedAmmo2Json))
        {
            return JsonUtility.FromJson<EquippedAmmoType>(equippedAmmo2Json);
        }
        else
        {
            // Return a default value if no equipped ammo is found
            return EquippedAmmoType.DEFAULTAMMO;
        }
    }

    public static void DeleteSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        
    }
    public static bool GetHasPlayedBefore()
    {
        return string.IsNullOrEmpty(playerHealthKey);
    }
    // Call this method to load the player's health
    public static int LoadPlayerHealth()
    {
        return PlayerPrefs.GetInt(playerHealthKey, 100); // 100 is the default health if not found
    }
}
