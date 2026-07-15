using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject dialoguePanel;
    public Text speakerText;
    public Text contentText;
    public Image portraitImage;
    public Text promptText; // e.g. "Press Space to continue"

    private Queue<DialogueLine> linesQueue = new Queue<DialogueLine>();
    private Action onCompleteCallback;
    private bool isActive = false;

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

        // Hide panel at start
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    private void Start()
    {
        FixEventSystemInputModule();
    }

    private void FixEventSystemInputModule()
    {
        #if ENABLE_INPUT_SYSTEM
        try
        {
            var eventSystem = UnityEngine.EventSystems.EventSystem.current;
            if (eventSystem != null)
            {
                var standalone = eventSystem.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                if (standalone != null)
                {
                    Destroy(standalone);
                    eventSystem.gameObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
                    Debug.Log("DialogueManager: Automatically replaced StandaloneInputModule with InputSystemUIInputModule to prevent legacy Input exceptions.");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("DialogueManager: Failed to auto-replace EventSystem module: " + e.Message);
        }
        #endif
    }

    private void Update()
    {
        if (!isActive) return;

        // Advance dialogue on Space key or Left Click
        bool advancePressed = false;

        #if ENABLE_INPUT_SYSTEM
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        var mouse = UnityEngine.InputSystem.Mouse.current;
        if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame) advancePressed = true;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame) advancePressed = true;

        if (keyboard == null || mouse == null)
        {
            try
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) advancePressed = true;
            }
            catch (System.Exception) { }
        }
        #else
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) advancePressed = true;
        #endif

        if (advancePressed)
        {
            DisplayNextLine();
        }
    }

    public void StartDialogue(List<DialogueLine> lines, Action onComplete = null)
    {
        if (lines == null || lines.Count == 0) return;

        linesQueue.Clear();
        foreach (var line in lines)
        {
            linesQueue.Enqueue(line);
        }

        onCompleteCallback = onComplete;
        isActive = true;

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        
        // Pause player movement
        var playerCtrl = FindObjectOfType<PlayerController>();
        if (playerCtrl != null)
        {
            var rb = playerCtrl.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (linesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = linesQueue.Dequeue();

        if (speakerText != null) speakerText.text = currentLine.speaker;
        if (contentText != null) contentText.text = currentLine.content;
        
        if (portraitImage != null)
        {
            if (currentLine.portrait != null)
            {
                portraitImage.sprite = currentLine.portrait;
                portraitImage.gameObject.SetActive(true);
            }
            else
            {
                portraitImage.gameObject.SetActive(false);
            }
        }
    }

    public void EndDialogue()
    {
        isActive = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        onCompleteCallback?.Invoke();
    }

    public bool IsDialogueActive()
    {
        return isActive;
    }
}

[System.Serializable]
public struct DialogueLine
{
    public string speaker;
    [TextArea(2, 5)]
    public string content;
    public Sprite portrait;
}
