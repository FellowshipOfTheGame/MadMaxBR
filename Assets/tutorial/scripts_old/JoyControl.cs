using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyControl : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image bgImg;
    [SerializeField]
    private Image joyImg;

    public Vector3 imputDir;

    private void Start()
    {
        bgImg = GetComponent<Image>();
        joyImg = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform,ped.position,ped.pressEventCamera,out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            imputDir = new Vector3(pos.x *2, pos.y *2, 0);
            imputDir = (imputDir.magnitude > 1) ? imputDir.normalized : imputDir;

            joyImg.rectTransform.anchoredPosition = new Vector3(imputDir.x *(bgImg.rectTransform.sizeDelta.x / 3),imputDir.y * (bgImg.rectTransform.sizeDelta.y /3));
           
        }

   
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        imputDir = Vector3.zero;
        joyImg.rectTransform.anchoredPosition = Vector3.zero;
    }
    public float Hori()
    {
        if (imputDir.x != 0)
        {
            return imputDir.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public float Vert()
    {
        if (imputDir.y != 0)
        {
            return imputDir.y;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
}