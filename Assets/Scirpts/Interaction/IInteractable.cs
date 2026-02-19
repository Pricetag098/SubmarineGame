using UnityEngine;

public interface IInteractable
{
    public void Focus(Interactor interactor);

    public void Unfocus(Interactor interactor);

    public void Interact(Interactor interactor);

}
