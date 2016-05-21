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

        Transform grid = GameObject.Find("Grid").transform;
        if (grid == null)
            grid = GameObject.Find("Grid (1)").transform;
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
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            if (o != null)
            {
                if (!o.transform.IsChildOf(exception))
                    DestroyImmediate(o);
            }
        }
    }


}
