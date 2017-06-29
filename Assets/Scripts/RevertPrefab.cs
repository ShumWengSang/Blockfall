#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

public class RevertPrefab : MonoBehaviour {
    [ExecuteInEditMode]
    void Start()
    {
        PrefabUtility.RevertPrefabInstance(this.gameObject);
    }
}
#endif