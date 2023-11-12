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

    Vector2 direction;
    float scaleMultiplier = 0.8f;

    Animator enemyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        ScaleEnemy();
        enemyAnimator = GetComponent<Animator>();
        enemyRenderer = GetComponent<Renderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        // Make the enemy invisible
        MakeInvisible();
    }

    void Spawn()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > SpawnTime)
        {
            elapsedTime = 0;
            MakeVisible();

            enemyAnimator.SetBool("isAttacking", true);
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
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Spawn();
        FlipSprite();
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
