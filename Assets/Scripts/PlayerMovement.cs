using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    Vector2 moveInput;
    Rigidbody2D rigidbody;
    float runSpeed = 10f;

    float scaleMultiplier = 0.3f;
    Vector2 scaleVector;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        ScalePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rigidbody.velocity.y);
        rigidbody.velocity = playerVelocity;
    }

    void FlipSprite()
    {
        bool playerHasSpeed = Mathf.Abs(rigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody.velocity.x) * scaleVector.x, scaleVector.y);
        }
    }

    void ScalePlayer()
    {
        // Get the current scale of the player
        Vector3 currentScale = transform.localScale;

        // Multiply the current scale by the scaleMultiplier
        Vector2 newScale = new Vector2(currentScale.x * scaleMultiplier, currentScale.y * scaleMultiplier);

        // Apply the new scale to the player's Transform component
        transform.localScale = newScale;
        scaleVector = newScale;
        // Print a message to the console to indicate the scaling
        Debug.Log("Player scaled to: " + newScale);
    }
}
