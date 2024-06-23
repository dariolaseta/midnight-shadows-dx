using UnityEngine;
using UnityEngine.UI;

interface IInteractable {
    void Interact();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] float InteractRange;
    
    [SerializeField] Transform InteractorSource;

    [SerializeField] Image bigCursor;
    [SerializeField] Image grabCursor;

    private void Start() {

        bigCursor.enabled = false;
        grabCursor.enabled = false;

    }

    void Update() {

        HandleInteraction();
    }

    private void HandleInteraction() {
        
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange)) {

            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)) {

                UpdateCursor(hitInfo.collider.gameObject.tag);

                // TODO: Cambiare nel caso volessi mettere tasti configurabili
                if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) {

                    interactObj.Interact();
                }
            } else {

                ResetCursors();
            }
        } else {

            ResetCursors();
        }

        Debug.DrawRay(r.origin, r.direction * InteractRange, Color.green, 1f);
    }

    private void UpdateCursor(string interactObjTag) {

        if (interactObjTag == "Pickup") {

            grabCursor.enabled = true;
        } else {

            grabCursor.enabled = false;
            bigCursor.enabled = true;
        }
    }

    private void ResetCursors() {

        grabCursor.enabled = false;
        bigCursor.enabled = false;
    }
}
