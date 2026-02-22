using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public System.Action<IInteractable> onChangedFocus;
    public InputActionReference Input => interactInput;

    [SerializeField] InteractionDisplay display;
    [SerializeField] float interactionRange;
    [SerializeField] Transform playerHead;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] InputActionReference interactInput;

    IInteractable currentFocus;

    public Vector3 GetHead()
    {
        return playerHead.position;
    }
    public Vector3 GetLookPoint(float distance)
    {
        return playerHead.position + (playerHead.forward * distance);
    }

    private void Start()
    {
        display.Initialise(this);
    }

    private void OnEnable()
    {
        interactInput.action.Enable();
    }

    private void Update()
    {
        //get input
        if (currentFocus != null)
        {
            if (Vector3.Distance(playerHead.position, currentFocus.GameObject.transform.position) > interactionRange)
            {
                SetFocus(null);
                return;
            }

            if (interactInput.action.WasPressedThisFrame())
            {
                currentFocus.Interact(this);
                return;
            }
        }


        if (Physics.Raycast(playerHead.position, playerHead.forward, out RaycastHit hit, interactionRange, interactionLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {

                if (currentFocus == null)

                    SetFocus(interactable);

                else if(currentFocus.RequestDefocus(this, interactable))

                    SetFocus(interactable);

            }

            else if (currentFocus != null && currentFocus.RequestDefocus(this, null))
                SetFocus(null);
        }

        else if (currentFocus != null && currentFocus.RequestDefocus(this, null))
            SetFocus(null);


       
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

        onChangedFocus?.Invoke(currentFocus);
    }

}
