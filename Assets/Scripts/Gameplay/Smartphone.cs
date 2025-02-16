using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Smartphone : MonoBehaviour
{
    [Header("Input actions")]
    [SerializeField] private InputActionReference smartphoneAction;
    [SerializeField] private InputActionReference smartphoneLightAction;
    
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Light smartphoneLight;
    
    [Header("Animator settings")]
    [SerializeField] private string smartphoneOnTrigger;
    [SerializeField] private string smartphoneOffTrigger;
    
    private bool isSmartphoneOn = false;
    
    private void OnEnable()
    {
        smartphoneAction.action.Enable();
        smartphoneLightAction.action.Enable();

        smartphoneAction.action.performed += SmartphoneBehavior;
        smartphoneLightAction.action.performed += SmartphoneLightBehavior;
    }

    private void OnDisable()
    {
        smartphoneAction.action.performed -= SmartphoneBehavior;
        smartphoneLightAction.action.performed -= SmartphoneLightBehavior;
        
        smartphoneAction.action.Disable();
        smartphoneLightAction.action.Disable();
    }

    private void Awake()
    {
        anim.gameObject.SetActive(false);
        
        smartphoneLight.enabled = false;
    }

    private void SmartphoneLightBehavior(InputAction.CallbackContext obj)
    {
        if (isSmartphoneOn && GameController.Instance.State == GameState.FREEROAM)
        {
            smartphoneLight.enabled = !smartphoneLight.enabled;
        }
    }

    private void SmartphoneBehavior(InputAction.CallbackContext obj)
    {
        if (!CanUseSmartphone()) return;

        if (!isSmartphoneOn)
        {
            anim.gameObject.SetActive(true);
            anim.SetTrigger(smartphoneOnTrigger);
            isSmartphoneOn = true;
        }
        else
        {
            smartphoneLight.enabled = false;
            anim.SetTrigger(smartphoneOffTrigger);
            isSmartphoneOn = false;
        }
    }

    private bool CanUseSmartphone()
    {
        return GameController.Instance.State == GameState.FREEROAM &&
               Flags.Instance.IsFlagTrue(FlagEnum.HAS_SMARTPHONE);
    }
}
