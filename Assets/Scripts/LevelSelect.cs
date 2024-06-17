using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    public GameObject levelButtonPrefab; // Prefab for the level buttons
    public Transform levelButtonContainer; // Container for the level buttons

    private int levelCount = 1;

    private SaveData saveData;

    void Start()
    {
        LoadSaveData();
        GenerateLevelButtons();
    }

    void LoadSaveData()
    {
        saveData = SaveSystem.LoadGame();
        if (saveData == null)
        {
            saveData = new SaveData();
            SaveSystem.SaveToFile(saveData);
        }
    }

    void GenerateLevelButtons()
    {
        foreach (var level in saveData.levels)
        {
            CreateLevelButton(level.Key, level.Value);
        }

        // Must manually change levels: Change to a dynamic way in the future
        for (int i = 1; i <= levelCount; i++)
        {
            string levelName = "Level" + i;
            if (!saveData.levels.ContainsKey(levelName))
            {
                CreateLevelButton(levelName, new LevelData { bestTime = 0f, isUnlocked = false });
            }
        }
    }

    void CreateLevelButton(string levelName, LevelData levelData)
    {
        GameObject buttonObject = Instantiate(levelButtonPrefab, levelButtonContainer);
        buttonObject.name = levelName;

        Button button = buttonObject.GetComponentInChildren<Button>();
        TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();

        string minutes = ((int)levelData.bestTime / 60).ToString("00");
        string seconds = (levelData.bestTime % 60).ToString("00");
        string milliseconds = ((int)((levelData.bestTime * 1000) % 1000)).ToString("000");

        buttonText.text = levelName + "\nBest Time: " + minutes + ":" + seconds + ":" + milliseconds;

        button.onClick.AddListener(delegate { LoadLevel(levelName); });

        button.interactable = levelData.isUnlocked; // Only enable button if the level is reached
    }

    void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}