
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
        if (GUILayout.Button("SetPedastals"))
        {
            myScript.CalculatePedastalPositions(myScript.pKills, myScript.numPlayers);
        }
  
    }
}
#endif
