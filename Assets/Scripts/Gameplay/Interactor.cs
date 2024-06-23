using UnityEngine;
using UnityEngine.UI;

interface IInteractable {
    void Interact();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] float InteractRange;
    
    [SerializeField] Transform InteractorSource;

    [SerializeField] Image cursor;
    [SerializeField] Image grabCursor;

    private Vector3 normalCurorsScale;

    private void Start() {

        cursor.enabled = false;
        grabCursor.enabled = false;

        normalCurorsScale = cursor.transform.localScale;
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

            cursor.enabled = false;
            grabCursor.enabled = true;
        } else {

            grabCursor.enabled = false;
            cursor.enabled = true;

            cursor.transform.localScale *= 2;
        }
    }

    private void ResetCursors() {

        cursor.enabled = true;
        grabCursor.enabled = false;

        cursor.transform.localScale = normalCurorsScale;
    }
}
