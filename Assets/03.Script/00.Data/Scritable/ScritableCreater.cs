
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CreaterScriptable))]
public class ScritableCreater : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CreaterScriptable createrScriptable = (CreaterScriptable)target;

        if (GUILayout.Button("Create Scritable"))
        {
            createrScriptable.Create();
        }
    }
}
#endif
