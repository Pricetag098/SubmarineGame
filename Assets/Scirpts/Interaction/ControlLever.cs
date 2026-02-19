using UnityEngine;

public class ControlLever : MonoBehaviour, IInteractable
{
    public float moveDistance;
    [SerializeField] float moveRailLength;
    [SerializeField] float zeroingThreshold;
    [SerializeField] float zeroingSpeed;

    Interactor currentInteractor;
    [SerializeField] bool active;
    float yInitial;
    Vector3 tInitial;

    private void Start()
    {
        yInitial = transform.position.y;
        tInitial = transform.position;
    }

    public void Focus(Interactor interactor)
    {

    }

    public void Unfocus(Interactor interactor)
    {
        if (active)
            Cancel(interactor);

    }

    public void Interact(Interactor interactor)
    {
        if (!active)
        {
            currentInteractor = interactor;
            active = true;
            interactor.SetFocusLock(true);
        }

        else
            Cancel(interactor);
    }

    void Cancel(Interactor interactor)
    {
        active = false;
        interactor.SetFocusLock(false);
    }

    private void Update()
    {

        //stupid transform maths for stupid problems

        if (active)
        {
            Vector3 localOrigin = transform.InverseTransformPoint(tInitial);

            Vector3 lookPoint = currentInteractor.GetLookPoint(Vector3.Distance(transform.position, currentInteractor.GetHead()));
            Vector3 localPoint = transform.InverseTransformPoint(lookPoint);

            float deltaX = localPoint.x - localOrigin.x;

            moveDistance = Mathf.Clamp(deltaX, -moveRailLength, moveRailLength);

            Vector3 newLocal = new Vector3(localOrigin.x + moveDistance, 0f, 0f);
            transform.position = transform.TransformPoint(newLocal);

        }
        else
        {

            if (Mathf.Abs(moveDistance) < 0.01)
                moveDistance = 0f;
            else if (Mathf.Abs(moveDistance) < zeroingThreshold)
            {
                moveDistance -= zeroingSpeed * Mathf.Sign(moveDistance) * Time.deltaTime;
            }

            Vector3 localOrigin = transform.InverseTransformPoint(tInitial);
            Vector3 newLocal = new Vector3(localOrigin.x + moveDistance, 0f, 0f);
            transform.position = transform.TransformPoint(newLocal);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(tInitial, tInitial + (transform.right * moveRailLength));
        Gizmos.DrawLine(tInitial, tInitial + (transform.right * -moveRailLength));

        //if (active)
        //{
        //    Gizmos.color = Color.cyan;
        //    Vector3 projection = Vector3.Project(currentInteractor.GetLookDirection(), transform.right);
        //    Gizmos.DrawLine(tInitial, tInitial + projection);
        //}

    }


}
