using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsometricPlayerMovement : MonoBehaviour
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


    void Jump()
    {
        bool playerIsInTheAir = Mathf.Abs(myRigidbody.velocity.y) > 1.0f;
        myAnimator.SetBool("isJumping", playerIsInTheAir);
    }

    void Walk()
    {
        Vector2 movement = new Vector2(moveInput.x, moveInput.y) * runSpeed * Time.deltaTime;

        // Apply the movement directly to the Rigidbody (without using forces or gravity)
        myRigidbody.position += movement;

        // Assuming you have an isometric view, you might want to adjust the Z position to maintain the isometric perspective
        float zPosition = myRigidbody.position.y * 0.01f; // Adjust the multiplier based on your needs

        transform.position = new Vector3(myRigidbody.position.x, myRigidbody.position.y, zPosition);
        bool isMoving = moveInput.magnitude > 0;

        // Set the "IsMoving" parameter in the Animator
        myAnimator.SetBool("IsWalking", isMoving);
    }

    void FlipSprite()
    {
        if (Mathf.Abs(moveInput.x) > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * scaleVector.x, scaleVector.y, 1f);
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
