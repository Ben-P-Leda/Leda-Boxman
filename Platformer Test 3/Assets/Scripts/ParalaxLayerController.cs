using UnityEngine;
using System.Collections;

public class ParalaxLayerController : MonoBehaviour
{
    private Transform _transform;
    private Vector2 _basePosition;
    private Transform _cameraTransform;
    private Vector2 _cameraStartPosition;

    public float LayerSpeedStep;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _cameraTransform = Camera.main.GetComponent<Transform>();
    }

    private void Start()
    {
        _basePosition = _transform.position;
        _cameraStartPosition = _cameraTransform.position;
    }

    private void Update()
    {
        Vector2 cameraOffset = new Vector2(_cameraTransform.position.x, _cameraTransform.position.y) - _cameraStartPosition;
        _transform.position = _basePosition + cameraOffset * LayerSpeedStep;
    }
}
