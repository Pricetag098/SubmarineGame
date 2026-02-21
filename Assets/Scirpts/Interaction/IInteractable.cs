using UnityEngine;

public interface IInteractable
{
    //the moment you admit your interface needed to be a monobehaviour
    public GameObject GameObject { get; }

    public string GetInteractionText();
    public void Focus(Interactor interactor);

    public void Unfocus(Interactor interactor);

    public void Interact(Interactor interactor);

    public bool RequestDefocus(Interactor interactor, IInteractable newTarget);

}
