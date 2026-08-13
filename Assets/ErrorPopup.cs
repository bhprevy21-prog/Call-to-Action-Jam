using UnityEngine;

public class ErrorPopup : MonoBehaviour
{
    public void Setup()
    {
        Debug.Log("Download cannot be closed!");
    }

    public void CloseError()
    {
        Destroy(gameObject);
    }
}