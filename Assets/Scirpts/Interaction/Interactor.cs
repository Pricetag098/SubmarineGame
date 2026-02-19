using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] float interactionRange;
    [SerializeField] Transform playerHead;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] InputActionReference interactInput;

    bool focusLocked;
    IInteractable currentFocus;
    public void SetFocusLock(bool isLocked)
    {
        focusLocked = isLocked;
    }

    public Vector3 GetHead()
    {
        return playerHead.position;
    }
    public Vector3 GetLookDirection()
    {
        return playerHead.forward;
    }

    public Vector3 GetLookPoint(float distance)
    {
        return playerHead.position + (playerHead.forward * distance);
    }

    private void OnEnable()
    {
        interactInput.action.Enable();
    }

    private void Update()
    {
        //get input
        if (interactInput.action.WasPressedThisFrame() && currentFocus != null)
        {
            currentFocus.Interact(this);
            return;
        }

        if (focusLocked)
            return;

        if (Physics.Raycast(playerHead.position, playerHead.forward, out RaycastHit hit, interactionRange, interactionLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {

                SetFocus(interactable);

            }

            else
                SetFocus(null);
        }

        else
            SetFocus(null);


       
    }

    void DoInteract(IInteractable interactable)
    {

    }
    void SetFocus(IInteractable interactable)
    {
        if (currentFocus == interactable)
            return;

        if (currentFocus != null)
            currentFocus.Unfocus(this);

        currentFocus = interactable;

        if (currentFocus != null)
            currentFocus.Focus(this);
    }

}
