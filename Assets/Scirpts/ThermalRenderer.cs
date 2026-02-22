using UnityEngine;
using UnityEngine.UI;

public class ThermalRenderer : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private ComputeShader _shader;
    [SerializeField] private RenderTexture _camTargetTexture, _displayTarget;
    [SerializeField] private RawImage _displayObject, test2;
    [SerializeField] private int _displayHeight = 2048, _displayWidth = 2048;
    [SerializeField] private float _multipler = 1;
    [SerializeField] private float _captureTime;

    public RenderTexture Texture => _displayTarget;

    private bool _capturing = false;
    private float _captureStartTime;


    void Awake()
    {
        _camTargetTexture = new(_displayHeight, _displayWidth, 16, UnityEngine.Experimental.Rendering.DefaultFormat.HDR);
        _camTargetTexture.format = RenderTextureFormat.ARGBFloat;
        _camTargetTexture.enableRandomWrite = true;
        _displayTarget = new(_displayHeight, _displayWidth, 16, UnityEngine.Experimental.Rendering.DefaultFormat.HDR);
        _displayTarget.format = RenderTextureFormat.ARGBFloat;
        _displayTarget.enableRandomWrite = true;
        _camera.targetTexture = _camTargetTexture;
        if(_displayObject)
            _displayObject.texture = _displayTarget;
        if (test2)
            test2.texture = _camTargetTexture;
    }

    private void Start()
    {
        StartCapture();
    }

    private void OnDestroy()
    {
        _camTargetTexture.Release();
        _displayTarget.Release();
    }

    // Update is called once per frame
    void Update()
    {
        if (_capturing)
        {
            UpdateTexture();
            if(Time.timeSinceLevelLoad - _captureStartTime > _captureTime)
            {
                _capturing = false;
                StartCapture();
            }
        }
    }

    void UpdateTexture()
    {
        _camera.Render();
        _shader.SetTexture(0, "Result", _displayTarget);
        _shader.SetTexture(0, "Caputure", _camTargetTexture);
        _shader.SetFloat("Multiplier", (Time.deltaTime / _captureTime) * _multipler);
        _shader.Dispatch(0, _displayWidth / 8, _displayHeight / 8, 1);
    }
    [ContextMenu("Capture")]
    public void StartCapture()
    {
        _capturing = true;
        _shader.SetTexture(1, "Result", _displayTarget);
        _shader.Dispatch(1, _displayWidth / 8, _displayHeight / 8, 1);
        _captureStartTime = Time.timeSinceLevelLoad;
    }
    
}
