using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookieClicker : MonoBehaviour
{
    [Header("Cookie Settings")]
    public int clicksNeeded = 10000;

    // Current score shown to the player.
    private int currentClicks = 0;

    // Permanent score.
    // Events NEVER decrease this.
    private int globalScore = 0;

    [Header("Progress Bar")]
    public Slider progressBar;

    [Header("Score Display")]
    public TMP_Text currentScoreText;

    [Header("Victory Screen")]
    public GameObject victoryScreen;

    [Header("Event Manager")]
    public EventManager eventManager;

    private bool gameWon = false;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // Setup progress bar.
        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = clicksNeeded;
            progressBar.value = currentClicks;
        }
        else
        {
            Debug.LogError(
                "CookieClicker: Progress Bar is NOT assigned!"
            );
        }

        // Hide victory screen when the game starts.
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
        }
        else
        {
            Debug.LogWarning(
                "CookieClicker: Victory Screen is NOT assigned!"
            );
        }

        UpdateScoreText();
    }


    // =========================================================
    // DEBUG
    // =========================================================

    private void Update()
    {
        // Press X to instantly fill the score.
        if (Input.GetKeyDown(KeyCode.X) && !gameWon)
        {
            DebugAddCurrentScore();
        }
    }


    // =========================================================
    // COOKIE CLICK
    // =========================================================

    public void ClickCookie()
    {
        if (gameWon)
            return;

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

        CheckForVictory();
    }


    // =========================================================
    // ADD PROGRESS
    // =========================================================

    public void AddProgress(int amount)
    {
        if (gameWon)
            return;

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

        CheckForVictory();
    }


    // =========================================================
    // REMOVE PROGRESS
    // =========================================================

    public void RemoveProgress(int amount)
    {
        if (gameWon)
            return;

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


    // =========================================================
    // REMOVE HALF SCORE
    // =========================================================

    public void RemoveHalfScore()
    {
        if (gameWon)
            return;

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


    // =========================================================
    // GET CURRENT SCORE
    // =========================================================

    public int GetCurrentScore()
    {
        return currentClicks;
    }


    // =========================================================
    // GET GLOBAL SCORE
    // =========================================================

    public int GetGlobalScore()
    {
        return globalScore;
    }


    // =========================================================
    // SPEND CURRENT SCORE
    // =========================================================

    public bool SpendCurrentScore(int amount)
    {
        if (gameWon)
            return false;

        if (currentClicks < amount)
        {
            Debug.Log(
                "Not enough current score!"
            );

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


    // =========================================================
    // UPDATE PROGRESS BAR
    // =========================================================

    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.value = currentClicks;
        }

        UpdateScoreText();
    }


    // =========================================================
    // UPDATE SCORE TEXT
    // =========================================================

    private void UpdateScoreText()
    {
        if (currentScoreText != null)
        {
            currentScoreText.text =
                "Score: " +
                currentClicks.ToString("N0");
        }
    }


    // =========================================================
    // CHECK VICTORY
    // =========================================================

    private void CheckForVictory()
    {
        if (gameWon)
            return;

        if (currentClicks >= clicksNeeded)
        {
            Victory();
        }
    }


    // =========================================================
    // VICTORY
    // =========================================================

    private void Victory()
    {
        gameWon = true;

        currentClicks = clicksNeeded;

        UpdateProgressBar();

        Debug.Log(
            "VICTORY! Player reached " +
            clicksNeeded +
            "!"
        );

        // =====================================================
        // SHOW VICTORY SCREEN
        // =====================================================

        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
        }

        // =====================================================
        // STOP ALL EVENTS
        // =====================================================

        if (eventManager != null)
        {
            eventManager.StopAllEvents();
        }
        else
        {
            Debug.LogWarning(
                "CookieClicker: Event Manager is NOT assigned!"
            );
        }
    }


    // =========================================================
    // DEBUG BUTTON
    // =========================================================

    public void DebugAddCurrentScore()
    {
        if (gameWon)
            return;

        currentClicks += 100000;

        currentClicks = Mathf.Min(
            currentClicks,
            clicksNeeded
        );

        UpdateProgressBar();

        Debug.Log(
            "DEBUG: Added 100,000 to Current Score. " +
            "Current: " +
            currentClicks +
            " | Global: " +
            globalScore
        );

        CheckForVictory();
    }
}