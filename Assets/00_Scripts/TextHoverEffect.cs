using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHoverEffect : MonoBehaviour
{

    public Image backgroundImage;

    private Text textComponent;
    private Color originalTextColor;
    private Color highlightTextColor = Color.black;

    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<Text>();
        originalTextColor = textComponent.color;
    }

    public void OnPointerEnter()
    {
        textComponent.color = highlightTextColor;
        backgroundImage.gameObject.SetActive(true);
    }

    public void OnPointerExit()
    {
        textComponent.color = originalTextColor;
        backgroundImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
