using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float wallJumpForce = 7f;
    public float slideSpeed = 10f;
    public float slideDuration = 1f;
    public float momentumMultiplier = 1.2f; // Mnożnik momentum dla skoków

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isDashing = false;
    private bool isSliding = false;
    private bool canWallJump = false;
    private float dashTimer;
    private float slideTimer;
    private float momentum = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && !isSliding)
        {
            StartSlide();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDashing)
        {
            StartDash();
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                StopSlide();
            }
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                StopDash();
            }
        }

        // Odbijanie od ścian
        if (Input.GetKeyDown(KeyCode.Space) && canWallJump)
        {
            WallJump();
        }
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * momentum;

        if (isSliding)
        {
            rb.MovePosition(transform.position + move.normalized * slideSpeed * Time.fixedDeltaTime);
        }
        else if (isDashing)
        {
            rb.MovePosition(transform.position + move.normalized * dashSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(transform.position + move * Time.fixedDeltaTime);
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce * momentum, ForceMode.Impulse);
        isGrounded = false;
        momentum = 1f; // Resetujemy momentum po skoku
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
    }

    void StopDash()
    {
        isDashing = false;
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        rb.AddForce(-transform.up * slideSpeed, ForceMode.Impulse); // Dodaj siłę w dół
    }

    void StopSlide()
    {
        isSliding = false;
    }

    void WallJump()
    {
        rb.AddForce(Vector3.up * wallJumpForce, ForceMode.Impulse);
        rb.AddForce(-transform.forward * wallJumpForce, ForceMode.Impulse); // Siła w kierunku od ściany
        canWallJump = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Sprawdzamy, czy gracz uderza w ścianę
        if (collision.gameObject.CompareTag("Wall"))
        {
            canWallJump = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            canWallJump = false;
        }
    }

    void OnJumpButtonPressed()
    {
        if (isGrounded)
        {
            momentum *= momentumMultiplier; // Zwiększamy momentum przy odpowiednim czasie naciśnięcia skoku
        }
    }
}