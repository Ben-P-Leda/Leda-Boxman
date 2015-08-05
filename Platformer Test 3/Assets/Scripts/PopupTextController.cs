using UnityEngine;

public class PopupTextController : MonoBehaviour
{
    private Transform _transform;
    private float _transformer;
    
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        _transformer = 0.0f;
        _transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
    }

    private void Update()
    {
        _transformer += Time.deltaTime * 2.0f;
        _transform.rotation = Quaternion.Euler(Mathf.Cos(_transformer) * 40.0f, Mathf.Sin(_transformer) * 50.0f, Mathf.Sin(_transformer) * 10.0f);

        if (_transform.localScale.x < 1.0f)
        {
            _transform.localScale = new Vector3(_transformer, _transformer, 1.0f);
        }
        else if (Input.anyKeyDown)
        {
            Application.LoadLevel("TestScene");
        }
    }
}
