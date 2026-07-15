using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Renders a live quest checklist: a titled pixel panel with one row per objective
// (checkbox + label + progress). Rows re-tick as objectives complete.
public class QuestChecklistUI : MonoBehaviour
{
    public static QuestChecklistUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject panel;
    public Text titleText;
    public RectTransform rowContainer;   // has a VerticalLayoutGroup
    public GameObject rowTemplate;       // Image(checkbox) + Text(label); inactive template

    [Header("Checkbox Sprites")]
    public Sprite checkboxEmpty;
    public Sprite checkboxDone;

    private static readonly Color PendingColor = new Color(0.96f, 0.91f, 0.85f, 1f);
    private static readonly Color DoneColor = new Color(0.60f, 0.85f, 0.55f, 1f);

    private List<QuestObjective> objectives;
    private readonly List<Image> rowBoxes = new List<Image>();
    private readonly List<Text> rowLabels = new List<Text>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        if (panel != null) panel.SetActive(false);
        if (rowTemplate != null) rowTemplate.SetActive(false);
    }

    // Show a new quest with a fresh set of objectives.
    public void SetQuest(string title, List<QuestObjective> objs)
    {
        objectives = objs;
        if (titleText != null) titleText.text = title;
        if (panel != null) panel.SetActive(true);
        BuildRows();
        Refresh();
    }

    public void Hide()
    {
        if (panel != null) panel.SetActive(false);
    }

    private void BuildRows()
    {
        // clear previous rows (deactivate immediately so they don't linger for a frame
        // before Destroy runs at end of frame)
        rowBoxes.Clear();
        rowLabels.Clear();
        if (rowContainer != null)
        {
            for (int i = rowContainer.childCount - 1; i >= 0; i--)
            {
                var child = rowContainer.GetChild(i).gameObject;
                child.SetActive(false);
                Destroy(child);
            }
        }
        if (rowTemplate == null || rowContainer == null || objectives == null) return;

        foreach (var _ in objectives)
        {
            GameObject row = Instantiate(rowTemplate, rowContainer);
            row.SetActive(true);
            var box = row.GetComponentInChildren<Image>();
            var label = row.GetComponentInChildren<Text>();
            rowBoxes.Add(box);
            rowLabels.Add(label);
        }
    }

    // Call every frame (or after progress changes) to re-tick the checklist.
    public void Refresh()
    {
        if (objectives == null) return;
        for (int i = 0; i < objectives.Count && i < rowBoxes.Count; i++)
        {
            var o = objectives[i];
            bool done = o.IsComplete;
            if (rowBoxes[i] != null)
                rowBoxes[i].sprite = done ? checkboxDone : checkboxEmpty;
            if (rowLabels[i] != null)
            {
                rowLabels[i].text = o.label + o.ProgressText;
                rowLabels[i].color = done ? DoneColor : PendingColor;
            }
        }
    }
}
