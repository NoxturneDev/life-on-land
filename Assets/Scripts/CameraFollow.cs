using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Target")]
    public Transform target;

    [Header("Settings")]
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        if (target == null)
        {
            // Try to find player automatically if target is null
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj == null) playerObj = GameObject.Find("Player");
            if (playerObj != null) target = playerObj.transform;
        }

        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
