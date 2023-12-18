using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ScriptSearcher : EditorWindow
{
    private MonoScript scriptToSearch;
    private List<string> searchResults = new List<string>();
    private int selectedIndex = -1; // -1 indicates no selection

    [MenuItem("Window/ScriptSearcher")]
    public static void ShowWindow()
    {
        GetWindow<ScriptSearcher>("Script Searcher");
    }

    void OnGUI()
    {
        GUILayout.Label("Search for Script in Hierarchy", EditorStyles.boldLabel);

        scriptToSearch = EditorGUILayout.ObjectField("Script", scriptToSearch, typeof(MonoScript), false) as MonoScript;

        if (GUILayout.Button("Search"))
        {
            SearchForScriptInHierarchy();
        }

        if (GUILayout.Button("Clear"))
        {
            ClearSearchResults();
        }

        EditorGUI.BeginDisabledGroup(selectedIndex == -1); // Disable if no selection
        if (GUILayout.Button("Copy Name"))
        {
            EditorGUIUtility.systemCopyBuffer = searchResults[selectedIndex];
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Label("Search Results:");
        for (int i = 0; i < searchResults.Count; i++)
        {
            var style = (i == selectedIndex) ? EditorStyles.boldLabel : EditorStyles.label;
            if (GUILayout.Button(searchResults[i], style))
            {
                selectedIndex = i;
            }
        }
    }

    void SearchForScriptInHierarchy()
    {
        searchResults.Clear();
        selectedIndex = -1; // Reset selection
        if (scriptToSearch != null)
        {
            var scriptType = scriptToSearch.GetClass();
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.GetComponent(scriptType) != null)
                {
                    searchResults.Add(obj.name);
                }
            }
        }
    }

    void ClearSearchResults()
    {
        searchResults.Clear();
        selectedIndex = -1; // Reset selection
        Repaint();
    }
}
