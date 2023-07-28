using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float shakeDuration;
    public float shakeMagnitude;

    private Vector3 originalPosition;
    private float shakeTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        shakeDuration = 0.2f;
        shakeMagnitude = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeTimer = 0f;
            transform.localPosition = Vector3.zero;
        }
    }

    public void Shake()
    {
        originalPosition = transform.localPosition;
        shakeTimer = shakeDuration;
    }
}
