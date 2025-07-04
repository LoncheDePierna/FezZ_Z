using UnityEngine;

public class Floor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Transform player = GameManager.Instance.player.transform;
        Vector3 playerPos = player.position;
        Vector3 floorPos = transform.position;

        switch (GameManager.Instance.currentFace)
        {
            case GameManager.Face.Front:
            case GameManager.Face.Back:
                // Alinear eje X y sumar 1 al eje Z
                playerPos.z = floorPos.z - 1f;
                break;

            case GameManager.Face.Left:
            case GameManager.Face.Right:
                // Alinear eje Z y sumar 1
                playerPos.z = floorPos.z - 1f;
                break;
        }

        player.position = playerPos;
    }
}
