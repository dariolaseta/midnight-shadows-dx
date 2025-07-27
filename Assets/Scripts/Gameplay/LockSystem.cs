using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LockSystem : MonoBehaviour, IInteractable
{
    [Header("References")] 
    [SerializeField] private BoxCollider unlockCollider;

    [SerializeField] private Image lockImage;
    
    [SerializeField] private InputActionReference closeInputAction;

    [SerializeField] private TMP_Text[] digitTexts;
    [SerializeField] private TMP_Text lightHouseCode;

    [Header("Configs")]
    [Tooltip("Combinazione corretta, l'ultimo numero dev'essere 2")]
    [SerializeField] private int[] correctCombination = new int[3] { 0, 0, 2 };

    private int[] currentDigits = new int[3] { 0, 0, 0 };
    
    private BoxCollider boxCollider;

    private void Awake()
    {
        closeInputAction.action.performed += CloseLockAction;
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        PuzzleGenerator.Instance.GenerateLockCode(correctCombination);
        lightHouseCode.text = correctCombination[0].ToString();
        UpdateAllDigitsUI();
    }

    private void OnDestroy()
    {
        closeInputAction.action.performed -= CloseLockAction;
    }

    public void Interact()
    {
        OpenLock();
    }

    private void OpenLock()
    {
        if (!lockImage.gameObject.activeSelf && GameController.Instance.State == GameState.FREEROAM)
        {
            GameController.Instance.ChangeState(GameState.LOCK_PUZZLE);
            GameController.Instance.EnableCursor();
            lockImage.gameObject.SetActive(true);
        }
    }

    private void CloseLockAction(InputAction.CallbackContext ctx)
    {
        CloseLock();
    }

    private void CloseLock()
    {
        if (lockImage.gameObject.activeSelf && GameController.Instance.State == GameState.LOCK_PUZZLE)
        {
            lockImage.gameObject.SetActive(false);
            GameController.Instance.DisableCursor();
            GameController.Instance.GoToPrevState();
        }
    }

    private void UpdateDigitUI(int index)
    {
        digitTexts[index].text = currentDigits[index].ToString();
    }

    private void UpdateAllDigitsUI()
    {
        for (int i = 0; i < currentDigits.Length; i++)
            UpdateDigitUI(i);
    }

    private void CheckForCombination()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentDigits[i] != correctCombination[i])
            {
                return;
            }
        }
        
        Unlock();
    }

    private void Unlock()
    {
        StartCoroutine(UnlockCoroutine());
    }

    private IEnumerator UnlockCoroutine()
    {
        foreach (var text in digitTexts)
        {
            text.GetComponentInParent<Button>().interactable = false;
        }
        
        // TODO UNLOCK SOUND
        yield return new WaitForSeconds(2f);
        
        boxCollider.enabled = false;
        unlockCollider.enabled = true;
        CloseLock();
        gameObject.SetActive(false);
    }

    public void IncrementDigit(int index)
    {
        currentDigits[index] = (currentDigits[index] + 1) % 10;
        UpdateDigitUI(index);
        CheckForCombination();
    }
}
