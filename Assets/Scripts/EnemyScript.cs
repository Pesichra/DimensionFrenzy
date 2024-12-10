using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health;
    public int dimension;
    protected GameObject player;
    public GameObject glowingOrbPrefab;
    public GameObject sprite;
    protected virtual void Awake()
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
            sprite.SetActive(true);
            glowingOrbPrefab.SetActive(false);
        }else{
            if(GetComponent<Rigidbody2D>() != null){
                GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            }
            sprite.SetActive(false);
            glowingOrbPrefab.transform.position = transform.position;
            glowingOrbPrefab.SetActive(true);
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
