using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlagEnum { HAS_BACKPACK, HAS_TORCH, HAS_SMARTPHONE }
public class Flags : MonoBehaviour
{
    public static Flags Instance { get; private set; }

    private Dictionary<FlagEnum, bool> flags = new Dictionary<FlagEnum, bool>();

    void Awake() {

        CreateInstance();

        InitializeFlags();
    }

    private void CreateInstance() {

        if (Instance != null && Instance != this) {

            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void InitializeFlags() {

        foreach (FlagEnum flag in Enum.GetValues(typeof(FlagEnum)))
        {
            flags[flag] = false;
        }
    }

    public bool GetFlag(FlagEnum flagName) {

        if (flags.ContainsKey(flagName)) return flags[flagName];
        else return false;
    }

    public void SetFlags(FlagEnum flagName, bool value) {

        if (flags.ContainsKey(flagName)) flags[flagName] = value;
    }

    public bool IsFlagTrue(FlagEnum flagName) {

        return flags.ContainsKey(flagName) && flags[flagName];
    }
}
