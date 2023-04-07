using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TextHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Image backgroundImage;
    public UnityEngine.Events.UnityEvent onClick;
    

    private TextMeshProUGUI textComponent;
    private Color originalTextColor;
    private Color highlightTextColor = Color.black;
    private bool isHovering;
    private bool isClicked;

    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        originalTextColor = textComponent.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked)
        {
        textComponent.color = highlightTextColor;
        backgroundImage.gameObject.SetActive(true);
        }

        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked)
        {
        textComponent.color = originalTextColor;
        backgroundImage.gameObject.SetActive(false);
        }

        isHovering = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isHovering && !isClicked)
        {
            
            onClick.Invoke();
            isClicked = true;
        }
        else
        {
            textComponent.color = originalTextColor;
            backgroundImage.gameObject.SetActive(false);

            isClicked = false;
        }

        isHovering = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
