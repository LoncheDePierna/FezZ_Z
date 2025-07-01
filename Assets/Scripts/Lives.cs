using UnityEngine;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour
{
    private GameManager gameManager;
    private Transform[] lifeIcons;
    private Animator playerAnimator;
    private bool isGameOver = false;

    [SerializeField] private float delayBeforeMenu = 4f; // Tiempo de espera antes de volver al menú
    private static readonly string LOOSE_TRIGGER = "Loose"; // Nombre del trigger en el Animator

    void Start()
    {
        gameManager = GameManager.Instance;
        // Obtener todos los hijos y guardarlos en orden
        lifeIcons = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            lifeIcons[i] = transform.GetChild(i);
        }
        
        // Obtener el animator del player
        if (gameManager.player != null)
        {
            playerAnimator = gameManager.player.GetComponent<Animator>();
        }
        
        // Actualizar la visualización inicial de vidas
        UpdateLivesDisplay();
    }

    void Update()
    {
        if (!isGameOver && gameManager.lives != GetActiveLifeCount())
        {
            UpdateLivesDisplay();
            
            if (gameManager.lives <= 0)
            {
                GameOver();
            }
        }
    }

    private void UpdateLivesDisplay()
    {
        int currentLives = gameManager.lives;
        
        // Activar/desactivar los iconos de vida según el número de vidas actual
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            // Los íconos se desactivan de arriba hacia abajo (último hijo primero)
            lifeIcons[lifeIcons.Length - 1 - i].gameObject.SetActive(i < currentLives);
        }
    }

    private int GetActiveLifeCount()
    {
        int count = 0;
        foreach (Transform lifeIcon in lifeIcons)
        {
            if (lifeIcon.gameObject.activeSelf)
                count++;
        }
        return count;
    }

    private void GameOver()
    {
        isGameOver = true;
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger(LOOSE_TRIGGER);
            Invoke("ReturnToMainMenu", delayBeforeMenu);
        }
        else
        {
            Debug.LogWarning("No se encontró el Animator del player. Volviendo al menú principal inmediatamente.");
            ReturnToMainMenu();
        }
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
