using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static int playerDimension = 0;
    public Color originalDimensionColor;  // Background for original dimension
    public Color alternateDimensionColor;  // Background for alternate dimension
    public Camera mainCamera;  // Reference to the main camera
    public GameObject menu;
    public static event Action<int> OnDimensionChange;
    // Start is called before the first frame update
    void Start()
    {
        playerDimension = 0;
        OnDimensionChange?.Invoke(playerDimension);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu(){
        menu.SetActive(!menu.activeSelf);
        Time.timeScale = menu.activeSelf ? 0 : 1;
    }

    public int ChangeDimension(){
        playerDimension = playerDimension == 0 ? 1 : 0;
        if(playerDimension == 0){
            mainCamera.backgroundColor = originalDimensionColor;
        }
        else if(playerDimension == 1){
            mainCamera.backgroundColor = alternateDimensionColor;
        }
        OnDimensionChange?.Invoke(playerDimension);
        return playerDimension;
    }
}
