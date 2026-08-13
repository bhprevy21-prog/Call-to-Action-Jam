using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordCracker : MonoBehaviour
{
    [Header("Main UI")]
    public GameObject passwordCrackerPanel;
    public TMP_Text passwordText;
    public TMP_Text wordText;
    public TMP_Text attemptsText;
    public TMP_Text messageText;

    [Header("Character Buttons")]
    public GameObject buttonPrefab;
    public Transform buttonContainer;

    [Header("Settings")]
    public int maxWrongAttempts = 6;

    [Header("Time Limit")]
    public float timeLimit = 20f;
    public TMP_Text timerText;

    [Header("Password Solver")]
    public float solverRevealInterval = 10f;

    [Header("Passwords")]
    public string[] passwords =
    {
        "FIREWALL",
        "HACKER",
        "VIRUS",
        "SECURE9",
        "NETWORK",
        "CYBER",
        "PASSWORD",
        "MALWARE",
        "PHISHING",
        "ENCRYPT",
        "DATABASE",
        "FIREWALL7",
        "CYBER21",
        "H4CKER",
        "V1RUS"
    };

    private string currentPassword;
    private char[] revealedPassword;

    private int wrongAttempts;
    private float currentTime;

    private bool eventActive;

    // Password Solver
    private bool passwordSolverActive;
    private float passwordSolverTimer;

    private List<GameObject> spawnedButtons =
        new List<GameObject>();

    private EventManager eventManager;

    private void Start()
    {
        if (passwordCrackerPanel != null)
            passwordCrackerPanel.SetActive(false);

        eventManager =
            FindFirstObjectByType<EventManager>();
    }

    private void Update()
    {
        if (!eventActive)
            return;

        // =====================================================
        // NORMAL TIMER
        // =====================================================

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;

            UpdateTimerDisplay();

            PasswordFailed();

            return;
        }

        UpdateTimerDisplay();

        // =====================================================
        // PASSWORD SOLVER
        // =====================================================

        if (passwordSolverActive)
        {
            passwordSolverTimer -= Time.deltaTime;

            if (passwordSolverTimer <= 0f)
            {
                passwordSolverTimer =
                    solverRevealInterval;

                RevealNextCharacter();

                if (IsPasswordComplete())
                {
                    PasswordCracked();
                }
            }
        }
    }

    // =========================================================
    // START PASSWORD CRACKER
    // =========================================================

    public void StartPasswordCracker()
    {
        if (eventActive)
            return;

        eventActive = true;

        wrongAttempts = 0;
        currentTime = timeLimit;

        // Password Solver
        passwordSolverActive = false;
        passwordSolverTimer = solverRevealInterval;

        currentPassword =
            passwords[
                Random.Range(
                    0,
                    passwords.Length
                )
            ].ToUpper();

        revealedPassword =
            new char[currentPassword.Length];

        // Start everything hidden.
        for (int i = 0;
             i < revealedPassword.Length;
             i++)
        {
            revealedPassword[i] = '_';
        }

        passwordCrackerPanel.SetActive(true);

        passwordText.text = "PASSWORD";

        messageText.text =
            "CRACK THE PASSWORD";

        // =====================================================
        // ACTIVATE PASSWORD SOLVER
        // =====================================================

        if (eventManager != null &&
            eventManager.passwordSolversOwned > 0)
        {
            passwordSolverActive = true;

            RevealHalfPassword();

            messageText.text =
                "PASSWORD SOLVER ACTIVE!";
        }

        UpdateWordDisplay();
        UpdateAttemptsDisplay();
        UpdateTimerDisplay();

        GenerateCharacterButtons();

        Debug.Log(
            "PASSWORD CRACKER: New password selected: " +
            currentPassword
        );

        if (passwordSolverActive)
        {
            Debug.Log(
                "PASSWORD SOLVER: Revealed half of password!"
            );
        }
    }

    // =========================================================
    // PASSWORD SOLVER - REVEAL HALF
    // =========================================================

    private void RevealHalfPassword()
    {
        int amountToReveal =
            Mathf.CeilToInt(
                currentPassword.Length / 2f
            );

        List<int> hiddenPositions =
            new List<int>();

        for (int i = 0;
             i < currentPassword.Length;
             i++)
        {
            hiddenPositions.Add(i);
        }

        // Randomly choose half of the characters.
        for (int i = 0;
             i < amountToReveal;
             i++)
        {
            if (hiddenPositions.Count == 0)
                break;

            int randomIndex =
                Random.Range(
                    0,
                    hiddenPositions.Count
                );

            int position =
                hiddenPositions[randomIndex];

            hiddenPositions.RemoveAt(randomIndex);

            revealedPassword[position] =
                currentPassword[position];
        }

        Debug.Log(
            "PASSWORD SOLVER: " +
            amountToReveal +
            " characters revealed."
        );
    }

    // =========================================================
    // PASSWORD SOLVER - REVEAL NEXT CHARACTER
    // =========================================================

    private void RevealNextCharacter()
    {
        List<int> hiddenPositions =
            new List<int>();

        for (int i = 0;
             i < revealedPassword.Length;
             i++)
        {
            if (revealedPassword[i] == '_')
            {
                hiddenPositions.Add(i);
            }
        }

        if (hiddenPositions.Count == 0)
            return;

        int randomIndex =
            Random.Range(
                0,
                hiddenPositions.Count
            );

        int position =
            hiddenPositions[randomIndex];

        revealedPassword[position] =
            currentPassword[position];

        UpdateWordDisplay();

        messageText.text =
            "PASSWORD SOLVER FOUND A CHARACTER!";

        Debug.Log(
            "PASSWORD SOLVER: Revealed character at position " +
            position
        );
    }

    // =========================================================
    // CHARACTER BUTTONS
    // =========================================================

    private void GenerateCharacterButtons()
    {
        ClearButtons();

        List<char> characters =
            new List<char>();

        // A-Z
        for (char c = 'A'; c <= 'Z'; c++)
        {
            characters.Add(c);
        }

        // 0-9
        for (char c = '0'; c <= '9'; c++)
        {
            characters.Add(c);
        }

        foreach (char character in characters)
        {
            GameObject newButton =
                Instantiate(
                    buttonPrefab,
                    buttonContainer
                );

            spawnedButtons.Add(newButton);

            TMP_Text buttonText =
                newButton.GetComponentInChildren<TMP_Text>();

            if (buttonText != null)
            {
                buttonText.text =
                    character.ToString();
            }

            Button button =
                newButton.GetComponent<Button>();

            if (button != null)
            {
                char selectedCharacter =
                    character;

                button.onClick.AddListener(() =>
                {
                    GuessCharacter(
                        selectedCharacter,
                        button
                    );
                });
            }
        }
    }

    // =========================================================
    // GUESS
    // =========================================================

    private void GuessCharacter(
        char guessedCharacter,
        Button clickedButton)
    {
        if (!eventActive)
            return;

        bool correct = false;

        for (int i = 0;
             i < currentPassword.Length;
             i++)
        {
            if (currentPassword[i] ==
                guessedCharacter)
            {
                revealedPassword[i] =
                    guessedCharacter;

                correct = true;
            }
        }

        clickedButton.interactable = false;

        if (correct)
        {
            messageText.text =
                "CORRECT!";
        }
        else
        {
            wrongAttempts++;

            messageText.text =
                "INCORRECT!";

            UpdateAttemptsDisplay();

            if (wrongAttempts >=
                maxWrongAttempts)
            {
                PasswordFailed();
                return;
            }
        }

        UpdateWordDisplay();

        if (IsPasswordComplete())
        {
            PasswordCracked();
        }
    }

    // =========================================================
    // DISPLAY
    // =========================================================

    private void UpdateWordDisplay()
    {
        string display = "";

        for (int i = 0;
             i < revealedPassword.Length;
             i++)
        {
            display += revealedPassword[i];

            if (i <
                revealedPassword.Length - 1)
            {
                display += " ";
            }
        }

        if (wordText != null)
        {
            wordText.text = display;
        }
    }

    private void UpdateAttemptsDisplay()
    {
        if (attemptsText != null)
        {
            attemptsText.text =
                "FAILED ATTEMPTS: " +
                wrongAttempts +
                " / " +
                maxWrongAttempts;
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text =
                "TIME: " +
                Mathf.CeilToInt(currentTime);
        }
    }

    // =========================================================
    // CHECK PASSWORD
    // =========================================================

    private bool IsPasswordComplete()
    {
        for (int i = 0;
             i < revealedPassword.Length;
             i++)
        {
            if (revealedPassword[i] == '_')
                return false;
        }

        return true;
    }

    // =========================================================
    // PASSWORD CRACKED
    // =========================================================

    private void PasswordCracked()
    {
        if (!eventActive)
            return;

        eventActive = false;
        passwordSolverActive = false;

        messageText.text =
            "PASSWORD CRACKED!";

        wordText.text =
            currentPassword;

        DisableButtons();

        Debug.Log(
            "PASSWORD CRACKER: Password cracked!"
        );

        if (eventManager != null)
        {
            eventManager.PasswordCracked();
        }

        Invoke(
            nameof(ClosePasswordCracker),
            1.5f
        );
    }

    // =========================================================
    // PASSWORD FAILED
    // =========================================================

    private void PasswordFailed()
    {
        if (!eventActive)
            return;

        eventActive = false;
        passwordSolverActive = false;

        messageText.text =
            "PASSWORD FAILED!";

        wordText.text =
            currentPassword;

        DisableButtons();

        Debug.Log(
            "PASSWORD CRACKER: Password failed!"
        );

        if (eventManager != null)
        {
            eventManager.PasswordFailed();
        }

        Invoke(
            nameof(RestartPasswordCracker),
            1f
        );
    }

    // =========================================================
    // RESTART
    // =========================================================

    private void RestartPasswordCracker()
    {
        ClearButtons();

        StartPasswordCracker();
    }

    // =========================================================
    // CLOSE
    // =========================================================

    private void ClosePasswordCracker()
    {
        passwordCrackerPanel.SetActive(false);

        ClearButtons();
    }

    // =========================================================
    // DISABLE BUTTONS
    // =========================================================

    private void DisableButtons()
    {
        foreach (GameObject buttonObject
                 in spawnedButtons)
        {
            if (buttonObject == null)
                continue;

            Button button =
                buttonObject.GetComponent<Button>();

            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    // =========================================================
    // CLEAR BUTTONS
    // =========================================================

    private void ClearButtons()
    {
        foreach (GameObject button
                 in spawnedButtons)
        {
            if (button != null)
            {
                Destroy(button);
            }
        }

        spawnedButtons.Clear();
    }
}