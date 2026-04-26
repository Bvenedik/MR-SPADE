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
    [SerializeField] private float rotationStep = 15f;

    [Header("UI")]
    [SerializeField] private GameObject actionPanel;

    private float currentPreviewYRotation = 0f;

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
        if (actionPanel != null)
        {
            actionPanel.SetActive(false);
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
        currentPreviewYRotation = 0f;
        CreatePreviewObject();

        if (actionPanel != null)
        {
            actionPanel.SetActive(true);
        }

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
        Quaternion spawnRotation = Quaternion.Euler(0f, currentPreviewYRotation, 0f);

        GameObject placedObject = Instantiate(
            currentPendingItemData.prefab,
            spawnPosition,
            spawnRotation,
            furnitureRoot
        );

        placedObject.transform.localScale = Vector3.one * 0.7f;

        FurnitureInstance instance = placedObject.GetComponent<FurnitureInstance>();
        if (instance == null)
        {
            instance = placedObject.AddComponent<FurnitureInstance>();
        }

        instance.Initialize(currentPendingItemData);
        placedObject.name = currentPendingItemData.displayName;

        DestroyCurrentPreviewIfAny();
        isInPlacementMode = false;

        currentPendingItemData = null;
        currentPreviewYRotation = 0f;

        if (selectionManager != null)
        {
            selectionManager.SelectObject(placedObject);
        }

        if (helperHintManager != null)
        {
            helperHintManager.SetEditHint();
        }
        if (actionPanel != null)
        {
            actionPanel.SetActive(false);
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
        if (actionPanel != null)
        {
            actionPanel.SetActive(false);
        }
    }

    private void CreatePreviewObject()
    {
        Vector3 previewPosition = GetCurrentPlacementPosition();

        currentPreviewObject = Instantiate(
            currentPendingItemData.prefab,
            previewPosition,
            Quaternion.Euler(0f, currentPreviewYRotation, 0f),
            placementPreviewRoot
        );

        if (currentPreviewObject == null)
        {
            Debug.LogError("Preview object was not created.");
            return;
        }

        currentPreviewObject.transform.localScale = Vector3.one * 0.7f;

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

    public void RotatePreviewLeft()
    {
        if (!isInPlacementMode || currentPreviewObject == null)
        {
            Debug.Log("Rotate ignored: no active preview.");
            return;
        }

        RotatePreview(-rotationStep);
    }

    public void RotatePreviewRight()
    {
        if (!isInPlacementMode || currentPreviewObject == null)
        {
            Debug.Log("Rotate ignored: no active preview.");
            return;
        }

        RotatePreview(rotationStep);
    }

    private void RotatePreview(float amount)
    {
        if (!isInPlacementMode || currentPreviewObject == null) return;

        currentPreviewYRotation += amount;
        currentPreviewObject.transform.rotation = Quaternion.Euler(0f, currentPreviewYRotation, 0f);
    }


}