using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // =========================================================
    // GENERAL EVENT SETTINGS
    // =========================================================

    [Header("Automatic Event Spawning")]
    [Tooltip("Minimum time between events.")]
    public float minEventDelay = 15f;

    [Tooltip("Maximum time between events.")]
    public float maxEventDelay = 30f;

    private bool eventSchedulerRunning = false;
   

    // =========================================================
    // DANGER DOWNLOADING
    // =========================================================

    [Header("Danger Downloading")]
    public DangerDownloading dangerDownloading;

    // =========================================================
    // POPUP FRENZY
    // =========================================================

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

    // =========================================================
    // POPUP CLOSER
    // =========================================================

    [Header("Popup Closer")]
    public int popupClosersOwned = 0;
    public float popupCloserInterval = 0.5f;

    [Header("Popup Closer End Effect")]
    public GameObject popupCloserEndImage;
    public AudioSource popupCloserAudioSource;
    public AudioClip popupCloserSound;
    public float popupCloserEndImageDuration = 1.5f;

    // =========================================================
    // VIRABUSTER
    // =========================================================

    [Header("Virabuster")]
    public GameObject virabusterPrefab;
    public int virabustersOwned = 0;
    public float virabusterInterval = 5f;
    public float virabusterMoveSpeed = 500f;

    // =========================================================
    // VIRUS OUTBREAK
    // =========================================================

    [Header("Virus Outbreak")]
    public GameObject virusPrefab;

    public int minViruses = 5;
    public int maxVirusesToSpawn = 20;

    [Tooltip("Maximum number of viruses allowed at once.")]
    public int maxActiveViruses = 20;

    [Tooltip("How much progress is lost every second while viruses exist.")]
    public int virusDrainAmount = 1;

    // =========================================================
    // EVENT STATES
    // =========================================================

    private bool eventRunning = false;
    private bool virusEventRunning = false;
    private bool passwordCrackerRunning = false;
    private bool callingChaosRunning = false;
    private bool dangerDownloadingRunning = false;

    // =========================================================
    // ACTIVE OBJECTS
    // =========================================================

    private List<GameObject> activePopups =
        new List<GameObject>();

    private List<GameObject> activeViruses =
        new List<GameObject>();

    public int score = 0;

    // =========================================================
    // PASSWORD CRACKER
    // =========================================================

    [Header("Password Cracker")]
    public PasswordCracker passwordCracker;

    public float passwordCrackerDuration = 20f;
    public int passwordCrackerFailPenalty = 100;
    public int passwordCrackerReward = 500;

    // =========================================================
    // PASSWORD SOLVER
    // =========================================================

    [Header("Password Solver")]
    public int passwordSolversOwned = 0;

    // =========================================================
    // SCAM KILLER
    // =========================================================

    [Header("Scam Killer")]
    public int scamKillersOwned = 0;

    public bool HasScamKiller()
    {
        return scamKillersOwned > 0;
    }

    // =========================================================
    // FILE MANAGER
    // =========================================================

    [Header("File Manager")]
    public int fileManagersOwned = 0;

    public bool HasFileManager()
    {
        return fileManagersOwned > 0;
    }

    // =========================================================
    // CALLING CHAOS
    // =========================================================

    [Header("Calling Chaos")]
    public CallingChaos callingChaos;

    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        StartCoroutine(EventScheduler());
    }

    // =========================================================
    // EVENT SCHEDULER
    // =========================================================

   private IEnumerator EventScheduler()
{
    eventSchedulerRunning = true;

    while (eventSchedulerRunning)
    {
        // Wait until there is currently no event running.
        yield return new WaitUntil(
            () => !IsAnyEventRunning()
        );

        // Wait 15-30 seconds AFTER the previous event ended.
        float delay = Random.Range(
            minEventDelay,
            maxEventDelay
        );

        yield return new WaitForSeconds(delay);

        // Make absolutely sure another event
        // hasn't started during the wait.
        if (IsAnyEventRunning())
            continue;

        // Start the next random event.
        StartRandomEvent();
    }
}

    // =========================================================
    // CHECK IF ANY EVENT IS RUNNING
    // =========================================================

    private bool IsAnyEventRunning()
    {
        return
            eventRunning ||
            virusEventRunning ||
            passwordCrackerRunning ||
            callingChaosRunning ||
            dangerDownloadingRunning;
    }

    // =========================================================
    // RANDOM EVENT
    // =========================================================

    private void StartRandomEvent()
    {
        List<int> availableEvents =
            new List<int>();

        // 0 = Popup Frenzy
        // 1 = Virus Outbreak
        // 2 = Password Cracker
        // 3 = Calling Chaos
        // 4 = Danger Downloading

        availableEvents.Add(0);
        availableEvents.Add(1);
        availableEvents.Add(2);
        availableEvents.Add(4);

        // Only add Calling Chaos if Scam Killer
        // isn't owned.
        if (scamKillersOwned <= 0)
        {
            availableEvents.Add(3);
        }

        if (availableEvents.Count == 0)
            return;

        int randomIndex = Random.Range(
            0,
            availableEvents.Count
        );

        int selectedEvent =
            availableEvents[randomIndex];

        switch (selectedEvent)
        {
            case 0:
                StartCoroutine(PopupFrenzy());
                break;

            case 1:
                StartCoroutine(VirusOutbreak());
                break;

            case 2:
                StartPasswordCrackerEvent();
                break;

            case 3:
                StartCallingChaosEvent();
                break;

            case 4:
                StartDangerDownloadingEvent();
                break;
        }
    }

    // =========================================================
    // CALLING CHAOS
    // =========================================================

    private void StartCallingChaosEvent()
    {
        if (callingChaos == null)
        {
            Debug.LogError(
                "CALLING CHAOS ERROR: CallingChaos is not assigned!"
            );

            return;
        }

        if (scamKillersOwned > 0)
        {
            Debug.Log(
                "SCAM KILLER: Calling Chaos blocked!"
            );

            return;
        }

        callingChaosRunning = true;

        callingChaos.StartCallingChaos(this);
    }

    public void CallingChaosEnded()
    {
        callingChaosRunning = false;

        Debug.Log(
            "EVENT: Calling Chaos completely ended!"
        );
    }

    // =========================================================
    // POPUP FRENZY
    // =========================================================

    private IEnumerator PopupFrenzy()
    {
        eventRunning = true;

        Debug.Log(
            "EVENT: Popup Frenzy started!"
        );

        if (popupClosersOwned > 0)
        {
            StartCoroutine(
                PopupCloserRoutine()
            );
        }

        float timer = 0f;

        while (timer < eventDuration)
        {
            SpawnPopup();

            yield return new WaitForSeconds(
                spawnInterval
            );

            timer += spawnInterval;
        }

        eventRunning = false;

        Debug.Log(
            "EVENT: Popup Frenzy ended!"
        );

        if (popupClosersOwned > 0)
        {
            yield return StartCoroutine(
                PopupCloserEndEffect()
            );
        }
    }

    // =========================================================
    // SPAWN POPUP
    // =========================================================

    private void SpawnPopup()
    {
        if (popupPrefab == null)
        {
            Debug.LogError(
                "Popup Prefab is not assigned!"
            );

            return;
        }

        if (activePopups.Count >= maxPopups)
        {
            RemoveRandomPopup();
        }

        GameObject popup = Instantiate(
            popupPrefab,
            popupParent
        );

        activePopups.Add(popup);

        RectTransform popupRect =
            popup.GetComponent<RectTransform>();

        if (popupRect != null &&
            spawnArea != null)
        {
            float x = Random.Range(
                -spawnArea.rect.width / 2f,
                spawnArea.rect.width / 2f
            );

            float y = Random.Range(
                -spawnArea.rect.height / 2f,
                spawnArea.rect.height / 2f
            );

            popupRect.anchoredPosition =
                new Vector2(x, y);

            PopupMovement movement =
                popup.GetComponent<PopupMovement>();

            if (movement != null)
            {
                movement.Setup(spawnArea);
            }
        }

        PopupClick popupClick =
            popup.GetComponent<PopupClick>();

        if (popupClick != null)
        {
            popupClick.Setup(this);
        }
    }

    // =========================================================
    // REMOVE RANDOM POPUP
    // =========================================================

    private void RemoveRandomPopup()
    {
        if (activePopups.Count == 0)
            return;

        int randomIndex =
            Random.Range(
                0,
                activePopups.Count
            );

        GameObject popupToRemove =
            activePopups[randomIndex];

        activePopups.RemoveAt(
            randomIndex
        );

        if (popupToRemove != null)
        {
            Destroy(popupToRemove);
        }
    }

    // =========================================================
    // POPUP CLICKED
    // =========================================================

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
    // POPUP CLOSER
    // =========================================================

    private IEnumerator PopupCloserRoutine()
    {
        while (eventRunning)
        {
            yield return new WaitForSeconds(
                popupCloserInterval
            );

            if (!eventRunning)
                break;

            if (activePopups.Count == 0)
                continue;

            int amountToClose =
                Random.Range(1, 3);

            for (int i = 0;
                 i < amountToClose;
                 i++)
            {
                if (activePopups.Count == 0)
                    break;

                RemoveRandomPopup();
            }
        }
    }

    // =========================================================
    // POPUP CLOSER END EFFECT
    // =========================================================

    private IEnumerator PopupCloserEndEffect()
    {
        for (
            int i = activePopups.Count - 1;
            i >= 0;
            i--
        )
        {
            if (activePopups[i] != null)
            {
                Destroy(activePopups[i]);
            }
        }

        activePopups.Clear();

        if (popupCloserEndImage != null)
        {
            popupCloserEndImage.SetActive(true);
        }

        if (popupCloserAudioSource != null &&
            popupCloserSound != null)
        {
            popupCloserAudioSource.PlayOneShot(
                popupCloserSound
            );
        }

        yield return new WaitForSeconds(
            popupCloserEndImageDuration
        );

        if (popupCloserEndImage != null)
        {
            popupCloserEndImage.SetActive(false);
        }
    }

    // =========================================================
    // VIRUS OUTBREAK
    // =========================================================

    private IEnumerator VirusOutbreak()
    {
        virusEventRunning = true;

        Debug.Log(
            "EVENT: VIRUS OUTBREAK started!"
        );

        if (virabustersOwned > 0)
        {
            StartCoroutine(
                VirabusterRoutine()
            );
        }

        int virusAmount = Random.Range(
            minViruses,
            maxVirusesToSpawn + 1
        );

        for (
            int i = 0;
            i < virusAmount;
            i++
        )
        {
            SpawnVirus();
        }

        float outbreakTime = 0f;

        while (activeViruses.Count > 0)
        {
            yield return new WaitForSeconds(1f);

            if (activeViruses.Count > 0)
            {
                outbreakTime += 1f;

                int damage =
                    Mathf.FloorToInt(
                        outbreakTime / 5f
                    ) + 1;

                CookieClicker cookieClicker =
                    FindFirstObjectByType<CookieClicker>();

                if (cookieClicker != null)
                {
                    cookieClicker.RemoveProgress(
                        damage
                    );
                }
            }
        }

        virusEventRunning = false;

        Debug.Log(
            "EVENT: VIRUS OUTBREAK defeated!"
        );
    }

    // =========================================================
    // VIRABUSTER
    // =========================================================

    private IEnumerator VirabusterRoutine()
    {
        yield return new WaitForSeconds(
            virabusterInterval
        );

        while (virusEventRunning)
        {
            List<GameObject> availableViruses =
                new List<GameObject>(
                    activeViruses
                );

            for (
                int i = 0;
                i < virabustersOwned;
                i++
            )
            {
                if (!virusEventRunning)
                    break;

                if (availableViruses.Count == 0)
                    break;

                int randomIndex =
                    Random.Range(
                        0,
                        availableViruses.Count
                    );

                GameObject target =
                    availableViruses[randomIndex];

                availableViruses.RemoveAt(
                    randomIndex
                );

                if (target != null)
                {
                    SpawnVirabuster(target);
                }
            }

            yield return new WaitForSeconds(
                virabusterInterval
            );
        }
    }

    // =========================================================
    // FIND NEAREST VIRUS
    // =========================================================

    private GameObject FindNearestVirus()
    {
        if (activeViruses.Count == 0)
            return null;

        GameObject nearestVirus = null;
        float nearestDistance = Mathf.Infinity;

        Vector3 referencePosition =
            spawnArea != null
                ? spawnArea.position
                : transform.position;

        foreach (GameObject virus in activeViruses)
        {
            if (virus == null)
                continue;

            float distance =
                Vector3.Distance(
                    referencePosition,
                    virus.transform.position
                );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestVirus = virus;
            }
        }

        return nearestVirus;
    }

    // =========================================================
    // SPAWN VIRUS
    // =========================================================

    private void SpawnVirus()
    {
        if (
            activeViruses.Count >=
            maxActiveViruses
        )
        {
            return;
        }

        if (virusPrefab == null)
        {
            Debug.LogError(
                "Virus Prefab is not assigned!"
            );

            return;
        }

        GameObject virus =
            Instantiate(
                virusPrefab,
                popupParent
            );

        activeViruses.Add(virus);

        RectTransform virusRect =
            virus.GetComponent<RectTransform>();

        if (
            virusRect != null &&
            spawnArea != null
        )
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

    // =========================================================
    // VIRUS CLICKED
    // =========================================================

    public void VirusClicked(GameObject virus)
    {
        if (activeViruses.Contains(virus))
        {
            activeViruses.Remove(virus);
        }

        Destroy(virus);

        if (activeViruses.Count == 0)
        {
            virusEventRunning = false;

            Debug.Log(
                "EVENT: VIRUS OUTBREAK defeated!"
            );
        }
    }

    public bool IsVirusEventRunning()
    {
        return virusEventRunning;
    }

    // =========================================================
    // SPAWN VIRABUSTER
    // =========================================================

    private void SpawnVirabuster(
        GameObject targetVirus
    )
    {
        if (virabusterPrefab == null)
        {
            Debug.LogError(
                "Virabuster Prefab is not assigned!"
            );

            return;
        }

        if (targetVirus == null)
            return;

        GameObject virabuster =
            Instantiate(
                virabusterPrefab,
                popupParent
            );

        RectTransform virabusterRect =
            virabuster.GetComponent<RectTransform>();

        if (
            virabusterRect != null &&
            spawnArea != null
        )
        {
            float x = Random.Range(
                -spawnArea.rect.width / 2f,
                spawnArea.rect.width / 2f
            );

            float y = Random.Range(
                -spawnArea.rect.height / 2f,
                spawnArea.rect.height / 2f
            );

            virabusterRect.anchoredPosition =
                new Vector2(x, y);
        }

        VirabusterMovement movement =
            virabuster.GetComponent<
                VirabusterMovement
            >();

        if (movement != null)
        {
            movement.Setup(
                this,
                targetVirus,
                virabusterMoveSpeed
            );
        }
    }

    // =========================================================
    // PASSWORD CRACKER
    // =========================================================

    private void StartPasswordCrackerEvent()
    {
        if (passwordCracker == null)
        {
            Debug.LogError(
                "Password Cracker is not assigned!"
            );

            return;
        }

        passwordCrackerRunning = true;

        Debug.Log(
            "EVENT: Password Cracker started!"
        );

        passwordCracker.StartPasswordCracker();
    }

    public void PasswordCracked()
    {
        CookieClicker cookieClicker =
            FindFirstObjectByType<CookieClicker>();

        if (cookieClicker != null)
        {
            cookieClicker.AddProgress(
                passwordCrackerReward
            );
        }

        Debug.Log(
            "PASSWORD CRACKER SUCCESS! +" +
            passwordCrackerReward +
            " progress!"
        );

        passwordCrackerRunning = false;
    }

    public void PasswordFailed()
    {
        CookieClicker cookieClicker =
            FindFirstObjectByType<CookieClicker>();

        if (cookieClicker != null)
        {
            cookieClicker.RemoveProgress(
                passwordCrackerFailPenalty
            );
        }

        Debug.Log(
            "PASSWORD CRACKER FAILED! -" +
            passwordCrackerFailPenalty +
            " progress!"
        );
    }

    // =========================================================
    // DANGER DOWNLOADING
    // =========================================================

    private void StartDangerDownloadingEvent()
    {
        if (dangerDownloading == null)
        {
            Debug.LogError(
                "Danger Downloading is not assigned!"
            );

            return;
        }

        dangerDownloadingRunning = true;

        Debug.Log(
            "EVENT: Danger Downloading started!"
        );

        dangerDownloading.StartDangerDownloading(
            this
        );
    }

    public void DangerDownloadingEnded()
    {
        dangerDownloadingRunning = false;

        Debug.Log(
            "EVENT: Danger Downloading completely ended!"
        );
    }
}