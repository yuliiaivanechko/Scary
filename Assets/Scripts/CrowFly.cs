using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrowFly : MonoBehaviour
{
    Rigidbody2D crowBody;
    [SerializeField] float runSpeed = 10f;

    [SerializeField] Vector2 scaleVector;
    private CapsuleCollider2D capsuleCollider;
    private float changeDirectionTime = 4;

    Vector2 direction;
    float scaleMultiplier = 1f;
    float elapsedTime = 0.0f;
    float soundTime = 0.0f;
    float timeToSound = 23f;

    Animator enemyAnimator;
    [SerializeField] private GameObject Blood;
    AudioSource m_MyAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        crowBody = GetComponent<Rigidbody2D>();
        ScaleCrow();
        m_MyAudioSource = GetComponent<AudioSource>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        direction = Random.insideUnitCircle.normalized;
        m_MyAudioSource.Play();
        // Make the enemy invisible
    }

    void Direction()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > changeDirectionTime)
        {
            elapsedTime = 0;
            direction = Random.insideUnitCircle.normalized;
        }
    }

    void Sound()
    {
        soundTime += Time.deltaTime;

        if (soundTime > timeToSound)
        {
            soundTime = 0;
            m_MyAudioSource.Play();
        }
    }


    // Update is called once per frame
    void Update()
    {
        Fly();
        FlipSprite();
        Direction();
        Sound();
    }


    void Fly()
    {
        // Update the timer

        Vector2 movement = direction * runSpeed * Time.deltaTime;

        // Apply the movement directly to the Rigidbody (without using forces or gravity)
        crowBody.position += movement;

        // Assuming you have an isometric view, you might want to adjust the Z position to maintain the isometric perspective
        float zPosition = crowBody.position.y * 0.01f; // Adjust the multiplier based on your needs

        transform.position = new Vector3(crowBody.position.x, crowBody.position.y, zPosition);
    }

    void FlipSprite()
    {
        if (Mathf.Abs(direction.x) > 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x) * scaleVector.x, scaleVector.y, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("crow");
        direction = Random.insideUnitCircle.normalized;
    }

    void ScaleCrow()
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
