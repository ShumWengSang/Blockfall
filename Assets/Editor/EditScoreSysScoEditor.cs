using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AdvancedInspector;


public class EditScoreSysScoEditor : EditorWindow
{
    private ExternalEditor editor;
    private static ScoreSysScript score = ScoreSysScript.Instance;

    [MenuItem("Tools/Scoring Editor")]
    static void Init()
    {
        EditScoreSysScoEditor window = EditorWindow.GetWindow<EditScoreSysScoEditor>();
        window.wantsMouseMove = true;
        window.editor = ExternalEditor.CreateInstance<ExternalEditor>();
        
        window.editor.DraggableSeparator = false;
        window.editor.DivisionSeparator = 150;

    }

    private void OnGUI()
    {
        GUILayout.Label("Puzzle Score System saver and loader editor");
        if (editor == null)
        {
            Init();
        }
        editor.Instances = new object[] { score };
        if (editor.Draw(new Rect(0, 16, position.width, position.height - 16)))
            Repaint();
    }
}