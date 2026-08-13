using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DangerDownloading : MonoBehaviour
{
    [Header("Download Settings")]
    public float downloadDuration = 30f;
    public float startingProgress = 0f;

    [Header("UI")]
    public GameObject eventPanel;
    public Slider progressBar;
    public TMP_Text progressText;
    public TMP_Text fileNameText;

    [Header("Close Buttons")]
    public GameObject closeButtonPrefab;
    public Transform buttonParent;
    public int numberOfButtons = 6;

    [Header("Error Popup")]
    public GameObject errorPopupPrefab;
    public Transform errorParent;

    [Header("Score")]
    public CookieClicker cookieClicker;

    private float progress;
    private bool eventRunning;

    private List<GameObject> closeButtons = new List<GameObject>();

    public void StartDangerDownloading()
    {
        if (eventRunning)
            return;

        eventRunning = true;
        progress = startingProgress;

        if (eventPanel != null)
            eventPanel.SetActive(true);

        CreateCloseButtons();

        StartCoroutine(DownloadRoutine());

        Debug.Log("Danger Downloading event started!");
    }

    private IEnumerator DownloadRoutine()
    {
        while (eventRunning)
        {
            progress += (100f / downloadDuration) * Time.deltaTime;

            UpdateProgressUI();

            if (progress >= 100f)
            {
                progress = 100f;

                UpdateProgressUI();

                DownloadCompleted();

                yield break;
            }

            yield return null;
        }
    }

    private void UpdateProgressUI()
    {
        if (progressBar != null)
        {
            progressBar.value = progress / 100f;
        }

        if (progressText != null)
        {
            progressText.text = Mathf.RoundToInt(progress) + "%";
        }
    }

    private void CreateCloseButtons()
    {
        ClearButtons();

        if (closeButtonPrefab == null || buttonParent == null)
        {
            Debug.LogError(
                "Danger Downloading: Close Button Prefab or Button Parent is not assigned!"
            );

            return;
        }

        for (int i = 0; i < numberOfButtons; i++)
        {
            GameObject buttonObject = Instantiate(
                closeButtonPrefab,
                buttonParent
            );

            closeButtons.Add(buttonObject);

            DownloadButton button =
                buttonObject.GetComponent<DownloadButton>();

            if (button != null)
            {
                button.Setup(this, false);
            }
            else
            {
                Debug.LogError(
                    "Danger Downloading: Close Button Prefab is missing DownloadButton component!"
                );
            }
        }

        // Pick ONE random button to be the real one
        if (closeButtons.Count > 0)
        {
            int realIndex = Random.Range(0, closeButtons.Count);

            DownloadButton realButton =
                closeButtons[realIndex].GetComponent<DownloadButton>();

            if (realButton != null)
            {
                realButton.Setup(this, true);
            }
        }
    }

    public void FakeButtonClicked(GameObject button)
    {
        if (!eventRunning)
            return;

        Debug.Log("Fake download button clicked!");

        // Remove fake button from our list
        if (closeButtons.Contains(button))
        {
            closeButtons.Remove(button);
        }

        // Destroy the fake button
        if (button != null)
        {
            Destroy(button);
        }

        // Show error popup
        if (errorPopupPrefab != null && errorParent != null)
        {
            GameObject error = Instantiate(
                errorPopupPrefab,
                errorParent
            );

            ErrorPopup popup =
                error.GetComponent<ErrorPopup>();

            if (popup != null)
            {
                popup.Setup();
            }
        }
    }

    public void RealButtonClicked()
    {
        if (!eventRunning)
            return;

        Debug.Log("REAL download button found!");

        EndEvent();
    }

    private void DownloadCompleted()
    {
        if (!eventRunning)
            return;

        Debug.Log("CRITICAL: Download reached 100%!");

        // Remove half of the current score
        if (cookieClicker != null)
        {
            cookieClicker.RemoveHalfScore();
        }
        else
        {
            Debug.LogError(
                "Danger Downloading: CookieClicker is not assigned!"
            );
        }

        EndEvent();
    }

    public void EndEvent()
    {
        eventRunning = false;

        StopAllCoroutines();

        ClearButtons();

        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }

        Debug.Log("Danger Downloading event ended!");
    }

    private void ClearButtons()
    {
        foreach (GameObject button in closeButtons)
        {
            if (button != null)
            {
                Destroy(button);
            }
        }

        closeButtons.Clear();
    }
}