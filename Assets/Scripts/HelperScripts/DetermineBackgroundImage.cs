#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using AdvancedInspector;
using UnityEngine.UI;
using System.Collections.Generic;

using UnityEditor.SceneManagement;
public class DetermineBackgroundImage : MonoBehaviour {
    public Sprite World1Bg;
    public Sprite World2Bg;
    public Sprite World3Bg;
    public Sprite World4Bg;

    [Inspect, Restrict("Restrict")]
    public int World;

    private IList Restrict()
    {
        return new List<object>() { 1, 2, 3, 4 };
    }

    void OnEnable()
    {
        ScoreSystem.OnParsed += OnParsed;
    }

    void OnDisable()
    {
        ScoreSystem.OnParsed -= OnParsed;
    }

    void OnParsed(int world, int level)
    {
        World = world;
        ChangeToBackground();
    }

    [Inspect]
    void ChangeToBackground()
    {
        
        RawImage thisImage = GetComponent<RawImage>();
        switch (World)
        {
            case 1:
                thisImage.texture = World1Bg.texture;
                break;
            case 2:
                thisImage.texture = World2Bg.texture;
                break;
            case 3:
                thisImage.texture = World3Bg.texture;
                break;
            case 4:
                thisImage.texture = World4Bg.texture;
                break;
        }
        if(!Application.isPlaying)
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

}
#endif