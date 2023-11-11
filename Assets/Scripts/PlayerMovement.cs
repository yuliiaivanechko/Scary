using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    [SerializeField] float runSpeed = 2f;
    [SerializeField] float jumpSpeed = 2f;
    float myGravityScale = 0.0f;

    float scaleMultiplier = 0.3f;
    Vector2 scaleVector;
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        ScalePlayer();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Jump();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Jump()
    {
        bool playerIsInTheAir = Mathf.Abs(myRigidbody.velocity.y) > 1.0f;
        myAnimator.SetBool("isJumping", playerIsInTheAir);
    }

    void Walk()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        bool playerHasSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("IsWalking", playerHasSpeed);
    }

    void FlipSprite()
    {
        bool playerHasSpeed = (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon) && (Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon);
        if (playerHasSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x) * scaleVector.x, scaleVector.y);
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
    }
}
