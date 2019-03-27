using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform circleOutline;
    public RectTransform currPosition;
    
    private Image outlineImage;

    /// <summary>
    /// This represents the joystick's position inside the circular bounds. The vector's magnitude ranges from [0-1],
    /// with a magnitude of 0 being when the joystick is centered and 1 when it's on the outer ring.
    /// </summary>
    public Vector2 NormalizedVelocity { private set; get; }
    
    void Start()
    {
        outlineImage = circleOutline.GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerEventData.InputButton button = eventData.button;
        currPosition.gameObject.SetActive(true);
        
        UpdateVelocity(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        currPosition.gameObject.SetActive(false);
        NormalizedVelocity = Vector2.zero;
    }

    private void UpdateVelocity(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(outlineImage.rectTransform, eventData.position,
            eventData.pressEventCamera, out localPoint))
        {
            Rect outlineRect = outlineImage.rectTransform.rect;
            
            localPoint.x = (localPoint.x / outlineRect.width) * 2; 
            localPoint.y = (localPoint.y / outlineRect.height) * 2;

            if (localPoint.magnitude > 1)
            {
                localPoint.Normalize();
            }

            NormalizedVelocity = localPoint;
            currPosition.anchoredPosition = localPoint * outlineRect.size * 0.5f;
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        UpdateVelocity(eventData);
    }
}
