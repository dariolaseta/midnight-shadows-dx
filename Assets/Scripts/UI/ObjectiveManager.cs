using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    private Animator anim;

    private bool isShowingCurrentObjective = false;

    [SerializeField] TMP_Text objectiveText;

    [SerializeField] InputActionReference objectiveButton;

    private void Awake() {
        
        CreateInstance();
        ObtainComponents();
    }

    private void Start() {

        objectiveButton.action.performed += HandleCurrentObjective;
    }

    private void OnDisable() {

        objectiveButton.action.performed -= HandleCurrentObjective;
    }

    public void SetNewObjective(string newObjective) {

        objectiveText.text = newObjective;

        StartCoroutine(ShowCurrentObjective());
    }

    private void HandleCurrentObjective(InputAction.CallbackContext context) {

        if (!isShowingCurrentObjective && GameController.Instance.State == GameState.FREEROAM) {

            isShowingCurrentObjective = true;

            StartCoroutine(ShowCurrentObjective());
        }
    }

    private IEnumerator ShowCurrentObjective() {

        isShowingCurrentObjective = true;

        anim.SetTrigger("Appear");

        yield return new WaitForSeconds(2f);

        anim.SetTrigger("Disappear");

        yield return new WaitForSeconds(2f);

        isShowingCurrentObjective = false;
    }

    private void CreateInstance() {

        if (Instance != null && Instance != this) {

            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void ObtainComponents() {

        anim = GameObject.FindGameObjectWithTag("Objective").GetComponent<Animator>();
    }
}
