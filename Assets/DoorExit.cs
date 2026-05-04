using UnityEngine;

public class DoorExit : MonoBehaviour
{
    [Header("Panel final")]
    public GameObject continuePanel;

    private bool playerNear = false;
    private bool canExit = false;

    private Inventory playerInventory;
    private Player player;

    void Update()
    {
        if (!playerNear || !canExit) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (continuePanel != null)
                continuePanel.SetActive(true);

            if (player != null)
                player.enabled = false;

            Time.timeScale = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponentInParent<Player>();

        if (player == null) return;

        playerInventory = player.GetComponent<Inventory>();
        playerNear = true;

        bool hasFlashlight = playerInventory.HasItem(PickableItem.ItemType.Flashlight);
        bool hasKey = playerInventory.HasItem(PickableItem.ItemType.Key);

        canExit = hasFlashlight && hasKey;

        if (canExit)
        {
            player.ShowDoorInteraction(transform);
        }
        else
        {
            Debug.Log("Necesitas la linterna y la llave.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player p = collision.GetComponentInParent<Player>();

        if (p == null) return;

        playerNear = false;
        canExit = false;

        if (player != null)
            player.HideDoorInteraction();

        playerInventory = null;
        player = null;
    }
}