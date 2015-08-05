using UnityEngine;
using System.Collections.Generic;

public class SwitchController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private bool _actionState;

    public List<Sprite> StateSprites;
    public int StateIndex;
    public int LockStateAtIndex;
    public GameObject KeyItem;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetState(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) { _actionState = true; }
    }

    private void SetState(int step)
    {
        StateIndex = (StateIndex + StateSprites.Count + step) % StateSprites.Count;
        _spriteRenderer.sprite = StateSprites[StateIndex];
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _actionState = false;
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if ((_actionState) && (StateIndex != LockStateAtIndex) && (collider.tag == "Player")) 
        {
            if ((KeyItem == null) || (InventoryController.CarryingItem(KeyItem)))
            {
                SetState(1);
                _actionState = false;
                SoundEffectManager.PlaySound("Switch");

                if (KeyItem != null) { InventoryController.UseItem(KeyItem); }
            }
        }
    }
}