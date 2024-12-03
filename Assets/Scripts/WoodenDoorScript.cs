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
    public bool locked;
    public TextMeshProUGUI lockedText;
    void Awake()
    {
        if(!isOpen){

            gameObject.GetComponent<SpriteRenderer>().sprite = doorClosed;
            
        }
        else{
            gameObject.GetComponent<SpriteRenderer>().sprite = doorOpen;
        }
    }
    public void Interact(GameObject player)
    {
        Vector3 directionToDoor = transform.position - player.transform.position;
        if (!isOpen)
        {
            if(locked && (directionToDoor.y > 0)){
                StartCoroutine(animateLockedText());
                return;
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = doorOpen;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            isOpen = true;
        }
    }

    IEnumerator animateLockedText(){
        lockedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        lockedText.gameObject.SetActive(false);
    }
}