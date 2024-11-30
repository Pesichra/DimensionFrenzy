using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonkMovement : PlayerState
{

    public override IEnumerator Attack()
    {
        AudioSource characterAudio = gameObject.GetComponent<AudioSource>();
        if (characterAudio == null) yield break;
        characterAudio.PlayOneShot(characterAudio.clip);
    }
}
