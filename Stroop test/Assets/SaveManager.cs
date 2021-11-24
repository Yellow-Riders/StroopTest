using UnityEngine;
using System.IO;
using System;

public class SaveManager : MonoBehaviour
{
    System.Threading.Thread m_SaveThread = null;
    System.Threading.Thread m_LoadThread = null;

    string m_DataPath;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        m_SaveThread = new System.Threading.Thread(SaveThread);
        //print(Application.persistentDataPath);

        m_DataPath = Application.persistentDataPath + "/../StroopTestSaveFile.txt";

        m_LoadThread = new System.Threading.Thread(LoadData);
        m_LoadThread.Start();

        DataStore.SM = this;
        //Debug.Log(m_DataPath);
    }

    public void SaveData() //call this somwhere
    {
        if (m_SaveThread.IsAlive == false)
        {
            m_SaveThread.Start();
        }

        print(m_DataPath);
    }

    public void LoadData()
    {
        if (File.Exists(m_DataPath))
        {
            string saveString = File.ReadAllText(m_DataPath);

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            // //Load it somewhere else
            DataStore.data_Difficulty = saveObject.difficulty;
            DataStore.data_Time = saveObject.time;
            DataStore.data_Mistakes = saveObject.mistakes;
        }
        else
        {
            //default if no save file
            DataStore.data_Difficulty = "NONE";
            DataStore.data_Time = "no time";
            DataStore.data_Mistakes = -1;
        }

        m_LoadThread = new System.Threading.Thread(LoadData);
    }

    private void SaveThread()
    {
        SaveObject saveObject = new SaveObject //make a new object with the data to be saved
        {
            difficulty = DataStore.data_Difficulty,
            time = DataStore.data_Time,
            mistakes = DataStore.data_Mistakes,
        };

        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(m_DataPath, json); //Instead set the json to a player pref

        m_SaveThread = new System.Threading.Thread(SaveThread);
    }

    public bool GetLoadThread()
    {
        return m_LoadThread.IsAlive;
    }
}

[Serializable]
public struct SaveObject
{
    public string difficulty;
    public string time;
    public int mistakes;
}

// { "Difficulty", difficulty },
// { "Time Taken", time },
// { "Mistakes made", maxScore-score }

