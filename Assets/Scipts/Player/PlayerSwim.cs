using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSwim : MonoBehaviour
{
    [Header("Swim Settings")]
    public float swimSpeed = 2f;
    public float verticalSwimSpeed = 2f;
    public float waterGravityScale = 0.2f;

    [Header("Breathing System")]
    public float maxBreathTime = 20f; // Tempo máximo na água antes de começar a se afogar
    public float currentBreathTime;

    [Header("Status")]
    public bool isInWater;
    public bool isSwimming;

    private Rigidbody2D rb;
    private float originalGravity;
    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private float damageTimer;
    private bool isDrowning;
    private PlayerKnockBack knockback;
    public Collider2D headCollider;
    private Animator animator;

    [Header("Input System")]
    [SerializeField] private InputActionReference swimAction;
    [SerializeField] private InputActionReference moveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
        knockback = GetComponent<PlayerKnockBack>();
        originalGravity = rb.gravityScale;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isInWater) return;

        HandleSwimming();
        HandleBreathing();
    }

    private void HandleSwimming()
    {
        if (knockback != null && knockback.IsKnockback) return;

        float xInput = moveAction.action.ReadValue<float>();
        float yInput = 0f;

        if (swimAction.action.ReadValue<float>() > 0.1f)
            yInput = 1f;
        else if (swimAction.action.ReadValue<float>() < -0.1f)
            yInput = -1f;

        rb.linearVelocity = new Vector2(
            xInput * swimSpeed,
            yInput * verticalSwimSpeed
        );

        isSwimming = Mathf.Abs(xInput) > 0.1f || Mathf.Abs(yInput) > 0.1f;

        if (animator != null)
        {
            animator.SetBool("IsSwimming", isSwimming);
            animator.SetFloat("swimX", xInput);
        }
    }

    private void HandleBreathing()
    {
        if (currentBreathTime > 0)
        {
            currentBreathTime -= Time.deltaTime;
        }
        else if (!isDrowning)
        {
            isDrowning = true;
            damageTimer = 0f;
        }

        if (isDrowning)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= 2f)
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }
                damageTimer = 0f;
            }
        }
    }
    private void EnterWater()
    {
        isInWater = true;
        rb.gravityScale = waterGravityScale;
        currentBreathTime = maxBreathTime;
        isDrowning = false;
        damageTimer = 0f;

        if (playerController != null)
        {
            playerController.isGrounded = false;
        }
    }

    private void ExitWater()
    {
        isInWater = false;
        isSwimming = false;
        rb.gravityScale = originalGravity;
        isDrowning = false;
        currentBreathTime = maxBreathTime;
        damageTimer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") && collision.IsTouching(headCollider))
        {
            EnterWater();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            if (!isInWater && collision.IsTouching(headCollider))
            {
                EnterWater();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") && !collision.IsTouching(headCollider))
        {
            ExitWater();
        }
    }

}
