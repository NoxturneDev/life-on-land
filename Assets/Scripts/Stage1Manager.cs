using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage1Manager : MonoBehaviour
{
    public static Stage1Manager Instance { get; private set; }

    [Header("Stage NPCs")]
    public GameObject malizNPC;
    public GameObject villainNPC;

    [Header("Portraits")]
    public Sprite malizPortrait;
    public Sprite villainPortrait;
    public Sprite playerPortrait;

    [Header("Quest HUD Overlay")]
    public Text questHUDText;

    private Player player;
    private bool openingPlayed = false;
    private bool waterQuestActive = false;
    private bool waterQuestCompleted = false;
    private bool plantQuestActive = false;
    private bool stageCompleted = false;
    private GameObject malizPrompt = null;

    // Checklist objectives for the reforestation (main) quest.
    private QuestObjective objPurify;
    private QuestObjective objGrow;
    private QuestObjective objOxygen;
    // Water sub-quest objectives.
    private QuestObjective objCollectWater;
    private QuestObjective objDeliverWater;

    private const int PurifyGoal = 5;
    private const int GrowGoal = 5;
    private const float O2Goal = 18.0f;
    private const int WaterGoal = 10;

    private float interactionDistance = 3.5f;

    // Space both advances dialogue AND interacts; without a one-frame gap the keypress
    // that closes a dialogue instantly reopens it, trapping the player in a loop.
    private bool dialogueFreeLastFrame = false;

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

        // Fallback checks
        if (malizNPC == null) malizNPC = GameObject.Find("Maliz");
        if (villainNPC == null) villainNPC = GameObject.Find("Villain");

        if (questHUDText == null)
        {
            var hudGo = GameObject.Find("QuestHUDText");
            if (hudGo != null) questHUDText = hudGo.GetComponent<Text>();
        }

        if (questHUDText != null)
        {
            questHUDText.gameObject.SetActive(false);
        }

        // Trigger the stage opening dialogue shortly after load
        Invoke("PlayOpeningDialogue", 0.5f);
    }

    private void PlayOpeningDialogue()
    {
        if (openingPlayed) return;

        List<DialogueLine> lines = new List<DialogueLine>
        {
            new DialogueLine { speaker = "Villain", content = "The last green speck on a dead world. How... sentimental.", portrait = villainPortrait },
            new DialogueLine { speaker = "Villain", content = "(sets the trees alight) There. Cleaner already. Chase me if you like, little Restorer.", portrait = villainPortrait },
            new DialogueLine { speaker = "Maliz", content = "(roars, then sniffles) ...You're the Restorer, aren't you? Please — I couldn't stop them. My oasis is gone.", portrait = malizPortrait }
        };

        DialogueManager.Instance.StartDialogue(lines, () =>
        {
            openingPlayed = true;

            // Villain vanishes from the opening spot
            if (villainNPC != null) villainNPC.SetActive(false);

            // Activate water retrieval quest
            waterQuestActive = true;
            objCollectWater = new QuestObjective("Collect water from the pond", WaterGoal);
            objDeliverWater = new QuestObjective("Deliver the water to Maliz", 1);
            QuestChecklistUI.Instance?.SetQuest("Help Maliz", new List<QuestObjective> { objCollectWater, objDeliverWater });
            UpdateQuestHUD("Objective: Bring 10 units of water from the pond to Maliz.");
            NotificationManager.Instance?.Show("Quest Started: Fetch Water for Maliz");
        });
    }

    private void Update()
    {
        if (player == null) return;

        // Check proximity and interaction with Maliz
        bool inRange = false;
        float distToMaliz = 999f;
        if (malizNPC != null && malizNPC.activeSelf && openingPlayed && DialogueManager.Instance != null && !DialogueManager.Instance.IsDialogueActive())
        {
            distToMaliz = Vector2.Distance(player.transform.position, malizNPC.transform.position);
            if (distToMaliz <= interactionDistance)
            {
                inRange = true;
            }
        }

        if (inRange)
        {
            if (malizPrompt == null && NotificationManager.Instance != null && NotificationManager.Instance.toastPrefab != null && NotificationManager.Instance.toastContainer != null)
            {
                malizPrompt = Instantiate(NotificationManager.Instance.toastPrefab, NotificationManager.Instance.toastContainer);
                malizPrompt.SetActive(true);
                Text label = malizPrompt.GetComponentInChildren<Text>();
                if (label != null)
                {
                    string promptMsg = "Press [E] or [Space] to talk to Maliz";
                    if (waterQuestActive && !waterQuestCompleted && player.CurrentWaterInventory >= 10)
                    {
                        promptMsg = "Press [E] or [Space] to deliver water to Maliz";
                    }
                    label.text = promptMsg;
                }
            }
        }
        else
        {
            if (malizPrompt != null)
            {
                Destroy(malizPrompt);
                malizPrompt = null;
            }
        }

        if (inRange)
        {
            bool interactPressed = false;
            #if ENABLE_INPUT_SYSTEM
            var kb = UnityEngine.InputSystem.Keyboard.current;
            if (kb != null)
            {
                if (kb.eKey.wasPressedThisFrame || kb.spaceKey.wasPressedThisFrame)
                {
                    interactPressed = true;
                }
            }
            else
            {
                try
                {
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
                    {
                        interactPressed = true;
                    }
                }
                catch (System.Exception) { }
            }
            #else
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                interactPressed = true;
            }
            #endif

            if (interactPressed && dialogueFreeLastFrame)
            {
                if (malizPrompt != null)
                {
                    Destroy(malizPrompt);
                    malizPrompt = null;
                }
                InteractWithMaliz();
            }
        }

        dialogueFreeLastFrame = DialogueManager.Instance == null || !DialogueManager.Instance.IsDialogueActive();

        // Water sub-quest checklist progress
        if (waterQuestActive && !waterQuestCompleted && objCollectWater != null)
        {
            objCollectWater.current = player.CurrentWaterInventory;
            QuestChecklistUI.Instance?.Refresh();
        }

        // Reforestation (main) quest checklist + completion
        if (plantQuestActive && !stageCompleted)
        {
            int purified = (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentGrid != null)
                ? EnvironmentManager.Instance.EnvironmentGrid.TilesPurifiedCount : 0;
            int matureShrubs = CountMatureShrubs();
            float currentO2 = EnvironmentManager.Instance != null ? EnvironmentManager.Instance.GlobalO2Percentage : 15.0f;

            if (objPurify != null) objPurify.current = purified;
            if (objGrow != null) objGrow.current = matureShrubs;
            if (objOxygen != null) objOxygen.current = Mathf.RoundToInt(currentO2 * 10f); // 0.1% resolution
            QuestChecklistUI.Instance?.Refresh();

            UpdateQuestHUD($"Reforest the oasis. Mature Shrubs: {matureShrubs}/{GrowGoal} | O2: {currentO2:F1}% / {O2Goal:F0}%");

            bool allDone = purified >= PurifyGoal && matureShrubs >= GrowGoal && currentO2 >= O2Goal;
            if (allDone)
            {
                TriggerStageCompletion();
            }
        }
    }

    private void InteractWithMaliz()
    {
        if (waterQuestActive && !waterQuestCompleted)
        {
            if (player.CurrentWaterInventory >= 10)
            {
                // Deliver water and reward seeds
                List<DialogueLine> lines = new List<DialogueLine>
                {
                    new DialogueLine { speaker = "Maliz", content = "You actually did it. The green... it's coming back.", portrait = malizPortrait },
                    new DialogueLine { speaker = "Maliz", content = "Here, take these Desert Shrub seeds. They hold soil moisture retention and need very little water. Purify 5 corrupted tiles (shovel first, then water) and grow 5 mature shrubs to help us reach 18% O2!", portrait = malizPortrait }
                };

                DialogueManager.Instance.StartDialogue(lines, () =>
                {
                    player.AddWater(-10); // consume 10 water
                    player.AddSeeds("desert_shrub", 15); // give shrub seeds
                    NotificationManager.Instance?.Show("Received Desert Shrub Seeds x15");

                    if (objDeliverWater != null) objDeliverWater.forceComplete = true;
                    QuestChecklistUI.Instance?.Refresh();

                    waterQuestActive = false;
                    waterQuestCompleted = true;
                    plantQuestActive = true;

                    // Swap the checklist to the reforestation objectives.
                    objPurify = new QuestObjective("Purify corrupted tiles", PurifyGoal);
                    objGrow = new QuestObjective("Grow Desert Shrubs to maturity", GrowGoal);
                    objOxygen = new QuestObjective("Raise Oxygen to 18%", Mathf.RoundToInt(O2Goal * 10f));
                    QuestChecklistUI.Instance?.SetQuest("Restore the Oasis",
                        new List<QuestObjective> { objPurify, objGrow, objOxygen });
                    NotificationManager.Instance?.Show("Quest Updated: Restore the Oasis");
                });
            }
            else
            {
                // Remind to fetch water
                List<DialogueLine> lines = new List<DialogueLine>
                {
                    new DialogueLine { speaker = "Maliz", content = "The pond runs deep to the east. Bring me ten cans of water, and I'll give you my last seeds — Desert Shrubs. They hold moisture. Hurry.", portrait = malizPortrait }
                };
                DialogueManager.Instance.StartDialogue(lines);
            }
        }
        else if (waterQuestCompleted && plantQuestActive)
        {
            // Helpful hints dialogue
            List<DialogueLine> lines = new List<DialogueLine>
            {
                new DialogueLine { speaker = "Maliz", content = "Remember, to purify a corrupted tile:\n1. Use Shovel (slot 1) to clear ash.\n2. Use Watering Can (slot 2) with water to purify to normal soil.\n3. Select Desert Shrub seeds (slot 3) and plant!", portrait = malizPortrait }
            };
            DialogueManager.Instance.StartDialogue(lines);
        }
    }

    private int CountMatureShrubs()
    {
        int count = 0;
        var allTrees = FindObjectsOfType<Tree>();
        foreach (var tree in allTrees)
        {
            if (tree.TreeTypeID == "desert_shrub" && tree.CurrentFSMState == GrowthState.MatureTree)
            {
                count++;
            }
        }
        return count;
    }

    private void TriggerStageCompletion()
    {
        stageCompleted = true;
        plantQuestActive = false;

        // Villain mocks player one last time at the boundary
        if (villainNPC != null)
        {
            villainNPC.transform.position = new Vector3(14f, -6f, 0f);
            villainNPC.SetActive(true);
        }

        List<DialogueLine> lines = new List<DialogueLine>
        {
            new DialogueLine { speaker = "Villain", content = "A handful of shrubs? Adorable. I'll be in the Orange Grove — do try to keep up.", portrait = villainPortrait }
        };

        // All objectives satisfied - tick them and dismiss the checklist.
        QuestChecklistUI.Instance?.Refresh();
        QuestChecklistUI.Instance?.Hide();

        DialogueManager.Instance.StartDialogue(lines, () =>
        {
            if (villainNPC != null) villainNPC.SetActive(false);
            UpdateQuestHUD("Stage 1 Completed! Biosphere restoration successful.");
            Debug.Log("Stage 1 Cleared! Pathway open.");

            if (EnvironmentManager.Instance != null)
            {
                EnvironmentManager.Instance.AdvanceStage();
            }

            NotificationManager.Instance?.Show("Stage 1 Complete!");
            VictoryUI.Instance?.Show(
                "Stage 1 Complete",
                "The Arid Oasis breathes again. Maliz's oasis is restored, and the villain has fled toward the Orange Grove.\n\nOrange Region — coming soon.\n\nThanks for playing!"
            );
        });
    }

    private void UpdateQuestHUD(string message)
    {
        if (questHUDText != null)
        {
            questHUDText.text = message;
        }
    }
}
