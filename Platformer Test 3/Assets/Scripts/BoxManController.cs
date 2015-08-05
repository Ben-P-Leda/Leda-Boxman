using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BoxManController : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody2D _rigidBody2D;
    private Animator _animator;
    private Collider2D[] _colliders;
    private bool _facingRight;
    private VerticalMovementState _verticalMovementState;
    private int _starsCollected;

    public bool Dead { get; private set; }

    public GameObject GameOverPopup;
    public GameObject WinPopup;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        GetColliders();
    }

    private void Start()
    {
        _starsCollected = 0;

        _facingRight = true;
        _verticalMovementState = VerticalMovementState.OnGround;
    }

    private void GetColliders()
    {
        List<Collider2D> colliders = GetComponents<Collider2D>().ToList();
        List<Collider2D> childColliders = GetComponentsInChildren<Collider2D>().ToList();

        colliders.AddRange(childColliders);

        _colliders = colliders.ToArray();
    }

    private void Update()
    {
        if ((!Dead) && (_starsCollected < 3))
        {
            UpdateVerticalMovement();
            UpdateHorizontalMovement();

            if (HasFallenOutOfLevel()) { StartDeathSequence(CauseOfDeath.FellOutOfLevel); }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
    }

    private void UpdateHorizontalMovement()
    {
        if (_verticalMovementState == VerticalMovementState.OnGround)
        {
            int direction = 0;
            if (Input.GetKey(KeyCode.A)) { direction = -1; }
            if (Input.GetKey(KeyCode.D)) { direction = 1; }

            _rigidBody2D.velocity = new Vector2(direction * Speed, _rigidBody2D.velocity.y);

            if (FacingDirectionHasChanged(direction)) { Flip(); }
            _animator.SetBool("Running", direction != 0);
        }
    }

    private bool FacingDirectionHasChanged(int movementDirection)
    {
        bool directionHasChanged = false;
        if ((_facingRight) && (movementDirection < 0)) { directionHasChanged = true; }
        if ((!_facingRight) && (movementDirection > 0)) { directionHasChanged = true; }

        return directionHasChanged;
    }

    private void Flip()
    {
        Vector3 currentScale = _transform.localScale;
        currentScale.x *= -1;
        _transform.localScale = currentScale;

        _facingRight = !_facingRight;
    }

    private void UpdateVerticalMovement()
    {
        if ((_rigidBody2D.velocity.y < 0.0f) && (_verticalMovementState != VerticalMovementState.Falling)) { StartFalling(); }

        switch (_verticalMovementState)
        {
            case VerticalMovementState.OnGround: CheckForJumpStart(); break;
            case VerticalMovementState.Falling: HandleFalling(); break;
        }
    }

    private void StartFalling()
    {
        _verticalMovementState = VerticalMovementState.Falling;
        _animator.SetBool("Falling", true);
        _animator.SetBool("Jumping", false);
    }

    private void CheckForJumpStart()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _rigidBody2D.AddForce(new Vector2(0.0f, 300.0f));
            _verticalMovementState = VerticalMovementState.Rising;
            _animator.SetBool("Jumping", true);

            SoundEffectManager.PlaySound("Jump");
        }
    }

    private void HandleFalling()
    {
        if (_rigidBody2D.velocity.y >= -0.01f)
        {
            _verticalMovementState = VerticalMovementState.OnGround;
            _animator.SetBool("Falling", false);
            _animator.SetBool("Running", _rigidBody2D.velocity.x != 0.0f);
        }
    }

    private bool HasFallenOutOfLevel()
    {
        return (Camera.main.IsOffBottomOfScreen(_transform.position, 1.0f));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if ((!Dead) && (_starsCollected < 3))
        {
            if (collider.tag == "Enemy") { StartDeathSequence(CauseOfDeath.Generic); }
            if (collider.tag == "Lava") { StartDeathSequence(CauseOfDeath.FellIntoLava); }
            if (collider.tag == "Star") { CollectStar(collider.gameObject); }
        }
    }

    private void StartDeathSequence(CauseOfDeath causeOfDeath)
    {
        Dead = true;

        for (int i = 0; i < _colliders.Length; i++) { _colliders[i].enabled = false; }

        switch (causeOfDeath)
        {
            case CauseOfDeath.Generic: PlayGenericDeathAnimation(); break;
            case CauseOfDeath.FellIntoLava: PlayLavaDeathEffect(); break;
            case CauseOfDeath.FellOutOfLevel: SoundEffectManager.PlaySound("Death"); break;
        }

        GameOverPopup.SetActive(true);
    }

    private void PlayGenericDeathAnimation()
    {
        Vector2 velocity = new Vector2(-_rigidBody2D.velocity.x, 0.0f);
        _rigidBody2D.velocity = velocity;
        _rigidBody2D.AddForce(new Vector2(0.0f, 100.0f));

        _animator.SetBool("Dead", true);

        SoundEffectManager.PlaySound("Death");
    }

    private void PlayLavaDeathEffect()
    {
        _transform.FindChild("BlockMan_Body").gameObject.SetActive(false);
        _transform.FindChild("BlockMan_DeathParticles").gameObject.SetActive(true);

        SoundEffectManager.PlaySound("Death");
    }

    private void CollectStar(GameObject star)
    {
        _starsCollected++;

        if (_starsCollected == 3)
        {
            WinPopup.SetActive(true);
            SoundEffectManager.PlaySound("Music");
        }
    }

    private enum VerticalMovementState
    {
        OnGround,
        Rising,
        Falling
    }

    private enum CauseOfDeath
    {
        Generic,
        FellOutOfLevel,
        FellIntoLava
    }

    private const float Speed = 4.0f;
    private const float Moving_Platform_Offset_Base = 0.255f;
    private const float Moving_Platform_Offset_Tolerance = 0.001f;
}
