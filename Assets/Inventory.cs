using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class Inventory : MonoBehaviour
{
    [Header("UI Slots")]
    public Image[] slotsUI = new Image[6];

    [Header("Linterna en mano")]
    public SpriteRenderer handItemRenderer;
    public Sprite flashlightHandSprite;
    public Light2D flashlightLight;
    public Transform flashlightLightTransform;

    [Header("Posición linterna")]
    public Vector3 handRightPosition = new Vector3(0.25f, 0.048f, 0f);
    public Vector3 handLeftPosition = new Vector3(-0.25f, 0.048f, 0f);

    private Sprite[] itemIcons = new Sprite[6];
    private PickableItem.ItemType[] itemTypes = new PickableItem.ItemType[6];

    private bool hasFlashlight = false;
    private bool flashlightEquipped = false;
    private bool flashlightOn = false;
    private bool facingRight = true;

    void Start()
    {
        if (handItemRenderer != null)
        {
            handItemRenderer.sprite = null;
            handItemRenderer.enabled = false;
        }

        if (flashlightLight != null)
            flashlightLight.enabled = false;
    }

    void Update()
    {
        // Equipar / guardar linterna
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleFlashlight();
        }

        // Encender / apagar
        if (Input.GetMouseButtonDown(1))
        {
            ToggleFlashlightLight();
        }
    }

    // ===============================
    // AGREGAR ITEMS
    // ===============================
    public bool AddItem(PickableItem item)
    {
        // 🔦 Linterna siempre en slot 0
        if (item.itemType == PickableItem.ItemType.Flashlight)
        {
            hasFlashlight = true;
            itemIcons[0] = item.itemIcon;
            itemTypes[0] = item.itemType;

            ShowItemInSlot(0, item.itemIcon);
            EquipFlashlight();

            return true;
        }

        // 🧱 Otros objetos (ej: llave)
        for (int i = 1; i < itemIcons.Length; i++)
        {
            if (itemIcons[i] == null)
            {
                itemIcons[i] = item.itemIcon;
                itemTypes[i] = item.itemType;

                ShowItemInSlot(i, item.itemIcon);
                return true;
            }
        }

        Debug.Log("Inventario lleno");
        return false;
    }

    private void ShowItemInSlot(int index, Sprite icon)
    {
        if (slotsUI[index] == null) return;

        slotsUI[index].sprite = icon;
        slotsUI[index].enabled = true;
    }

    // ===============================
    // EQUIPAR / GUARDAR LINTERNA
    // ===============================
    private void ToggleFlashlight()
    {
        if (!hasFlashlight)
        {
            Debug.Log("No tienes linterna.");
            return;
        }

        if (flashlightEquipped)
            UnequipFlashlight();
        else
            EquipFlashlight();
    }

    private void EquipFlashlight()
    {
        flashlightEquipped = true;

        // Siempre inicia apagada al sacarla
        flashlightOn = false;

        if (handItemRenderer != null)
        {
            handItemRenderer.sprite = flashlightHandSprite;
            handItemRenderer.enabled = true;
            UpdateHandDirection(facingRight);
        }

        if (flashlightLight != null)
            flashlightLight.enabled = false;
    }

    private void UnequipFlashlight()
    {
        flashlightEquipped = false;
        flashlightOn = false;

        if (handItemRenderer != null)
        {
            handItemRenderer.sprite = null;
            handItemRenderer.enabled = false;
        }

        if (flashlightLight != null)
            flashlightLight.enabled = false;
    }

    // ===============================
    // ENCENDER / APAGAR LUZ
    // ===============================
    private void ToggleFlashlightLight()
    {
        if (!flashlightEquipped) return;

        flashlightOn = !flashlightOn;

        if (flashlightLight != null)
            flashlightLight.enabled = flashlightOn;
    }

    // ===============================
    // FLIP + POSICIÓN
    // ===============================
    public void UpdateHandDirection(bool isFacingRight)
    {
        facingRight = isFacingRight;

        if (handItemRenderer == null) return;

        if (facingRight)
        {
            handItemRenderer.transform.localPosition = handRightPosition;
            handItemRenderer.flipX = false;
            handItemRenderer.transform.localRotation = Quaternion.identity;

            if (flashlightLightTransform != null)
                flashlightLightTransform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            handItemRenderer.transform.localPosition = handLeftPosition;
            handItemRenderer.flipX = true;
            handItemRenderer.transform.localRotation = Quaternion.identity;

            if (flashlightLightTransform != null)
                flashlightLightTransform.localRotation = Quaternion.Euler(0, 0, 90);
        }
    }

    // ===============================
    // VERIFICAR ITEMS (para la puerta)
    // ===============================
    public bool HasItem(PickableItem.ItemType type)
    {
        for (int i = 0; i < itemTypes.Length; i++)
        {
            if (itemTypes[i] == type)
                return true;
        }

        return false;
    }
}