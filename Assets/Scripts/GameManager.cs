using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int playerDimension = 0;
    public Color originalDimensionColor;  // Background for original dimension
    public Color alternateDimensionColor;  // Background for alternate dimension
    public Camera mainCamera;  // Reference to the main camera
    public GameObject menu;
    public GameObject deathMenu;
    public GameObject winMenu;
    public GameObject PropsDim0;
    public GameObject PropsDim1;
    
    public static event Action<int> OnDimensionChange;
    // Start is called before the first frame update
    void Start()
    {
        playerDimension = 0;
        OnDimensionChange?.Invoke(playerDimension);
        mainCamera.backgroundColor = originalDimensionColor;
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
    public void ReturnToHomepage(){
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void GameOver(){
        
        deathMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void TryAgain(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
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

    public void EndGame(){
        winMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
