using UnityEngine;
using UnityEngine.UI;

public class DamageSegment : MonoBehaviour
{
    [SerializeField] private Image DisplayElement;
    [SerializeField] private Image DamageBar;
    public SegmentedHealthbar healthbar;
    public DamageType Type;
    public float Amount;
    public void UpdateDisplay()
    {
        if (Amount <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        DisplayElement.sprite = Type.icon;
        DisplayElement.color = Type.color;
        DamageBar.color = Type.color;
        var rectTransform = DamageBar.rectTransform;
        var width = (rectTransform.parent as RectTransform).rect.width * (Amount / healthbar.MaxHp);
        rectTransform.sizeDelta = new(width, rectTransform.sizeDelta.y);
    }
}
