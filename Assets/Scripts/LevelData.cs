using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public float bestTime;
    public bool isUnlocked;
}

[System.Serializable]
public class SaveData
{
    public Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();

    public SaveData()
    {
        // Ensure Level1 is always unlocked
        levels["Level1"] = new LevelData { bestTime = 999f, isUnlocked = true };
    }
}
