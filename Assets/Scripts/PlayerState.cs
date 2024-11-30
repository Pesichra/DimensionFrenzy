using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    public Transform playerParent;
    public abstract IEnumerator Attack(); // Unique attack for each state
}

