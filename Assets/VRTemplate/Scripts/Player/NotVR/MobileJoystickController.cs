using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Managed the position of the joystick and updated his interface
/// </summary>
public class MobileJoystickController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Tooltip("Rear image of the joystick that does not move")]
    [SerializeField] RectTransform background;
    [Tooltip("Image that moves when you use the joystick")]
    [SerializeField] RectTransform stick;

    /// <summary>
    /// Value of the position Vector2(Horizontal,Vertical) position of joystick
    /// </summary>
    public Vector2 pointPosition;

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");

        pointPosition = new Vector2(
            (eventData.position.x - background.position.x ) / ( background.rect.size.x / 2 - stick.rect.size.x / 2),
            (eventData.position.y - background.position.y) / (background.rect.size.y / 2 - stick.rect.size.y / 2)
            );
        //if (pointPosition.magnitude > 1.0f) pointPosition.Normalize();
        pointPosition = pointPosition.magnitude > 1.0f ? pointPosition.normalized : pointPosition;
        Debug.Log("x; " + pointPosition.x + " y: " + pointPosition.y   );

        stick.transform.position = new Vector2(
            pointPosition.x * (background.rect.size.x / 2 - stick.rect.size.x / 2) + background.position.x,
            pointPosition.y * (background.rect.size.y / 2 - stick.rect.size.y / 2) + background.position.y
            );
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        pointPosition = new Vector2(0.0f, 0.0f);
        stick.transform.position = background.position;
    }
}

