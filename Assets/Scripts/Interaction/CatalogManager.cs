using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatalogManager : MonoBehaviour
{
    [System.Serializable]
    public class FurnitureButtonBinding
    {
        public Button button;
        public TMP_Text label;
        public FurnitureItemData itemData;
    }

    [Header("UI")]
    [SerializeField] private Button buttonCategoryChairs;
    [SerializeField] private Button buttonCategoryTables;
    [SerializeField] private TMP_InputField inputSearch;

    [Header("Furniture Buttons")]
    [SerializeField] private List<FurnitureButtonBinding> furnitureButtons = new();

    [Header("Managers")]
    [SerializeField] private PlacementManager placementManager;

    private FurnitureCategory currentCategory = FurnitureCategory.Chairs;
    private string currentSearch = "";

    private void Start()
    {
        HookCategoryButtons();
        HookSearchField();
        HookFurnitureButtons();

        RefreshCatalog();
    }

    private void HookCategoryButtons()
    {
        if (buttonCategoryChairs != null)
        {
            buttonCategoryChairs.onClick.AddListener(() =>
            {
                currentCategory = FurnitureCategory.Chairs;
                RefreshCatalog();
            });
        }

        if (buttonCategoryTables != null)
        {
            buttonCategoryTables.onClick.AddListener(() =>
            {
                currentCategory = FurnitureCategory.Tables;
                RefreshCatalog();
            });
        }
    }

    private void HookSearchField()
    {
        if (inputSearch != null)
        {
            inputSearch.onValueChanged.AddListener(OnSearchChanged);
        }
    }

    private void HookFurnitureButtons()
    {
        foreach (FurnitureButtonBinding entry in furnitureButtons)
        {
            if (entry == null || entry.button == null || entry.itemData == null) continue;

            FurnitureItemData capturedItem = entry.itemData;
            entry.button.onClick.AddListener(() => OnFurnitureItemClicked(capturedItem));

            if (entry.label != null)
            {
                entry.label.text = capturedItem.displayName;
            }
        }
    }

    private void OnSearchChanged(string searchText)
    {
        currentSearch = searchText == null ? "" : searchText.Trim().ToLower();
        RefreshCatalog();
    }

    private void OnFurnitureItemClicked(FurnitureItemData itemData)
    {
        Debug.Log("Clicked furniture item: " + itemData.displayName);
        if (placementManager == null)
        {
            Debug.LogWarning("CatalogManager: PlacementManager is not assigned.");
            return;
        }

        placementManager.StartPlacement(itemData);
    }

    private void RefreshCatalog()
    {
        foreach (FurnitureButtonBinding entry in furnitureButtons)
        {
            if (entry == null || entry.button == null || entry.itemData == null) continue;

            bool matchesCategory = entry.itemData.category == currentCategory;
            bool matchesSearch = string.IsNullOrEmpty(currentSearch) ||
                                 entry.itemData.displayName.ToLower().Contains(currentSearch);

            bool shouldShow = matchesCategory && matchesSearch;
            entry.button.gameObject.SetActive(shouldShow);
        }
    }
}