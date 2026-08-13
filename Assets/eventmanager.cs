using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Popup Frenzy")]
    public GameObject popupPrefab;
    public Transform popupParent;

    public float spawnInterval = 0.3f;
    public float eventDuration = 30f;

    [Header("Popup Settings")]
    public int maxPopups = 50;
    public int popupScore = 5;

    [Header("Spawn Area")]
    public RectTransform spawnArea;

    private bool eventRunning = false;

    private List<GameObject> activePopups = new List<GameObject>();

    public int score = 0;

    private void Update()
    {
        // DEBUG: Press = to start Popup Frenzy
        if (Input.GetKeyDown(KeyCode.Equals) && !eventRunning)
        {
            StartCoroutine(PopupFrenzy());
        }
    }

    private IEnumerator PopupFrenzy()
    {
        eventRunning = true;

        Debug.Log("EVENT: Popup Frenzy started!");

        float timer = 0f;

        while (timer < eventDuration)
        {
            SpawnPopup();

            yield return new WaitForSeconds(spawnInterval);
            timer += spawnInterval;
        }

        eventRunning = false;

        Debug.Log("EVENT: Popup Frenzy ended!");
    }

    private void SpawnPopup()
    {
        // Make sure we never have more than the maximum
        if (activePopups.Count >= maxPopups)
        {
            RemoveRandomPopup();
        }

        GameObject popup = Instantiate(
            popupPrefab,
            popupParent
        );

        activePopups.Add(popup);

        RectTransform popupRect = popup.GetComponent<RectTransform>();

        if (popupRect != null && spawnArea != null)
        {
            float x = Random.Range(
                -spawnArea.rect.width / 2f,
                spawnArea.rect.width / 2f
            );

            float y = Random.Range(
                -spawnArea.rect.height / 2f,
                spawnArea.rect.height / 2f
            );

            popupRect.anchoredPosition = new Vector2(x, y);

            PopupMovement movement = popup.GetComponent<PopupMovement>();

            if (movement != null)
            {
                movement.Setup(spawnArea);
            }
        }

        // Give the popup its EventManager reference
        PopupClick popupClick = popup.GetComponent<PopupClick>();

        if (popupClick != null)
        {
            popupClick.Setup(this);
        }
    }

    private void RemoveRandomPopup()
    {
        if (activePopups.Count == 0)
            return;

        int randomIndex = Random.Range(0, activePopups.Count);

        GameObject popupToRemove = activePopups[randomIndex];

        activePopups.RemoveAt(randomIndex);

        if (popupToRemove != null)
        {
            Destroy(popupToRemove);
        }
    }

    public void PopupClicked(GameObject popup)
    {
        // Give the player 5 points
        score += popupScore;

        Debug.Log("Popup clicked! +" + popupScore + " score. Total score: " + score);

        // Remove it from the active popup list
        if (activePopups.Contains(popup))
        {
            activePopups.Remove(popup);
        }

        Destroy(popup);
    }
}