using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    void Update()
    {
        // Moves the player to the right every frame
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}