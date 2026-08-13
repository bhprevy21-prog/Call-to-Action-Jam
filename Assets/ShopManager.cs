using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Shop")]
    public GameObject shopPanel;

    [Header("Shop Balance")]
    public TMP_Text shopBalanceText;

    [Header("Cookie Clicker")]
    public CookieClicker cookieClicker;

    // =========================================================
    // HACKER HELPER
    // =========================================================

    [Header("Hacker Helper")]
    public Button hackerHelperButton;
    public TMP_Text hackerHelperOwnedText;
    public TMP_Text hackerHelperPriceText;

    public int hackerHelperPrice = 100;
    public int hackerHelperOwned = 0;
    public int maxHackerHelpers = 5;

    // =========================================================
    // POPUP CLOSER
    // =========================================================

    [Header("Popup Closer")]
    public Button popupCloserButton;
    public TMP_Text popupCloserOwnedText;
    public TMP_Text popupCloserPriceText;

    public int maxPopupClosers = 5;
    public int popupCloserPrice = 250;

    // =========================================================
    // VIRABUSTER
    // =========================================================

    [Header("Virabuster")]
    public Button virabusterButton;
    public TMP_Text virabusterOwnedText;
    public TMP_Text virabusterPriceText;

    public int maxVirabusters = 5;
    public int virabusterPrice = 500;

    // =========================================================
    // EVENT MANAGER
    // =========================================================

    public EventManager eventManager;

// =========================================================
// PASSWORD SOLVER
// =========================================================

[Header("Password Solver")]
public Button passwordSolverButton;
public TMP_Text passwordSolverOwnedText;
public TMP_Text passwordSolverPriceText;

public int maxPasswordSolvers = 5;
public int passwordSolverPrice = 750;

// =========================================================
// SCAM KILLER
// =========================================================

[Header("Scam Killer")]
public Button scamKillerButton;
public TMP_Text scamKillerOwnedText;
public TMP_Text scamKillerPriceText;

public int scamKillerPrice = 1000;
public int maxScamKillers = 1;

// =========================================================
// FILE MANAGER
// =========================================================

[Header("File Manager")]
public Button fileManagerButton;
public TMP_Text fileManagerOwnedText;
public TMP_Text fileManagerPriceText;

