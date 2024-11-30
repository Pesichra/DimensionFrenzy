using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health;
    public int dimension;
    protected GameObject player;
    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
        GameManager.OnDimensionChange += HandleDimensionChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected virtual void OnEnable()
    {
        player = GameObject.FindWithTag("Player");
    }
    void HandleDimensionChange(int newDimension)
    {
        if (newDimension == dimension)
        {
            gameObject.SetActive(true);
        }else{
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("Explosion") || collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage();
        }
    }

    protected virtual void TakeDamage()
    {
        health--;
    }

    void OnDestroy()
    {
        GameManager.OnDimensionChange -= HandleDimensionChange;
    }
}
