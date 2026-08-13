using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Shop")]
    public GameObject shopPanel;

    [Header("Hacker Helper")]
    public Button hackerHelperButton;
    public TMP_Text hackerHelperOwnedText;
    public TMP_Text hackerHelperPriceText;

    public int hackerHelperPrice = 100;
    public int hackerHelperOwned = 0;
    public int maxHackerHelpers = 5;

    [Header("Other Items")]
    public Button eventItem2Button;
    public Button eventItem3Button;
    public Button eventItem4Button;
    public Button eventItem5Button;
    public Button eventItem6Button;

    private void Start()
    {
        shopPanel.SetActive(false);

        UpdateShopUI();

        hackerHelperButton.onClick.AddListener(BuyHackerHelper);

        // These currently do nothing.
        eventItem2Button.onClick.AddListener(DoNothing);
        eventItem3Button.onClick.AddListener(DoNothing);
        eventItem4Button.onClick.AddListener(DoNothing);
        eventItem5Button.onClick.AddListener(DoNothing);
        eventItem6Button.onClick.AddListener(DoNothing);
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        UpdateShopUI();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    private void BuyHackerHelper()
    {
        if (hackerHelperOwned >= maxHackerHelpers)
        {
            return;
        }

        // We'll connect this to your actual score variable.
        // once we use the exact CookieClicker script.
        
        Debug.Log("Tried to buy Hacker Helper.");
    }

    private void DoNothing()
    {
        Debug.Log("This shop item isn't implemented yet.");
    }

    private void UpdateShopUI()
    {
        hackerHelperOwnedText.text =
            hackerHelperOwned + "/" + maxHackerHelpers;

        hackerHelperPriceText.text =
            "$" + hackerHelperPrice;

        hackerHelperButton.interactable =
            hackerHelperOwned < maxHackerHelpers;
    }
}