using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AdvancedInspector;

public class SaveLoadExternalEditor : EditorWindow
{
    private ExternalEditor editor;

    private static SaveLoadBridge saveload = SaveLoadBridge.Instance;

    [MenuItem("Tools/Save Load Menu")]
    static void Init()
    {
        SaveLoadExternalEditor window = EditorWindow.GetWindow<SaveLoadExternalEditor>();
        window.wantsMouseMove = true;
        window.editor = ExternalEditor.CreateInstance<ExternalEditor>();

        window.editor.DraggableSeparator = false;
        window.editor.DivisionSeparator = 150;
        saveload.UpdateWorldLevel();
        
    }

    private void OnGUI()
    {
        GUILayout.Label("Puzzle saver and loader editor");
        if(editor == null)
        {
            Init();
        }
        editor.Instances = new object[] { saveload };
        if (editor.Draw(new Rect(0, 16, position.width, position.height - 16)))
            Repaint();
    }


}
