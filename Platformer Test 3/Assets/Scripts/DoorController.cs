using UnityEngine;
using System.Collections.Generic;

public class DoorController : MonoBehaviour
{
    private List<SwitchController> _switchControllers;
    private Collider2D _collider2D;
    private Transform _transform;
    private float _originalScale;

    public List<GameObject> ControllingSwitches;
    public List<int> RequiredSwitchStates;

    private void Awake()
    {
        _switchControllers = new List<SwitchController>();
        for (int i = 0; i < ControllingSwitches.Count; i++)
        {
            _switchControllers.Add(ControllingSwitches[i].GetComponent<SwitchController>());
        }

        _collider2D = GetComponent<Collider2D>();
        _transform = GetComponent<Transform>();
        _originalScale = _transform.localScale.y;
    }

    void Start()
    {

    }

    void Update()
    {
        bool shouldBeActive = false;
        for (int i = 0; i < RequiredSwitchStates.Count; i++)
        {
            if (_switchControllers[i].StateIndex != RequiredSwitchStates[i]) { shouldBeActive = true; }
        }

        _collider2D.enabled = shouldBeActive;
        Vector3 scale = _transform.localScale;
        scale.y = shouldBeActive ? _originalScale : 0.0f;
        _transform.localScale = scale;
    }
}