using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainAudio : MonoBehaviour
{
    private Transform player;

    private AudioSource audioSource;

    [SerializeField] float maxDistance = 10f;

    void Awake() {

        ObtainComponent();
    }

    void Update() {

        SetRainVolume();
    }

    private void SetRainVolume() {

        if (player != null) {

            float distance = Vector3.Distance(player.position, transform.position);
            float volumeFactor = Mathf.Clamp01(1.0f - (distance / maxDistance));

            audioSource.volume = volumeFactor;
        } else {

            Debug.Log("Player non trovato");
        }
    }

    private void ObtainComponent() {

        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
}
