using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public int ChangeDimension(){
        playerDimension = playerDimension == 0 ? 1 : 0;
        if(playerDimension == 0){
            background.color = originalDimensionColor;
        }
        else if(playerDimension == 1){
            background.color = alternateDimensionColor;
        }
        OnDimensionChange?.Invoke(playerDimension);
        return playerDimension;
    }
}
