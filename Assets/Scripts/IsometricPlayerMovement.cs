using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsometricPlayerMovement : MonoBehaviour
{

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    [SerializeField] float runSpeed = 3f;
    [SerializeField] float jumpSpeed = 2f;
    float myGravityScale = 0.0f;
    bool isAlive = true;

    float scaleMultiplier = 0.3f;
    Vector2 scaleVector;

    private CapsuleCollider2D myCapsuleCollider;
    bool isAttacking = false;
    Animator myAnimator;

    [SerializeField] int EnemyCount = 9;
    [SerializeField] private AudioClip audioSound;
    void CheckEnemiesStatus()
    {
        EnemyCount--;
        if (EnemyCount == 0)
        {
            GameObject.FindGameObjectWithTag("OMFO").GetComponent<MusicClass>().StopMusic();
            GameObject.FindGameObjectWithTag("Load1Scene").GetComponent<LevelChangerScript>().FadeIntoLevel(3);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        ScalePlayer();
        myAnimator = GetComponent<Animator>();
        EnemyDeathEvent.OnEnemyDeath += CheckEnemiesStatus;
        GameObject.FindGameObjectWithTag("OMFO").GetComponent<MusicClass>().PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Walk();
            FlipSprite();
            Hurt();
            CheckIfAttacking();
        }
    }
    void CheckIfAttacking()
    {
        if (isAttacking)
        {
            AnimatorStateInfo stateInfo = myAnimator.GetCurrentAnimatorStateInfo(0);

            // Check if the "isAttacking" parameter is still true in the animator
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                // Fire animation has ended, start walking animation
                myAnimator.SetBool("isAttacking", false);
                AudioSource.PlayClipAtPoint(audioSound, Camera.main.transform.position);
                myAnimator.SetBool("IsWalking", true);
                isAttacking = false; // Reset the attacking flag
            }
        }
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnFire(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Attack started");
            myAnimator.SetBool("isAttacking", true);
            isAttacking = true;
        }
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

    void Hurt()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            myAnimator.SetBool("isHurt", true);
            //AudioSource.PlayClipAtPoint(isHurt, Camera.main.transform.position);

        }
        else
        {
            myAnimator.SetBool("isHurt", false);
        }
    }
}
