using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject wallPrefab;
    private GameObject currentInstance;

    public GameObject floorPrefabF;
    public GameObject floorPrefabS;
    private GameObject floorInstanceF;
    private GameObject floorInstanceS;

    [Header("Floor")]
    public bool floorFront;
    public bool floorLeft;
    public bool floorRight;
    public bool floorBack;


    [Space(10), Header("Walls")]
    public bool faceFront;
    public bool faceLeft;
    public bool faceRight;
    public bool faceBack;

    private void OnDisable()
    {
        GameManager.Instance.onRotate -= HandleRotation;
    }
    private void Start()
    {
        floorInstanceF = Instantiate(floorPrefabF, transform);
        floorInstanceF.transform.localPosition = new Vector3(0f, 0.395f, 0f);

        floorInstanceS = Instantiate(floorPrefabS, transform);
        floorInstanceS.transform.localPosition = new Vector3(0f, 0.395f, 0f);

        GameManager.Instance.onRotate += HandleRotation;

        HandleRotation(GameManager.Instance.currentFace);
    }

    private void HandleRotation(GameManager.Face face)
    {
        if (currentInstance != null) Destroy(currentInstance);

        currentInstance = Instantiate(wallPrefab, transform);

        currentInstance.transform.localPosition = GetLocalPosition(face);
        currentInstance.transform.localRotation = Quaternion.Euler(GetLocalRotation(face));

        if ((face == GameManager.Face.Front) ||
            (face == GameManager.Face.Back))
        {
            floorInstanceF.gameObject.SetActive(true);
            floorInstanceS.gameObject.SetActive(false);
        }
        if ((face == GameManager.Face.Left) ||
            (face == GameManager.Face.Right))
        {
            floorInstanceF.gameObject.SetActive(false);
            floorInstanceS.gameObject.SetActive(true);
        }

        if ((face == GameManager.Face.Front && !faceFront) ||
            (face == GameManager.Face.Left && !faceLeft) ||
            (face == GameManager.Face.Right && !faceRight) ||
            (face == GameManager.Face.Back && !faceBack))
        {
            currentInstance.gameObject.SetActive(false);
        }

        if ((face == GameManager.Face.Front && !floorFront) ||
            (face == GameManager.Face.Left && !floorLeft) ||
            (face == GameManager.Face.Right && !floorRight) ||
            (face == GameManager.Face.Back && !floorBack))
        {
            floorInstanceF.gameObject.SetActive(false);
            floorInstanceS.gameObject.SetActive(false);
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
