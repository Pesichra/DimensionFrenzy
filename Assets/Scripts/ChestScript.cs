using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour, IInteractable
{
    public Sprite chestClosed;
    public Sprite chestOpen;
    private bool isOpen;
    private GameObject player;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        isOpen = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = chestClosed;
    }
    public void Interact()
    {
        if (!isOpen)
        {
            player.GetComponent<PlayerMovement>().IncreaseMaxHealth(2);
            gameObject.GetComponent<SpriteRenderer>().sprite = chestOpen;
            isOpen = true;
        }
    }
}