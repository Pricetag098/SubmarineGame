using UnityEngine;

public class ThermalDisplay : MonoBehaviour
{
    public ThermalRenderer camera;
    [SerializeField] private string shaderKey;
    Renderer renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();
        
    }

    public void Setup()
    {
        renderer.material.SetTexture(shaderKey, camera.Texture);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
