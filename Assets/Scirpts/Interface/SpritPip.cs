using UnityEngine;

public class SpritPip : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;

    public void SetState(bool isActive)
    {
        if (isActive)
            spriteRenderer.sprite = activeSprite;
        else
            spriteRenderer.sprite = inactiveSprite;

    }

    public void SetColour(Color color)
    {
        spriteRenderer.color = color;
    }

}
