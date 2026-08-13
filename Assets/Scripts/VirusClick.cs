using UnityEngine;
using UnityEngine.EventSystems;

public class VirusClick : MonoBehaviour, IPointerClickHandler
{
    private EventManager eventManager;

    public void Setup(EventManager manager)
    {
        eventManager = manager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventManager != null)
        {
            eventManager.VirusClicked(gameObject);
        }
    }
}