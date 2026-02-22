using UnityEngine;

public class DamageSource : MonoBehaviour, IInteractable
{
    public DamageType DamageType;
    public float Amount;

    private SegmentedHealthbar.Damage damage;

    public GameObject GameObject => gameObject;

    [SerializeField] AudioClip repairSound;
    [SerializeField] SoundPlayer clank;
    [SerializeField] SoundPlayer splash;

    private void Start()
    {
        damage = new SegmentedHealthbar.Damage();
        damage.Amount = Amount;
        damage.Type = DamageType;
        SegmentedHealthbar.Instance.AddDamage(damage);
        clank.Play();
        splash.Play();
    }

    private void Update()
    {
        if(damage.Amount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Repair(float amount)
    {
        damage.Amount -= amount;
    }

    [ContextMenu("Repeair")]
    void TestRepair()
    {
        Repair(10);
    }

    private void OnDestroy()
    {
        SegmentedHealthbar.Instance.ClearDamage(damage);
    }

    public string GetInteractionText()
    {
        return "(E) FIX!";
    }

    public void Focus(Interactor interactor)
    {
      
    }

    public void Unfocus(Interactor interactor)
    {
    
    }

    public void Interact(Interactor interactor)
    {
        Repair(10);
        interactor.interactorAudio.Play(repairSound);
    }

    public bool RequestDefocus(Interactor interactor, IInteractable newTarget)
    {
        return true;
    }
}
