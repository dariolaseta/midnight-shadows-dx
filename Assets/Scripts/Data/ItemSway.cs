using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    [Header("Input actions")]
    [SerializeField] private InputActionReference lookActionReference;
    
    private void OnEnable() => lookActionReference.action.Enable();
    private void OnDisable() => lookActionReference.action.Disable();
    private void Update() => SwayItem(); 

    private void SwayItem() {

        if (GameController.Instance.State != GameState.FREEROAM) return;
        
        Vector2 mouseDelta = lookActionReference.action.ReadValue<Vector2>();
        float mouseX = mouseDelta.x * swayMultiplier;
        float mouseY = mouseDelta.y * swayMultiplier;
        
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
