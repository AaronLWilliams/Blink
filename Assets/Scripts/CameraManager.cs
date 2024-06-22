using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float levelWidth = 60.0f; // The width level
    public float levelHeight = 25.0f; // The height level
    void Start()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        // Get the aspect ratio
        float aspectRatio = (float)Screen.width / (float)Screen.height;

        // Calculate the orthographic size needed to fit the level width
        float orthographicSizeBasedOnWidth = levelWidth / (2 * aspectRatio);

        // Calculate the orthographic size needed to fit the level height
        float orthographicSizeBasedOnHeight = levelHeight / 2;

        // Set the camera's orthographic size to the larger of the two
        Camera.main.orthographicSize = Mathf.Max(orthographicSizeBasedOnWidth, orthographicSizeBasedOnHeight);
    }

    void OnValidate()
    {
        // Adjust the camera in the editor when values change
        AdjustCamera();
    }
}