public int fileManagerPrice = 1500;
public int maxFileManagers = 1;

    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        shopPanel.SetActive(false);

        UpdateShopUI();

        // Hacker Helper
        hackerHelperButton.onClick.AddListener(
            BuyHackerHelper
        );

        // Popup Closer
        popupCloserButton.onClick.AddListener(
            BuyPopupCloser
        );

        // Virabuster
        virabusterButton.onClick.AddListener(
            BuyVirabuster
        );

        

        passwordSolverButton.onClick.AddListener(
    BuyPasswordSolver
);

       scamKillerButton.onClick.AddListener(
    BuyScamKiller
);

        fileManagerButton.onClick.AddListener(
    BuyFileManager
);
    }

    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        if (shopPanel.activeSelf)
        {
            UpdateShopUI();
        }
    }

    // =========================================================
    // OPEN / CLOSE SHOP
    // =========================================================

    public void OpenShop()
    {
        shopPanel.SetActive(true);

        UpdateShopUI();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    // =========================================================
    // BUY HACKER HELPER
    // =========================================================

    private void BuyHackerHelper()
    {
        if (cookieClicker == null)
        {
            Debug.LogError(
                "SHOP ERROR: CookieClicker is NOT assigned!"
            );

            return;
        }

        int currentScore =
            cookieClicker.GetCurrentScore();

        if (hackerHelperOwned >= maxHackerHelpers)
        {
            Debug.Log(
                "SHOP: Hacker Helper is already at maximum: " +
                hackerHelperOwned +
                "/" +
                maxHackerHelpers
            );

            return;
        }

        if (currentScore < hackerHelperPrice)
        {
            Debug.Log(
                "SHOP: Hacker Helper purchase failed. " +
                "Not enough score! " +
                "Balance: " +
                currentScore +
                " | Price: " +
                hackerHelperPrice
            );

            return;
        }

        bool purchased =
            cookieClicker.SpendCurrentScore(
                hackerHelperPrice
            );

        if (!purchased)
        {
            Debug.LogError(
                "SHOP ERROR: Hacker Helper purchase failed."
            );

            return;
        }

        hackerHelperOwned++;

        Debug.Log(
            "SHOP SUCCESS: Hacker Helper purchased! " +
            "Price: " +
            hackerHelperPrice +
            " | New Balance: " +
            cookieClicker.GetCurrentScore() +
            " | Owned: " +
            hackerHelperOwned +
            "/" +
            maxHackerHelpers
        );

        UpdateShopUI();
    }

    // =========================================================
    // BUY POPUP CLOSER
    // =========================================================

    private void BuyPopupCloser()
    {
        if (cookieClicker == null)
        {
            Debug.LogError(
                "SHOP ERROR: CookieClicker is NOT assigned!"
            );

            return;
        }

        if (eventManager == null)
        {
            Debug.LogError(
                "SHOP ERROR: EventManager is NOT assigned!"
            );

            return;
        }

        if (eventManager.popupClosersOwned >= maxPopupClosers)
        {
            Debug.Log(
                "SHOP: Popup Closer is already at maximum: " +
                eventManager.popupClosersOwned +
                "/" +
                maxPopupClosers
            );

            return;
        }

        int currentScore =
            cookieClicker.GetCurrentScore();

        if (currentScore < popupCloserPrice)
        {
            Debug.Log(
                "SHOP: Popup Closer purchase failed. " +
                "Not enough score! " +
                "Balance: " +
                currentScore +
                " | Price: " +
                popupCloserPrice
            );

            return;
        }

        bool purchased =
            cookieClicker.SpendCurrentScore(
                popupCloserPrice
            );

        if (!purchased)
        {
            Debug.LogError(
                "SHOP ERROR: Popup Closer purchase failed."
            );

            return;
        }

        eventManager.popupClosersOwned++;

        Debug.Log(
            "SHOP SUCCESS: Popup Closer purchased! " +
            "Price: " +
            popupCloserPrice +
            " | New Balance: " +
            cookieClicker.GetCurrentScore() +
            " | Owned: " +
            eventManager.popupClosersOwned +
            "/" +
            maxPopupClosers
        );

        UpdateShopUI();
    }

    // =========================================================
    // BUY VIRABUSTER
    // =========================================================

    private void BuyVirabuster()
    {
        if (cookieClicker == null)
        {
            Debug.LogError(
                "SHOP ERROR: CookieClicker is NOT assigned!"
            );

            return;
        }

        if (eventManager == null)
        {
            Debug.LogError(
                "SHOP ERROR: EventManager is NOT assigned!"
            );

            return;
        }

        if (eventManager.virabustersOwned >= maxVirabusters)
        {
            Debug.Log(
                "SHOP: Virabuster is already at maximum: " +
                eventManager.virabustersOwned +
                "/" +
                maxVirabusters
            );

            return;
        }

        int currentScore =
            cookieClicker.GetCurrentScore();

        if (currentScore < virabusterPrice)
        {
            Debug.Log(
                "SHOP: Virabuster purchase failed. " +
                "Not enough score! " +
                "Balance: " +
                currentScore +
                " | Price: " +
                virabusterPrice
            );

            return;
        }

        bool purchased =
            cookieClicker.SpendCurrentScore(
                virabusterPrice
            );

        if (!purchased)
        {
            Debug.LogError(
                "SHOP ERROR: Virabuster purchase failed."
            );

            return;
        }

        eventManager.virabustersOwned++;

        Debug.Log(
            "SHOP SUCCESS: Virabuster purchased! " +
            "Price: " +
            virabusterPrice +
            " | New Balance: " +
            cookieClicker.GetCurrentScore() +
            " | Owned: " +
            eventManager.virabustersOwned +
            "/" +
            maxVirabusters
        );

        UpdateShopUI();
    }

    // =========================================================
    // OTHER ITEMS
    // =========================================================

    private void DoNothing()
    {
        Debug.Log(
            "This shop item isn't implemented yet."
        );
    }

    // =========================================================
    // SHOP UI
    // =========================================================

    private void UpdateShopUI()
    {
        // -------------------------
        // Balance
        // -------------------------

        if (cookieClicker != null &&
            shopBalanceText != null)
        {
            shopBalanceText.text =
                "Balance: " +
                cookieClicker
                    .GetCurrentScore()
                    .ToString("N0");
        }

        // -------------------------
        // Hacker Helper
        // -------------------------

        if (hackerHelperOwnedText != null)
        {
            hackerHelperOwnedText.text =
                hackerHelperOwned +
                "/" +
                maxHackerHelpers;
        }

        if (hackerHelperPriceText != null)
        {
            hackerHelperPriceText.text =
                "$" +
                hackerHelperPrice.ToString("N0");
        }

        if (hackerHelperButton != null)
        {
            hackerHelperButton.interactable =
                hackerHelperOwned < maxHackerHelpers &&
                cookieClicker != null &&
                cookieClicker.GetCurrentScore()
                >= hackerHelperPrice;
        }

        // -------------------------
        // Popup Closer
        // -------------------------

        if (eventManager != null)
        {
            if (popupCloserOwnedText != null)
            {
                popupCloserOwnedText.text =
                    eventManager.popupClosersOwned +
                    "/" +
                    maxPopupClosers;
            }

            if (popupCloserPriceText != null)
            {
                popupCloserPriceText.text =
                    "$" +
                    popupCloserPrice.ToString("N0");
            }

            if (popupCloserButton != null)
            {
                popupCloserButton.interactable =
                    eventManager.popupClosersOwned
                    < maxPopupClosers &&
                    cookieClicker != null &&
                    cookieClicker.GetCurrentScore()
                    >= popupCloserPrice;
            }

            // -------------------------
// Virabuster
// -------------------------

if (virabusterOwnedText != null)
{
    virabusterOwnedText.text =
        eventManager.virabustersOwned +
        "/" +
        maxVirabusters;
}

if (virabusterPriceText != null)
{
    virabusterPriceText.text =
        "$" +
        virabusterPrice.ToString("N0");
}

if (virabusterButton != null)
{
    virabusterButton.interactable =
        eventManager.virabustersOwned
        < maxVirabusters &&
        cookieClicker != null &&
        cookieClicker.GetCurrentScore()
        >= virabusterPrice;
}

// -------------------------
// Password Solver
// -------------------------

if (passwordSolverOwnedText != null)
{
    passwordSolverOwnedText.text =
        eventManager.passwordSolversOwned +
        "/" +
        maxPasswordSolvers;
}

if (passwordSolverPriceText != null)
{
    passwordSolverPriceText.text =
        "$" +
        passwordSolverPrice.ToString("N0");
}

if (passwordSolverButton != null)
{
    passwordSolverButton.interactable =
        eventManager.passwordSolversOwned
        < maxPasswordSolvers &&
        cookieClicker != null &&
        cookieClicker.GetCurrentScore()
        >= passwordSolverPrice;
}

// -------------------------
// Scam Killer
// -------------------------

if (scamKillerOwnedText != null)
{
    scamKillerOwnedText.text =
        eventManager.scamKillersOwned +
        "/" +
        maxScamKillers;
}

if (scamKillerPriceText != null)
{
    scamKillerPriceText.text =
        "$" +
        scamKillerPrice.ToString("N0");
}

if (scamKillerButton != null)
{
    scamKillerButton.interactable =
        eventManager.scamKillersOwned
        < maxScamKillers &&
        cookieClicker != null &&
        cookieClicker.GetCurrentScore()
        >= scamKillerPrice;
}
        }
        // -------------------------
// File Manager
// -------------------------

if (eventManager != null)
{
    if (fileManagerOwnedText != null)
    {
        fileManagerOwnedText.text =
            eventManager.fileManagersOwned +
            "/" +
            maxFileManagers;
    }

    if (fileManagerPriceText != null)
    {
        fileManagerPriceText.text =
            "$" +
            fileManagerPrice.ToString("N0");
    }

    if (fileManagerButton != null)
    {
        fileManagerButton.interactable =
            eventManager.fileManagersOwned < maxFileManagers &&
            cookieClicker != null &&
            cookieClicker.GetCurrentScore()
            >= fileManagerPrice;
    }
}
    }
    // =========================================================
// BUY PASSWORD SOLVER
// =========================================================

private void BuyPasswordSolver()
{
    if (cookieClicker == null)
    {
        Debug.LogError(
            "SHOP ERROR: CookieClicker is NOT assigned!"
        );

        return;
    }

    if (eventManager == null)
    {
        Debug.LogError(
            "SHOP ERROR: EventManager is NOT assigned!"
        );

        return;
    }

    if (eventManager.passwordSolversOwned >= maxPasswordSolvers)
    {
        Debug.Log(
            "SHOP: Password Solver is already at maximum: " +
            eventManager.passwordSolversOwned +
            "/" +
            maxPasswordSolvers
        );

        return;
    }

    int currentScore =
        cookieClicker.GetCurrentScore();

    if (currentScore < passwordSolverPrice)
    {
        Debug.Log(
            "SHOP: Password Solver purchase failed. " +
            "Not enough score! " +
            "Balance: " +
            currentScore +
            " | Price: " +
            passwordSolverPrice
        );

        return;
    }

    bool purchased =
        cookieClicker.SpendCurrentScore(
            passwordSolverPrice
        );

    if (!purchased)
    {
        Debug.LogError(
            "SHOP ERROR: Password Solver purchase failed."
        );

        return;
    }

    eventManager.passwordSolversOwned++;

    Debug.Log(
        "SHOP SUCCESS: Password Solver purchased! " +
        "Price: " +
        passwordSolverPrice +
        " | New Balance: " +
        cookieClicker.GetCurrentScore() +
        " | Owned: " +
        eventManager.passwordSolversOwned +
        "/" +
        maxPasswordSolvers
    );

    UpdateShopUI();
}

// =========================================================
// BUY SCAM KILLER
// =========================================================

private void BuyScamKiller()
{
    if (cookieClicker == null)
    {
        Debug.LogError(
            "SHOP ERROR: CookieClicker is NOT assigned!"
        );

        return;
    }

    if (eventManager == null)
    {
        Debug.LogError(
            "SHOP ERROR: EventManager is NOT assigned!"
        );

        return;
    }

    if (eventManager.scamKillersOwned >= maxScamKillers)
    {
        Debug.Log(
            "SHOP: Scam Killer is already owned."
        );

        return;
    }

    int currentScore =
        cookieClicker.GetCurrentScore();

    if (currentScore < scamKillerPrice)
    {
        Debug.Log(
            "SHOP: Scam Killer purchase failed. " +
            "Not enough score! " +
            "Balance: " +
            currentScore +
            " | Price: " +
            scamKillerPrice
        );

        return;
    }

    bool purchased =
        cookieClicker.SpendCurrentScore(
            scamKillerPrice
        );

    if (!purchased)
    {
        Debug.LogError(
            "SHOP ERROR: Scam Killer purchase failed."
        );

        return;
    }

    eventManager.scamKillersOwned++;

    Debug.Log(
        "SHOP SUCCESS: Scam Killer purchased! " +
        "Price: " +
        scamKillerPrice +
        " | New Balance: " +
        cookieClicker.GetCurrentScore() +
        " | Owned: " +
        eventManager.scamKillersOwned +
        "/" +
        maxScamKillers
    );

    UpdateShopUI();

}
// =========================================================
// BUY FILE MANAGER
// =========================================================

private void BuyFileManager()
{
    if (cookieClicker == null)
    {
        Debug.LogError(
            "SHOP ERROR: CookieClicker is NOT assigned!"
        );

        return;
    }

    if (eventManager == null)
    {
        Debug.LogError(
            "SHOP ERROR: EventManager is NOT assigned!"
        );

        return;
    }

    if (eventManager.fileManagersOwned >= maxFileManagers)
    {
        Debug.Log(
            "SHOP: File Manager is already owned."
        );

        return;
    }

    int currentScore =
        cookieClicker.GetCurrentScore();

    if (currentScore < fileManagerPrice)
    {
        Debug.Log(
            "SHOP: File Manager purchase failed. " +
            "Not enough score! " +
            "Balance: " +
            currentScore +
            " | Price: " +
            fileManagerPrice
        );

        return;
    }

    bool purchased =
        cookieClicker.SpendCurrentScore(
            fileManagerPrice
        );

    if (!purchased)
    {
        Debug.LogError(
            "SHOP ERROR: File Manager purchase failed."
        );

        return;
    }

    eventManager.fileManagersOwned++;

    Debug.Log(
        "SHOP SUCCESS: File Manager purchased! " +
        "Price: " +
        fileManagerPrice +
        " | New Balance: " +
        cookieClicker.GetCurrentScore() +
        " | Owned: " +
        eventManager.fileManagersOwned +
        "/" +
        maxFileManagers
    );

    UpdateShopUI();
}
}