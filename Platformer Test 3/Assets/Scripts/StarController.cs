using UnityEngine;

public class StarController : MonoBehaviour
{
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            _particleSystem.Play();
            _collider.enabled = false;
            _spriteRenderer.enabled = false;

            SoundEffectManager.PlaySound("GetStar");
        }
    }
}
