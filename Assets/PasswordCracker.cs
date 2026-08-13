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

    private List<GameObject> spawnedButtons =
        new List<GameObject>();

    private EventManager eventManager;

    private void Start()
    {
        if (passwordCrackerPanel != null)
            passwordCrackerPanel.SetActive(false);

        eventManager = FindFirstObjectByType<EventManager>();
    }

    private void Update()
    {
        if (!eventActive)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;

            UpdateTimerDisplay();

            PasswordFailed();

            return;
        }

        UpdateTimerDisplay();
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

        currentPassword =
            passwords[Random.Range(0, passwords.Length)].ToUpper();

        revealedPassword =
            new char[currentPassword.Length];

        for (int i = 0; i < revealedPassword.Length; i++)
        {
            revealedPassword[i] = '_';
        }

        passwordCrackerPanel.SetActive(true);

        passwordText.text = "PASSWORD";

        messageText.text = "CRACK THE PASSWORD";

        UpdateWordDisplay();
        UpdateAttemptsDisplay();
        UpdateTimerDisplay();

        GenerateCharacterButtons();

        Debug.Log(
            "PASSWORD CRACKER: New password selected: " +
            currentPassword
        );
    }

    // =========================================================
    // CHARACTER BUTTONS
    // =========================================================

    private void GenerateCharacterButtons()
{
    ClearButtons();

    List<char> characters = new List<char>();

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

    // Create buttons in order
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
            buttonText.text = character.ToString();
        }

        Button button =
            newButton.GetComponent<Button>();

        if (button != null)
        {
            char selectedCharacter = character;

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

            if (wrongAttempts >= maxWrongAttempts)
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

        wordText.text = display;
    }

    private void UpdateAttemptsDisplay()
    {
        attemptsText.text =
            "FAILED ATTEMPTS: " +
            wrongAttempts +
            " / " +
            maxWrongAttempts;
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
    eventActive = false;

    messageText.text = "PASSWORD CRACKED!";
    wordText.text = currentPassword;

    DisableButtons();

    Debug.Log("PASSWORD CRACKER: Password cracked!");

    if (eventManager != null)
    {
        eventManager.PasswordCracked();
    }

    Invoke(nameof(ClosePasswordCracker), 1.5f);
}

private void PasswordFailed()
{
    if (!eventActive)
        return;

    eventActive = false;

    messageText.text = "PASSWORD FAILED!";
    wordText.text = currentPassword;

    DisableButtons();

    Debug.Log("PASSWORD CRACKER: Password failed!");

    if (eventManager != null)
    {
        eventManager.PasswordFailed();
    }

    Invoke(nameof(RestartPasswordCracker), 1f);
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