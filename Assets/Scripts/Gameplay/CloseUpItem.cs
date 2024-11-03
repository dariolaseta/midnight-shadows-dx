using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CloseUpItem : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite closeUpImage;
    
    [SerializeField] private GameObject oldPictureObj;
    [SerializeField] private GameObject documentObject;
    
    [SerializeField] private GameObject imageHolder;

    [SerializeField] private bool isOldPicture = false;

    private void Awake() {
        
        Init();
    }

    public void Interact() {

        GameController.Instance.ChangeState(GameState.DOCUMENT);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        imageHolder.SetActive(true);
        
        if (isOldPicture) {
            
            documentObject.SetActive(false);
            
            oldPictureObj.SetActive(true);
        }
        else {
            
            oldPictureObj.SetActive(false);
            
            documentObject.SetActive(true);
            documentObject.GetComponent<Image>().sprite = closeUpImage;
        }
    }

    private void Init()
    {

        oldPictureObj = GameObject.FindGameObjectWithTag("OldPicture");
        oldPictureObj.SetActive(false);
        
        documentObject = GameObject.FindGameObjectWithTag("DocumentImg");
        documentObject.SetActive(false);
        
        imageHolder = GameObject.FindGameObjectWithTag("ImageHolder");
        imageHolder.SetActive(false);
    }
}
