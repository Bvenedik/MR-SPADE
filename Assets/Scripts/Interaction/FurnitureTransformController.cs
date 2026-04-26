using UnityEngine;
using UnityEngine.UI;

public class FurnitureTransformController : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button buttonRotateLeft;
    [SerializeField] private Button buttonRotateRight;
    [SerializeField] private Button buttonScaleUp;
    [SerializeField] private Button buttonScaleDown;

    [Header("Managers")]
    [SerializeField] private FurnitureSelectionManager selectionManager;

    [Header("Transform Settings")]
    [SerializeField] private float rotationStepDegrees = 15f;
    [SerializeField] private float scaleMultiplierUp = 1.1f;
    [SerializeField] private float scaleMultiplierDown = 0.9f;
    [SerializeField] private float minUniformScale = 0.5f;
    [SerializeField] private float maxUniformScale = 2.0f;

    private void Start()
    {
        if (buttonRotateLeft != null)
            buttonRotateLeft.onClick.AddListener(RotateLeft);

        if (buttonRotateRight != null)
            buttonRotateRight.onClick.AddListener(RotateRight);

        if (buttonScaleUp != null)
            buttonScaleUp.onClick.AddListener(ScaleUp);

        if (buttonScaleDown != null)
            buttonScaleDown.onClick.AddListener(ScaleDown);
    }

    public void RotateLeft()
    {
        GameObject target = GetSelectedObject();
        if (target == null) return;

        target.transform.Rotate(0f, -rotationStepDegrees, 0f, Space.World);
    }

    public void RotateRight()
    {
        GameObject target = GetSelectedObject();
        if (target == null) return;

        target.transform.Rotate(0f, rotationStepDegrees, 0f, Space.World);
    }

    public void ScaleUp()
    {
        GameObject target = GetSelectedObject();
        if (target == null) return;

        ApplyScale(target, scaleMultiplierUp);
    }

    public void ScaleDown()
    {
        GameObject target = GetSelectedObject();
        if (target == null) return;

        ApplyScale(target, scaleMultiplierDown);
    }

    private void ApplyScale(GameObject target, float multiplier)
    {
        Vector3 currentScale = target.transform.localScale;
        Vector3 newScale = currentScale * multiplier;

        float uniformMagnitude = newScale.x;

        if (uniformMagnitude < minUniformScale || uniformMagnitude > maxUniformScale)
        {
            return;
        }

        target.transform.localScale = newScale;
    }

    private GameObject GetSelectedObject()
    {
        if (selectionManager == null)
        {
            Debug.LogWarning("FurnitureTransformController: SelectionManager not assigned.");
            return null;
        }

        return selectionManager.CurrentSelectedObject;
    }
}