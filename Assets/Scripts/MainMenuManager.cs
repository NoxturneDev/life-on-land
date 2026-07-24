using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    [Header("Menu Styling")]
    public Color titleColor = new Color(0.95f, 0.82f, 0.45f, 1.0f); // Cozy gold
    public Color subtitleColor = new Color(0.75f, 0.7f, 0.72f, 1.0f); // Muted purple-grey
    public Color panelBgColor = new Color(0.12f, 0.09f, 0.11f, 0.98f); // Deep cozy violet-black
    public Color buttonBgColor = new Color(0.32f, 0.25f, 0.29f, 1.0f);

    private GameObject mainMenuPanel;
    private GameObject achievementsPanel;
    private RectTransform titleTransform;
    private float titleStartY;

    private Text[] achievementStatusTexts = new Text[3];

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CreateMainMenuUI();
    }

    private void Update()
    {
        if (titleTransform != null)
        {
            titleTransform.anchoredPosition = new Vector2(0f, titleStartY + Mathf.Sin(Time.time * 2.5f) * 10f);
        }
    }

    public void StartGame()
    {
        Debug.Log("MainMenuManager: Starting Stage 1...");
        SceneManager.LoadScene("BasicScene");
    }

    public void ShowAchievements()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (achievementsPanel != null)
        {
            achievementsPanel.SetActive(true);
            UpdateAchievementsList();
        }
    }

    public void HideAchievements()
    {
        if (achievementsPanel != null) achievementsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("MainMenuManager: Quitting application...");
        Application.Quit();
    }

    private void UpdateAchievementsList()
    {
        if (achievementStatusTexts == null || achievementStatusTexts[0] == null) return;

        bool firstSteps = PlayerPrefs.GetInt("Achievement_FirstSteps", 0) == 1;
        bool waterBearer = PlayerPrefs.GetInt("Achievement_WaterBearer", 0) == 1;
        bool greenOasis = PlayerPrefs.GetInt("Achievement_GreenOasis", 0) == 1;

        achievementStatusTexts[0].text = "First Steps: " + (firstSteps ? "<color=#55FF55>[COMPLETED]</color>" : "<color=#FF5555>[LOCKED]</color>") + "\n<color=#BBBBBB>Talk to Maliz the Bear.</color>";
        achievementStatusTexts[1].text = "Water Bearer: " + (waterBearer ? "<color=#55FF55>[COMPLETED]</color>" : "<color=#FF5555>[LOCKED]</color>") + "\n<color=#BBBBBB>Deliver 10 cans of water to Maliz.</color>";
        achievementStatusTexts[2].text = "Green Oasis: " + (greenOasis ? "<color=#55FF55>[COMPLETED]</color>" : "<color=#FF5555>[LOCKED]</color>") + "\n<color=#BBBBBB>Restore the oasis to 50% O2.</color>";
    }

    private Font FindAvailableFont()
    {
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font != null) return font;

        font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        if (font != null) return font;

        Font[] allFonts = Resources.FindObjectsOfTypeAll<Font>();
        foreach (var f in allFonts)
        {
            if (f != null && !string.IsNullOrEmpty(f.name)) return f;
        }

        var ui = FindFirstObjectByType<UIManager>();
        if (ui != null && ui.staminaText != null) return ui.staminaText.font;

        return null;
    }

    private void CreateMainMenuUI()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGo = new GameObject("MainMenuCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
        }

        var existingEventSystem = FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>();
        if (existingEventSystem != null)
        {
            DestroyImmediate(existingEventSystem.gameObject);
        }

#if ENABLE_INPUT_SYSTEM
        GameObject esGo = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem));
        esGo.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
        new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
