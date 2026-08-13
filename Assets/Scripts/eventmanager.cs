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

    [Header("Virus Outbreak")]
    public GameObject virusPrefab;

    public int minViruses = 5;
    public int maxVirusesToSpawn = 20;

    [Tooltip("Maximum number of viruses allowed at once.")]
    public int maxActiveViruses = 20;

    [Tooltip("How much progress is lost every second while viruses exist.")]
    public int virusDrainAmount = 1;

    private bool eventRunning = false;
    private bool virusEventRunning = false;

    private List<GameObject> activePopups = new List<GameObject>();
    private List<GameObject> activeViruses = new List<GameObject>();

    public int score = 0;

    private void Update()
    {
        // DEBUG:
        // Press = for Popup Frenzy
        // Press - for Virus Outbreak

        if (Input.GetKeyDown(KeyCode.Equals) && !eventRunning)
        {
            StartCoroutine(PopupFrenzy());
        }

        if (Input.GetKeyDown(KeyCode.Minus) && !eventRunning && !virusEventRunning)
        {
            StartCoroutine(VirusOutbreak());
        }
    }

    // =========================================================
    // POPUP FRENZY
    // =========================================================

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
        score += popupScore;

        Debug.Log(
            "Popup clicked! +" +
            popupScore +
            " score. Total score: " +
            score
        );

        if (activePopups.Contains(popup))
        {
            activePopups.Remove(popup);
        }

        Destroy(popup);
    }

    // =========================================================
    // VIRUS OUTBREAK
    // =========================================================

 private IEnumerator VirusOutbreak()
{
    virusEventRunning = true;

    Debug.Log("EVENT: VIRUS OUTBREAK started!");

    // Pick a random number of viruses between 5 and 20.
    int virusAmount = Random.Range(
        minViruses,
        maxVirusesToSpawn + 1
    );

    Debug.Log("Virus Outbreak spawning " + virusAmount + " viruses.");

    // Spawn the viruses.
    for (int i = 0; i < virusAmount; i++)
    {
        SpawnVirus();
    }

    float outbreakTime = 0f;

    // Keep going until all viruses are destroyed.
    while (activeViruses.Count > 0)
    {
        yield return new WaitForSeconds(1f);

        if (activeViruses.Count > 0)
        {
            outbreakTime += 1f;

            // Damage increases every 5 seconds.
            int damage = Mathf.FloorToInt(outbreakTime / 5f) + 1;

            CookieClicker cookieClicker =
                FindFirstObjectByType<CookieClicker>();

            if (cookieClicker != null)
            {
                cookieClicker.RemoveProgress(damage);
            }

            Debug.Log(
                "VIRUS OUTBREAK: -" +
                damage +
                " progress! " +
                "Outbreak time: " +
                outbreakTime +
                "s. " +
                "Viruses remaining: " +
                activeViruses.Count
            );
        }
    }

    virusEventRunning = false;

    Debug.Log("EVENT: VIRUS OUTBREAK defeated!");
}

    private void SpawnVirus()
    {
        if (activeViruses.Count >= maxActiveViruses)
        {
            return;
        }

        if (virusPrefab == null)
        {
            Debug.LogError("Virus Prefab is not assigned!");
            return;
        }

        GameObject virus = Instantiate(
            virusPrefab,
            popupParent
        );

        activeViruses.Add(virus);

        RectTransform virusRect =
            virus.GetComponent<RectTransform>();

        if (virusRect != null && spawnArea != null)
        {
            float x = Random.Range(
                -spawnArea.rect.width / 2f,
                spawnArea.rect.width / 2f
            );

            float y = Random.Range(
                -spawnArea.rect.height / 2f,
                spawnArea.rect.height / 2f
            );

            virusRect.anchoredPosition =
                new Vector2(x, y);

            VirusMovement movement =
                virus.GetComponent<VirusMovement>();

            if (movement != null)
            {
                movement.Setup(spawnArea);
            }
        }

        VirusClick virusClick =
            virus.GetComponent<VirusClick>();

        if (virusClick != null)
        {
            virusClick.Setup(this);
        }
    }

    public void VirusClicked(GameObject virus)
    {
        if (activeViruses.Contains(virus))
        {
            activeViruses.Remove(virus);
        }

        Destroy(virus);

        Debug.Log(
            "Virus destroyed! Viruses remaining: " +
            activeViruses.Count
        );

        // If that was the last virus, the event ends.
        if (activeViruses.Count == 0)
        {
            virusEventRunning = false;

            Debug.Log("EVENT: VIRUS OUTBREAK defeated!");
        }
    }

    public bool IsVirusEventRunning()
    {
        return virusEventRunning;
    }
}