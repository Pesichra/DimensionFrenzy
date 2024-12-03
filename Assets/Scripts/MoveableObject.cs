using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour, IInteractable
{
    public void Interact(GameObject player)
    {
        transform.SetParent(player.transform);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.localPosition = player.GetComponent<PlayerMovement>().carryOffset;
    }
    public void Drop(int dimension){
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject props = dimension == 0 ? manager.PropsDim0 : manager.PropsDim1;
        transform.SetParent(props.transform);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
