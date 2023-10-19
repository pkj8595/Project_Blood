using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : UnityEngine.Component
    {
        T component = obj.GetComponent<T>();
        if (component == null)
            component = obj.AddComponent<T>();

        return component;
    }

    public static void BindEvent(this GameObject obj, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        if(obj.GetComponent<Transform>() is not RectTransform)
        {
            Debug.LogError("해당 오브젝트가 RectTransform를 포함하고 있지 않습니다.");
            return;
        }

        UI_EventHandler evt = obj.GetOrAddComponent<UI_EventHandler>();


        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
    
}
