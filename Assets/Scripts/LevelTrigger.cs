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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            CompleteMenu.SetActive(true);
            Time.timeScale = 0f;
            timerText.text = "Time: " + gameManager.timerText.text;
        }
    }

    
}
