using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManagerEvents
{
    public static UnityAction<GameManager.GameStates> switchState;
    public static UnityAction<int> currentScene;
    public static UnityAction loadData;
    public static UnityAction<int> loadSavedScene;
}
