using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public int maxJumps = 2;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private int jumpCount;
    private bool isGrounded;
    private float horizontalInput;
    private GameObject meshObject;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameManager.Instance.onRotate += RotatePlayer;
        RotatePlayer(GameManager.Instance.currentFace);

        if (transform.childCount > 0)
        {
            meshObject = transform.GetChild(0).gameObject;
        }
        else
        {
            Debug.LogWarning("Este objeto no tiene hijos, no se puede asignar meshObject.");
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.onRotate -= RotatePlayer;
    }

    void Update()
    {
        // Entrada de movimiento
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Detección de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            jumpCount = 0;
            animator.SetInteger("JumpIndex", -1); // También aquí colocas la animación de en suelo
        }

        // Saltar / Doble salto
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            if (jumpCount == 0)
                animator.SetInteger("JumpIndex", 0); // Salto inicial
            else if (jumpCount == 1)
                animator.SetInteger("JumpIndex", 1); // Doble salto

            jumpCount++; // Se incrementa al final
        }

        // Animaciones
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);

        // Rotación visual en eje Y
        if (horizontalInput > 0)
        {
            meshObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 120f); // Mira a la derecha
        }
        else if (horizontalInput < 0)
        {
            meshObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 240f); // Mira a la izquierda
        }
        else
        {
            meshObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 180f); // Mira hacia el frente
        }
    }

    void FixedUpdate()
    {
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void RotatePlayer(GameManager.Face face)
    {
        if (face == GameManager.Face.Front) transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
        if (face == GameManager.Face.Left) transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        if (face == GameManager.Face.Right) transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        if (face == GameManager.Face.Back) transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
    }
}
