using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CallingChaos : MonoBehaviour
{
    [Header("Calling Chaos Panel")]
    public GameObject callPanel;
    public TMP_Text phoneNumberText;

    [Header("Buttons")]
    public Button acceptCallButton;
    public Button endCallButton;

    [Header("Audio")]
    public AudioSource audioSource;

    [Tooltip("Sound that plays while the phone is ringing.")]
    public AudioClip ringingSound;

    [Tooltip("Sound that plays after accepting the call.")]
    public AudioClip callSound;

    [Header("Call Settings")]
    [Tooltip("How long before another call appears after ending one.")]
    public float timeBetweenCalls = 3f;

    [Header("Fake Phone Numbers")]
    public string[] phoneNumbers =
    {
        "+1 (555) 013-4829",
        "+1 (555) 019-7362",
        "+1 (555) 024-9183",
        "+1 (555) 031-4427",
        "+1 (555) 047-2819"
    };

    private int callsMade = 0;
    private int maxCalls = 0;

    private bool eventRunning = false;
    private bool acceptingCall = false;

    private EventManager eventManager;

    private void Start()
    {
        callPanel.SetActive(false);

        acceptCallButton.onClick.AddListener(AcceptCall);
        endCallButton.onClick.AddListener(EndCall);
    }

    // =========================================================
    // START EVENT
    // =========================================================

    public void StartCallingChaos(EventManager manager)
    {
        if (eventRunning)
            return;

        eventManager = manager;

        eventRunning = true;
        acceptingCall = false;
        callsMade = 0;

        // Randomly decide how many calls this event will have.
        // 1 through 10.
        maxCalls = Random.Range(1, 11);

        Debug.Log(
            "EVENT: Calling Chaos started! " +
            "Maximum calls: " + maxCalls
        );

        ShowCall();
    }

    // =========================================================
    // SHOW CALL
    // =========================================================

    private void ShowCall()
    {
        if (!eventRunning)
            return;

        if (callsMade >= maxCalls)
        {
            EndEvent();
            return;
        }

        callsMade++;
        acceptingCall = false;

        // Pick a random phone number.
        string randomNumber =
            phoneNumbers[Random.Range(0, phoneNumbers.Length)];

        phoneNumberText.text = randomNumber;

        // Enable buttons.
        acceptCallButton.interactable = true;
        endCallButton.interactable = true;

        // Show panel.
        callPanel.SetActive(true);

        // Start ringing.
        StartRinging();

        Debug.Log(
            "CALLING CHAOS: Incoming call " +
            callsMade + "/" +
            maxCalls +
            " from " +
            randomNumber
        );
    }

    // =========================================================
    // RINGING
    // =========================================================

    private void StartRinging()
    {
        if (audioSource == null || ringingSound == null)
            return;

        audioSource.Stop();

        audioSource.clip = ringingSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void StopAudio()
    {
        if (audioSource == null)
            return;

        audioSource.Stop();
        audioSource.loop = false;
    }

    // =========================================================
    // ACCEPT CALL
    // =========================================================

    public void AcceptCall()
    {
        if (!eventRunning || acceptingCall)
            return;

        acceptingCall = true;

        Debug.Log(
            "CALLING CHAOS: Call accepted!"
        );

        // Disable both buttons while audio plays.
        acceptCallButton.interactable = false;
        endCallButton.interactable = false;

        // Stop ringing.
        StopAudio();

        // Play actual call.
        if (audioSource != null && callSound != null)
        {
            audioSource.clip = callSound;
            audioSource.loop = false;
            audioSource.Play();

            StartCoroutine(WaitForCallToFinish());
        }
        else
        {
            Debug.LogWarning(
                "Calling Chaos: Call audio or AudioSource is missing!"
            );

            EndEvent();
        }
    }

    private IEnumerator WaitForCallToFinish()
    {
        while (audioSource != null && audioSource.isPlaying)
        {
            yield return null;
        }

        Debug.Log(
            "CALLING CHAOS: Call audio finished."
        );

        EndEvent();
    }

    // =========================================================
    // END CALL
    // =========================================================

    public void EndCall()
    {
        if (!eventRunning || acceptingCall)
            return;

        Debug.Log(
            "CALLING CHAOS: Call ended. " +
            "Calls: " + callsMade + "/" + maxCalls
        );

        StopAudio();

        // Hide the panel.
        callPanel.SetActive(false);

        // If this was the final call, end the event.
        if (callsMade >= maxCalls)
        {
            EndEvent();
            return;
        }

        // Otherwise wait before calling again.
        StartCoroutine(NextCall());
    }

    // =========================================================
    // NEXT CALL
    // =========================================================

    private IEnumerator NextCall()
    {
        Debug.Log(
            "CALLING CHAOS: Next call in " +
            timeBetweenCalls +
            " seconds."
        );

        yield return new WaitForSeconds(timeBetweenCalls);

        ShowCall();
    }

    // =========================================================
    // END EVENT
    // =========================================================

    private void EndEvent()
    {
        if (!eventRunning)
            return;

        eventRunning = false;
        acceptingCall = false;

        StopAllCoroutines();
        StopAudio();

        callPanel.SetActive(false);

        Debug.Log(
            "EVENT: Calling Chaos ended!"
        );

        // Tell EventManager the event is finished.
        if (eventManager != null)
        {
            eventManager.CallingChaosEnded();
        }

        eventManager = null;
    }

    // =========================================================
    // DEBUG / SAFETY
    // =========================================================

    public void StopCallingChaos()
    {
        EndEvent();
    }

    public bool IsRunning()
    {
        return eventRunning;
    }
}