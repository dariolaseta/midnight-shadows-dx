using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    private Animator objectiveAnimator;

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

        //TODO: Implementazione

        isShowingCurrentObjective = true;
        objectiveText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        objectiveText.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

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

        
    }
}
