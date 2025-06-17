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

    public bool FaceFront;
    public bool FaceLeft;
    public bool FaceRight;
    public bool FaceBack;

    [Space(20)]
    public bool noFloor;

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
            Destroy(floorInstanceF);
            Destroy(floorInstanceS);
        }

        currentInstance = Instantiate(wallPrefab, transform);

        floorInstanceF = Instantiate(floorPrefabF, transform);
        floorInstanceF.transform.localPosition = new Vector3(0f,0.32f,0f);

        floorInstanceS = Instantiate(floorPrefabS, transform);
        floorInstanceS.transform.localPosition = new Vector3(0f, 0.32f, 0f);

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

        if ((face == GameManager.Face.Front && !FaceFront) ||
            (face == GameManager.Face.Left && !FaceLeft) ||
            (face == GameManager.Face.Right && !FaceRight) ||
            (face == GameManager.Face.Back && !FaceBack))
        {
            currentInstance.gameObject.SetActive(false);
        }
        if (noFloor)
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
                return new Vector3(0f, 0.2f, 0f);
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
