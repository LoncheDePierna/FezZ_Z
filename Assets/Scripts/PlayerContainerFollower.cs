using UnityEngine;

public class PlayerContainerFollower : MonoBehaviour
{
    public Transform map;
    public Vector3 localOffset = Vector3.zero;

    void LateUpdate()
    {
        // Solo rota y posiciona el contenedor, no el jugador
        transform.position = map.position + map.rotation * localOffset;
        transform.rotation = map.rotation;
    }
}
