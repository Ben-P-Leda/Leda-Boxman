using UnityEngine;

public class KeyController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            InventoryController.AddItem(this.gameObject);
            SoundEffectManager.PlaySound("GetKey");
        }
    }
}
