using UnityEngine;

public class FurnitureInstance : MonoBehaviour
{
    [Header("Metadata")]
    public string itemId;
    public string displayName;
    public FurnitureCategory category;

    public void Initialize(FurnitureItemData data)
    {
        if (data == null) return;

        itemId = data.itemId;
        displayName = data.displayName;
        category = data.category;
    }
}