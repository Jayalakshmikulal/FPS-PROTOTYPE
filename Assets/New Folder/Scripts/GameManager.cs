using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score")]
    public int score = 0;
    public TMP_Text scoreText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    [Header("Win UI")]
    public GameObject winPanel;
    public TMP_Text winScoreText;

    [Header("Gameplay UI")]
    public GameObject waveTextObject;
    public TMP_Text waveText;

    [Header("Systems")]
    public EnemySpawner enemySpawner;

    public bool IsGameOver { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateScoreUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        SetWaveTextVisible(true);
    }

    public void AddScore(int points)
    {
        if (IsGameOver)
        {
            return;
        }

        score += points;
        UpdateScoreUI();
    }

    public void GameOver()
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;

        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
        }

        SetWaveTextVisible(false);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.SetAsLastSibling();
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "FINAL SCORE " + score;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;

        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
        }

        SetWaveTextVisible(false);

        if (winPanel != null)
        {
            winPanel.SetActive(true);
            winPanel.transform.SetAsLastSibling();
        }

        if (winScoreText != null)
        {
            winScoreText.text = "FINAL SCORE " + score;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetWaveTextVisible(bool isVisible)
    {
        if (waveTextObject != null)
        {
            waveTextObject.SetActive(isVisible);
        }

        if (waveText != null)
        {
            waveText.gameObject.SetActive(isVisible);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE " + score;
        }
    }
}