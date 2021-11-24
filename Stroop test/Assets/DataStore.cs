using UnityEngine;

public class DataStore : MonoBehaviour
{
    public static string data_Difficulty;
    public static string data_Time;
    public static int data_Mistakes;
    
    public static SaveManager SM;

    public static void LoadData()
    {
        SM.LoadData();
    }

    public static void SaveData()
    {
        SM.SaveData();
    }
}
