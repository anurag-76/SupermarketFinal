using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ATMUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject atmPanel;
    public GameObject promptPanel;

    [Header("UI Elements")]
    public TextMeshProUGUI balanceText;
    public TMP_InputField amountInput;
    public TextMeshProUGUI messageText;

    [Header("Buttons")]
    public Button withdrawButton;
    public Button depositButton;
    public Button closeButton;

    private PlayerBalance playerBalance;
    public bool IsOpen { get; private set; } = false;

    void Start()
    {
        // FindObjectOfType searches all GameObjects regardless of tag
        playerBalance = FindObjectOfType<PlayerBalance>();

        if (playerBalance == null)
            Debug.LogError("ATMUIManager: Could not find PlayerBalance anywhere in the scene!");

        withdrawButton.onClick.AddListener(Withdraw);
        depositButton.onClick.AddListener(Deposit);
        closeButton.onClick.AddListener(CloseATM);

        atmPanel.SetActive(false);
        //promptPanel.SetActive(false);
        messageText.text = "";
    }

    public void ShowPrompt(bool show)
    {
        promptPanel.SetActive(show);
    }

    public void OpenATM()
    {
        IsOpen = true;
        atmPanel.SetActive(true);
        amountInput.text = "";
        messageText.text = "";
        RefreshBalance();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Removed Time.timeScale = 0f — it blocks UI clicks
    }

    public void CloseATM()
    {
        IsOpen = false;
        atmPanel.SetActive(false);
        messageText.text = "";

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Withdraw()
    {
        if (playerBalance == null)
        {
            ShowMessage("Error: No player balance found!", Color.red);
            return;
        }

        if (!float.TryParse(amountInput.text, out float amount) || amount <= 0)
        {
            ShowMessage("Enter a valid amount.", Color.yellow);
            return;
        }

        if (amount > playerBalance.currentBalance)
        {
            ShowMessage("Insufficient funds.", Color.red);
            return;
        }

        playerBalance.Withdraw(amount);
        RefreshBalance();
        ShowMessage($"Withdrew ${amount:N0} successfully.", Color.green);
        amountInput.text = "";
    }

    void Deposit()
    {
        if (playerBalance == null)
        {
            ShowMessage("Error: No player balance found!", Color.red);
            return;
        }

        if (!float.TryParse(amountInput.text, out float amount) || amount <= 0)
        {
            ShowMessage("Enter a valid amount.", Color.yellow);
            return;
        }

        playerBalance.currentBalance += amount;
        playerBalance.UpdateHUD();
        RefreshBalance();
        ShowMessage($"Deposited ${amount:N0} successfully.", Color.green);
        amountInput.text = "";
    }

    void RefreshBalance()
    {
        if (playerBalance != null)
            balanceText.text = $"Balance: ${playerBalance.currentBalance:N0}";
        else
            balanceText.text = "Balance: ERROR";
    }

    void ShowMessage(string msg, Color color)
    {
        messageText.text = msg;
        messageText.color = color;
    }
}
