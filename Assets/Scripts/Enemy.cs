using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;
    public float arriveDistance = 0.1f;
    public float waitTimeAtPoint = 2f; // Tiempo detenido en cada punto

    private int currentTargetIndex = 0;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isDead = false;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameManager.Instance.onRotate += OnWorldRotated;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onRotate -= OnWorldRotated;
    }

    private void Update()
    {
        if (isDead || patrolPoints.Length < 2) return;

        if (isWaiting)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0f);
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtPoint)
            {
                isWaiting = false;
                waitTimer = 0f;
                currentTargetIndex = (currentTargetIndex + 1) % patrolPoints.Length;
            }

            return;
        }

        MoveToTarget();

        // Animación de caminar
        animator.SetFloat("Speed", moveSpeed);
    }

    void MoveToTarget()
    {
        Vector3 target = patrolPoints[currentTargetIndex].position;
        Vector3 moveDir = (target - transform.position).normalized;

        Vector2 move = Vector2.zero;

        switch (GameManager.Instance.currentFace)
        {
            case GameManager.Face.Front:
            case GameManager.Face.Back:
                move = new Vector2(moveDir.x, 0f);
                break;

            case GameManager.Face.Left:
            case GameManager.Face.Right:
                move = new Vector2(0f, moveDir.y);
                break;
        }

        rb.linearVelocity = move * moveSpeed;

        // Orientación visual
        if (move.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(move.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (Vector3.Distance(transform.position, target) < arriveDistance)
        {
            isWaiting = true;
            animator.SetFloat("Speed", 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead || !collision.gameObject.CompareTag("Player")) return;

        Vector3 playerPos = collision.transform.position;
        float enemyTop = transform.position.y + (GetComponent<Collider2D>().bounds.size.y / 2f);
        bool stomped = false;

        switch (GameManager.Instance.currentFace)
        {
            case GameManager.Face.Front:
            case GameManager.Face.Back:
                stomped = playerPos.y > enemyTop + 0.1f;
                break;
            case GameManager.Face.Left:
                stomped = playerPos.x < transform.position.x - 0.1f;
                break;
            case GameManager.Face.Right:
                stomped = playerPos.x > transform.position.x + 0.1f;
                break;
        }

        if (stomped)
        {
            Die();
        }
        else
        {
            GameManager.Instance.lives--;

            Animator playerAnim = GameManager.Instance.player.GetComponent<Animator>();
            if (playerAnim != null)
            {
                playerAnim.SetTrigger("Hit");
            }
        }
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("Die");

        Destroy(gameObject, 0.35f);
    }

    private void OnWorldRotated(GameManager.Face newFace)
    {
        rb.linearVelocity = Vector2.zero;
    }
}
