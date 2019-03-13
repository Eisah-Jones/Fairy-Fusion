
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WinnerPedastal))]
public class WinnerPedastalEditor : Editor
{
    public int numPlayers;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WinnerPedastal myScript = (WinnerPedastal)target;
        if (GUILayout.Button("Calculate Logs"))
        {
            myScript.CalculatePedastalPositions(myScript.pKills, myScript.num_Players);
        }

        if (GUILayout.Button("Reset Logs"))
        {
            myScript.Reset_Set_StartPositions();
        }

        if (GUILayout.Button("Move Logs"))
        {
            myScript.MovePlatforms();
        }

        if (GUILayout.Button("Set Win Text"))
        {
            myScript.SetWinText(myScript.pKills, myScript.num_Players);
        } 
        if (GUILayout.Button("Win Sequence"))
        {
            myScript.StartWinSequence(myScript.pKills, myScript.num_Players);
        }

    }
}
#endif
