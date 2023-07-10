using UnityEngine;

public class Phoenix : MonoBehaviour
{
    public float radius = 5f; // Radius of the circular path
    public float speed = 2f; // Speed of movement along the path
    public float diveAmplitude = 2f; // Amplitude of the dive
    public float diveFrequency = 2f; // Frequency of the dive
    private float angle = 0f; // Current angle on the circular path

    private Vector3 startPosition;
    private float diveTimer = 0f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Update the angle based on time and speed
        angle += speed * Time.deltaTime;

        // Calculate the new position on the circle
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // Calculate the vertical position based on the dive amplitude and frequency
        float y = startPosition.y + Mathf.Sin(diveTimer * diveFrequency) * diveAmplitude;

        // Update the bird's position
        transform.position = new Vector3(x, y, z);

        // Rotate the bird to face the center of the circle
        transform.LookAt(Vector3.zero);

        // Update the dive timer
        diveTimer += Time.deltaTime;
    }
}