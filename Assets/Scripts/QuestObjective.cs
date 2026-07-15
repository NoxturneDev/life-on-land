using UnityEngine;

// A single checklist line in a quest (e.g. "Grow Desert Shrubs 3/5").
public class QuestObjective
{
    public string label;
    public int current;
    public int target;
    public bool forceComplete; // for one-shot objectives with no numeric progress

    public QuestObjective(string label, int target)
    {
        this.label = label;
        this.target = Mathf.Max(1, target);
        this.current = 0;
    }

    public bool IsComplete => forceComplete || current >= target;

    // "(3/5)" for multi-count objectives, blank for single-step ones.
    public string ProgressText => target > 1 ? $" ({Mathf.Min(current, target)}/{target})" : "";
}
