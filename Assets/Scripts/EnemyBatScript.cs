using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBatScript : EnemyMobScript
{
    private int rollHash = Animator.StringToHash("Roll");
    protected override void AggroTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction.y = 0;
        }
        else
        {
            direction.x = 0;
        }
        direction.z = 0;
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }

    protected override IEnumerator Attack()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        animator.CrossFade(rollHash, 0, 0);
        float attackDuration = 1.0f; // Duration of the attack in seconds
        float elapsedTime = 0f;

        // Determine the primary direction of the roll
        Vector3 rollDirection = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? new Vector3(direction.x, 0, 0) : new Vector3(0, direction.y, 0);

        while (elapsedTime < attackDuration)
        {
            GetComponent<Rigidbody2D>().linearVelocity = rollDirection * speed * 2;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
    }
}
