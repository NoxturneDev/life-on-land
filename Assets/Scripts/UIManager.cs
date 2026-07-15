using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Player & Env Info UI")]
    public Text staminaText;
    public Image staminaBarFill;
    public Text o2Text;
    public Image o2BarFill;
    public Text stageText;
    public Text questText;

    [Header("Overall Quest Progress UI")]
    public Image overallProgressBarFill;
    public Text overallProgressText;

    [Header("Hotbar UI")]
    public Image[] slotOutlines = new Image[6];
    public Image[] slotIcons = new Image[6];
    public Text[] slotQuantities = new Text[6];

    [Header("Hotbar Highlighting")]
    public Sprite highlightOutlineSprite;
    public Sprite normalOutlineSprite;

    [Header("Icons")]
    public Sprite shovelIcon;
    public Sprite wateringCanIcon;
    public Sprite foodIcon;
    public Sprite shrubSeedIcon;
    public Sprite pineSeedIcon;
    public Sprite fernSeedIcon;
    public Sprite blueprintIcon;

    private Player player;

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
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();

        // Setup Overall Progress UI dynamically
        CreateProgressUI();

        // Spawn Pause Menu manager dynamically
        if (FindObjectOfType<PauseMenu>() == null)
        {
            GameObject pmGo = new GameObject("PauseMenuManager", typeof(PauseMenu));
        }
    }

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        if (player == null) return;

        UpdateStaminaUI();
        UpdateO2UI();
        UpdateStageUI();
        UpdateHotbarUI();
        UpdateQuestUI();
        UpdateOverallProgressUI();
    }

    private void UpdateStaminaUI()
    {
        float stamina = player.CurrentStamina;
        if (staminaBarFill != null)
        {
            staminaBarFill.fillAmount = stamina / 100f;
            // Green to Red gradient based on stamina level
            staminaBarFill.color = Color.Lerp(Color.red, new Color(0.2f, 0.8f, 0.2f, 1.0f), stamina / 100f);
        }
        if (staminaText != null)
        {
            staminaText.text = $"Stamina: {Mathf.RoundToInt(stamina)}/100";
        }
    }

    private void UpdateO2UI()
    {
        if (EnvironmentManager.Instance == null) return;
        float o2 = EnvironmentManager.Instance.GlobalO2Percentage;
        float targetO2 = EnvironmentManager.Instance.GetCurrentO2Goal();
        if (o2Text != null)
        {
            o2Text.text = $"Oxygen Buffer: {o2:F1}% / {targetO2:F1}%";
        }
        if (o2BarFill != null && targetO2 > 0f)
        {
            o2BarFill.fillAmount = Mathf.Clamp01(o2 / targetO2);
        }
    }

    private void UpdateStageUI()
    {
        if (EnvironmentManager.Instance == null) return;
        int level = EnvironmentManager.Instance.currentLevel;
        string regionName = "";
        switch (level)
        {
            case 1:
                regionName = "Stage 1: Red Region (The Arid Oasis)";
                break;
            case 2:
                regionName = "Stage 2: Orange Region (The Scorched Grove)";
                break;
            case 3:
                regionName = "Stage 3: Pink Bloom (The Biosphere)";
                break;
            default:
                regionName = "Unknown Region";
                break;
        }

        if (stageText != null)
        {
            stageText.text = regionName;
        }
    }

    private void UpdateQuestUI()
    {
        // Disable the legacy quest text component to avoid overlapping with the checklist box
        if (questText != null)
        {
            questText.gameObject.SetActive(false);
        }
    }

    private void UpdateHotbarUI()
    {
        int activeSlot = player.ActiveHotbarSlot;

        for (int i = 0; i < 6; i++)
        {
            // Highlight frame
            if (slotOutlines[i] != null)
            {
                if (i == activeSlot)
                {
                    slotOutlines[i].sprite = highlightOutlineSprite;
                }
                else
                {
                    slotOutlines[i].sprite = normalOutlineSprite;
                }
                slotOutlines[i].color = Color.white; // let the pixel-art frame sprites carry the styling
            }

            // Slot icons
            if (slotIcons[i] != null)
            {
                Sprite targetIcon = null;
                switch (i)
                {
                    case 0:
                        targetIcon = shovelIcon;
                        break;
                    case 1:
                        targetIcon = wateringCanIcon;
                        break;
                    case 2:
                        targetIcon = foodIcon;
                        break;
                    case 3:
                        targetIcon = shrubSeedIcon;
                        break;
                    case 4:
                        targetIcon = blueprintIcon;
                        break;
                    case 5:
                        targetIcon = null;
                        break;
                }

                if (targetIcon != null)
                {
                    slotIcons[i].sprite = targetIcon;
                    slotIcons[i].gameObject.SetActive(true);
                }
                else
                {
                    slotIcons[i].gameObject.SetActive(false);
                }
            }

            // Quantities
            if (slotQuantities[i] != null)
            {
                switch (i)
                {
                    case 0:
                        slotQuantities[i].text = "Shovel";
                        break;
                    case 1:
                        slotQuantities[i].text = $"{player.CurrentWaterInventory} Water";
                        break;
                    case 2:
                        slotQuantities[i].text = $"x{GetItemQty(player.rationItemID)} Food";
                        break;
                    case 3:
                        slotQuantities[i].text = $"x{GetItemQty(CurrentSeedID())}";
                        break;
                    case 4:
                        slotQuantities[i].text = "Build";
                        break;
                    case 5:
                        slotQuantities[i].text = "";
                        break;
                }
            }
        }
    }

    private string CurrentSeedID()
    {
        var prof = player.CurrentStageSeedProfile;
        return prof != null ? prof.treeTypeID : "desert_shrub";
    }

    private string GetItemQty(string itemID)
    {
        foreach (var item in player.Inventory)
        {
            if (item.itemID == itemID)
            {
                return item.quantity.ToString();
            }
        }
        return "0";
    }

    private void UpdateOverallProgressUI()
    {
        float progress = Stage1Manager.Instance != null ? Stage1Manager.Instance.GetOverallProgressFraction() : 0f;
        if (overallProgressBarFill != null)
        {
            overallProgressBarFill.fillAmount = progress;
        }
        if (overallProgressText != null)
        {
            overallProgressText.text = $"Restoration Progress: {Mathf.RoundToInt(progress * 100f)}%";
        }
    }

    private void CreateProgressUI()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null) return;

        Transform existing = canvas.transform.Find("OverallProgressPanel");
        if (existing != null)
        {
            overallProgressBarFill = existing.Find("FillArea/Fill")?.GetComponent<Image>();
            var labelText = existing.Find("Label")?.GetComponent<Text>();
            if (labelText != null)
            {
                labelText.fontSize = 33; // 11 * 3 for crisp rendering
                var existingTextRt = labelText.GetComponent<RectTransform>();
                existingTextRt.anchorMin = new Vector2(0.5f, 0.5f);
                existingTextRt.anchorMax = new Vector2(0.5f, 0.5f);
                existingTextRt.pivot = new Vector2(0.5f, 0.5f);
                existingTextRt.sizeDelta = new Vector2(280f * 3f, 26f * 3f);
                existingTextRt.localScale = new Vector3(0.3333f, 0.3333f, 1f);
                
                var existingShadow = labelText.GetComponent<Shadow>();
                if (existingShadow != null)
                {
                    existingShadow.effectDistance = new Vector2(3f, -3f);
                }
            }
            overallProgressText = labelText;
            return;
        }

        // Panel Container
        GameObject panelGo = new GameObject("OverallProgressPanel", typeof(RectTransform));
        panelGo.transform.SetParent(canvas.transform, false);
        
        RectTransform rt = panelGo.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.anchoredPosition = new Vector2(0f, -12f);
        rt.sizeDelta = new Vector2(280f, 26f);

        Image bgImg = panelGo.AddComponent<Image>();
        bgImg.sprite = Resources.Load<Sprite>("UI/Pixel/bar_frame");
        if (bgImg.sprite == null) bgImg.sprite = staminaBarFill?.transform.parent?.GetComponent<Image>()?.sprite;
        bgImg.type = Image.Type.Sliced;
        bgImg.color = new Color(0.15f, 0.12f, 0.15f, 0.9f); // cosy dark panel color

        // Fill Area
        GameObject fillArea = new GameObject("FillArea", typeof(RectTransform));
        fillArea.transform.SetParent(panelGo.transform, false);
        RectTransform fillRt = fillArea.GetComponent<RectTransform>();
        fillRt.anchorMin = Vector2.zero;
        fillRt.anchorMax = Vector2.one;
        fillRt.sizeDelta = new Vector2(-4f, -4f);

        GameObject fillGo = new GameObject("Fill", typeof(RectTransform), typeof(Image));
        fillGo.transform.SetParent(fillArea.transform, false);
        RectTransform fillImgRt = fillGo.GetComponent<RectTransform>();
        fillImgRt.anchorMin = Vector2.zero;
        fillImgRt.anchorMax = Vector2.one;
        fillImgRt.sizeDelta = Vector2.zero;

        Image fillImg = fillGo.GetComponent<Image>();
        fillImg.sprite = staminaBarFill?.sprite;
        fillImg.type = Image.Type.Filled;
        fillImg.fillMethod = Image.FillMethod.Horizontal;
        fillImg.fillAmount = 0f;
        fillImg.color = new Color(0.25f, 0.75f, 0.85f, 1f); // Sky blue/cyan color

        // Label
        GameObject textGo = new GameObject("Label", typeof(RectTransform), typeof(Text));
        textGo.transform.SetParent(panelGo.transform, false);
        RectTransform textRt = textGo.GetComponent<RectTransform>();
        textRt.anchorMin = new Vector2(0.5f, 0.5f);
        textRt.anchorMax = new Vector2(0.5f, 0.5f);
        textRt.pivot = new Vector2(0.5f, 0.5f);
        textRt.sizeDelta = new Vector2(280f * 3f, 26f * 3f);
        textRt.localScale = new Vector3(0.3333f, 0.3333f, 1f);

        Text label = textGo.GetComponent<Text>();
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (label.font == null) label.font = staminaText?.font;
        label.fontSize = 33; // 11 * 3 for crisp rendering
        label.fontStyle = FontStyle.Bold;
        label.alignment = TextAnchor.MiddleCenter;
        label.color = Color.white;
        var shadow = textGo.AddComponent<Shadow>();
        shadow.effectColor = Color.black;
        shadow.effectDistance = new Vector2(3f, -3f); // Scaled for 0.33 scale

        overallProgressBarFill = fillImg;
        overallProgressText = label;
    }
}
