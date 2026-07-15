using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    [Header("UI References")]
    public RectTransform toastContainer;
    public GameObject toastPrefab; // must have an Image (panel_frame) + child Text

    [Header("Timing")]
    public float displayDuration = 2.0f;
    public float fadeDuration = 0.4f;

    private Queue<string> pendingMessages = new Queue<string>();
    private readonly List<GameObject> activeToasts = new List<GameObject>();
    private const int maxVisible = 4;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Show(string message)
    {
        if (string.IsNullOrEmpty(message)) return;

        if (toastContainer == null || toastPrefab == null)
        {
            Debug.Log($"[Notification] {message}");
            return;
        }
        StartCoroutine(SpawnToast(message));
    }

    private IEnumerator SpawnToast(string message)
    {
        GameObject toast = Instantiate(toastPrefab, toastContainer);
        toast.SetActive(true);
        activeToasts.Add(toast);

        Text label = toast.GetComponentInChildren<Text>();
        if (label != null) label.text = message;

        CanvasGroup group = toast.GetComponent<CanvasGroup>();
        if (group == null) group = toast.AddComponent<CanvasGroup>();
        group.alpha = 0f;

        // Fade in
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        group.alpha = 1f;

        yield return new WaitForSeconds(displayDuration);

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = 1f - Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        activeToasts.Remove(toast);
        Destroy(toast);
    }
}
