using UnityEngine;
using System.Collections;
using AdvancedInspector;
using System.Collections.Generic;
using System.IO;
public class ReadAnswerFile : MonoBehaviour {

    public List<string> List_Answer = new List<string>();

    void OnEnable()
    {
        ScoreSystem.OnParsed += OnLevelParsed;
    }

    void OnDisable()
    {
        ScoreSystem.OnParsed -= OnLevelParsed;
    }
    void OnLevelParsed(int world, int level)
    {
        ReadFile(world, level);
    }


    void ReadFile(int world, int level)
    {
        int count = 0;
        string line;

        StreamReader sr = new StreamReader("Answers/Level" + world.ToString() + "-" + level.ToString() + ".txt");

        while ((line = sr.ReadLine()) != null)
        {
            if (line.Contains("Left"))
            {
                List_Answer.Add("Left");
            }
            else
            {
                List_Answer.Add("Right");
            }
            count++;
        }
        

        foreach(string direction in List_Answer)
        {
            Debug.Log(direction);
        }
    }	
}
