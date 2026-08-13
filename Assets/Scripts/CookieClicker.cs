using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookieClicker : MonoBehaviour
{
    [Header("Cookie Settings")]
    public int clicksNeeded = 10000;

    // The score the player currently sees.
    // Events can increase or decrease this.
    private int currentClicks = 0;

    // Permanent score.
    // Events NEVER decrease this.
    private int globalScore = 0;

    [Header("Progress Bar")]
    public Slider progressBar;

    [Header("Score Display")]
    public TMP_Text currentScoreText;

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

        UpdateScoreText();
    }
private void Update()
{
    if (Input.GetKeyDown(KeyCode.X))
    {
        DebugAddCurrentScore();
    }
}
    public void ClickCookie()
    {
        currentClicks++;
        globalScore++;

        UpdateProgressBar();

        Debug.Log(
            "Cookie clicked! Current: " +
            currentClicks +
            "/" +
            clicksNeeded +
            " | Global: " +
            globalScore
        );

        if (currentClicks >= clicksNeeded)
        {
            CookieComplete();
        }
    }

    public void AddProgress(int amount)
    {
        currentClicks += amount;

        currentClicks = Mathf.Min(
            currentClicks,
            clicksNeeded
        );

        globalScore += amount;

        UpdateProgressBar();

        Debug.Log(
            "Progress increased by " +
            amount +
            ". Current: " +
            currentClicks +
            "/" +
            clicksNeeded +
            " | Global: " +
            globalScore
        );

        if (currentClicks >= clicksNeeded)
        {
            CookieComplete();
        }
    }

    public void RemoveProgress(int amount)
    {
        currentClicks -= amount;

        currentClicks = Mathf.Max(
            currentClicks,
            0
        );

        UpdateProgressBar();

        Debug.Log(
            "Progress decreased by " +
            amount +
            ". Current: " +
            currentClicks +
            "/" +
            clicksNeeded +
            " | Global: " +
            globalScore
        );
    }

    public void RemoveHalfScore()
    {
        int oldScore = currentClicks;

        currentClicks = currentClicks / 2;

        UpdateProgressBar();

        Debug.Log(
            "DANGER DOWNLOADING: Score reduced from " +
            oldScore +
            " to " +
            currentClicks +
            " | Global: " +
            globalScore
        );
    }

    public int GetCurrentScore()
    {
        return currentClicks;
    }

    public int GetGlobalScore()
    {
        return globalScore;
    }

    public bool SpendCurrentScore(int amount)
    {
        if (currentClicks < amount)
        {
            Debug.Log("Not enough current score!");
            return false;
        }

        currentClicks -= amount;

        UpdateProgressBar();

        Debug.Log(
            "Spent " +
            amount +
            " current score. Current: " +
            currentClicks +
            " | Global: " +
            globalScore
        );

        return true;
    }

    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.value = currentClicks;
        }

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (currentScoreText != null)
        {
            currentScoreText.text =
                "Score: " + currentClicks.ToString("N0");
        }
    }

    private void CookieComplete()
    {
        Debug.Log(
            "COOKIE COMPLETE! Global Score: " +
            globalScore
        );

        // Only current score resets.
        // Global score stays forever.
        currentClicks = 0;

        UpdateProgressBar();
    }

    // DEBUG BUTTON
    public void DebugAddCurrentScore()
    {
        currentClicks += 100000;

        // Keep this capped for now because the progress bar
        // currently has a maximum of clicksNeeded.
        currentClicks = Mathf.Min(
            currentClicks,
            clicksNeeded
        );

        UpdateProgressBar();

        Debug.Log(
            "DEBUG: Added 100,000 to Current Score. " +
            "Current: " + currentClicks +
            " | Global: " + globalScore
        );

        if (currentClicks >= clicksNeeded)
        {
            CookieComplete();
        }
    }
}