using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string nextSceneName; // Nombre de la escena a la que queremos ir
    public float teleportDelay = 5f; // Tiempo antes de cambiar de escena

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Obtener referencias necesarias
            PlayerController playerController = collision.GetComponent<PlayerController>();
            Animator playerAnimator = collision.GetComponent<Animator>();

            if (playerController != null && playerAnimator != null)
            {
                // Desactivar el movimiento del jugador
                playerController.enabled = false;

                // Desactivar la rotación en el GameManager
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.canRotate = false;
                }

                // Activar la animación de teletransporte
                playerAnimator.SetTrigger("Teleport");

                // Programar el cambio de escena
                Invoke("ChangeScene", teleportDelay);
            }
        }
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Portal: No se ha especificado el nombre de la siguiente escena");
        }
    }
} 