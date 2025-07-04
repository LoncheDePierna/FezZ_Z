using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;
    public float arriveDistance = 0.1f;
    public float waitTimeAtPoint = 2f;

    public GameObject enemyFB; // Enemy for Front/Back movement
    public GameObject enemyLR; // Enemy for Left/Right movement

    private int currentTargetIndex = 0;
    private Rigidbody2D rb;
    private Animator currentAnimator;
    private bool isDead = false;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.Instance.onRotate += OnWorldRotated;
        
        // Asegurarnos que los hijos estén configurados correctamente
        if (enemyFB != null)
        {
            ConfigureChild(enemyFB);
        }
        if (enemyLR != null)
        {
            ConfigureChild(enemyLR);
        }

        UpdateActiveEnemy(GameManager.Instance.currentFace);
    }

    private void ConfigureChild(GameObject child)
    {
        // Asegurarnos que el hijo tenga un EnemyCollisionHandler
        var handler = child.GetComponent<EnemyCollisionHandler>();
        if (handler == null)
        {
            handler = child.AddComponent<EnemyCollisionHandler>();
        }
        handler.controller = this;

        // El Collider2D debe estar en el hijo
        if (child.GetComponent<Collider2D>() == null)
        {
            Debug.LogError($"El objeto {child.name} necesita un Collider2D");
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.onRotate -= OnWorldRotated;
    }

    private void Update()
    {
        if (isDead || patrolPoints.Length < 2) return;

        if (isWaiting)
        {
            rb.linearVelocity = Vector2.zero;
            if (currentAnimator != null)
                currentAnimator.SetFloat("Speed", 0f);
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

        // Actualizar animación
        if (currentAnimator != null)
            currentAnimator.SetFloat("Speed", moveSpeed);
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
            if (currentAnimator != null)
                currentAnimator.SetFloat("Speed", 0f);
        }
    }

    public void HandleCollision(Collision2D collision, GameObject childEnemy)
    {
        if (isDead || !collision.gameObject.CompareTag("Player")) return;

        Vector3 playerPos = collision.transform.position;
        float enemyTop = childEnemy.transform.position.y + (childEnemy.GetComponent<Collider2D>().bounds.size.y / 2f);
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

        if (currentAnimator != null)
            currentAnimator.SetTrigger("Die");

        Destroy(gameObject, 0.35f);
    }

    private void OnWorldRotated(GameManager.Face newFace)
    {
        rb.linearVelocity = Vector2.zero;
        UpdateActiveEnemy(newFace);
    }

    private void UpdateActiveEnemy(GameManager.Face face)
    {
        switch (face)
        {
            case GameManager.Face.Front:
            case GameManager.Face.Back:
                enemyFB.SetActive(true);
                enemyLR.SetActive(false);
                currentAnimator = enemyFB.GetComponent<Animator>();
                break;

            case GameManager.Face.Left:
            case GameManager.Face.Right:
                enemyFB.SetActive(false);
                enemyLR.SetActive(true);
                currentAnimator = enemyLR.GetComponent<Animator>();
                break;
        }
    }
} 