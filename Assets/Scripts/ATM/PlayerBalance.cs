using UnityEngine;
using TMPro; // Use TextMeshPro for better text

public class PlayerBalance : MonoBehaviour
{
    public float currentBalance = 5000f; // Starting money
    public TMP_Text hudBalanceText; // Assign in Inspector (Top Right HUD)

    void Start()
    {
        UpdateHUD();
    }

    public void Withdraw(float amount)
    {
        if (amount <= currentBalance)
        {
            currentBalance -= amount;
            UpdateHUD();
            Debug.Log($"Withdrew ${amount}. New balance: ${currentBalance}");
        }
        else
        {
            Debug.Log("Insufficient funds!");
        }
    }

    public void UpdateHUD()
    {
        if (hudBalanceText != null)
            hudBalanceText.text = $"Balance: ${currentBalance:N0}";
    }
}