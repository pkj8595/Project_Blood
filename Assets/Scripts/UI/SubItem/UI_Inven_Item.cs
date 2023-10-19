using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    string _name;

    enum GameObjects
    {
        ItemText,
        ItemImage
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        var itemText = Get<GameObject>((int)GameObjects.ItemText).GetOrAddComponent<Text>();
        itemText.text = _name;
        itemText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        itemText.fontSize = 20;

        Get<GameObject>((int)GameObjects.ItemImage).BindEvent((PointerEventData) => { Debug.Log($"{_name} 클릭"); });
    }

    // 생성시 넣어주면 셋팅이 된 상태로 랜더링이 되기 때문에 문제없이 텍스트가 바뀐다.
    public void SetInfo(string name)
    {
        _name = name;

    }
}
