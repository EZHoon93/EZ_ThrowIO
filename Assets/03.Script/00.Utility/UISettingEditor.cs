using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CustomEditor(typeof(UISetting))]
public class UISettingEditor : MonoBehaviour
{

    //[MenuItem("Assets/Open UI Setting")]
    //public static void OpenInspector()
    //{
    //    Selection.activeObject = UISetting.Instance;
    //}

    /// <summary>
    /// 인스펙터 창에서 변경후 강제로 바꿔준다. 안해도 바꿔주긴하지만 바로바로 변경및 혹시모르는 오류방지를위해
    /// </summary>
    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //    if (GUI.changed)
    //    {
    //        EditorUtility.SetDirty(target);
    //        AssetDatabase.SaveAssets();
    //    }
    //}
}
