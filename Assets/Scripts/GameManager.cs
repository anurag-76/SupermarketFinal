using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public GameObject scoreUIParent;

    public int currentScore = 0;
    public bool IsPlayerInArea { get; private set; } = false;

    private void Start()
    {
        if (scoreUIParent != null)
            scoreUIParent.SetActive(false);
        UpdateScoreUI();
    }

    public void PlayerEnteredArea()
    {
        IsPlayerInArea = true;
        if (scoreUIParent != null) scoreUIParent.SetActive(true);
    }

    public void PlayerExitedArea()
    {
        IsPlayerInArea = false;
        if (scoreUIParent != null) scoreUIParent.SetActive(false);
    }

    public void AddScore(int points)
    {
        if (!IsPlayerInArea) return;
        currentScore += points;
        UpdateScoreUI();
        Debug.Log($"Score +{points} | Total: {currentScore}");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();
    }
}