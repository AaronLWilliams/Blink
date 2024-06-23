using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public static void SaveBestTime(string levelName, float newTime)
    {
        SaveData data = LoadGame() ?? new SaveData();

        if (data.levels.ContainsKey(levelName))
        {
            if (newTime < data.levels[levelName].bestTime)
            {
                data.levels[levelName].bestTime = newTime;
            }
        }
        else
        {
            data.levels[levelName] = new LevelData { bestTime = newTime, isUnlocked = true };
        }

        SaveToFile(data);
    }

    public static void UnlockLevel(string levelName)
    {
        SaveData data = LoadGame() ?? new SaveData();

        if (data.levels.ContainsKey(levelName))
        {
            data.levels[levelName].isUnlocked = true;
        }
        else
        {
            data.levels[levelName] = new LevelData { bestTime = 900f, isUnlocked = true };
        }

        SaveToFile(data);
    }

    public static void SaveToFile(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    private static string path = Application.persistentDataPath + "/savefile.bin";

    public static SaveData LoadGame()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            // Create a new save data if the file does not exist
            SaveData newSaveData = new SaveData();
            SaveToFile(newSaveData);
            return newSaveData;
        }
    }

    public static bool SaveFileExists()
    {
        return File.Exists(path);
    }

    public static void ClearSaveData()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save data cleared.");
        }
    }
}
