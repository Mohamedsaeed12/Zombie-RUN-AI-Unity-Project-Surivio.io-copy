using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private Label highScoreLabel;
    private Label scoreLabel;
    private Label healthLabel;
    private Button restartButton;

    public PlayerController playerController;
    public int highScore = 0;
    public int score = 0;

    private void OnEnable()
    {
        // Assuming this script is on a GameObject with a UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;

        // Retrieve UI elements using their names in the UXML
        highScoreLabel = rootVisualElement.Q<Label>("HighscoreLabel");
        scoreLabel = rootVisualElement.Q<Label>("ScoreLabel");
        healthLabel = rootVisualElement.Q<Label>("HealthLabel");
        restartButton = rootVisualElement.Q<Button>("RestartButton");

        // Set up the restart button click listener
        restartButton.clicked += RestartGame;

        // Initialize scores and health display
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreDisplay();
        UpdateScoreDisplay();
        UpdateHealthDisplay(playerController.CurrentHealth);
    }

    // Update the health display in the UI
    public void UpdateHealthDisplay(int currentHealth)
    {
        healthLabel.text = "Health: " + Mathf.Max(currentHealth, 0).ToString();
    }

    // Update the score display in the UI
    public void UpdateScoreDisplay()
    {
        scoreLabel.text = "Score: " + score.ToString();
    }

    // Update the high score display in the UI
    public void UpdateHighScoreDisplay()
    {
        highScoreLabel.text = "High Score: " + highScore.ToString();
    }

    // Call this method when the player's score increases
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();

        // Check for a new high score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreDisplay();
        }
    }

    // Restart the game
    private void RestartGame()
    {
        // Reset the time scale in case the game is paused
        Time.timeScale = 1;
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Call this method when the player dies
    public void PlayerDied()
    {
        // Freeze the game
        Time.timeScale = 0;
        // Display game over logic, disable player controls, etc.
    }
}
