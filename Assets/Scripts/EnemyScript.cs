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
    public PlayerBreadcrumbScript detectedBreadcrumb = null;
    public float aggroRange;
    public Vector3 positionOffset;
    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player");

        GameManager.OnDimensionChange += HandleDimensionChange;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        detectedBreadcrumb = DetectBreadcrumbs();
    }

    PlayerBreadcrumbScript DetectBreadcrumbs()
    {
        GameObject[] allBreadcrumbs = GameObject.FindGameObjectsWithTag("Breadcrumb");
        List<GameObject> validBreadcrumbs = new List<GameObject>();

        foreach (GameObject breadcrumb in allBreadcrumbs)
        {
            float distance = Vector2.Distance(transform.position + positionOffset, breadcrumb.transform.position);
            if (distance <= aggroRange)
            {
                Vector2 direction = (breadcrumb.transform.position - transform.position + positionOffset).normalized;
                RaycastHit2D[] hits = Physics2D.CircleCastAll(
                transform.position + positionOffset, 
                0.5f, // Adjust radius to match the breadcrumb's approximate size
                direction, 
                distance
                );

                bool pathClear = true;
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null && hit.collider.gameObject != breadcrumb && hit.collider.gameObject != gameObject)
                    {
                        pathClear = false;
                        break;
                    }
                }

                if (pathClear)
                {
                    validBreadcrumbs.Add(breadcrumb);
                }
            }
        }

        PlayerBreadcrumbScript closestBreadcrumb = null;
        float minActiveTime = float.MaxValue;

        foreach (GameObject breadcrumb in validBreadcrumbs)
        {
            PlayerBreadcrumbScript breadcrumbScript = breadcrumb.GetComponent<PlayerBreadcrumbScript>();
            if (breadcrumbScript != null && breadcrumbScript.activeTime < minActiveTime)
            {
                minActiveTime = breadcrumbScript.activeTime;
                closestBreadcrumb = breadcrumbScript;
            }
        }

        return closestBreadcrumb;
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
        }
        else
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
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
