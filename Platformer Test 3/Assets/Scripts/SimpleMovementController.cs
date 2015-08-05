using UnityEngine;
using System.Collections.Generic;

public class SimpleMovementController : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody2D _rigidBody2D;
    private int _targetEndpointIndex;
    private Vector2 _velocity;
    private bool _xAxisIsFixed;
    private bool _yAxisIsFixed;

    public bool FlipIfMovingLeft;
    public float Speed;
    public List<Vector2> MovementEndpoints;
   
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _transform.position = MovementEndpoints[0];
        _targetEndpointIndex = 0;

        SetTrajectory();
    }

    private void SetTrajectory()
    {
        Vector2 currentEndpoint = MovementEndpoints[_targetEndpointIndex];

        _targetEndpointIndex = (_targetEndpointIndex + 1) % MovementEndpoints.Count;
        _xAxisIsFixed = (currentEndpoint.x == MovementEndpoints[_targetEndpointIndex].x);
        _yAxisIsFixed = (currentEndpoint.y == MovementEndpoints[_targetEndpointIndex].y);

        Vector2 direction = MovementEndpoints[_targetEndpointIndex] - new Vector2(_transform.position.x, _transform.position.y);
        direction.Normalize();
        _velocity = direction * Speed;

        if (FlipIfMovingLeft)
        {
            if (Mathf.Sign(direction.x) != Mathf.Sign(_transform.localScale.x))
            {
                Vector3 currentScale = _transform.localScale;
                currentScale.x *= -1;
                _transform.localScale = currentScale;
            }
        }
    }

    private void Update()
    {
        if ((HasPassedEndpoint(_rigidBody2D.velocity.x, MovementEndpoints[_targetEndpointIndex].x,_transform.position.x, _xAxisIsFixed)) || 
            (HasPassedEndpoint(_rigidBody2D.velocity.y, MovementEndpoints[_targetEndpointIndex].y,_transform.position.y, _yAxisIsFixed)))
        {
            SetTrajectory();
        }

        _rigidBody2D.velocity = _velocity;
    }

    private bool HasPassedEndpoint(float velocity, float endpointPosition, float position, bool axisIsFixed)
    {
        if (axisIsFixed) { return false; }

        if (Mathf.Sign(velocity) != Mathf.Sign(endpointPosition - position)) { return true; }

        return false;
    }
}
