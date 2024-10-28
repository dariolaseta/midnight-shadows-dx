using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObtainItemUI : MonoBehaviour
{
    public static ObtainItemUI Instance { get; private set; }

    [SerializeField] GameObject obtainItemObj;

    [SerializeField] Image itemImage;

    [SerializeField] TMP_Text itemDescription;

    [SerializeField] InputActionReference closeUIButton;

    public void ShowObtainingItemUI(Sprite newItemImage, string newItemDescription) {

        GameController.Instance.ChangeState(GameState.OBTAIN_ITEM);

        obtainItemObj.SetActive(true);

        itemImage.sprite = newItemImage;

        itemDescription.text = newItemDescription;
    }

    void Awake() {

        CreateInstance();
    }

    private void CreateInstance() {

        if (Instance != null && Instance != this) {

            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void CloseItemUI() {

        if (Input.GetKeyDown(KeyCode.F)) {

            obtainItemObj.SetActive(false);

            GameController.Instance.GoToPrevState();
        }
    }
}
