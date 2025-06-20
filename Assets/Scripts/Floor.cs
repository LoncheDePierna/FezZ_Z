using UnityEngine;

public class Floor : MonoBehaviour
{
    private bool isInFloor = false;

    private void OnDisable()
    {
        GameManager.Instance.onRotate -= MovePlayerToFloor;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.onRotate += MovePlayerToFloor;

        MovePlayerToFloor(GameManager.Instance.currentFace);
    }

    private void MovePlayerToFloor(GameManager.Face face)
    {
        if (isInFloor)
        {
        Debug.Log("Se tepio");
        GameManager.Instance.player.transform.position = transform.position + new Vector3(0f, 0f, 0f);

        //Vector3 currentPosition = GameManager.Instance.player.transform.position;
        //currentPosition.x = 1.5f;
        //GameManager.Instance.player.transform.position = currentPosition;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) isInFloor = true;
        Debug.Log("Entro");
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) isInFloor = false;
        Debug.Log("Salio");
    }
}
