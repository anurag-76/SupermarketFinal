using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ATMUI : MonoBehaviour
{
    public GameObject atmPanel;           // Main ATM menu panel
    public TMP_Text screenBalanceText;    // Balance shown inside ATM screen
    public TMP_Text statusText;           // For messages like "Take your cash"

    public Button[] withdrawButtons;      // Buttons for 500, 1000, 2000, 5000 etc.
    public Button exitButton;

    private PlayerBalance currentPlayer;

    private void Start()
    {
        atmPanel.SetActive(false);

        // Setup buttons
        for (int i = 0; i < withdrawButtons.Length; i++)
        {
            int index = i;
            withdrawButtons[i].onClick.AddListener(() => WithdrawAmount(index));
        }

        if (exitButton != null)
            exitButton.onClick.AddListener(CloseATM);
    }

    public void OpenATM(PlayerBalance player)
    {
        currentPlayer = player;
        atmPanel.SetActive(true);
        UpdateBalanceDisplay();

        // Optional: Pause game or lock player movement
        // Time.timeScale = 0;
    }

    private void WithdrawAmount(int buttonIndex)
    {
        float[] amounts = { 500f, 1000f, 2000f, 5000f }; // Customize as needed

        if (buttonIndex < amounts.Length)
        {
            currentPlayer.Withdraw(amounts[buttonIndex]);
            UpdateBalanceDisplay();
            statusText.text = $"Withdrew ${amounts[buttonIndex]}";
            Invoke("ClearStatus", 2f);
        }
    }

    private void UpdateBalanceDisplay()
    {
        if (screenBalanceText != null && currentPlayer != null)
            screenBalanceText.text = $"Balance: ${currentPlayer.currentBalance:N0}";
    }

    private void ClearStatus()
    {
        if (statusText != null) statusText.text = "";
    }

    public void CloseATM()
    {
        atmPanel.SetActive(false);
        currentPlayer = null;
        // Time.timeScale = 1;
    }
}