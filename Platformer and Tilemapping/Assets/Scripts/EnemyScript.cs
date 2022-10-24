using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    bool start = true;

    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Transform endMarker;
    public bool flipper;

    // Movement speed in units/sec.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    void Start()
        {
        // Keep a note of the time the movement started.
            startTime = Time.time;

        // Calculate the journey length.
            journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        }

    // Follows the target position like with a spring
    void FixedUpdate()
        {
        // Distance moved = time * speed.
            float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers and pingpong the movement.
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, Mathf.PingPong (fracJourney, 1));

        if (transform.position == startMarker.position)
        {
          if (!start && flipper)
          {
            Flip();
          }
         }

        if (transform.position == endMarker.position)
        {
          if (flipper)
          {
            Flip();
            start = false;
          }
         }
        }

    void  Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
    }
}
