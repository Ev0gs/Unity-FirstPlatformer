using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float speed;
    private float movement;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float checkRadius;
    [SerializeField] private float jumpStartTime;
    [SerializeField] private Transform feetPos;
    [SerializeField] private LayerMask whatIsGround;
    private float jumpTime;
    private bool isGrounded;
    private bool isJumping;

    [Header("Crouch")]
    [SerializeField] private float crouchMoveSpeed;
    private bool isCrouching;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    public static PlayerMovement Instance;

    public void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("Il y a plus d'une Instance de Player_Health dans la sc�ne");
            return;
        }
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");
        FaceMoveDirection();
        Jump();
        AnimationState();
    }

    void FixedUpdate()
    {
        if (isCrouching)
        {
            speed = crouchMoveSpeed;
        }
        else if (!isCrouching)
        {
            speed = moveSpeed;
        }
        rb.velocity = new Vector2(movement * speed, rb.velocity.y);

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded == true && Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJumping = true;
            //animator.SetBool("IsJumping", true);
            jumpTime = jumpStartTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.UpArrow) && isJumping == true)
        {
            if (jumpTime > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isJumping = false;
        }
    }

    void FaceMoveDirection()
    {
        if (movement > 0)
        {
            sr.flipX = false;
        }
        else if (movement < 0)
        {
            sr.flipX = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
    }

    void AnimationState()
    {
        //Jump animation
        if(rb.velocity.y > 1)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
        }
        if(rb.velocity.y < -1)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }
        if(rb.velocity.y == 0)
        {
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
        }
        //Crouch Animation
        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("Crouch", true);
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            animator.SetBool("Crouch", false);
            isCrouching = false;
        }
    }
}