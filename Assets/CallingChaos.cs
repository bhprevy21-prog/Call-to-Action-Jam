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


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        if (callPanel != null)
            callPanel.SetActive(false);

        if (acceptCallButton != null)
            acceptCallButton.onClick.AddListener(AcceptCall);

        if (endCallButton != null)
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

        // =====================================================
        // SCAM KILLER CHECK
        // =====================================================

        if (eventManager != null &&
            eventManager.HasScamKiller())
        {
            Debug.Log(
                "CALLING CHAOS BLOCKED: Scam Killer is owned!"
            );

            eventManager.CallingChaosEnded();

            eventManager = null;

            return;
        }

        // =====================================================
        // START EVENT
        // =====================================================

        eventRunning = true;
        acceptingCall = false;
        callsMade = 0;

        // Randomly choose between 1 and 10 calls.
        maxCalls = Random.Range(1, 11);

        Debug.Log(
            "EVENT: Calling Chaos started! " +
            "Maximum calls: " +
            maxCalls
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

        // =====================================================
        // SCAM KILLER SAFETY CHECK
        // =====================================================

        if (eventManager != null &&
            eventManager.HasScamKiller())
        {
            Debug.Log(
                "CALLING CHAOS BLOCKED: Scam Killer is owned!"
            );

            EndEvent();
            return;
        }

        // =====================================================
        // CHECK MAX CALLS
        // =====================================================

        if (callsMade >= maxCalls)
        {
            EndEvent();
            return;
        }

        callsMade++;
        acceptingCall = false;

        // =====================================================
        // PICK RANDOM PHONE NUMBER
        // =====================================================

        string randomNumber =
            phoneNumbers[
                Random.Range(
                    0,
                    phoneNumbers.Length
                )
            ];

        if (phoneNumberText != null)
        {
            phoneNumberText.text = randomNumber;
        }

        // =====================================================
        // ENABLE BUTTONS
        // =====================================================

        if (acceptCallButton != null)
            acceptCallButton.interactable = true;

        if (endCallButton != null)
            endCallButton.interactable = true;

        // =====================================================
        // SHOW PANEL
        // =====================================================

        if (callPanel != null)
            callPanel.SetActive(true);

        // =====================================================
        // START RINGING
        // =====================================================

        StartRinging();

        Debug.Log(
            "CALLING CHAOS: Incoming call " +
            callsMade +
            "/" +
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
        if (audioSource == null ||
            ringingSound == null)
            return;

        audioSource.Stop();

        audioSource.clip = ringingSound;
        audioSource.loop = true;
        audioSource.Play();
    }


    // =========================================================
    // STOP AUDIO
    // =========================================================

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
        if (!eventRunning ||
            acceptingCall)
            return;

        acceptingCall = true;

        Debug.Log(
            "CALLING CHAOS: Call accepted!"
        );

        // Disable buttons while call is happening.
        if (acceptCallButton != null)
            acceptCallButton.interactable = false;

        if (endCallButton != null)
            endCallButton.interactable = false;

        // Stop ringing.
        StopAudio();

        // =====================================================
        // PLAY CALL AUDIO
        // =====================================================

        if (audioSource != null &&
            callSound != null)
        {
            audioSource.clip = callSound;
            audioSource.loop = false;
            audioSource.Play();

            StartCoroutine(
                WaitForCallToFinish()
            );
        }
        else
        {
            Debug.LogWarning(
                "Calling Chaos: Call audio or AudioSource is missing!"
            );

            EndEvent();
        }
    }


    // =========================================================
    // WAIT FOR CALL AUDIO
    // =========================================================

    private IEnumerator WaitForCallToFinish()
    {
        while (audioSource != null &&
               audioSource.isPlaying)
        {
            yield return null;
        }

        Debug.Log(
            "CALLING CHAOS: Call audio finished."
        );

        EndEvent();
    }


    // =========================================================
    // END CURRENT CALL
    // =========================================================

    public void EndCall()
    {
        if (!eventRunning ||
            acceptingCall)
            return;

        Debug.Log(
            "CALLING CHAOS: Call ended. " +
            "Calls: " +
            callsMade +
            "/" +
            maxCalls
        );

        // Stop ringing.
        StopAudio();

        // Hide phone panel.
        if (callPanel != null)
            callPanel.SetActive(false);

        // =====================================================
        // CHECK IF THIS WAS THE LAST CALL
        // =====================================================

        if (callsMade >= maxCalls)
        {
            EndEvent();
            return;
        }

        // Otherwise wait before next call.
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

        yield return new WaitForSeconds(
            timeBetweenCalls
        );

        // =====================================================
        // SCAM KILLER CHECK
        // =====================================================

        if (eventManager != null &&
            eventManager.HasScamKiller())
        {
            Debug.Log(
                "CALLING CHAOS: Scam Killer detected. " +
                "Stopping event."
            );

            EndEvent();
            yield break;
        }

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

        // Stop any running coroutines.
        StopAllCoroutines();

        // Stop sounds.
        StopAudio();

        // Hide panel.
        if (callPanel != null)
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


    // =========================================================
    // CHECK IF RUNNING
    // =========================================================

    public bool IsRunning()
    {
        return eventRunning;
    }
}