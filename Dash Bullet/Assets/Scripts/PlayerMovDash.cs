using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovDash : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Vector2 moveDirection;
    private bool canDash = true;
    private bool isDashing;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Animator animator;

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        ProcessInputs();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rb.velocity = moveDirection * moveSpeed;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // Calculate dash velocity
        Vector2 dashVelocity = moveDirection * dashSpeed;
        rb.velocity = dashVelocity;

        // Play dash animation
        AdjustDashAnimationDirection();
        animator.SetBool("Dash", isDashing);

        // Enable trail renderer
        tr.emitting = true;

        yield return new WaitForSeconds(dashDuration);

        // End dash
        tr.emitting = false;
        rb.velocity = Vector2.zero;
        isDashing = false;
        animator.SetBool("Dash", isDashing);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void AdjustDashAnimationDirection()
    {
        if (moveDirection.x > 0) // Dash right
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (moveDirection.x < 0) // Dash left
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (moveDirection.y > 0) // Dash up
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveDirection.y < 0) // Dash down
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }
}