using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Note : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject noteObject;

    [SerializeField] private InputActionReference closeAction;

    private void Awake()
    {
        closeAction.action.performed += CloseNote;
    }

    private void OnDestroy()
    {
        closeAction.action.performed -= CloseNote;
    }
    
    public void Interact()
    {
        OpenNote();
    }

    private void CloseNote(InputAction.CallbackContext obj)
    {
        CloseNote();
    }

    private void OpenNote()
    {
        GameController.Instance.ChangeState(GameState.NOTE);
        noteObject.SetActive(true);
    }

    private void CloseNote()
    {
        if (GameController.Instance.State != GameState.NOTE) return;
        
        noteObject.SetActive(false);
        GameController.Instance.GoToPrevState();
    }
}
