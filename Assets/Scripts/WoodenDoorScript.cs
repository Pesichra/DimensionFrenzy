using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WoodenDoorScript : MonoBehaviour, IInteractable
{
    public Sprite doorClosed;
    public Sprite doorOpen;
    public bool isOpen;
    public int lockedFront;
    public int lockedBack;
    public TextMeshProUGUI lockedText;
    void OnEnable()
    {
        if(!isOpen){

            gameObject.GetComponent<SpriteRenderer>().sprite = doorClosed;
            
        }
        else{
            gameObject.GetComponent<SpriteRenderer>().sprite = doorOpen;
        }
        lockedText.alpha = 0;
    }

    public bool Unlock(){
        bool unlocked = false;
        if(lockedFront > 0){
            lockedFront--;
            unlocked |= lockedFront == 0;
        }
        if(lockedBack > 0){
            lockedBack--;
            unlocked |= lockedBack == 0;
        }
        return unlocked;
    }
    public void Interact(GameObject player)
    {
        Vector3 directionToDoor = transform.position - player.transform.position;
        if (!isOpen)
        {
            if(lockedFront > 0 && (directionToDoor.y > 0)){
                StartCoroutine(animateLockedText());
                return;
            }else if(lockedBack > 0 && (directionToDoor.y < 0)){
                StartCoroutine(animateLockedText());
                return;
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = doorOpen;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            isOpen = true;
        }
    }

    IEnumerator animateLockedText(){
        lockedText.GetComponent<Animation>().Play();
        yield return null;
    }
}