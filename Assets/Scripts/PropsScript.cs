using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsScript : MonoBehaviour
{
    public int dimension;
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.OnDimensionChange += HandleDimensionChange;
    }

    void HandleDimensionChange(int newDimension)
    {
        if (newDimension == dimension)
        {
            gameObject.SetActive(true);
        }else{
            gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        GameManager.OnDimensionChange -= HandleDimensionChange;
    }
}