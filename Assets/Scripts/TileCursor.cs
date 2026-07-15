using UnityEngine;

// Gold highlight frame that snaps to the grid cell under the mouse,
// showing where the active hotbar tool will act.
public class TileCursor : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        bool dialogueUp = DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive();
        if (dialogueUp || Camera.main == null)
        {
            if (sr != null) sr.enabled = false;
            return;
        }

        Vector3 mouseWorld;
        #if ENABLE_INPUT_SYSTEM
        var mouse = UnityEngine.InputSystem.Mouse.current;
        if (mouse != null)
        {
            Vector2 mp = mouse.position.ReadValue();
            mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mp.x, mp.y, 0f));
        }
        else
        {
            try
            {
                mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            catch (System.Exception)
            {
                if (sr != null) sr.enabled = false;
                return;
            }
        }
        #else
        mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        #endif

        // Snap to the centre of the exact tile the mouse is over (same mapping the tools use).
        Vector2Int g = GridUtil.WorldToGrid(mouseWorld);
        transform.position = GridUtil.GridToWorldCenter(g);
        if (sr != null) sr.enabled = true;
    }
}
