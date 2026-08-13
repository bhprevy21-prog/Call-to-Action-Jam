using UnityEngine;
using UnityEngine.UI;

public class CookieClicker : MonoBehaviour
{
    [Header("Cookie Settings")]
    public int clicksNeeded = 10000;

    private int currentClicks = 0;

    [Header("Progress Bar")]
    public Slider progressBar;

    private void Start()
    {
        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = clicksNeeded;
            progressBar.value = currentClicks;
        }
        else
        {
            Debug.LogError("CookieClicker: Progress Bar is NOT assigned!");
        }
    }

    public void ClickCookie()
    {
        currentClicks++;

        UpdateProgressBar();

        Debug.Log(
            "Cookie clicked! " +
            currentClicks +
            "/" +
            clicksNeeded
        );

        if (currentClicks >= clicksNeeded)
        {
            CookieComplete();
        }
    }

    public void RemoveProgress(int amount)
    {
        currentClicks -= amount;

        currentClicks = Mathf.Max(currentClicks, 0);

        UpdateProgressBar();

        Debug.Log(
            "Progress decreased by " +
            amount +
            ". Current: " +
            currentClicks +
            "/" +
            clicksNeeded
        );
    }

    public void AddProgress(int amount)
    {
        currentClicks += amount;

        currentClicks = Mathf.Min(
            currentClicks,
            clicksNeeded
        );

        UpdateProgressBar();

        Debug.Log(
            "Progress increased by " +
            amount +
            ". Current: " +
            currentClicks +
            "/" +
            clicksNeeded
        );

        if (currentClicks >= clicksNeeded)
        {
            CookieComplete();
        }
    }

    // Removes half of the current score/progress
    public void RemoveHalfScore()
    {
        int oldScore = currentClicks;

        currentClicks = currentClicks / 2;

        UpdateProgressBar();

        Debug.Log(
            "DANGER DOWNLOADING: Score reduced from " +
            oldScore +
            " to " +
            currentClicks
        );
    }

    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.value = currentClicks;
        }
    }

    private void CookieComplete()
    {
        Debug.Log("COOKIE COMPLETE!");

        currentClicks = 0;

        UpdateProgressBar();
    }
}