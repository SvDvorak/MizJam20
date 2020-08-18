using TMPro;
using UnityEngine;

public class TextContainerResizer : MonoBehaviour
{
    public bool Width;
    public bool Height;
    private FontStyles _usedWeight;
    private TMP_Text _textElement;

    public void Start()
    {
        _textElement = GetComponent<TMP_Text>();
        Resize();
    }

    public void Update()
    {
        if(_textElement.fontStyle != _usedWeight)
        {
            _usedWeight = _textElement.fontStyle;
            Resize();
        }
    }

    private void Resize()
    {
        var size = _textElement.GetPreferredValues(_textElement.text);
        var currentDelta = _textElement.rectTransform.sizeDelta;
        var newSizeDelta = new Vector2(Width ? size.x : currentDelta.x, Height ? size.y : currentDelta.y);
        _textElement.rectTransform.sizeDelta = newSizeDelta;
    }
}
