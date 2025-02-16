using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlagEnum { HAS_BACKPACK, HAS_TORCH, HAS_SMARTPHONE }
public class Flags : MonoBehaviour
{
    public static Flags Instance { get; private set; }

    private Dictionary<string, bool> flags = new Dictionary<string, bool>();

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

        foreach (var flag in Enum.GetValues(typeof(FlagEnum)))
        {
            flags[flag.ToString()] = false;
        }
    }

    public bool GetFlag(FlagEnum flagName) {

        if (flags.ContainsKey(flagName.ToString())) return flags[flagName.ToString()];
        else return false;
    }

    public void SetFlags(FlagEnum flagName, bool value) {

        if (flags.ContainsKey(flagName.ToString())) flags[flagName.ToString()] = value;
    }

    public bool IsFlagTrue(FlagEnum flagName) {

        return flags.ContainsKey(flagName.ToString()) && flags[flagName.ToString()];
    }
}
