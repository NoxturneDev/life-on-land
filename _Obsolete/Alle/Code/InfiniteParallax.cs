using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    [Tooltip("1 = Sky (follows camera exactly). 0 = Foreground (normal world speed).")]
    public float parallaxFactor;

    private float length, startpos;
    private Transform cam;
    
    // This stops our clones from creating their own clones endlessly!
    public bool isClone = false;

    void Start()
    {
        // If this is a cloned object, stop here. We just want it to be a visual dummy.
        if (isClone) return;

        cam = Camera.main.transform;
        startpos = transform.position.x;
        
        // Dynamically grab the exact width of your specific image
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // Spawn a clone exactly one image-width to the right
        GameObject cloneRight = Instantiate(gameObject, transform);
        cloneRight.GetComponent<InfiniteParallax>().isClone = true;
        cloneRight.transform.localPosition = new Vector3(length, 0, 0);

        // Spawn a clone exactly one image-width to the left
        GameObject cloneLeft = Instantiate(gameObject, transform);
        cloneLeft.GetComponent<InfiniteParallax>().isClone = true;
        cloneLeft.transform.localPosition = new Vector3(-length, 0, 0);
    }

    void Update()
    {
        if (isClone) return;

        // Calculate how far the camera has moved
        float temp = (cam.position.x * (1 - parallaxFactor));
        float dist = (cam.position.x * parallaxFactor);

        // Move the layer
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // The Treadmill Logic: If the camera moves past the image width, teleport the group forward!
        if (temp > startpos + length) {
            startpos += length;
        } else if (temp < startpos - length) {
            startpos -= length;
        }
    }
}