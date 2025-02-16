using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoregisterAnimator : MonoBehaviour
{
    private Animator animator;
    
    void Awake() => animator = GetComponent<Animator>();
    void OnEnable() => StartCoroutine(RegisterWhenReady());
    void OnDisable() => GameController.Instance.UnregisterAnimator(animator);

    private IEnumerator RegisterWhenReady()
    {
        while (GameController.Instance == null)
        {
            yield return null;   
        }
        
        GameController.Instance.RegisterAnimator(animator);
    }
}
