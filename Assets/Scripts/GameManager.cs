using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerMonk;
    public GameObject playerHood;
    static int playerDimension = 0;
    public Color originalDimensionColor;  // Background for original dimension
    public Color alternateDimensionColor;  // Background for alternate dimension
    public SpriteRenderer background;  // Reference to the main camera
    public static event Action<int> OnDimensionChange;
    // Start is called before the first frame update
    void Start()
    {
        OnDimensionChange?.Invoke(playerDimension);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDimension(){
        playerDimension = playerDimension == 0 ? 1 : 0;
        if(playerDimension == 0){
            background.color = originalDimensionColor;
            playerMonk.transform.position = playerHood.transform.position;
            playerMonk.transform.rotation = playerHood.transform.rotation;
            playerMonk.SetActive(true);
            playerHood.SetActive(false);
        }
        else if(playerDimension == 1){
            background.color = alternateDimensionColor;
            playerHood.transform.position = playerMonk.transform.position;
            playerHood.transform.rotation = playerMonk.transform.rotation;
            playerMonk.SetActive(false);
            playerHood.SetActive(true);
        }
        OnDimensionChange?.Invoke(playerDimension);
    }
}
