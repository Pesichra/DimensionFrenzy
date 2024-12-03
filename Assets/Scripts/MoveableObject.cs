using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour, IInteractable
{
    private GameObject previousParent;
    public void Interact(GameObject player)
    {
        previousParent = transform.parent.gameObject;
        transform.SetParent(player.transform);
        transform.localPosition = player.GetComponent<PlayerMovement>().carryOffset;
    }
    public void Drop(){
        transform.SetParent(previousParent.transform);
    }
}
