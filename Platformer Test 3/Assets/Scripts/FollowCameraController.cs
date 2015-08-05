using UnityEngine;

public class FollowCameraController : MonoBehaviour
{
    private Vector2 _distanceFromCenterToEdges;
    private Vector2 _scrollingPushClampingLimits;
    private Rect _levelClampingLimits;
    private Transform _transform;
    private Transform _playerTransform;
    private BoxManController _playerController;

    public GameObject Player;
    public float VerticalScrollMargin;
    public Rect LevelDimensions;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _playerTransform = Player.GetComponent<Transform>();
        _playerController = Player.GetComponent<BoxManController>();
    }

    private void Start()
    {
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));
        _distanceFromCenterToEdges = new Vector2(topRight.x, topRight.y);
        _scrollingPushClampingLimits = new Vector2(_distanceFromCenterToEdges.x, _distanceFromCenterToEdges.y * VerticalScrollMargin);
        _levelClampingLimits = new Rect()
        {
            xMin = Mathf.Min(LevelDimensions.xMin, -_distanceFromCenterToEdges.x),
            xMax = Mathf.Max(LevelDimensions.xMax, _distanceFromCenterToEdges.x),
            yMin = Mathf.Min(LevelDimensions.yMin, -_distanceFromCenterToEdges.y),
            yMax = Mathf.Max(LevelDimensions.yMax, _distanceFromCenterToEdges.y)
        };
    }

    private void Update()
    {
        if (!_playerController.Dead)
        {
            Vector3 position = _transform.position;

            position.x = _playerTransform.position.x;
            position.y = Mathf.Clamp(position.y, _playerTransform.position.y - _scrollingPushClampingLimits.y, _playerTransform.position.y + _scrollingPushClampingLimits.y);

            position.x = Mathf.Clamp(position.x, _levelClampingLimits.xMin, _levelClampingLimits.xMax);
            position.y = Mathf.Clamp(position.y, _levelClampingLimits.yMin, _levelClampingLimits.yMax);

            _transform.position = position;
        }
    }
}
