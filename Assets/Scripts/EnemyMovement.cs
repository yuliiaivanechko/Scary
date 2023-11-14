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

    float soundTime = 0.0f;
    float timeToSound = 22f;
    Vector2 direction;
    float scaleMultiplier = 0.8f;
    float timeToInvisible = 2f;
    bool appearing = false;
    Animator enemyAnimator;
    [SerializeField] private GameObject Blood;
    AudioSource m_MyAudioSource;
    bool isSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        enemyRenderer = GetComponent<Renderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        m_MyAudioSource = GetComponent<AudioSource>();
        m_MyAudioSource.Stop();
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
                //AudioSource.PlayClipAtPoint(enemyHurt, Camera.main.transform.position);
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

    void Sound()
    {
        if (isSpawned)
        {
            soundTime += Time.deltaTime;

            if (soundTime > timeToSound)
            {
                soundTime = 0;
                m_MyAudioSource.Play();
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

                isSpawned = true;
                soundTime = 0;
                MakeVisible();
                appearing = true;
            }
            if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Spawn 0"))
            {
                m_MyAudioSource.Play();
                enemyAnimator.SetBool("isAttacking", true);
                direction = Random.insideUnitCircle.normalized;
            }

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
            Sound();
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
            Instantiate(Blood, enemyBody.position, Quaternion.identity);
            capsuleCollider.size = capsuleCollider.size / 2;
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
