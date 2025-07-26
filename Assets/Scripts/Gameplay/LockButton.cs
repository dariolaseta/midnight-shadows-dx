using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockButton : MonoBehaviour
{
    [SerializeField] private LockSystem lockSystem;

    [SerializeField] private int digitIndex;

    public void OnPress()
    {
        lockSystem.IncrementDigit(digitIndex);
    }
}
