using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public DamageType DamageType;
    public float Amount;

    private SegmentedHealthbar.Damage damage;
    private void Start()
    {
        damage = new SegmentedHealthbar.Damage();
        damage.Amount = Amount;
        damage.Type = DamageType;
        SegmentedHealthbar.Instance.AddDamage(damage);
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
}
