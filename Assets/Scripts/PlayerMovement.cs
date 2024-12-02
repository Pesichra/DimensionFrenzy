using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Monk;
    public GameObject Hood;
    private PlayerState CurrentState;
    public float moveSpeed;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f; 

    private Vector2 movement; 
    private bool isDashing = false; 
    public GameObject dashWind;

    GameManager gameManager;
    public float DashCooldown = 1f;
    public int playerMaxHealth = 3;
    public int playerHealth = 3;
    public Vector3 playerPosition => transform.position + new Vector3(0.15f, 0.4f, 0);
    private bool busy = false;
    public GameObject summonPrefab;
    private Animator animator;

    public GameObject healthbar;
    public GameObject heartPrefab;
    public Sprite heartSprite;
    public Sprite lostHeartSprite;
    private List<GameObject> hearts = new List<GameObject>();
    protected virtual void Start()
    {
        animator = Monk.GetComponent<Animator>();
        CurrentState = Monk.GetComponent<PlayerState>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        for(int i = 0; i < playerMaxHealth; i++){
            GameObject heart = Instantiate(heartPrefab, healthbar.transform);
            heart.transform.localPosition = new Vector3(heart.transform.localPosition.x + i * 0.8f, heart.transform.localPosition.y, heart.transform.localPosition.z);
            hearts.Add(heart);
        }
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
            Vector3 direction = (mousePos - transform.position).normalized;
            bool rightTurn = direction.x > 0;

            // Flip the character based on mouse position
            transform.rotation = Quaternion.Euler(0, rightTurn ? 0 : 180, 0);

            

            Vector2 moveDirection = movement.normalized;
            GetComponent<Rigidbody2D>().velocity = moveDirection * moveSpeed; //cainos movement
            //transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World); // my movement
            
            // Handle dash
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(1)) && !isDashing)
            {
                StartCoroutine(Dash());
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchDimension();
            }
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Attack());
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact(movement.x, movement.y);
            }
        }else{
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        Vector2 dashDirection = movement.normalized;
        float startTime = Time.time;
        dashWind.SetActive(true);
        while (Time.time < startTime + dashDuration)
        {
            //transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            GetComponent<Rigidbody2D>().velocity = dashDirection * dashSpeed; //cainos movement
            yield return null;
        }
        yield return ResetDash();
    }
    IEnumerator ResetDash(){
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        dashWind.SetActive(false);
        yield return new WaitForSeconds(DashCooldown);
        isDashing = false;
    }

    protected virtual IEnumerator Attack()
    {
        busy = true;
        animator.SetTrigger("Attack");
        StartCoroutine(CurrentState.Attack());
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        busy = false;
    }
    void SwitchDimension()
    {
        int dimension = gameManager.ChangeDimension();
        if (dimension == 0)
        {
            Monk.GetComponent<SpriteRenderer>().sortingLayerName =  Hood.GetComponent<SpriteRenderer>().sortingLayerName;
            Monk.SetActive(true);
            Hood.SetActive(false);
            animator = Monk.GetComponent<Animator>();
            CurrentState = Monk.GetComponent<PlayerState>();
        }
        else if (dimension == 1)
        {
            Hood.GetComponent<SpriteRenderer>().sortingLayerName =  Monk.GetComponent<SpriteRenderer>().sortingLayerName;
            Monk.SetActive(false);
            Hood.SetActive(true);
            animator = Hood.GetComponent<Animator>();
            CurrentState = Hood.GetComponent<PlayerState>();
        }
        busy = true;
        StartCoroutine(SummonCircle());
    }

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
        Vector3 position = transform.position;
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
        hearts[playerHealth].GetComponent<SpriteRenderer>().sprite = lostHeartSprite;
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

    void Interact(float movementX, float movementY){
        Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D interactable in interactables)
        {
            if (interactable.CompareTag("Interactable"))
            {
                interactable.GetComponent<IInteractable>().Interact();
            }
        }
        Collider2D closestInteractable = null;
        float closestDistance = 1f;
        Vector2 facingDirection = new Vector2(movementX, movementY);

        foreach (Collider2D interactable in interactables)
        {
            if (interactable.CompareTag("Interactable"))
            {
                Vector2 directionToInteractable = (interactable.transform.position - transform.position).normalized;
                float distanceToInteractable = Vector2.Distance(transform.position, interactable.transform.position);

                if (Vector2.Dot(facingDirection, directionToInteractable) > 0.5f && distanceToInteractable < closestDistance)
                {
                    closestDistance = distanceToInteractable;
                    closestInteractable = interactable;
                }
            }
        }

        if (closestInteractable != null)
        {
            closestInteractable.GetComponent<IInteractable>().Interact();
        }
    }

    public void Heal(int amount){
        for(int i = 0; i < amount; i++){
            if(playerHealth < playerMaxHealth){
                hearts[playerHealth].GetComponent<SpriteRenderer>().sprite = heartSprite;
                playerHealth++;
            }
        }
    }

    public void IncreaseMaxHealth(int amount){
        int excess = amount - (playerMaxHealth - playerHealth);
        Heal(amount);
        for(int i = 0; i < amount; i++){
            GameObject heart = Instantiate(heartPrefab, healthbar.transform);
            heart.transform.localPosition = new Vector3(heartPrefab.transform.localPosition.x + (playerMaxHealth + i) * 0.8f, heartPrefab.transform.localPosition.y, heartPrefab.transform.localPosition.z);
            heart.GetComponent<SpriteRenderer>().sprite = i < excess ? heartSprite : lostHeartSprite;
            hearts.Add(heart);
        }
        playerMaxHealth += amount;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("DamageObject"))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
