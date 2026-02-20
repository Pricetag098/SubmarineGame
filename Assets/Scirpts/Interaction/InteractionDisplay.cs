using TMPro;
using UnityEngine;

public class InteractionDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textField;

    public void Initialise(Interactor interactor)
    {
        textField.text = "";
        interactor.onChangedFocus += ChangeFocus;
    }

    void ChangeFocus(IInteractable interactable)
    {
        if (interactable == null)
            textField.text = "";
        else
            textField.text = interactable.GetInteractionText();

    }
}
