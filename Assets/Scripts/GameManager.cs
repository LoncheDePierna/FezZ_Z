using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum Face { Front, Left, Back, Right }
    public Face currentFace = Face.Front;
    public GameObject map;
    public GameObject player;
    public float frontOffset = 0.5f; // distancia por delante del mapa

    public event System.Action<Face> onRotate;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) RotateWorld(-90);
        if (Input.GetKeyDown(KeyCode.E)) RotateWorld(90);
    }

    private void RotateWorld(float angle)
    {
        // 1. Rotar el mundo
        map.transform.Rotate(0, angle, 0);
        currentFace = (Face)(((int)currentFace + (angle < 0 ? 1 : 3)) % 4);
        onRotate?.Invoke(currentFace);

        // 2. Reposicionar player
        Vector3 center = map.transform.position;
        Vector3 localPos = player.transform.position - center;

        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 newLocal = rot * localPos;

        // 3. Ajustar z al frente del mapa
        float halfDepth = GetMapDepth() / 2f;
        newLocal.z = halfDepth + frontOffset;

        player.transform.position = center + newLocal;
    }

    private float GetMapDepth()
    {
        // Debes ajustar esto según tu nivel. Puede ser collider, bounds, valor fijo...
        Collider col = map.GetComponent<Collider>();
        return (col != null) ? col.bounds.size.z : 10f;
    }
}
