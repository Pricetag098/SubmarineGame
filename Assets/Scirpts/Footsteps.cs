using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] float footstepDistance;
    [SerializeField] SoundPlayer soundPlayer;

    Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, lastPosition) >= footstepDistance)
        {
            soundPlayer.Play();
            lastPosition = transform.position;
        }

    }
}
