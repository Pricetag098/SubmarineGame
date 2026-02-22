using System;
using System.Collections.Generic;
using UnityEngine;

public class SegmentedHealthbar : MonoBehaviour
{
    public class Damage
    {
        public DamageType Type;
        public float Amount;
    }
    public float MaxHp;
    public List<DamageType> DamageTypes = new();
    private List<DamageSegment> DamageSegments = new();
    private List<Damage> damages = new List<Damage>();
    [SerializeField] private DamageSegment damageSegmentPrefab;
    public static SegmentedHealthbar Instance;
    public Action OnDeath;
    bool dead = false;

    private void Awake()
    {
        Instance = this;
    }

    public void AddDamage(Damage damage)
    {
        damages.Add(damage);
        if (!DamageTypes.Contains(damage.Type))
        {
            DamageSegments.Add(Instantiate(damageSegmentPrefab, transform));
            DamageTypes.Add(damage.Type);
        }
    }
    public void ClearDamage(Damage damage)
    {
        damages.Remove(damage);
        
    }
    private void Update()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        var totalDamage = 0f;
        Comparison<DamageType> value = (DamageType a, DamageType b) => { return a.priority.CompareTo(b.priority); };
        DamageTypes.Sort(value);
        for (int i = 0; i < DamageTypes.Count; i++)
        {
            var damageType = DamageTypes[i];
            var segment = DamageSegments[i];
            var amount = 0f;
            foreach (var damage in damages)
            {
                if(damage.Type == damageType)
                    amount += damage.Amount;
            }
            segment.Type = damageType;
            segment.Amount = amount;
            segment.healthbar = this;
            segment.UpdateDisplay();
            totalDamage += amount;
        }
        if (totalDamage > MaxHp&& !dead)
        {
            dead = true;
            OnDeath?.Invoke();
        }
    }


}
