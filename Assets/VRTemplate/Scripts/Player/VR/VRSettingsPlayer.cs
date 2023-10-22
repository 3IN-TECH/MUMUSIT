using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Managed the setting of the VR
/// Managed the height adjuster menu
/// </summary>
public class VRSettingsPlayer : MonoBehaviour
{
    [Tooltip("Transform where is the XROrigin")]
    [SerializeField] Transform playerTransform;

    [Tooltip("Transform where is the TrackedPoseDriver of the head")]
    [SerializeField] Transform headTransform;

    [Tooltip("Transforms where are the TrackedPoseDriver of the hands")]
    [SerializeField] Transform[] handsTransform;

    [Tooltip("GameObject where is the Canvas of the settings")]
    [SerializeField] GameObject settingsCanvas;

    [Tooltip("GameObject where is the Panel of the heightSettings")]
    [SerializeField] GameObject heightHelperPanel;

    [Tooltip("Action that show or hide de settings menu")]
    [SerializeField] InputActionReference SettingsMenuAction;

    bool isCalculatingHeight;
    float minHeight;

    /// <summary>
    /// Teorical distance of the width between the real floor and the center of a controller on the floor
    /// </summary>
    readonly float offsetHeightController = 0.02f;


    void Update()
    {
        if (SettingsMenuAction.action.WasReleasedThisFrame())
        {
            if (settingsCanvas.activeInHierarchy) CloseSettings();
            else OpenSettings();
        }
    }

    void OpenSettings()
    {
        settingsCanvas.transform.position = headTransform.position;
        settingsCanvas.transform.eulerAngles = new Vector3(settingsCanvas.transform.eulerAngles.x, headTransform.eulerAngles.y, settingsCanvas.transform.eulerAngles.z);
        settingsCanvas.SetActive(true);

        calculateHeightStart();
    }

    void CloseSettings()
    {
        if (heightHelperPanel.activeInHierarchy) calculateHeightEnd();

        settingsCanvas.SetActive(false);
    }

    /// <summary>
    /// Open the height adjuster menu.
    /// And start the process necesary to calculate the new height
    /// </summary>
    public void calculateHeightStart()
    {
        Debug.Log("calculateHeight Start");
        heightHelperPanel.SetActive(true);
        minHeight = float.MaxValue;
        isCalculatingHeight = true;
        calculatingMinHeight();
    }

    /// <summary>
    /// When the adjusted height system is ending
    /// Adjust the height and close the height adjuster menu
    /// </summary>
    public void calculateHeightEnd()
    {
        Debug.Log("calculateHeight End");
        heightHelperPanel.SetActive(false);
        isCalculatingHeight = false;
        playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y - minHeight + offsetHeightController, playerTransform.position.z);
        CapsuleCollider collider = this.GetComponent<CapsuleCollider>();
        if (collider)
        {
            collider.height = playerTransform.position.y;
            collider.center = new Vector3(0, playerTransform.position.y / 2f, 0);
        }
    }

    /// <summary>
    /// Process necesary to calculate the new height.
    /// Takes the less height of the hands to know the position of the real floor.
    /// </summary>
    private void calculatingMinHeight()
    {
        minHeight = Mathf.Min(minHeight, handsTransform[0].position.y, handsTransform[1].position.y, headTransform.position.y - 1);

        if (isCalculatingHeight)
        {
            Invoke(nameof(calculatingMinHeight), 0.1f);
        }
    }
}
