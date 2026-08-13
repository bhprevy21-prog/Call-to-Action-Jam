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
    public int numberOfButtons = 6;

    [Header("Button Spawn Area")]
    public RectTransform buttonSpawnArea;

    [Header("Error Popup")]
    public GameObject errorPopupPrefab;
    public RectTransform errorParent;

    [Header("Score")]
    public CookieClicker cookieClicker;

    private float progress;
    private bool eventRunning;

    private EventManager eventManager;

    private List<GameObject> closeButtons =
        new List<GameObject>();

    public void StartDangerDownloading(EventManager manager)
    {
        if (eventRunning)
            return;

        eventManager = manager;

        eventRunning = true;
        progress = startingProgress;

        if (eventPanel != null)
        {
            eventPanel.SetActive(true);
        }

        CreateCloseButtons();

        StartCoroutine(DownloadRoutine());

        Debug.Log(
            "Danger Downloading event started!"
        );
    }

    private IEnumerator DownloadRoutine()
    {
        while (eventRunning)
        {
            progress +=
                (100f / downloadDuration) *
                Time.deltaTime;

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
            progressBar.value =
                progress / 100f;
        }

        if (progressText != null)
        {
            progressText.text =
                Mathf.RoundToInt(progress) + "%";
        }
    }

    private void CreateCloseButtons()
    {
        ClearButtons();

        if (closeButtonPrefab == null)
        {
            Debug.LogError(
                "Danger Downloading: Close Button Prefab is not assigned!"
            );

            return;
        }

        if (buttonSpawnArea == null)
        {
            Debug.LogError(
                "Danger Downloading: Button Spawn Area is not assigned!"
            );

            return;
        }

        for (int i = 0; i < numberOfButtons; i++)
        {
            GameObject buttonObject =
                Instantiate(
                    closeButtonPrefab,
                    buttonSpawnArea
                );

            closeButtons.Add(buttonObject);

            // Random position inside the panel
            RectTransform buttonRect =
                buttonObject.GetComponent<RectTransform>();

            if (buttonRect != null)
            {
                float halfWidth =
                    buttonSpawnArea.rect.width / 2f;

                float halfHeight =
                    buttonSpawnArea.rect.height / 2f;

                float buttonHalfWidth =
                    buttonRect.rect.width / 2f;

                float buttonHalfHeight =
                    buttonRect.rect.height / 2f;

                float x = Random.Range(
                    -halfWidth + buttonHalfWidth,
                    halfWidth - buttonHalfWidth
                );

                float y = Random.Range(
                    -halfHeight + buttonHalfHeight,
                    halfHeight - buttonHalfHeight
                );

                buttonRect.anchoredPosition =
                    new Vector2(x, y);
            }

            DownloadButton button =
                buttonObject.GetComponent<DownloadButton>();

            if (button != null)
            {
                button.Setup(this, false);
            }
            else
            {
                Debug.LogError(
                    "Close Button Prefab is missing DownloadButton!"
                );
            }
        }

        // Pick ONE random button to be the real button
        if (closeButtons.Count > 0)
        {
            int realIndex =
                Random.Range(
                    0,
                    closeButtons.Count
                );

            DownloadButton realButton =
                closeButtons[realIndex]
                .GetComponent<DownloadButton>();

            if (realButton != null)
            {
                realButton.Setup(
                    this,
                    true
                );
            }
        }
    }

    public void FakeButtonClicked(GameObject button)
    {
        if (!eventRunning)
            return;

        Debug.Log(
            "Fake download button clicked!"
        );

        // Remove THIS exact button
        if (closeButtons.Contains(button))
        {
            closeButtons.Remove(button);
        }

        // Destroy THIS exact button
        if (button != null)
        {
            Destroy(button);
        }

        // Spawn error popup
        ShowErrorPopup();
    }

    private void ShowErrorPopup()
    {
        if (errorPopupPrefab == null)
        {
            Debug.LogError(
                "Danger Downloading: Error Popup Prefab is not assigned!"
            );

            return;
        }

        if (errorParent == null)
        {
            Debug.LogError(
                "Danger Downloading: Error Parent is not assigned!"
            );

            return;
        }

        GameObject error =
            Instantiate(
                errorPopupPrefab,
                errorParent
            );

        ErrorPopup popup =
            error.GetComponent<ErrorPopup>();

        if (popup != null)
        {
            popup.Setup();
        }

        // Put the error popup at a random position
        RectTransform errorRect =
            error.GetComponent<RectTransform>();

        if (errorRect != null)
        {
            float x = Random.Range(
                -errorParent.rect.width / 2f,
                errorParent.rect.width / 2f
            );

            float y = Random.Range(
                -errorParent.rect.height / 2f,
                errorParent.rect.height / 2f
            );

            errorRect.anchoredPosition =
                new Vector2(x, y);
        }
    }

    public void RealButtonClicked()
    {
        if (!eventRunning)
            return;

        Debug.Log(
            "REAL download button found!"
        );

        EndEvent();
    }

    private void DownloadCompleted()
    {
        if (!eventRunning)
            return;

        Debug.Log(
            "CRITICAL: Download reached 100%!"
        );

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

        if (eventManager != null)
        {
            eventManager.DangerDownloadingEnded();
        }

        Debug.Log(
            "Danger Downloading event ended!"
        );
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