using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        flags["hasBackpack"] = false;
        flags["hasTorch"] = false;
    }

    public bool GetFlag(string flagName) {

        if (flags.ContainsKey(flagName)) return flags[flagName];
        else return false;
    }

    public void SetFlags(string flagName, bool value) {

        if (flags.ContainsKey(flagName)) flags[flagName] = value;
    }

    public bool IsFlagTrue(string flagName) {

        return flags.ContainsKey(flagName) && flags[flagName];
    }
}
