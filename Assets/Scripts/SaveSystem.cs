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
            data.levels[levelName] = new LevelData { bestTime = 999f, isUnlocked = true };
        }

        SaveToFile(data);
    }

    private static void SaveToFile(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.bin";
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
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
