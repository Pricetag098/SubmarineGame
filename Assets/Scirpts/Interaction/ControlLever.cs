using UnityEngine;

public class ControlLever : MonoBehaviour, IInteractable
{

    public System.Action<float> onValueChanged;
    public System.Action<bool> onInteract;

    public enum ControlType { Thrust, Bouyancy, Rudder}

    public GameObject GameObject => gameObject;

    public float controlPercentage;

    [SerializeField] ControlType controlType;

    [SerializeField] PlayerConsole console;

    [SerializeField] string displayName;

    [SerializeField] float moveRailLength;
    [SerializeField] float zeroingThreshold;
    [SerializeField] float zeroingSpeed;

    [SerializeField] Transform wheel;

    [SerializeField] bool objectMoves = true;

    float previousValue;
    bool active;
    Vector3 tInitial;
    float moveDistance;

    Interactor currentInteractor;

    private void Start()
    {
        tInitial = transform.position;
    }

    public string GetInteractionText()
    {
        return displayName;
    }

    public void Focus(Interactor interactor)
    {

    }

    public void Unfocus(Interactor interactor)
    {
        if (active)
            Cancel(interactor);

    }

    public bool RequestDefocus(Interactor interactor, IInteractable newTarget)
    {
        if(newTarget != null)
            return true;

        else return !active;
    }

    public void Interact(Interactor interactor)
    {
        if (!active)
        {
            currentInteractor = interactor;
            active = true;
            onInteract?.Invoke(true);
        }

        else
            Cancel(interactor);
    }

    void Cancel(Interactor interactor)
    {
        active = false;
        onInteract?.Invoke(false);
    }

    private void Update()
    {
        controlPercentage = moveDistance / moveRailLength;
        if (controlPercentage != previousValue)
        {
            onValueChanged?.Invoke(controlPercentage);
            previousValue = controlPercentage;
        }

        switch (controlType)
        {
            case ControlType.Thrust:
                console.submarine.thrustControl = controlPercentage;
                break;
            case ControlType.Bouyancy:
                console.submarine.bouyancyControl = controlPercentage;
                break;
            case ControlType.Rudder:
                console.submarine.turnControl = controlPercentage;
                break;
        }

        //stupid transform maths for stupid problems

        if (active)
        {
            Vector3 localOrigin = transform.InverseTransformPoint(tInitial);

            Vector3 lookPoint = currentInteractor.GetLookPoint(Vector3.Distance(transform.position, currentInteractor.GetHead()));
            Vector3 localPoint = transform.InverseTransformPoint(lookPoint);

            float deltaX = localPoint.x - localOrigin.x;

            moveDistance = Mathf.Clamp(deltaX, -moveRailLength, moveRailLength);

            Vector3 newLocal = new Vector3(localOrigin.x + moveDistance, 0f, 0f);

            if(objectMoves)
                transform.position = transform.TransformPoint(newLocal);

            wheel.eulerAngles = new Vector3 (-90f, (moveDistance/moveRailLength) * 1080, 0f);

        }
        else
        {

            if (Mathf.Abs(controlPercentage) < 0.01)
                moveDistance = 0f;

            else if (Mathf.Abs(controlPercentage) < zeroingThreshold)
            {
                moveDistance -= zeroingSpeed * Mathf.Sign(moveDistance) * Time.deltaTime;
            }

            Vector3 localOrigin = transform.InverseTransformPoint(tInitial);
            Vector3 newLocal = new Vector3(localOrigin.x + moveDistance, 0f, 0f);

            if (objectMoves)
                transform.position = transform.TransformPoint(newLocal);
        }

    }


}
