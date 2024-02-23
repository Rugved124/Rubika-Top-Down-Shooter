using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class StateMachineDebugger : MonoBehaviour
{
    public Enemy enemy { get; private set; }

    public string stateName;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        stateName = enemy.GetCurrentState();
    }
}


//[CustomEditor(typeof(StateMachineDebugger))]
//public class TextDrawer : Editor
//{

//    void OnSceneGUI()
//    {

//        StateMachineDebugger t = target as StateMachineDebugger;

//        GUIStyle style = new GUIStyle();

//        if (t.stateName == "Not Playing")
//        {
//            style.normal.textColor = Color.red;
//        }
//        else
//        {
//            style.normal.textColor = Color.green;
//        }

//        style.fontSize = 18;
//        style.fontStyle = FontStyle.Bold;


//        UnityEditor.Handles.Label(t.transform.position, t.stateName, style);
//    }
//}
