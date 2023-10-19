using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    enum Buttons 
    {
        PointButton,
    }

    enum Texts 
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }



    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        Get<Text>((int)Texts.ScoreText).text = "Bind Test";

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        go.BindEvent((PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);
    }


}
