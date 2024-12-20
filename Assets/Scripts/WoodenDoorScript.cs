using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class WoodenDoorScript : MonoBehaviour, IInteractable
{
    public Sprite doorClosed;
    public Sprite doorOpen;
    public bool isOpen;
    public int lockedFront;
    public int lockedBack;
    private (int front, int back) initialState;
    public TextMeshProUGUI lockedText;

    void OnEnable()
    {
        initialState = (lockedFront, lockedBack);
        if (!isOpen)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = doorClosed;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = doorOpen;
        }
        lockedText.alpha = 0;
    }

    public bool Unlock()
    {
        bool unlocked = false;
        lockedFront--;
        lockedBack--;
        unlocked |= lockedFront == 0;
    
        unlocked |= lockedBack == 0;
        if(unlocked){
            gameObject.GetComponent<SpriteRenderer>().sprite = doorOpen;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            isOpen = true;
        }
        return unlocked;
    }
    public void Lock(){
        bool locked = false;

        lockedFront = Math.Min(lockedFront + 1, initialState.front);
        lockedBack = Math.Min(lockedBack + 1, initialState.back);
        locked |= lockedFront >= 1 || lockedBack >= 1;
        
        if(locked && isOpen){
            gameObject.GetComponent<SpriteRenderer>().sprite = doorClosed;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            isOpen = false;
        }
    }

    public void Interact(GameObject player)
    {
        Vector3 directionToDoor = transform.position - player.transform.position;
        if (!isOpen)
        {
            if (lockedFront > 0 && (directionToDoor.y > 0))
            {
                StartCoroutine(animateLockedText());
                return;
            }
            else if (lockedBack > 0 && (directionToDoor.y < 0))
            {
                StartCoroutine(animateLockedText());
                return;
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = doorOpen;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            isOpen = true;
        }
    }

    IEnumerator animateLockedText()
    {
        lockedText.GetComponent<Animation>().Play();
        yield return null;
    }
}