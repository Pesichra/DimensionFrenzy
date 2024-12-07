using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour, IInteractable
{
    private GameObject player;
    private bool isCarried = false;
    public void Interact(GameObject player)
    {
        this.player = player;
        transform.SetParent(player.transform);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        isCarried = true;
    }
    public void Drop(int dimension){
        isCarried = false;
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject props = dimension == 0 ? manager.PropsDim0 : manager.PropsDim1;
        transform.SetParent(props.transform);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    void Update(){
        if(isCarried){
            Vector3 movement = player.GetComponent<PlayerMovement>().movement;
            bool inverted = player.GetComponent<PlayerMovement>().lookRight;
            if (movement.x > 0)
            {
                transform.localPosition = new Vector3( inverted? 1 : -1, 0, 0);
            }
            else if (movement.x < 0)
            {
                transform.localPosition = new Vector3(inverted? -1 : 1, 0, 0);
            }
            else if (movement.y > 0)
            {
                transform.localPosition = new Vector3(0, 1, 0);
            }
            else if (movement.y < 0)
            {
                transform.localPosition = new Vector3(0, -1, 0);
            }
        }
    }
}
