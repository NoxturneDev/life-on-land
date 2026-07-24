using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    [Header("UI Panel Reference")]
    public GameObject pauseMenuPanel;

    private bool isPaused = false;
    private GameObject dialogWindow;
    private GameObject achievementsWindow;
    private Text[] achievementStatusTexts = new Text[3];

    private void Awake()
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
    }

    private void Start()
    {
        // Automatically set up the Pause Menu UI components on Canvas
        CreatePauseMenuUI();
    }

    private void Update()
    {
        bool escPressed = false;
#if ENABLE_INPUT_SYSTEM
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb != null && kb.escapeKey.wasPressedThisFrame)
        {
            escPressed = true;
        }
#else
        try
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                escPressed = true;
            }
        }
        catch (System.Exception) { }
#endif

        if (escPressed)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        if (achievementsWindow != null) achievementsWindow.SetActive(false);
        if (dialogWindow != null) dialogWindow.SetActive(true);

        Time.timeScale = 1.0f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Pause()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        Time.timeScale = 0.0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowAchievements()
    {
        if (dialogWindow != null) dialogWindow.SetActive(false);
        if (achievementsWindow != null)
        {
            achievementsWindow.SetActive(true);
            UpdateAchievementsList();
        }
    }

    public void HideAchievements()
    {
        if (achievementsWindow != null) achievementsWindow.SetActive(false);
        if (dialogWindow != null) dialogWindow.SetActive(true);
    }

    public void RestartStage()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting to Main Menu...");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenuScene");
    }

    private bool GetStage1Field(string fieldName)
    {
        if (Stage1Manager.Instance != null)
        {
            var field = typeof(Stage1Manager).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                bool val = (bool)field.GetValue(Stage1Manager.Instance);
                if (val) return true;
            }
        }

        // Fallback to PlayerPrefs
        if (fieldName == "openingPlayed") return PlayerPrefs.GetInt("Achievement_FirstSteps", 0) == 1;
        if (fieldName == "waterQuestCompleted") return PlayerPrefs.GetInt("Achievement_WaterBearer", 0) == 1;
        if (fieldName == "stageCompleted") return PlayerPrefs.GetInt("Achievement_GreenOasis", 0) == 1;

        return false;
    }

    private void UpdateAchievementsList()
    {
        if (achievementStatusTexts == null || achievementStatusTexts.Length < 3) return;

        bool firstSteps = GetStage1Field("openingPlayed");
        bool waterBearer = GetStage1Field("waterQuestCompleted");
        bool greenOasis = GetStage1Field("stageCompleted");

        achievementStatusTexts[0].text = "First Steps: " + (firstSteps ? "<color=#55FF55>[COMPLETED]</color>" : "<color=#FF5555>[LOCKED]</color>") + "\n<color=#BBBBBB>Talk to Maliz the Bear.</color>";
        achievementStatusTexts[1].text = "Water Bearer: " + (waterBearer ? "<color=#55FF55>[COMPLETED]</color>" : "<color=#FF5555>[LOCKED]</color>") + "\n<color=#BBBBBB>Deliver 10 cans of water to Maliz.</color>";
        achievementStatusTexts[2].text = "Green Oasis: " + (greenOasis ? "<color=#55FF55>[COMPLETED]</color>" : "<color=#FF5555>[LOCKED]</color>") + "\n<color=#BBBBBB>Restore the oasis to 50% O2.</color>";
    }

    private void CreatePauseMenuUI()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null) return;

        Transform existing = canvas.transform.Find("PauseMenuPanel");
        if (existing != null)
        {
            if (Application.isPlaying) Destroy(existing.gameObject);
            else DestroyImmediate(existing.gameObject);
        }

        // Screen-space overlay panel
        GameObject panelGo = new GameObject("PauseMenuPanel", typeof(RectTransform));
        panelGo.transform.SetParent(canvas.transform, false);
        
        RectTransform rt = panelGo.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;

        Image bgImg = panelGo.AddComponent<Image>();
        bgImg.color = new Color(0.0f, 0.0f, 0.0f, 0.65f); // semi-transparent dim

        // 1. Dialog Window container (Pause buttons)
        GameObject dialogGo = new GameObject("DialogWindow", typeof(RectTransform));
        dialogGo.transform.SetParent(panelGo.transform, false);
        dialogWindow = dialogGo;

        RectTransform dialogRt = dialogGo.GetComponent<RectTransform>();
        dialogRt.anchorMin = new Vector2(0.5f, 0.5f);
        dialogRt.anchorMax = new Vector2(0.5f, 0.5f);
        dialogRt.pivot = new Vector2(0.5f, 0.5f);
        dialogRt.sizeDelta = new Vector2(240f, 240f);

        Image dialogBg = dialogGo.AddComponent<Image>();
        dialogBg.sprite = Resources.Load<Sprite>("UI/Pixel/panel_frame");
        if (dialogBg.sprite == null) dialogBg.sprite = UIManager.Instance?.staminaBarFill?.transform.parent?.GetComponent<Image>()?.sprite;
        dialogBg.type = Image.Type.Sliced;
        dialogBg.color = new Color(0.2f, 0.16f, 0.19f, 0.95f); // cozy cozy violet panel tint

        // Title
        GameObject titleGo = new GameObject("Title", typeof(RectTransform), typeof(Text));
        titleGo.transform.SetParent(dialogGo.transform, false);
        RectTransform titleRt = titleGo.GetComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0f, 1f);
        titleRt.anchorMax = new Vector2(1f, 1f);
        titleRt.pivot = new Vector2(0.5f, 1f);
        titleRt.anchoredPosition = new Vector2(0f, -12f);
        titleRt.sizeDelta = new Vector2(0f, 25f);
        titleRt.localScale = Vector3.one;

        Text titleText = titleGo.GetComponent<Text>();
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (titleText.font == null) titleText.font = UIManager.Instance?.staminaText?.font;
        titleText.text = "GAME PAUSED";
        titleText.fontSize = 22; // Matches Dialog UI header sizes
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(0.95f, 0.82f, 0.45f, 1.0f); // warm cozy gold
        var titleShadow = titleGo.AddComponent<Shadow>();
        titleShadow.effectColor = Color.black;
        titleShadow.effectDistance = new Vector2(1f, -1f);

        // Vertical layout container
        GameObject layoutGo = new GameObject("ButtonLayout", typeof(RectTransform), typeof(VerticalLayoutGroup));
        layoutGo.transform.SetParent(dialogGo.transform, false);
        RectTransform layoutRt = layoutGo.GetComponent<RectTransform>();
        layoutRt.anchorMin = Vector2.zero;
        layoutRt.anchorMax = Vector2.one;
        layoutRt.offsetMin = new Vector2(20f, 20f);
        layoutRt.offsetMax = new Vector2(-20f, -40f);

        VerticalLayoutGroup vlg = layoutGo.GetComponent<VerticalLayoutGroup>();
        vlg.spacing = 8f;
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        string[] btnLabels = { "Resume", "Achievements", "Restart Stage", "Exit Game" };
        System.Action[] btnActions = {
            Resume,
            ShowAchievements,
            RestartStage,
            ExitGame
        };

        for (int i = 0; i < btnLabels.Length; i++)
        {
            string labelName = btnLabels[i];
            System.Action action = btnActions[i];

            GameObject btnGo = new GameObject($"Button_{labelName}", typeof(RectTransform), typeof(Image), typeof(Button));
            btnGo.transform.SetParent(layoutGo.transform, false);
            
            var layoutElement = btnGo.AddComponent<LayoutElement>();
            layoutElement.minHeight = 28f;

            Image btnImg = btnGo.GetComponent<Image>();
            btnImg.sprite = Resources.Load<Sprite>("UI/Pixel/slot_frame");
            if (btnImg.sprite == null) btnImg.sprite = UIManager.Instance?.staminaBarFill?.transform.parent?.GetComponent<Image>()?.sprite;
            btnImg.type = Image.Type.Sliced;
            btnImg.color = new Color(0.32f, 0.25f, 0.29f, 1.0f);

            Button btn = btnGo.GetComponent<Button>();
            btn.targetGraphic = btnImg;
            btn.onClick.AddListener(() => action());

            GameObject btnTextGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
            btnTextGo.transform.SetParent(btnGo.transform, false);
            RectTransform btnTextRt = btnTextGo.GetComponent<RectTransform>();
            btnTextRt.anchorMin = Vector2.zero;
            btnTextRt.anchorMax = Vector2.one;
            btnTextRt.sizeDelta = Vector2.zero;
            btnTextRt.localScale = Vector3.one;

            Text btnTxt = btnTextGo.GetComponent<Text>();
            btnTxt.font = titleText.font;
            btnTxt.text = labelName;
            btnTxt.fontSize = 15; // Clean, readable button labels
            btnTxt.alignment = TextAnchor.MiddleCenter;
            btnTxt.color = Color.white;
            var btnShadow = btnTextGo.AddComponent<Shadow>();
            btnShadow.effectColor = Color.black;
            btnShadow.effectDistance = new Vector2(1f, -1f);
        }

        // 2. Achievements Window container (Initially Inactive)
        GameObject achGo = new GameObject("AchievementsWindow", typeof(RectTransform));
        achGo.transform.SetParent(panelGo.transform, false);
        achievementsWindow = achGo;
        achievementsWindow.SetActive(false);

        RectTransform achRt = achGo.GetComponent<RectTransform>();
        achRt.anchorMin = new Vector2(0.5f, 0.5f);
        achRt.anchorMax = new Vector2(0.5f, 0.5f);
        achRt.pivot = new Vector2(0.5f, 0.5f);
        achRt.sizeDelta = new Vector2(280f, 280f); // slightly larger for readability

        Image achBg = achGo.AddComponent<Image>();
        achBg.sprite = dialogBg.sprite;
        achBg.type = Image.Type.Sliced;
        achBg.color = dialogBg.color;

        // Achievements Title
        GameObject achTitleGo = new GameObject("Title", typeof(RectTransform), typeof(Text));
        achTitleGo.transform.SetParent(achGo.transform, false);
        RectTransform achTitleRt = achTitleGo.GetComponent<RectTransform>();
        achTitleRt.anchorMin = new Vector2(0f, 1f);
        achTitleRt.anchorMax = new Vector2(1f, 1f);
        achTitleRt.pivot = new Vector2(0.5f, 1f);
        achTitleRt.anchoredPosition = new Vector2(0f, -12f);
        achTitleRt.sizeDelta = new Vector2(0f, 25f);
        achTitleRt.localScale = Vector3.one;

        Text achTitleText = achTitleGo.GetComponent<Text>();
        achTitleText.font = titleText.font;
        achTitleText.text = "ACHIEVEMENTS";
        achTitleText.fontSize = 22; // Matches Main Header Size
        achTitleText.fontStyle = FontStyle.Bold;
        achTitleText.alignment = TextAnchor.MiddleCenter;
        achTitleText.color = titleText.color;
        var achTitleShadow = achTitleGo.AddComponent<Shadow>();
        achTitleShadow.effectColor = Color.black;
        achTitleShadow.effectDistance = new Vector2(1f, -1f);

        // Achievements Vertical layout list
        GameObject achLayoutGo = new GameObject("AchievementsLayout", typeof(RectTransform), typeof(VerticalLayoutGroup));
        achLayoutGo.transform.SetParent(achGo.transform, false);
        RectTransform achLayoutRt = achLayoutGo.GetComponent<RectTransform>();
        achLayoutRt.anchorMin = Vector2.zero;
        achLayoutRt.anchorMax = Vector2.one;
        achLayoutRt.offsetMin = new Vector2(20f, 50f); // leave space for Back button at bottom
        achLayoutRt.offsetMax = new Vector2(-20f, -40f);

        VerticalLayoutGroup achVlg = achLayoutGo.GetComponent<VerticalLayoutGroup>();
        achVlg.spacing = 8f;
        achVlg.childAlignment = TextAnchor.UpperCenter;
        achVlg.childControlWidth = true;
        achVlg.childControlHeight = true;
        achVlg.childForceExpandWidth = true;
        achVlg.childForceExpandHeight = false;

        // Populate list rows
        for (int i = 0; i < 3; i++)
        {
            GameObject rowGo = new GameObject($"Status_{i}", typeof(RectTransform), typeof(Text));
            rowGo.transform.SetParent(achLayoutGo.transform, false);
            var layoutElement = rowGo.AddComponent<LayoutElement>();
            layoutElement.minHeight = 60f; // fits double line cleanly

            RectTransform rowRt = rowGo.GetComponent<RectTransform>();
            rowRt.localScale = Vector3.one;

            Text rowTxt = rowGo.GetComponent<Text>();
            rowTxt.font = titleText.font;
            rowTxt.fontSize = 14; // Matches general dialogue / HUD details size
            rowTxt.supportRichText = true;
            rowTxt.alignment = TextAnchor.MiddleLeft;
            rowTxt.color = Color.white;
            rowTxt.text = "Loading Achievement...";
            var rowShadow = rowGo.AddComponent<Shadow>();
            rowShadow.effectColor = Color.black;
            rowShadow.effectDistance = new Vector2(1f, -1f);

            achievementStatusTexts[i] = rowTxt;
        }

        // Achievements Back Button
        GameObject backBtnGo = new GameObject("Button_Back", typeof(RectTransform), typeof(Image), typeof(Button));
        backBtnGo.transform.SetParent(achGo.transform, false);
        RectTransform backBtnRt = backBtnGo.GetComponent<RectTransform>();
        backBtnRt.anchorMin = new Vector2(0.5f, 0f);
        backBtnRt.anchorMax = new Vector2(0.5f, 0f);
        backBtnRt.pivot = new Vector2(0.5f, 0f);
        backBtnRt.anchoredPosition = new Vector2(0f, 12f);
        backBtnRt.sizeDelta = new Vector2(100f, 28f);

        Image backBtnImg = backBtnGo.GetComponent<Image>();
        backBtnImg.sprite = Resources.Load<Sprite>("UI/Pixel/slot_frame");
        if (backBtnImg.sprite == null) backBtnImg.sprite = dialogBg.sprite;
        backBtnImg.type = Image.Type.Sliced;
        backBtnImg.color = new Color(0.32f, 0.25f, 0.29f, 1.0f);

        Button backBtn = backBtnGo.GetComponent<Button>();
        backBtn.targetGraphic = backBtnImg;
        backBtn.onClick.AddListener(HideAchievements);

        GameObject backTextGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
        backTextGo.transform.SetParent(backBtnGo.transform, false);
        RectTransform backTextRt = backTextGo.GetComponent<RectTransform>();
        backTextRt.anchorMin = Vector2.zero;
        backTextRt.anchorMax = Vector2.one;
        backTextRt.sizeDelta = Vector2.zero;
        backTextRt.localScale = Vector3.one;

        Text backTxt = backTextGo.GetComponent<Text>();
        backTxt.font = titleText.font;
        backTxt.text = "Back";
        backTxt.fontSize = 15; // Matches buttons
        backTxt.alignment = TextAnchor.MiddleCenter;
        backTxt.color = Color.white;
        var backShadow = backTextGo.AddComponent<Shadow>();
        backShadow.effectColor = Color.black;
        backShadow.effectDistance = new Vector2(1f, -1f);

        pauseMenuPanel = panelGo;
        pauseMenuPanel.SetActive(false);
    }
}
