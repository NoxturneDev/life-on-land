using UnityEngine;
using UnityEngine.UI;

// Full-screen end-of-stage panel. Stays open (no auto-dismiss) since it marks the end
// of the currently playable content.
public class VictoryUI : MonoBehaviour
{
    public static VictoryUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject panel;
    public Text titleText;
    public Text bodyText;
    public Button mainMenuButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (panel != null) panel.SetActive(false);
    }

    private void Start()
    {
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(ExitToMainMenu);
        }
    }

    public void ExitToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }

    public void Show(string title, string body)
    {
        if (panel != null) panel.SetActive(true);
        if (titleText != null) titleText.text = title;
        if (bodyText != null) bodyText.text = body;

        // Freeze the player in place for the ending beat
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
            player.enabled = false;
        }
    }
}
