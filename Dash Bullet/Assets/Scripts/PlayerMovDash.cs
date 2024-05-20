using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovDash : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public ParticleSystem bulletParticleSystem;

    public float enCollider = 0.5f;

    private Vector2 moveDirection;
    private bool canDash = true;
    private bool isDashing;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrailRenderer tr;
    //[SerializeField] private Animator animator;
    [SerializeField] private Collider2D playerCollider;

    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;

    public Enemy enemy; // Referencia al script del enemigo

    public RectTransform leftPanel; // Panel izquierdo
    public RectTransform rightPanel; // Panel derecho

    private void Start()
    {
        // Calcular los límites de la pantalla, considerando los bordes negros
        Camera mainCamera = Camera.main;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Obtener las dimensiones de los paneles en unidades del mundo
        float leftPanelWidth = ConvertToWorldUnits(leftPanel);
        float rightPanelWidth = ConvertToWorldUnits(rightPanel);

        float screenWidthWithoutPanels = cameraWidth - (leftPanelWidth + rightPanelWidth);

        screenBounds = new Vector2(screenWidthWithoutPanels / 2, cameraHeight / 2);

        playerWidth = spriteRenderer.bounds.extents.x; // La mitad del ancho del jugador
        playerHeight = spriteRenderer.bounds.extents.y; // La mitad del alto del jugador
    }

    private float ConvertToWorldUnits(RectTransform rectTransform)
    {
        // Convertir las dimensiones del RectTransform a unidades del mundo
        float canvasScaleFactor = rectTransform.GetComponentInParent<Canvas>().scaleFactor;
        return rectTransform.rect.width * canvasScaleFactor / 100f; // Convertir a unidades del mundo
    }

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
        ClampPosition();
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

        playerCollider.enabled = false;

        // Calculate dash velocity
        Vector2 dashVelocity = moveDirection * dashSpeed;
        rb.velocity = dashVelocity;

        // Play dash animation
        //AdjustDashAnimationDirection();
        //animator.SetBool("Dash", isDashing);

        // Enable trail renderer
        tr.emitting = true;

        enemy.IncreaseBulletSpeed();

        yield return new WaitForSeconds(dashDuration);

        // End dash
        tr.emitting = false;
        rb.velocity = Vector2.zero;
        isDashing = false;
        //animator.SetBool("Dash", isDashing);

        yield return new WaitForSeconds(enCollider);
        playerCollider.enabled = true;

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

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth);
        pos.y = Mathf.Clamp(pos.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);
        transform.position = pos;
    }
}
