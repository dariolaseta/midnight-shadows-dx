using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvisibleWallBehavior : MonoBehaviour
{
    [SerializeField] GameObject blockImageObj;

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag("Player")) {

            blockImageObj.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.gameObject.CompareTag("Player")) {

            blockImageObj.SetActive(false);
        }
    }
}
