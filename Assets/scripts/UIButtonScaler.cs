using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleFactor = 1.1f; // Büyüme oraný
    public float scaleSpeed = 10f; // Animasyon hýzý

    private bool isHovered = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Hedef boyutu belirle
        Vector3 targetScale = isHovered ? originalScale * scaleFactor : originalScale;

        // Lerp ile yumuþak geçiþ
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * scaleSpeed);


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        Debug.Log(gameObject.name + " üzerine gelindi");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
