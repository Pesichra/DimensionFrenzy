using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMushroomScript : EnemyMobScript
{
    

    protected override IEnumerator Attack(){
        animator.SetTrigger("Attack");
        busy = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        busy = false;
    }

    protected override void AggroTowardsPlayer(){
        bool lookright = (player.GetComponent<PlayerMovement>().playerPosition - transform.position).normalized.x > 0;
        transform.rotation = Quaternion.Euler(0, lookright ? 0 : 180, 0);
        
        Vector2 moveDirection = (player.GetComponent<PlayerMovement>().playerPosition - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = moveDirection * speed; // cainos movement
    }



}
