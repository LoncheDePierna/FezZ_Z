using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum Face { Front, Left, Back, Right }
    public Face currentFace = Face.Front;

    [Header("References")]
    public GameObject map;
    public GameObject player;
    public GameObject pauseMenu;
    public Animator cameraAnimator;

    [Header("Game State")]
    public int coins;
    public int lives;
    public bool asKey;
    private bool isPaused;
    public bool canRotate = true;

    public event Action<Face> onRotate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Control de pausa
        if (Input.GetKeyDown(KeyCode.M))
        {
            TogglePause();
        }

        // Solo permitir rotación si no está pausado y puede rotar
        if (canRotate && !isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateLeft();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotateRight();
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            // Activar pausa
            if (cameraAnimator != null)
            {
                cameraAnimator.Play("ZoomIn");
                // Esperar a que termine la animación antes de mostrar el menú
                Invoke("ShowPauseMenu", 0.5f); // Ajusta este tiempo según la duración de tu animación
            }
            else
            {
                ShowPauseMenu();
            }
            
            // Desactivar movimiento del jugador
            if (player != null)
            {
                var playerController = player.GetComponent<PlayerController>(); // Reemplaza MonoBehaviour con tu script de control del jugador
                if (playerController != null)
                {
                    playerController.enabled = false;
                }
            }
        }
        else
        {
            // Desactivar pausa
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }
            
            if (cameraAnimator != null)
            {
                cameraAnimator.Play("ZoomOut");
            }
            
            // Reactivar movimiento del jugador
            if (player != null)
            {
                var playerController = player.GetComponent<PlayerController>(); // Reemplaza MonoBehaviour con tu script de control del jugador
                if (playerController != null)
                {
                    playerController.enabled = true;
                }
            }
        }
    }

    private void ShowPauseMenu()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }
    }

    public void RotateLeft()
    {
        if (map != null)
        {
            // Mover al jugador al centro del bloque antes de rotar
            if (player != null)
            {
                Vector3 playerPos = player.transform.position;
                playerPos.z += 1f; // Compensar el -1 moviéndolo al centro
                player.transform.position = playerPos;
            }

            map.transform.Rotate(0, -90f, 0);
            currentFace = (Face)(((int)currentFace + 1) % 4);
            onRotate?.Invoke(currentFace);
        }
    }

    public void RotateRight()
    {
        if (map != null)
        {
            // Mover al jugador al centro del bloque antes de rotar
            if (player != null)
            {
                Vector3 playerPos = player.transform.position;
                playerPos.z += 1f; // Compensar el -1 moviéndolo al centro
                player.transform.position = playerPos;
            }

            map.transform.Rotate(0, 90f, 0);
            currentFace = (Face)(((int)currentFace + 3) % 4); // +3 equivale a -1 en modulo 4
            onRotate?.Invoke(currentFace);
        }
    }

    // Método público para ser llamado desde botones UI
    public void ResumeGame()
    {
        if (isPaused)
        {
            TogglePause();
        }
    }

    // Método público para volver al menú principal
    public void ReturnToMainMenu()
    {
        // Si hay alguna animación o efecto de transición, puedes agregarlo aquí
        SceneManager.LoadScene(0);
    }
}
