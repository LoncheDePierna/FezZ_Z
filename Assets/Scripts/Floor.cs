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
                // Alinear eje X
                playerPos.z = floorPos.z;
                break;

            case GameManager.Face.Left:
            case GameManager.Face.Right:
                // Alinear eje Z
                playerPos.z = floorPos.z;
                break;
        }

        player.position = playerPos;
    }
}
