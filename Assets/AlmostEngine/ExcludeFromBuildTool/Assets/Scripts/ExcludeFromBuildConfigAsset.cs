#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections.Generic;

namespace AlmostEngine.ExcludeFromBuildTool
{
    [System.Serializable]
    public class ExcludeFromBuildConfigAsset : ScriptableObject
    {
        public bool m_ExclusionFromBuildEnabled = false;        

        public List<string> m_FoldersToExclude = new List<string>();
        
        [HideInInspector]
        // Track the exclusion process to detect a non restored exclusion and restore the folders
        public bool m_ExclusionProcessStarted = false;
    }
}

#endif