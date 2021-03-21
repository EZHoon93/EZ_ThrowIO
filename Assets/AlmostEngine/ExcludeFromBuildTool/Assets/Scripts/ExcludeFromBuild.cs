#if UNITY_EDITOR

// Script is not in Editor folder so it can be called from any script
// But is excluded from build

using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if UNITY_2018_1_OR_NEWER

using UnityEditor.Build;
using UnityEditor.Build.Reporting;

#elif UNITY_5_6_OR_NEWER

using UnityEditor.Build;

#endif

namespace AlmostEngine.ExcludeFromBuildTool
{

    /// Helper class that enables automatic exclusion of folders from builds
    [InitializeOnLoad]
    public static class ExcludeFromBuild
    {
        #region EXCLUSION METHODS

        static ExcludeFromBuildConfigAsset config = null;

        public static void Exclude(bool exclude)
        {
            // Get folders list
            if (!exclude)
            {
                config = GetConfigAsset();
            }
            else
            {
                config = GetConfigAsset(false);
            }
            if (config == null)
            {
                Debug.LogWarning("Impossible to restore excluded folders, ExcludeFromBuildConfigAsset not found.");
            }

            // Skip exclusion if disabled (but don't skip restore)
            if (!config.m_ExclusionFromBuildEnabled)
            {
                return;
            }

            if (exclude)
            {
                Debug.Log("Exclude from build: starting folders exclusion.");
            }
            else
            {
                Debug.Log("Exclude from build: starting folders restoration.");

            }

            // Track that the exclusion process started
            if (exclude)
            {
                config.m_ExclusionProcessStarted = true;
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            // Sort folders
            List<string> sortedFolders = new List<string>();
            if (exclude)
            {
                // To exclude we want deepest folder first
                sortedFolders = config.m_FoldersToExclude.OrderByDescending(x => x).ToList();
            }
            else
            {
                // To restore we want higher folder first
                sortedFolders = config.m_FoldersToExclude.OrderBy(x => x).ToList();
            }

            // Exclude or restore each folder
            foreach (var folder in sortedFolders)
            {
                string fullpath = Application.dataPath + "/../" + folder;
                ExcludeFolderFromProject(fullpath, exclude);
            }

            // Refresh asset database to take it into account
            AssetDatabase.Refresh();

            // Get new reference to config asset (first is lost after refresh)
            config = GetConfigAsset(false);

            // Track that the restoration is complete
            if (exclude == false)
            {
                config.m_ExclusionProcessStarted = false;
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        static void ExcludeFolderFromProject(string fullpath, bool exclude)
        {
            string parent = fullpath.Substring(0, fullpath.LastIndexOf("/") + 1);
            string folderName = fullpath.Substring(fullpath.LastIndexOf("/") + 1);
            var hiddenpath = parent + "." + folderName;
            if (exclude)
            {
                if (System.IO.Directory.Exists(fullpath))
                {
                    Debug.Log("Excluding folder: " + fullpath);
                    System.IO.File.Delete(fullpath + ".meta");
                    System.IO.Directory.Move(fullpath, hiddenpath);
                }
                else
                {
                    Debug.LogWarning("Could not exclude folder " + fullpath + ", folder not found.");
                }
            }
            else
            {
                if (System.IO.Directory.Exists(hiddenpath))
                {
                    Debug.Log("Restoring folder: " + fullpath);
                    System.IO.Directory.Move(hiddenpath, fullpath);
                }
                else
                {
                    Debug.LogWarning("Could not restore folder " + fullpath + ", folder not found.");
                }
            }
        }

        static ExcludeFromBuildConfigAsset GetConfigAsset(bool canCreate = true)
        {
            if (config != null)
                return config;
            if (canCreate)
            {
                config = AssetUtils.GetOrCreate<ExcludeFromBuildConfigAsset>("Assets/AlmostEngine/ExcludeFromBuildTool/Assets/Editor/ExcludeFromBuildConfigAsset.asset", "ExcludeFromBuildConfigAsset", "Assets/AlmostEngine/ExcludeFromBuildTool/Assets/Editor/");
            }
            else
            {
                config = AssetUtils.GetFirst<ExcludeFromBuildConfigAsset>();
            }
            return config;
        }

        #endregion

        #region PUBLIC MANAGEMENT OF FOLDERS

        public static bool IsEnabled()
        {
            return GetConfigAsset().m_ExclusionFromBuildEnabled;
        }

        public static void SetEnabled(bool enabled)
        {
            config = GetConfigAsset();
            config.m_ExclusionFromBuildEnabled = enabled;
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void AddToExcludedFolders(List<string> fullpaths)
        {
            foreach (var path in fullpaths)
            {
                AddToExcludedFolders(path);
            }
        }

        public static void RemoveFromExcludedFolders(List<string> fullpaths)
        {
            foreach (var path in fullpaths)
            {
                RemoveFromExcludedFolders(path);
            }
        }

        public static void AddToExcludedFolders(string fullpath)
        {
            config = GetConfigAsset();
            if (!config.m_FoldersToExclude.Contains(fullpath))
            {
                Debug.Log("Adding " + fullpath + " to excluded folders");
                config.m_FoldersToExclude.Add(fullpath);
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        public static void RemoveFromExcludedFolders(string fullpath)
        {
            config = GetConfigAsset();
            if (config.m_FoldersToExclude.Contains(fullpath))
            {
                Debug.Log("Removing " + fullpath + " from excluded folders");
                config.m_FoldersToExclude.Remove(fullpath);
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        #endregion

        #region AUTOMATIC RESTORATION LOGIC

        static ExcludeFromBuild()
        {
            // Automatically runned at startup and after each compilation
            // Detect failed restore when scripts are loaded
            config = GetConfigAsset(false);
            if (config == null)
            {
                return;
            }
            if (config.m_ExclusionProcessStarted)
            {
                Debug.Log("Detecting non restored folder exclusion, restoring excluded folders.");
                Exclude(false);
            }
        }

        public static void OnBuildError(string condition, string stacktrace, LogType type)
        {
            // Detect build error and restore
            if (type == LogType.Error)
            {
                Debug.Log("Detecting build error, restoring excluded folders.");
                // Stop listening to build error
                Application.logMessageReceived -= ExcludeFromBuild.OnBuildError;
                // Restore
                Exclude(false);
            }
        }

        #endregion
    }


    #region EXCLUDE FROM BUILD LOGIC
#if UNITY_2018_1_OR_NEWER
	class ExcludeFromBuildprocess : IPreprocessBuildWithReport , IPostprocessBuildWithReport
#elif UNITY_5_6_OR_NEWER
    class ExcludeFromBuildProcess : IPreprocessBuild, IPostprocessBuild
#endif
    {
        public int callbackOrder { get { return 0; } }

#if UNITY_2018_1_OR_NEWER
        public void OnPreprocessBuild(BuildReport report)
#elif UNITY_5_6_OR_NEWER
        public void OnPreprocessBuild(BuildTarget target, string path)
#endif
        {
            // Debug.Log("Exclude from build: build starting, starting folders exclusion.");
            // Start listening to build error
            Application.logMessageReceived += ExcludeFromBuild.OnBuildError;
            // Exclude folders when build starts
            ExcludeFromBuild.Exclude(true);
        }


#if UNITY_2018_1_OR_NEWER
		public void OnPostprocessBuild(BuildReport report)
#elif UNITY_5_6_OR_NEWER
        public void OnPostprocessBuild(BuildTarget target, string path)
#endif
        {
            // Debug.Log("Exclude from build: build complete, starting folders restoration.");
            // Stop listening to build error
            Application.logMessageReceived -= ExcludeFromBuild.OnBuildError;
            // Restore folders when build ends
            ExcludeFromBuild.Exclude(false);
        }

    }
    #endregion

}

#endif