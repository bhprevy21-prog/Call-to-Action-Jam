using UnityEngine;

public class PopupClick : MonoBehaviour
{
    private EventManager eventManager;

    public void Setup(EventManager manager)
    {
        eventManager = manager;
    }

    public void ClickPopup()
    {
        if (eventManager != null)
        {
            eventManager.PopupClicked(gameObject);
        }
    }
}