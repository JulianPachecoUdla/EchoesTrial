using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Object"))
        {
            PickableItem item = collision.GetComponent<PickableItem>();

            if (item != null)
            {
                player.SetNearbyItem(item);
                Debug.Log("Objeto cerca: " + item.itemName);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Object"))
        {
            PickableItem item = collision.GetComponent<PickableItem>();

            if (item != null)
            {
                player.ClearNearbyItem(item);
                Debug.Log("Saliste del objeto");
            }
        }
    }
}