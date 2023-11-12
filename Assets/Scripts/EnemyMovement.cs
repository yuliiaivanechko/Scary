using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyMovement : MonoBehaviour
{

    Vector2 moveInput;
    Rigidbody2D enemyBody;
    [SerializeField] float runSpeed = 2f;
    [SerializeField] float jumpSpeed = 2f;
    [SerializeField] int SpawnTime = 0;
    [SerializeField] Vector2 scaleVector;
    private Renderer enemyRenderer;
    float myGravityScale = 0.0f;
    public float elapsedTime = 0.0f;
    float changeDirectionTimer = 0f;
    float changeDirectionInterval = 3f;

    private CapsuleCollider2D capsuleCollider;
    private bool isHurt = false;
    private bool isDead = false;
    private bool died = false;
    Vector2 direction;
    float scaleMultiplier = 0.8f;
    float timeToInvisible = 2f;
    bool appearing = false;

    Animator enemyAnimator;
    [SerializeField] private GameObject Blood;
    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        enemyRenderer = GetComponent<Renderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        ScaleEnemy();
        // Make the enemy invisible
        MakeInvisible();
    }

    void CheckIfHurt()
    {
        if (isHurt)
        {
            AnimatorStateInfo stateInfo = enemyAnimator.GetCurrentAnimatorStateInfo(0);
            // Check if the "isAttacking" parameter is still true in the animator
            if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            {
                // Fire animation has ended, start walking animation
                enemyAnimator.SetBool("isHurt", false);
                enemyAnimator.SetBool("isDead", true);
                elapsedTime = 0;
                isDead = true;
            }
        }
    }

    void Die()
    {
        if (isDead)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > timeToInvisible)
            {
                EnemyDeathEvent.TriggerEnemyDeath();
                died = true;
            }
        }
    }


    void Spawn()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > SpawnTime)
        {
            elapsedTime = 0;
            if (!enemyRenderer.enabled)
            {
                MakeVisible();
                appearing = true;
            }
            if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Spawn 0"))
            {
                enemyAnimator.SetBool("isAttacking", true);
            }
            direction = Random.insideUnitCircle.normalized;
        }
    }

    void MakeInvisible()
    {
        // Disable the renderer (option 1)
        enemyRenderer.enabled = false;
        capsuleCollider.enabled = false;
    }

    void MakeVisible()
    {
        enemyRenderer.enabled = true;
        capsuleCollider.enabled = true;
        enemyAnimator.SetBool("isAppearing", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!died)
        {
            CheckIfHurt();
            Die();
        }
        if (!isHurt)
        {
            Walk();
            Spawn();
            FlipSprite();
            Hurt();
        }
    }


    void Walk()
    {
        // Update the timer

        Vector2 movement = direction * runSpeed * Time.deltaTime;

        // Apply the movement directly to the Rigidbody (without using forces or gravity)
        enemyBody.position += movement;

        // Assuming you have an isometric view, you might want to adjust the Z position to maintain the isometric perspective
        float zPosition = enemyBody.position.y * 0.01f; // Adjust the multiplier based on your needs

        transform.position = new Vector3(enemyBody.position.x, enemyBody.position.y, zPosition);
    }

    void FlipSprite()
    {
        if (Mathf.Abs(direction.x) > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x) * scaleVector.x, scaleVector.y, 1f);
        }
    }

    void Hurt()
    {
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Weapon")))
        {
            Instantiate(Blood, transform.position, Quaternion.identity);
            enemyAnimator.SetBool("isHurt", true);
            isHurt = true;
        }

    }

    void ScaleEnemy()
    {
        // Get the current scale of the player
        Vector3 currentScale = transform.localScale;

        // Multiply the current scale by the scaleMultiplier
        Vector2 newScale = new Vector2(currentScale.x * scaleMultiplier, currentScale.y * scaleMultiplier);
        // Apply the new scale to the player's Transform component
        transform.localScale = newScale;
        scaleVector = newScale;
    }
}
