using UnityEngine;

public class EnemyFish : MonoBehaviour
{
    [SerializeField] private Transform model;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectionRange = 10;
    private State state;
    enum State
    {
        Idle,
        Roam,
        Chasing
    }
}
