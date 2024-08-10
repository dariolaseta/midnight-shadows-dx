using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerabobSystem : MonoBehaviour
{
    [SerializeField, Range(0.001f, 0.01f)] float amount = 0.002f;
    [SerializeField, Range(1f, 30f)] float frequency = 10.0f;
    [SerializeField, Range(10f, 100f)] float smooth = 10.0f;

    private float startAmount;
    private float startFrequency;
    private float startSmooth;

    private Vector3 startPos;

    public static CamerabobSystem Instance { get; private set; }

    void Awake() {

        if (Instance != null && Instance != this) {

            Destroy(this);
            return;
        }

        Instance = this;
    }

    void Start() {
        
        startPos = transform.localPosition;

        startAmount = amount;
        startFrequency = frequency;
        startSmooth = smooth;
    }

    void Update() {

        CheckForBobTrigger();
        StopHeadbob();
    }

    private void CheckForBobTrigger() {

        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0) StartHeadBob();
    }

    private Vector3 StartHeadBob() {

        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * frequency / 2f) * amount * 1.6f, smooth * Time.deltaTime);
        transform.localPosition += pos;

        return pos;
    }

    private void StopHeadbob() {

        if (transform.localPosition == startPos) return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, 1 * Time.deltaTime);
    }

    public void SetBobVelocity(float amount, float frequency, float smooth) {

        this.amount = amount;
        this.frequency = frequency;
        this.smooth = smooth;
    }

    public void ResetBobVelocity() {

        amount = startAmount;
        frequency = startFrequency;
        smooth = startSmooth;
    }
}
