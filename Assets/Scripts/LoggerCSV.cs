using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class LoggerCSV : MonoBehaviour {

    public static LoggerCSV instance = null;

    private List<string[]> rows = new List<string[]>();


    private void Awake()
    {
        if (instance == null){
            instance = this;
            this.CreateTitles();
        }
        else if(instance != this){
            Destroy(this);
        }

    }


	public static LoggerCSV GetInstance()
	{
		return instance;
	}

    private void CreateTitles(){
        string[] titles = {"Time", "Event"};
        rows.Add(titles);
	}

    public void AddEventLog(float time, string event_log){
        string[] toAdd = { time.ToString(), event_log };
        rows.Add(toAdd);
    }

    public void PrintLogger(){
        for (int i = 0; i < rows.Count; i++){
            string[] r = rows[i];
            string toPrint = "";
            for (int j = 0; j < r.Length; j++){
                toPrint += r[j] + "    ";
            }
            Debug.Log("Row " + i.ToString() + ": " +toPrint );
        }
    }
    public void SaveCSV(){
        Debug.Log("Saving CSV");
        string[][] output = new string[rows.Count][];
        for (int i = 0; i < output.Length; i++){
            output[i] = rows[i];
        }
		int len = output.GetLength(0);
		string divider = ",";

		StringBuilder sb = new StringBuilder();

        for (int index = 0; index < len; index++)
            sb.AppendLine(string.Join(divider, output[index]));


		string filePath = getPath();

		StreamWriter outStream = System.IO.File.CreateText(filePath);
		outStream.WriteLine(sb);
		outStream.Close();
    }

	// Following method is used to retrive the relative path as device platform
	private string getPath(){
        #if UNITY_EDITOR
        		return Application.dataPath + "/CSV/" + "event_logger.csv";
        #else
                return Application.dataPath +"/"+"Saved_data.csv";
        #endif
	}
    // Update is called once per frame
    void Update () {
		
	}
}