#endif

        Transform existingPanel = canvas.transform.Find("MainMenuPanel");
        if (existingPanel != null) DestroyImmediate(existingPanel.gameObject);
        Transform existingAch = canvas.transform.Find("AchievementsPanel");
        if (existingAch != null) DestroyImmediate(existingAch.gameObject);

        Font mainFont = FindAvailableFont();

        // Background image
        GameObject bgGo = new GameObject("MainMenuBg", typeof(RectTransform), typeof(Image));
        bgGo.transform.SetParent(canvas.transform, false);
        var bgRt = bgGo.GetComponent<RectTransform>();
        bgRt.anchorMin = Vector2.zero;
        bgRt.anchorMax = Vector2.one;
        bgRt.sizeDelta = Vector2.zero;
        var bgImg = bgGo.GetComponent<Image>();
        bgImg.color = new Color(0.08f, 0.06f, 0.07f, 1.0f);

        // Menu Panel container
        GameObject menuPanelGo = new GameObject("MainMenuPanel", typeof(RectTransform));
        menuPanelGo.transform.SetParent(canvas.transform, false);
        var menuRt = menuPanelGo.GetComponent<RectTransform>();
        menuRt.anchorMin = Vector2.zero;
        menuRt.anchorMax = Vector2.one;
        menuRt.sizeDelta = Vector2.zero;
        mainMenuPanel = menuPanelGo;

        // Title text (Floating)
        GameObject titleGo = new GameObject("TitleText", typeof(RectTransform), typeof(Text));
        titleGo.transform.SetParent(menuPanelGo.transform, false);
        titleTransform = titleGo.GetComponent<RectTransform>();
        titleTransform.anchorMin = new Vector2(0.5f, 0.72f);
        titleTransform.anchorMax = new Vector2(0.5f, 0.72f);
        titleTransform.pivot = new Vector2(0.5f, 0.5f);
        titleTransform.anchoredPosition = Vector2.zero;
        titleTransform.sizeDelta = new Vector2(800f, 150f);
        titleTransform.localScale = new Vector3(0.3333f, 0.3333f, 1f);
        titleStartY = 0f;

        Text titleTxt = titleGo.GetComponent<Text>();
        titleTxt.font = mainFont;
        titleTxt.text = "LIFE ON LAND";
        titleTxt.fontSize = 110;
        titleTxt.fontStyle = FontStyle.Bold;
        titleTxt.alignment = TextAnchor.MiddleCenter;
        titleTxt.color = titleColor;
        titleTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
        titleTxt.verticalOverflow = VerticalWrapMode.Overflow;
        var titleShadow = titleGo.AddComponent<Shadow>();
        titleShadow.effectColor = Color.black;
        titleShadow.effectDistance = new Vector3(3f, -3f, 0f);

        // Subtitle
        GameObject subtitleGo = new GameObject("SubtitleText", typeof(RectTransform), typeof(Text));
        subtitleGo.transform.SetParent(titleGo.transform, false);
        var subRt = subtitleGo.GetComponent<RectTransform>();
        subRt.anchorMin = new Vector2(0.5f, 0f);
        subRt.anchorMax = new Vector2(0.5f, 0f);
        subRt.pivot = new Vector2(0.5f, 1f);
        subRt.anchoredPosition = new Vector2(0f, -15f);
        subRt.sizeDelta = new Vector2(1000f, 60f);
        subRt.localScale = Vector3.one;

        Text subTxt = subtitleGo.GetComponent<Text>();
        subTxt.font = mainFont;
        subTxt.text = "The Last Restorer";
        subTxt.fontSize = 45;
        subTxt.fontStyle = FontStyle.Italic;
        subTxt.alignment = TextAnchor.MiddleCenter;
        subTxt.color = subtitleColor;
        subTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
        subTxt.verticalOverflow = VerticalWrapMode.Overflow;
        var subShadow = subtitleGo.AddComponent<Shadow>();
        subShadow.effectColor = Color.black;
        subShadow.effectDistance = new Vector3(3f, -3f, 0f);

        // Vertical Layout for Buttons
        GameObject buttonLayoutGo = new GameObject("ButtonLayout", typeof(RectTransform), typeof(VerticalLayoutGroup));
        buttonLayoutGo.transform.SetParent(menuPanelGo.transform, false);
        var layoutRt = buttonLayoutGo.GetComponent<RectTransform>();
        layoutRt.anchorMin = new Vector2(0.5f, 0.35f);
        layoutRt.anchorMax = new Vector2(0.5f, 0.35f);
        layoutRt.pivot = new Vector2(0.5f, 0.5f);
        layoutRt.sizeDelta = new Vector2(240f, 180f);

        var vlg = buttonLayoutGo.GetComponent<VerticalLayoutGroup>();
        vlg.spacing = 15f;
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        string[] buttonNames = { "Start Game", "Achievements", "Quit" };
        System.Action[] buttonCallbacks = { StartGame, ShowAchievements, QuitGame };

        for (int i = 0; i < buttonNames.Length; i++)
        {
            string label = buttonNames[i];
            System.Action callback = buttonCallbacks[i];

            GameObject btnGo = new GameObject($"Button_{label}", typeof(RectTransform), typeof(Image), typeof(Button));
            btnGo.transform.SetParent(buttonLayoutGo.transform, false);
            var layoutElement = btnGo.AddComponent<LayoutElement>();
            layoutElement.minHeight = 36f;

            Image btnImg = btnGo.GetComponent<Image>();
            btnImg.sprite = Resources.Load<Sprite>("UI/Pixel/slot_frame");
            if (btnImg.sprite == null) btnImg.sprite = UIManager.Instance?.staminaBarFill?.transform.parent?.GetComponent<Image>()?.sprite;
            btnImg.type = Image.Type.Sliced;
            btnImg.color = buttonBgColor;

            Button btn = btnGo.GetComponent<Button>();
            btn.targetGraphic = btnImg;
            btn.onClick.AddListener(() => callback());

            GameObject txtGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
            txtGo.transform.SetParent(btnGo.transform, false);
            var txtRt = txtGo.GetComponent<RectTransform>();
            txtRt.anchorMin = Vector2.zero;
            txtRt.anchorMax = Vector2.one;
            txtRt.sizeDelta = Vector2.zero;
            txtRt.localScale = new Vector3(0.3333f, 0.3333f, 1f);

            Text btnTxt = txtGo.GetComponent<Text>();
            btnTxt.font = mainFont;
            btnTxt.text = label;
            btnTxt.fontSize = 45;
            btnTxt.alignment = TextAnchor.MiddleCenter;
            btnTxt.color = Color.white;
            btnTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
            btnTxt.verticalOverflow = VerticalWrapMode.Overflow;
            var btnShadow = txtGo.AddComponent<Shadow>();
            btnShadow.effectColor = Color.black;
            btnShadow.effectDistance = new Vector3(3f, -3f, 0f);
        }

        // Achievements Panel Setup
        GameObject achPanelGo = new GameObject("AchievementsPanel", typeof(RectTransform), typeof(Image));
        achPanelGo.transform.SetParent(canvas.transform, false);
        var achRt = achPanelGo.GetComponent<RectTransform>();
        achRt.anchorMin = new Vector2(0.5f, 0.5f);
        achRt.anchorMax = new Vector2(0.5f, 0.5f);
        achRt.pivot = new Vector2(0.5f, 0.5f);
        achRt.sizeDelta = new Vector2(300f, 320f);

        Image achBg = achPanelGo.GetComponent<Image>();
        achBg.sprite = Resources.Load<Sprite>("UI/Pixel/panel_frame");
        if (achBg.sprite == null) achBg.sprite = bgImg.sprite;
        achBg.type = Image.Type.Sliced;
        achBg.color = panelBgColor;

        achievementsPanel = achPanelGo;
        achievementsPanel.SetActive(false);

        // Achievements Header
        GameObject achTitleGo = new GameObject("Title", typeof(RectTransform), typeof(Text));
        achTitleGo.transform.SetParent(achPanelGo.transform, false);
        var achTitleRt = achTitleGo.GetComponent<RectTransform>();
        achTitleRt.anchorMin = new Vector2(0.5f, 1f);
        achTitleRt.anchorMax = new Vector2(0.5f, 1f);
        achTitleRt.pivot = new Vector2(0.5f, 1f);
        achTitleRt.anchoredPosition = new Vector2(0f, -12f);
        achTitleRt.sizeDelta = new Vector2(260f, 30f);
        achTitleRt.localScale = new Vector3(0.3333f, 0.3333f, 1f);

        Text achTitleTxt = achTitleGo.GetComponent<Text>();
        achTitleTxt.font = mainFont;
        achTitleTxt.text = "ACHIEVEMENTS";
        achTitleTxt.fontSize = 66;
        achTitleTxt.fontStyle = FontStyle.Bold;
        achTitleTxt.alignment = TextAnchor.MiddleCenter;
        achTitleTxt.color = titleColor;
        achTitleTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
        achTitleTxt.verticalOverflow = VerticalWrapMode.Overflow;
        var achTitleShadow = achTitleGo.AddComponent<Shadow>();
        achTitleShadow.effectColor = Color.black;
        achTitleShadow.effectDistance = new Vector3(3f, -3f, 0f);

        // List layout
        GameObject listGo = new GameObject("List", typeof(RectTransform), typeof(VerticalLayoutGroup));
        listGo.transform.SetParent(achPanelGo.transform, false);
        var listRt = listGo.GetComponent<RectTransform>();
        listRt.anchorMin = Vector2.zero;
        listRt.anchorMax = Vector2.one;
        listRt.offsetMin = new Vector2(15f, 50f);
        listRt.offsetMax = new Vector2(-15f, -40f);

        var listVlg = listGo.GetComponent<VerticalLayoutGroup>();
        listVlg.spacing = 10f;
        listVlg.childAlignment = TextAnchor.UpperCenter;
        listVlg.childControlWidth = true;
        listVlg.childControlHeight = true;
        listVlg.childForceExpandWidth = true;
        listVlg.childForceExpandHeight = false;

        for (int i = 0; i < 3; i++)
        {
            GameObject rowGo = new GameObject($"AchievementRow_{i}", typeof(RectTransform), typeof(Text));
            rowGo.transform.SetParent(listGo.transform, false);
            var layoutElement = rowGo.AddComponent<LayoutElement>();
            layoutElement.minHeight = 65f;

            var rowRt = rowGo.GetComponent<RectTransform>();
            rowRt.localScale = new Vector3(0.3333f, 0.3333f, 1f);

            Text rowTxt = rowGo.GetComponent<Text>();
            rowTxt.font = mainFont;
            rowTxt.fontSize = 42;
            rowTxt.supportRichText = true;
            rowTxt.alignment = TextAnchor.MiddleLeft;
            rowTxt.color = Color.white;
            rowTxt.text = "Loading Status...";
            rowTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
            rowTxt.verticalOverflow = VerticalWrapMode.Overflow;
            var rowShadow = rowGo.AddComponent<Shadow>();
            rowShadow.effectColor = Color.black;
            rowShadow.effectDistance = new Vector3(3f, -3f, 0f);

            achievementStatusTexts[i] = rowTxt;
        }

        // Back Button
        GameObject backBtnGo = new GameObject("Button_Back", typeof(RectTransform), typeof(Image), typeof(Button));
        backBtnGo.transform.SetParent(achPanelGo.transform, false);
        var backRt = backBtnGo.GetComponent<RectTransform>();
        backRt.anchorMin = new Vector2(0.5f, 0f);
        backRt.anchorMax = new Vector2(0.5f, 0f);
        backRt.pivot = new Vector2(0.5f, 0f);
        backRt.anchoredPosition = new Vector2(0f, 12f);
        backRt.sizeDelta = new Vector2(100f, 28f);

        Image backImg = backBtnGo.GetComponent<Image>();
        backImg.sprite = Resources.Load<Sprite>("UI/Pixel/slot_frame");
        if (backImg.sprite == null) backImg.sprite = achBg.sprite;
        backImg.type = Image.Type.Sliced;
        backImg.color = buttonBgColor;

        Button backBtn = backBtnGo.GetComponent<Button>();
        backBtn.targetGraphic = backImg;
        backBtn.onClick.AddListener(HideAchievements);

        GameObject backTxtGo = new GameObject("Text", typeof(RectTransform), typeof(Text));
        backTxtGo.transform.SetParent(backBtnGo.transform, false);
        var backTxtRt = backTxtGo.GetComponent<RectTransform>();
        backTxtRt.anchorMin = Vector2.zero;
        backTxtRt.anchorMax = Vector2.one;
        backTxtRt.sizeDelta = Vector2.zero;
        backTxtRt.localScale = new Vector3(0.3333f, 0.3333f, 1f);

        Text backTxt = backTxtGo.GetComponent<Text>();
        backTxt.font = mainFont;
        backTxt.text = "Back";
        backTxt.fontSize = 45;
        backTxt.alignment = TextAnchor.MiddleCenter;
        backTxt.color = Color.white;
        backTxt.horizontalOverflow = HorizontalWrapMode.Overflow;
        backTxt.verticalOverflow = VerticalWrapMode.Overflow;
        var backShadow = backTxtGo.AddComponent<Shadow>();
        backShadow.effectColor = Color.black;
        backShadow.effectDistance = new Vector3(3f, -3f, 0f);
    }
}
