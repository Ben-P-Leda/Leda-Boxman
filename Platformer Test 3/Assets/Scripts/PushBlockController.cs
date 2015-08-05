using UnityEngine;
using System.Collections;

public class PushBlockController : MonoBehaviour
{
    private float _soundTimer;
    private Transform _transform;
    private Transform _playerTransform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _playerTransform = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player") 
        {
            if (_playerTransform == null) { _playerTransform = collision.collider.GetComponent<Transform>(); }
            if (BeingPushed()) { StartNewPush(); }
        }
    }

    private void StartNewPush()
    {
        SoundEffectManager.PlaySound("Push Block");
        _soundTimer = Sound_Interval;
    }

    private bool BeingPushed()
    {

        if (_playerTransform.position.y > _transform.position.y + (_transform.localScale.y * 0.55f)) { return false; }
        if (_playerTransform.position.y < _transform.position.y - (_transform.localScale.y * 0.55f)) { return false; }
        if (_playerTransform.position.x < _transform.position.x - (_transform.localScale.x * 0.55f)) { return true; }
        if (_playerTransform.position.x > _transform.position.x + (_transform.localScale.x * 0.55f)) { return true; }

        return false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.collider.tag == "Player") && (BeingPushed()))
        {
            _soundTimer -= Time.deltaTime;
            if (_soundTimer <= 0.0f) { StartNewPush(); }
        }
    }

    private const float Sound_Interval = 1.0f;
}
