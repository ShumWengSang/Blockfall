#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using AdvancedInspector;
using UnityEditor.SceneManagement;
using UnityEditor;
public class _ChangeGameHolderInScene : MonoBehaviour {

    public GameObject prefab;
    [Inspect]
	// Use this for initialization
	void ChangePrefab () {
        GameObject grid = GameObject.Find("Grid");
        grid.transform.SetParent(null);
        DestroyImmediate(GameObject.Find("Game Holder"));
        Transform[] gridChild = new Transform[grid.transform.childCount];
        Transform[] copiedChild = new Transform[grid.transform.childCount];
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            gridChild[i] = grid.transform.GetChild(i);
        }
        for (int i = 0; i < gridChild.Length; i++)
        {
            gridChild[i].SetParent(null);
        }
        for (int i = 0; i < gridChild.Length; i++)
        {
            copiedChild[i] = Object.Instantiate(gridChild[i]);
        }
        DestroyImmediate(grid);


        PrefabUtility.InstantiatePrefab(prefab);
        Transform realGrid = GameObject.Find("RealScene").transform.FindChild("Grid");
        Transform fakeGrid = GameObject.Find("FakeScene").transform.FindChild("Grid");
        for (int i = 0; i < realGrid.childCount; i++ )
        {
            DestroyImmediate(realGrid.GetChild(i).gameObject);
        }
        for (int i = 0; i < fakeGrid.childCount; i++)
        {
            DestroyImmediate(fakeGrid.GetChild(i).gameObject);
        }


        for (int i = 0; i < gridChild.Length; i++)
        {
            gridChild[i].SetParent(realGrid);
        }

        for (int i = 0; i < copiedChild.Length; i++)
        {
            copiedChild[i].SetParent(fakeGrid);
        }
        GameObject.Find("FakeScene").SetActive(false);

    }
}
#endif