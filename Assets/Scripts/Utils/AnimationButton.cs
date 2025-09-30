using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class AnimationButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public bool isCatchPointerDownEvt = true;
    public bool isCatchPointerUpEvt = true;
    public bool isCatchPointerExitEvt = true;
    
    public bool isManualSettingOriginScale = false;
    
    public Vector3 manualOriginScale = new Vector3(1f, 1f, 1f);
    
    Vector3 originScale;
    
    private void Awake()
    {
        originScale = isManualSettingOriginScale ? manualOriginScale : transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = originScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isCatchPointerDownEvt) return;
        
        //scale btn
        Vector3 tempBtn = gameObject.transform.localScale;
        if (tempBtn.x > 0) tempBtn = new Vector3(0.85f, 0.85f, 0.85f);
        transform.DOScale(tempBtn, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isCatchPointerExitEvt) return;
        
        Vector3 tempBtn = gameObject.transform.localScale;
        if (tempBtn.x > 0) tempBtn = originScale;
        transform.DOScale(tempBtn, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isCatchPointerUpEvt) return;
        
        Vector3 tempBtn = gameObject.transform.localScale;
        if (tempBtn.x > 0) tempBtn = originScale;
        transform.DOScale(tempBtn, 0.1f);
    }
}
