using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonkMovement : PlayerMovement
{

    protected override IEnumerator Attack()
    {
        yield return base.Attack();
        AudioSource characterAudio = gameObject.GetComponent<AudioSource>();
        if (characterAudio == null) yield break;
        characterAudio.PlayOneShot(characterAudio.clip);
    }
}
