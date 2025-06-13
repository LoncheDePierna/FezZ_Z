using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject prefab;
    private GameObject currentInstance;

    public bool noFaceFront;
    public bool noFaceLeft;
    public bool noFaceRight;
    public bool noFaceBack;

    private void OnEnable()
    {
        GameManager.Instance.onRotate += HandleRotation;
    }

    private void OnDisable()
    {
        GameManager.Instance.onRotate -= HandleRotation;
    }

    private void Start()
    {
        HandleRotation(GameManager.Instance.currentFace);
    }

    private void HandleRotation(GameManager.Face face)
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
        }

        currentInstance.gameObject.SetActive(true);
        currentInstance = Instantiate(prefab, transform);
        currentInstance.transform.localPosition = GetLocalPosition(face);
        currentInstance.transform.localRotation = Quaternion.Euler(GetLocalRotation(face));

        if ((face == GameManager.Face.Front && noFaceFront) ||
            (face == GameManager.Face.Left && noFaceLeft) ||
            (face == GameManager.Face.Right && noFaceRight) ||
            (face == GameManager.Face.Back && noFaceBack))
        {
            currentInstance.gameObject.SetActive(false);
        }
    }

    private Vector3 GetLocalPosition(GameManager.Face face)
    {
        switch (face)
        {
            case GameManager.Face.Front:
                return new Vector3(0.2f, 0.2f, 0f);
            case GameManager.Face.Left:
                return new Vector3(0f, 0.2f, -0.2f);
            case GameManager.Face.Right:
                return new Vector3(0f, 0.2f, 0.2f);
            case GameManager.Face.Back:
                return new Vector3(-0.2f, 0.2f, 0f);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetLocalRotation(GameManager.Face face)
    {
        switch (face)
        {
            case GameManager.Face.Front:
                return new Vector3(0f, -90f, 0f);
            case GameManager.Face.Left:
                return Vector3.zero;
            case GameManager.Face.Right:
                return Vector3.zero;
            case GameManager.Face.Back:
                return new Vector3(0f, -90f, 0f);
            default:
                return Vector3.zero;
        }
    }
}
