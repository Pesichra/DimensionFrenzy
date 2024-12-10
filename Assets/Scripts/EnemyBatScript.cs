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
        if (Mathf.Abs(direction.x ) >= 0.5f)
        {
            return;
        }
        direction.x = 0;
        direction.z = 0;
        transform.position += direction * speed * Time.deltaTime;
	}

	protected override IEnumerator Attack()
	{
        Vector3 direction = (player.transform.position - transform.position).normalized;
        animator.CrossFade(rollHash, 0, 0);
        float attackDuration = 1.0f; // Duration of the attack in seconds
        float elapsedTime = 0f;

        while (elapsedTime < attackDuration)
        {
            transform.position += new Vector3(0, direction.y, 0) * speed * 2 * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
	}
}
