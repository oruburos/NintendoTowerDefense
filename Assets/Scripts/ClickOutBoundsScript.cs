using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOutBoundsScript : MonoBehaviour, IPointerDownHandler {
    public void OnPointerDown(PointerEventData eventData)
    {
        if(PipeLocationScript.OpenCanvas != null)
        {
            PipeLocationScript.OpenCanvas.SetActive(false);
        }
    }
}
