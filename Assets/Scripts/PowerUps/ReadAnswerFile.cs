
using UnityEngine;
using System.Collections;
using AdvancedInspector;
using System.Collections.Generic;
using System.IO;
public class ReadAnswerFile : MonoBehaviour {

    public Stack<string> List_Answer = new Stack<string>();

    void OnEnable()
    {
        ReadFileManaul();
    }

    void OnDisable()
    {
        
    }
    void OnLevelParsed(int world, int level)
    {
        //ReadFile(world, level);
    }

    void ReadFileManaul()
    {
        int world = PlayerPrefs.GetInt("CurrentWorld");
        int level = PlayerPrefs.GetInt("CurrentLevel");
        ReadFile(world, level);
    }

    void ReadFile(int world, int level)
    {
        List_Answer.Clear();
        int count = 0;
        string line;
//#if UNITY_EDITOR_WIN
        string path = Application.streamingAssetsPath + "/Answers/Level" + world.ToString() + "-" + level.ToString() + ".txt";
        //#elif UNITY_ANDROID
        //string path = Application.streamingAssetsPath + "/Answers/Level" + world.ToString() + "-" + level.ToString() + ".txt";
        //string path = "jar:file://" + Application.dataPath + "!/assets/" + "/Answers/Level" + world.ToString() + "-" + level.ToString() + ".txt";
        //#endif
#if UNITY_EDITOR
        StreamReader sr = new StreamReader(path);

        while ((line = sr.ReadLine()) != null)
        {
            if (line.Contains("Left"))
            {
                List_Answer.Push("Left");
            }
            else
            {
                List_Answer.Push("Right");
            }
            count++;
        }
#elif UNITY_ANDROID
        StartCoroutine(readAnswerANDROID(path));
#endif
#if TESTING
        path = "file:///" + path;
        StartCoroutine(readAnswerANDROID(path));
#endif
    }

    IEnumerator readAnswerANDROID(string path)
    {
        path.Trim();
        WWW file = new WWW(path);
        yield return file;
        string[] lines;
        string line = file.text;
        if (file.text != null)
        {
            lines = file.text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Left"))
                {
                    List_Answer.Push("Left");
                }
                else if(lines[i].Contains("Right"))
                {
                    List_Answer.Push("Right");
                }
                Debug.Log("Line " + i + " is " + List_Answer.Peek());
            }
        }
    }
}
