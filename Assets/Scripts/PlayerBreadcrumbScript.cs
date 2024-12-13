using UnityEngine;

public class PlayerBreadcrumbScript : MonoBehaviour
{
    public float dissolveTime = 3.0f;
    public float activeTime = 0.0f;

    void Update()
    {
        activeTime += Time.deltaTime;
        if (activeTime >= dissolveTime)
        {
            Destroy(gameObject);
        }
    }
}
