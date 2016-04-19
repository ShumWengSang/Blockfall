using UnityEditor;
using UnityEngine;
using System.Collections;

public class DeleteMyPlayerPrefs : MonoBehaviour
{

    [MenuItem("Tools/DeletePlayerPrefs")]
    static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}