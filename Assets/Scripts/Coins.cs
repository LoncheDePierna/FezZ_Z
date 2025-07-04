using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Coins : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private TMP_Text coinText;
    private bool isWinning = false;

    [SerializeField] private float delayBeforeWin = 4f;
    [SerializeField] private int coinsToWin = 5;
    private static readonly string WIN_TRIGGER = "Win";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;

        if (coinText == null)
        {
            coinText = GetComponent<TMP_Text>();
            if (coinText == null)
            {
                Debug.LogError("No se encontró el componente TMP_Text en " + gameObject.name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Actualizar el texto con el número de monedas
        if (coinText != null && gameManager != null)
        {
            coinText.text = gameManager.coins.ToString();
            Debug.Log("Actualizando texto de monedas: " + gameManager.coins);
        }

        // Verificar condición de victoria
        if (!isWinning && gameManager != null && gameManager.coins >= coinsToWin)
        {
            StartWinSequence();
        }
    }

    private void StartWinSequence()
    {
        isWinning = true;
        Debug.Log("Iniciando secuencia de victoria");
        // Esperar el tiempo especificado antes de mostrar la animación
        Invoke("PlayWinAnimation", delayBeforeWin);
    }

    private void PlayWinAnimation()
    {
        if (gameManager.player != null)
        {
            Animator playerAnimator = gameManager.player.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger(WIN_TRIGGER);
                Debug.Log("Reproduciendo animación de victoria");
                // Esperar un segundo más que la animación para asegurar que se vea completa
                Invoke("ReturnToMainMenu", 1f);
            }
            else
            {
                ReturnToMainMenu();
            }
        }
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
