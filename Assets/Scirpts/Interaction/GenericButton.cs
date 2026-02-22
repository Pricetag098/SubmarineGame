using UnityEngine;
using UnityEngine.Events;

public class GenericButton : MonoBehaviour, IInteractable
{
    [SerializeField] string displayText;
    [SerializeField] UnityEvent onInteract;

    public GameObject GameObject => gameObject;

    public void Focus(Interactor interactor)
    {

    }

    public string GetInteractionText()
    {
        return displayText;
    }

    public void Interact(Interactor interactor)
    {
        onInteract?.Invoke();
    }

    public bool RequestDefocus(Interactor interactor, IInteractable newTarget)
    {
        return true;
    }

    public void Unfocus(Interactor interactor)
    {

    }
}
