using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoodMovement : PlayerState

{
    public GameObject arrowPrefab;
    public float arrowSpeed;
    public override IEnumerator Attack()
    {
        AudioSource characterAudio = gameObject.GetComponent<AudioSource>();
        if (characterAudio == null) yield break;
        characterAudio.PlayOneShot(characterAudio.clip);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the arrow is instantiated on the same plane

        Vector3 playerPosition = playerParent.GetComponent<PlayerMovement>().playerPosition;
        Vector3 direction = (mousePosition - playerPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject arrow = Instantiate(arrowPrefab, playerPosition, Quaternion.Euler(new Vector3(0, 180, -angle)));
        arrow.GetComponent<Rigidbody2D>().velocity = direction * arrowSpeed; // Assuming arrowSpeed is defined
        Destroy(arrow, 5f); // Destroy the arrow after 5 seconds to ensure it doesn't stay in the scene forever
        yield return null;
    }

}
