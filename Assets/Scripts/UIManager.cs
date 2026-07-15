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
}
