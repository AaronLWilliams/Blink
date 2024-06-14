using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public GameObject CompleteMenu;
    public GameManager gameManager;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestTimeText;

    public float time;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            CompleteMenu.SetActive(true);
            Time.timeScale = 0f;
            //Output this time
            timerText.text = "Time: " + gameManager.timerText.text;
            //Check for new best time
            string levelName = SceneManager.GetActiveScene().name;
            SaveSystem.SaveBestTime(levelName, gameManager.time);
            //Output best time
            SaveData data = SaveSystem.LoadGame();
            if (data != null && data.levels.ContainsKey(levelName))
            {
                LevelData levelData = data.levels[levelName];
                time = levelData.bestTime;
                string minutes = ((int)time / 60).ToString("00");
                string seconds = (time % 60).ToString("00");
                string milliseconds = ((int)((time * 1000) % 1000)).ToString("000");

                bestTimeText.text = "Best Time: " + minutes + ":" + seconds + ":" + milliseconds;
            }
            else
            {
                Debug.Log("No data found for level: " + levelName);
            }
        }
    }

    
}
