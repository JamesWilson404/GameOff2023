using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodTrail : MonoBehaviour
{
    public bool InDebug = false;

    public float minForce;
    public float maxForce;

    public float minSize = 0.5f;
    public float maxSize = 2.0f;

    public float CurrentSize = 0f;

    TrailRenderer tr;
    Rigidbody2D rb;

    private void Awake()
    {
        tr = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void ClearTrail()
    {
        GetComponent<TrailRenderer>().Clear();
    }

    public void Init()
    {
        // Randomize the size of the GameObject
        float randomSize = Random.Range(minSize, maxSize);
        CurrentSize = randomSize;
        transform.localScale = new Vector3(randomSize, randomSize, 1f);

        // Generate a random angle in radians
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        tr.startWidth = randomSize;
        tr.endWidth = randomSize;

        // Calculate the direction from the angle
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

        // Calculate a force factor based on size
        float forceFactor = Mathf.Lerp(0.5f, 1.0f, (randomSize - minSize) / (maxSize - minSize));

        rb.gravityScale = Mathf.Pow((maxSize - randomSize),2) * 0.2f;

        // Apply the force to the Rigidbody2D
        rb.AddForce(direction * Mathf.Pow((maxSize - randomSize),2) * 20, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (rb.gravityScale > 0)
        {
            rb.gravityScale = Mathf.Clamp(rb.gravityScale - (Time.deltaTime * 0.05f),0,1);
        }
    }


    private void OnValidate()
    {
        if (!InDebug)
        {
            ClearTrail();
        }
    }
}
