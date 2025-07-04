using UnityEngine;

public class Treasure : MonoBehaviour
{
    private GameManager gameManager;
    private Animator animator;
    private bool isOpened = false;

    [SerializeField] private string collectAnimation = "CollectChest";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpened && other.CompareTag("Player") && gameManager.asKey)
        {
            Debug.Log("Treasure opened");
            OpenTreasure();
        }
    }

    private void OpenTreasure()
    {
        isOpened = true;
        
        // Reproducir la animación del cofre
        if (animator != null)
        {
            animator.Play(collectAnimation);
            
            // Obtener la duración de la animación
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            float animationLength = 1f; // Valor por defecto
            
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == collectAnimation)
                {
                    animationLength = clip.length;
                    break;
                }
            }
            
            // Esperar a que termine la animación antes de dar la recompensa
            Invoke("GiveReward", animationLength);
        }
        
        // Desactivar la llave inmediatamente
        if (gameManager.player != null)
        {
            // Buscar y desactivar la llave
            Key key = FindObjectOfType<Key>();
            if (key != null)
            {
                key.gameObject.SetActive(false);
            }
        }
        
        gameManager.asKey = false;
    }

    private void GiveReward()
    {
        gameManager.coins++;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
