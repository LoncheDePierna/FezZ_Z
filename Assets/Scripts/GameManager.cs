using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum Face { Front, Left, Back, Right }
    public Face currentFace = Face.Front;

    public GameObject map;

    public event Action<Face> onRotate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateLeft();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateRight();
        }
    }

    public void RotateLeft()
    {
        if (map != null)
        {
            map.transform.Rotate(0, -90f, 0);
        }

        currentFace = (Face)(((int)currentFace + 1) % 4);
        onRotate?.Invoke(currentFace);
    }

    public void RotateRight()
    {
        if (map != null)
        {
            map.transform.Rotate(0, 90f, 0);
        }

        currentFace = (Face)(((int)currentFace + 3) % 4); // +3 equivale a -1 en modulo 4
        onRotate?.Invoke(currentFace);
    }
}
