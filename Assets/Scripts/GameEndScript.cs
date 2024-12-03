using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndScript : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.EndGame();
        }
    }
}
