using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    public EnemyController controller;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (controller != null)
        {
            controller.HandleCollision(collision, gameObject);
        }
    }
} 