using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;

    public ItemType itemType;

    public enum ItemType
    {
        Flashlight,
        Key,
        Other
    }
}