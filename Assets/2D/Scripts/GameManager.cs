using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public GameObject menuUI;
    public GameObject mainMenuPanel;
    public GameObject inGamePanel;
    public GameObject lossUI;
    public GameObject winUI;
    public TMP_Text heightText;
    public TMP_Text timerText;
    public TMP_Text healthText; // New health text reference

    [Header("Audio Settings")]
    public AudioClip backgroundMusic;
    private AudioSource audioSource;

    [Header("Game Settings")]
    public CharacterController2D player;
    public float startHeight;

    private float gameTimer;
    private bool isGameActive;
    private float highestY;
    private Health playerHealth; // Reference to player's Health component

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        Time.timeScale = 1f;
        if (player != null)
        {
            startHeight = player.transform.position.y;
            playerHealth = player.GetComponent<Health>(); // Get Health component
        }
        ShowMainMenu();

        if (!audioSource.isPlaying) audioSource.Play();
    }

    void Update()
    {
        if (isGameActive)
        {
            gameTimer += Time.deltaTime;
            UpdateGameUI();
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        menuUI.SetActive(true);
        mainMenuPanel.SetActive(false);
        inGamePanel.SetActive(true);
        lossUI.SetActive(false);
        winUI.SetActive(false);

        if (player != null)
        {
            player.enabled = true;
            startHeight = player.transform.position.y;
            playerHealth = player.GetComponent<Health>(); // Refresh reference
        }

        gameTimer = 0f;
        highestY = 0;
        Time.timeScale = 1f;
    }

    public void OnPlayerDeath()
    {
        isGameActive = false;
        inGamePanel.SetActive(false);
        lossUI.SetActive(true);
        if (player != null) player.enabled = false;
        Time.timeScale = 0f;
    }

    public void OnPlayerWin()
    {
        isGameActive = false;
        inGamePanel.SetActive(false);
        winUI.SetActive(true);
        if (player != null) player.enabled = false;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        player = FindObjectOfType<CharacterController2D>(); // Refresh player reference
        InitializeGame();
    }

    public void ShowMainMenu()
    {
        menuUI.SetActive(true);
        mainMenuPanel.SetActive(true);
        inGamePanel.SetActive(false);
        lossUI.SetActive(false);
        winUI.SetActive(false);
        if (player != null) player.enabled = false;
        Time.timeScale = 0f;
    }

    private void UpdateGameUI()
    {
        // Update height display
        if (player != null)
        {
            float currentHeight = player.transform.position.y - startHeight;
            if (currentHeight > highestY)
            {
                highestY = currentHeight;
            }
            heightText.text = $"Height: {highestY:F1}m";
        }

        // Update timer display
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";

        // Update health display
        if (playerHealth != null)
        {
            healthText.text = $"Health: {playerHealth.GetCurrentHealth():F0}";
        }
        else
        {
            healthText.text = "Health: --";
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }

    public void ToggleMusic(bool state)
    {
        audioSource.mute = !state;
    }
}