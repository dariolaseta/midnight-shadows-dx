using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpAction : MonoBehaviour
{
    public void Close() {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        gameObject.SetActive(false);
        
        GameController.Instance.GoToPrevState();
    }
}
