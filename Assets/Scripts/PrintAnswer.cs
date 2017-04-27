using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PrintAnswer : MonoBehaviour {

	public List<string> movesMadeList = new List<string>();

	public void AddMoveToList(string move)
	{
		movesMadeList.Add(move);
	}

	public void WriteToFile(string filename)
	{
		using (StreamWriter sw = new StreamWriter(filename)) {
			for (int i = 0; i < movesMadeList.Count; i++) {			
				if (i < 10)
					sw.Write("0"); //Add a 0 infront of single digit to make it easier to read
				sw.Write(i + " ");
				sw.WriteLine (movesMadeList[i]);
			}
		}
	}

	public int GetNumberOfMoves(string filename)
	{
        int count = 0;


		StreamReader sr = new StreamReader("Answers/" + filename);

        while ((sr.ReadLine()) != null)
        {
            count++;
        }

        sr.Close();

        return count;
	}
	/*
	// Use this for initialization
	void Start () {
		movesMadeList.Add("AnswersMeow");
		WriteToFile ("Answers/NEWINIT.txt");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) {
			movesMadeList.Add("Pot");
			WriteToFile ("New.txt");
		}
	}
	*/
}
