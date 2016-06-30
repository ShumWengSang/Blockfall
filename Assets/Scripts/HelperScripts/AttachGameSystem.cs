#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using AdvancedInspector;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
public class AttachGameSystem : MonoBehaviour {
    public GameObject prefab;
    //We change this method as needed to do some heavy lifting across several scenes.
    [Inspect, Method(AdvancedInspector.MethodDisplay.Invoke)]
    void AttachObject()
    {

        GameObject go_grid = GameObject.Find("Grid");
        if (go_grid == null)
            go_grid = GameObject.Find("Grid (1)");
        if (go_grid == null)
            go_grid = GameObject.Find("Grid (2)");
        Transform grid = go_grid.transform;
        DeleteAll(grid);
        Transform[] childArray = new Transform[grid.childCount];
        for(int i = 0; i < grid.childCount; i++)
        {
            childArray[i] = grid.GetChild(i);
        }

        for(int i = 0; i < childArray.Length; i++)
        {
            childArray[i].parent = null;
        }
        DestroyImmediate(grid.gameObject);
        PrefabUtility.InstantiatePrefab(prefab);

        grid = GameObject.Find("Grid").transform;
        if(grid == null)
            grid = GameObject.Find("Grid (1)").transform;
        if (grid == null)
            grid = GameObject.Find("Grid (2)").transform;
        Transform[] childArray2 = new Transform[grid.childCount];
        for (int i = 0; i < grid.childCount; i++)
        {
            childArray2[i] = grid.GetChild(i);
        }
        foreach(Transform child in childArray2)
        {
            DestroyImmediate(child.gameObject);
        }


        for (int i = 0; i < childArray.Length; i++)
        {
            childArray[i].parent = grid;
        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        DestroyImmediate(this);
    }

    public void DeleteAll(Transform exception)
    {
        GameObject[] AllObjects = Object.FindObjectsOfType<GameObject>();
        for (int i = AllObjects.Length - 1; i >= 0; i --)
        {
            if (AllObjects[i] != null)
            {
                if (!AllObjects[i].transform.IsChildOf(exception) && !AllObjects[i].transform != exception)
                    DestroyImmediate(AllObjects[i]);
            }
        }
    }


}
#endif
