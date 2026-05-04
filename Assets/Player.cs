using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private Inventory inventory;

    private Vector2 movement;

    [Header("Interacción")]
    private PickableItem nearbyItem;
    private Transform interactionTarget; // 🔥 NUEVO (objeto o puerta)

    [Header("UI Interacción")]
    public TextMeshProUGUI interactText;
    public Vector3 interactOffset = new Vector3(0.6f, 0.2f, 0f);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        // 🎮 Movimiento
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        animator.SetFloat("Speed", movement.magnitude);

        // 🔁 Flip + mano
        if (movement.x > 0)
        {
            sprite.flipX = false;

            if (inventory != null)
                inventory.UpdateHandDirection(true);
        }
        else if (movement.x < 0)
        {
            sprite.flipX = true;

            if (inventory != null)
                inventory.UpdateHandDirection(false);
        }

        // 🖐️ Recoger objetos
        if (Input.GetKeyDown(KeyCode.E) && nearbyItem != null)
        {
            if (inventory == null)
            {
                Debug.LogWarning("El Player no tiene Inventory.");
                return;
            }

            bool added = inventory.AddItem(nearbyItem);

            if (added)
            {
                Debug.Log("Recogiste: " + nearbyItem.itemName);

                Destroy(nearbyItem.gameObject);
                nearbyItem = null;
                interactionTarget = null;

                if (interactText != null)
                    interactText.gameObject.SetActive(false);
            }
        }

        // 🎯 Posicionar la "E"
        if (interactionTarget != null && interactText != null)
        {
            Vector3 worldPosition = interactionTarget.position + interactOffset;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            interactText.transform.position = screenPosition;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }

    // 🧱 OBJETOS (linterna, llave, etc.)
    public void SetNearbyItem(PickableItem item)
    {
        nearbyItem = item;
        interactionTarget = item.transform;

        if (interactText != null)
            interactText.gameObject.SetActive(true);
    }

    public void ClearNearbyItem(PickableItem item)
    {
        if (nearbyItem == item)
        {
            nearbyItem = null;
            interactionTarget = null;

            if (interactText != null)
                interactText.gameObject.SetActive(false);
        }
    }

    // 🚪 PUERTA (nuevo sistema separado)
    public void ShowDoorInteraction(Transform doorTransform)
    {
        nearbyItem = null; // 🔥 importante: cancela objeto
        interactionTarget = doorTransform;

        if (interactText != null)
            interactText.gameObject.SetActive(true);
    }

    public void HideDoorInteraction()
    {
        interactionTarget = null;

        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }
}