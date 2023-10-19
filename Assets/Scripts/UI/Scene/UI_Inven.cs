using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven : UI_Scene
{
    enum GameObjects 
    {
        GridPanel
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in gridPanel.transform)
        {
            Managers.Resource.Destory(child.gameObject);
        }

        for (int i = 0; i < 8; i++)
        {
            //GameObject items = Managers.Resource.Instantiate("UI/Scene/UI_Inven_Item", gridPanel.transform);
            //var item = items.GetOrAddComponent<UI_Inven_Item>();
            //item.SetInfo($"name test {i}");

            UI_Inven_Item item = Managers.UI.MakeSubItem<UI_Inven_Item>(gridPanel.transform);
            item.SetInfo($"name {i}");
        }

    }
}
