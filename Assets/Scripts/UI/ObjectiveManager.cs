using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    private Animator anim;

    private bool isShowingCurrentObjective = false;

    [SerializeField] TMP_Text objectiveText;

    private void Awake() {
        
        CreateInstance();
        ObtainComponents();
    }

    void Update() {
        
        HandleCurrentObjective();
    }

    public void SetNewObjective(string newObjective) {

        objectiveText.text = newObjective;

        StartCoroutine(ShowCurrentObjective());
    }

    private void HandleCurrentObjective() {

        // TODO: Cambiare se decido di implementare tasti modificabili
        if (Input.GetKeyDown(KeyCode.O) && !isShowingCurrentObjective) {

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

            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void ObtainComponents() {

        anim = GameObject.FindGameObjectWithTag("Objective").GetComponent<Animator>();
    }
}
