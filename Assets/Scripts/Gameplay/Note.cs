using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Note : MonoBehaviour, IInteractable
{
    [SerializeField] private Image noteObject;

    [SerializeField] private InputActionReference closeAction;

    [SerializeField] private TMP_Text noteText;
    [SerializeField] private TMP_Text noteTitle;
    
    [SerializeField] private NoteSO note;

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
        SetNoteText();
        
        GameController.Instance.ChangeState(GameState.NOTE);
        noteObject.gameObject.SetActive(true);
    }

    private void CloseNote()
    {
        if (GameController.Instance.State != GameState.NOTE) return;
        
        noteObject.gameObject.SetActive(false);
        GameController.Instance.GoToPrevState();
    }
    
    private void SetNoteText()
    {
        noteText.text = note.Text;
        noteTitle.text = note.Title;
    }
}
