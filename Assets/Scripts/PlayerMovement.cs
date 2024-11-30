using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;  // Movement speed
    public float dashSpeed = 10f;  // Dash speed
    public float dashDuration = 0.2f;  // Duration of the dash
    public Animator animator;

    private Vector2 movement;  // Stores movement input
    private bool isDashing = false;  // Tracks if the player is dashing 
    //public GameObject explosionPrefab;  // Explosion prefab
    public GameObject dashWind;

    private bool inAlternateDimension = false;  // Tracks current dimension
    //public TrailRenderer trailRenderer;  // TrailRenderer for drawing path
    //public AxeSwing axeSwing; 
    GameManager gameManager;
    public float DashCooldown = 1f;
    public int playerHealth = 3;
    public Transform playerTransform;
    public Vector3 playerPosition => playerTransform.position + new Vector3(0.15f, 0.4f, 0);
    private bool busy = false;
    public GameObject summonPrefab;

    protected virtual void Start()
    {
        // Disable the TrailRenderer initially
        //trailRenderer.emitting = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if(!busy){// Handle movement input
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
            if (movement.x != 0 || movement.y != 0)
            {
                animator.SetFloat("movementX", movement.x);
                animator.SetFloat("movementY", movement.y);
            }
            animator.SetFloat("Speed", movement.sqrMagnitude);

            // Handle rotation to face the mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Ensure z position is zero for 2D
            Vector3 direction = (mousePos - playerTransform.position).normalized;
            bool rightTurn = direction.x > 0;

            // Flip the character based on mouse position
            playerTransform.rotation = Quaternion.Euler(0, rightTurn ? 0 : 180, 0);

            // Handle dash
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(1)) && !isDashing)
            {
                StartCoroutine(Dash());
            }

            // Apply movement if not dashing
            Vector2 moveDirection = movement.normalized * moveSpeed * Time.deltaTime;
            playerTransform.Translate(moveDirection, Space.World);
            

            // Handle dimension switching
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchDimension();
            }
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Attack());
                //axeSwing.Swing();
            }
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        Vector2 dashDirection = movement.normalized; // Maintain direction during dash
        float startTime = Time.time;
        dashWind.SetActive(true);
        while (Time.time < startTime + dashDuration)
        {
            // Move the player during dash
            playerTransform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        yield return ResetDash();
    }
    IEnumerator ResetDash(){
        dashWind.SetActive(false);
        yield return new WaitForSeconds(DashCooldown);
        isDashing = false;
    }

    protected virtual IEnumerator Attack()
    {
        busy = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        busy = false;
    }
    void SwitchDimension()
    {
        gameManager.ChangeDimension();
    }

    /*IEnumerator TriggerExplosion()
    {
        // Get all positions from the TrailRenderer
        Vector3[] positions = new Vector3[trailRenderer.positionCount];
        trailRenderer.GetPositions(positions);

        // Instantiate fewer explosions along the trail
        for (int i = 0; i < positions.Length; i += 10)  // Adjust the step to control explosion frequency
        {
            GameObject explosionObject = Instantiate(explosionPrefab, positions[i], Quaternion.identity);
            Destroy(explosionObject, 0.06f);  // Destroy explosion prefab after 2 seconds
            yield return null;  // Small delay between explosions
        }

        // Clear the trail after explosions
        trailRenderer.Clear();
    }*/

    void OnDisable()
    {
        StopAllCoroutines();
        dashWind.SetActive(false);
        isDashing = false;
        busy = false;
    }
    void OnEnable(){
        busy = true;
        StartCoroutine(SummonCircle());
    }

    public IEnumerator SummonCircle()
    {
        Vector3 position = playerTransform.position;
        GameObject prefab = Instantiate(summonPrefab, new Vector3(position.x, position.y, position.z), Quaternion.identity);
        SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        float transparency = spriteRenderer.color.a;
        Animator animator = prefab.GetComponent<Animator>();
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5);
        prefab.GetComponent<AudioSource>().PlayOneShot(prefab.GetComponent<AudioSource>().clip) ;
        float duration = animator.GetCurrentAnimatorStateInfo(0).length;
        while (transparency > 0)
        {
            transparency -= 0.1f;
            spriteRenderer.color = new Color(1, 1, 1, transparency);
            yield return new WaitForSeconds(duration * 0.1f);
        }
        Destroy(prefab);
        busy = false;
    }

    public IEnumerator TakeDamage(){
        busy = true;
        playerHealth--;
        if(playerHealth <= 0){
            GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("Death");
            yield return null;
        }
        else{
            animator.SetTrigger("TakeDamage");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            busy = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("DamageObject"))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
