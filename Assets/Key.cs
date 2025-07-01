using UnityEngine;

public class Key : MonoBehaviour
{
    private GameManager gameManager;
    private bool isCollected = false;

    [Header("Follow Settings")]
    [SerializeField] private float followDistance = 1.5f; // Distancia a la que seguirá al player
    [SerializeField] private float followSpeed = 5f; // Velocidad de seguimiento
    [SerializeField] private Vector3 offset = new Vector3(0, 0.5f, 0); // Offset vertical u horizontal si lo deseas

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollected && gameManager.player != null)
        {
            FollowPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            Debug.Log("Key collected");
            CollectKey();
        }
    }

    private void CollectKey()
    {
        isCollected = true;
        gameManager.asKey = true;
        
        // Desactivar el collider para que no siga detectando colisiones
        if (TryGetComponent<Collider2D>(out Collider2D collider))
        {
            collider.enabled = false;
        }
    }

    private void FollowPlayer()
    {
        // Calcular la posición objetivo detrás del jugador
        Vector3 playerPosition = gameManager.player.transform.position;
        Vector3 directionToPlayer = (transform.position - playerPosition).normalized;
        Vector3 targetPosition = playerPosition + (directionToPlayer * followDistance) + offset;

        // Mover suavemente hacia la posición objetivo
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
