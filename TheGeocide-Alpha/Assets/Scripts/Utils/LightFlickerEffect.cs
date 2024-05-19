using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class LightFlickerEffect : MonoBehaviour
{
    [Tooltip("External light to flicker; you can leave this null if you attach script to a light")]
    public Light2D Light;
    [Tooltip("Minimum random light intensity")]
    public float minIntensity = 0f;
    [Tooltip("Maximum random light intensity")]
    public float maxIntensity = 1f;
    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
    [Range(1, 50)]
    public int smoothing = 5;

    public float flinkInterval;
    public float flinkTime;

    private float _currentFlickTime;
    private float _lastFlink;
    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    Queue<float> smoothQueue;
    float lastSum = 0;

    void Start()
    {
        _lastFlink = Random.Range(0, flinkInterval);
        smoothQueue = new Queue<float>(smoothing);
        Light.intensity = maxIntensity;
    }

    void Update()
    {
        if (Light == null)
            return;
        _lastFlink += Time.deltaTime;
        if (_lastFlink < flinkInterval)
        {
            return;
        }

        _currentFlickTime += Time.deltaTime;

        if (_currentFlickTime > flinkTime)
        {
            _lastFlink = 0;
            _currentFlickTime = 0;
            Light.intensity = maxIntensity;
            return;
        }

        // pop off an item if too big
        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        // Calculate new smoothed average
        Light.intensity = lastSum / (float)smoothQueue.Count;
    }

}