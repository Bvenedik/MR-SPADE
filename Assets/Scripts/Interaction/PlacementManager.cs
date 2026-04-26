using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private Transform furnitureRoot;
    [SerializeField] private Transform placementPreviewRoot;
    [SerializeField] private Transform placementAnchorRuntime;
    [SerializeField] private Transform spawnPointDefault;

    [Header("Managers")]
    [SerializeField] private FurnitureSelectionManager selectionManager;
    [SerializeField] private HelperHintManager helperHintManager;

    [Header("Placement Settings")]
    [SerializeField] private bool useSpawnPointDefault = true;
    [SerializeField] private Vector3 previewOffset = Vector3.zero;

    private FurnitureItemData currentPendingItemData;
    private GameObject currentPreviewObject;
    private bool isInPlacementMode = false;

    public bool IsInPlacementMode => isInPlacementMode;

    private void Start()
    {
        if (helperHintManager != null)
        {
            helperHintManager.SetIdleHint();
        }
    }

    public void StartPlacement(FurnitureItemData itemData)
    {
        if (itemData == null || itemData.prefab == null)
        {
            Debug.LogWarning("StartPlacement failed: itemData or prefab is null.");
            return;
        }

        currentPendingItemData = itemData;
        isInPlacementMode = true;

        DestroyCurrentPreviewIfAny();
        CreatePreviewObject();

        if (helperHintManager != null)
        {
            helperHintManager.SetPlacementHint();
        }

        if (selectionManager != null)
        {
            selectionManager.ClearSelection();
        }
    }

    public void ConfirmPlacement()
    {
        if (!isInPlacementMode || currentPendingItemData == null || currentPendingItemData.prefab == null)
        {
            Debug.LogWarning("ConfirmPlacement failed: no active placement item.");
            return;
        }

        Vector3 spawnPosition = GetCurrentPlacementPosition();
        Quaternion spawnRotation = Quaternion.identity;

        GameObject placedObject = Instantiate(
            currentPendingItemData.prefab,
            spawnPosition,
            spawnRotation,
            furnitureRoot
        );

        FurnitureInstance instance = placedObject.GetComponent<FurnitureInstance>();
        if (instance == null)
        {
            instance = placedObject.AddComponent<FurnitureInstance>();
        }

        instance.Initialize(currentPendingItemData);
        placedObject.name = currentPendingItemData.displayName;

        DestroyCurrentPreviewIfAny();
        isInPlacementMode = false;

        if (selectionManager != null)
        {
            selectionManager.SelectObject(placedObject);
        }

        if (helperHintManager != null)
        {
            helperHintManager.SetEditHint();
        }
    }

    public void CancelPlacement()
    {
        DestroyCurrentPreviewIfAny();
        currentPendingItemData = null;
        isInPlacementMode = false;

        if (helperHintManager != null)
        {
            helperHintManager.SetIdleHint();
        }
    }

    private void CreatePreviewObject()
    {
        Vector3 previewPosition = GetCurrentPlacementPosition();

        currentPreviewObject = Instantiate(
            currentPendingItemData.prefab,
            previewPosition,
            Quaternion.identity,
            placementPreviewRoot
        );

        currentPreviewObject.name = $"Preview_{currentPendingItemData.displayName}";

        SetPreviewAppearance(currentPreviewObject);
    }

    private Vector3 GetCurrentPlacementPosition()
    {
        if (useSpawnPointDefault && spawnPointDefault != null)
        {
            return spawnPointDefault.position + previewOffset;
        }

        if (placementAnchorRuntime != null)
        {
            return placementAnchorRuntime.position + previewOffset;
        }

        return Vector3.zero;
    }

    private void DestroyCurrentPreviewIfAny()
    {
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject);
            currentPreviewObject = null;
        }
    }

    private void SetPreviewAppearance(GameObject previewObject)
    {
        if (previewObject == null) return;

        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer rendererComponent in renderers)
        {
            Material[] materials = rendererComponent.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].HasProperty("_Color"))
                {
                    Color c = materials[i].color;
                    c.a = 0.5f;
                    materials[i].color = c;
                }
            }
        }
    }
}