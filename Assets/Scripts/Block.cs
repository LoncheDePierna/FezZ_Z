using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject FloorPrefab;
    private GameObject floorInstance;

    [Header("Floor")]
    public bool floorFront;
    public bool floorLeft;
    public bool floorRight;
    public bool floorBack;

    private void OnDisable()
    {
        GameManager.Instance.onRotate -= HandleRotation;
    }

    private void Start()
    {
        GameManager.Instance.onRotate += HandleRotation;
        HandleRotation(GameManager.Instance.currentFace);
    }

    private void HandleRotation(GameManager.Face face)
    {
        if (floorInstance == null)
        {
            floorInstance = Instantiate(FloorPrefab, transform);
            floorInstance.transform.localPosition = new Vector3(0f, 0.395f, 0f);
        }

        // Set rotation based on face
        if (face == GameManager.Face.Front || face == GameManager.Face.Back)
        {
            floorInstance.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else // Left or Right
        {
            floorInstance.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        // Handle visibility based on current face
        bool shouldShowFloor = (face == GameManager.Face.Front && floorFront) ||
                            (face == GameManager.Face.Left && floorLeft) ||
                            (face == GameManager.Face.Right && floorRight) ||
                            (face == GameManager.Face.Back && floorBack);

        floorInstance.gameObject.SetActive(shouldShowFloor);
    }
}
