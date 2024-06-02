using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject bulletPrefab;
    void Start()
    {
        GameObject bullet = Instantiate(bulletPrefab, Vector3.zero, transform.rotation);
        Bullet bulletObj = bullet.GetComponent<Bullet>();

        bulletObj.SetVelocity(GetRandomDirection());
    }

    public Vector2 GetRandomDirection()
    {
        // Generate a random angle in radians
        float randomAngle = Random.Range(0f, Mathf.PI * 2);

        // Convert the angle to a direction vector
        float x = Mathf.Cos(randomAngle);
        float y = Mathf.Sin(randomAngle);

        return new Vector2(x, y).normalized;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
