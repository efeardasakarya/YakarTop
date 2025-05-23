using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleFactor = 1.1f; // B�y�me oran�
    public float scaleSpeed = 10f; // Animasyon h�z�

    private bool isHovered = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Hedef boyutu belirle
        Vector3 targetScale = isHovered ? originalScale * scaleFactor : originalScale;

        // Lerp ile yumu�ak ge�i�
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * scaleSpeed);


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        Debug.Log(gameObject.name + " �zerine gelindi");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
