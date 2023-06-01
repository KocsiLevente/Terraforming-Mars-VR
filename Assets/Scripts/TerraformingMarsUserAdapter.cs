using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerraformingMarsUserAdapter : MonoBehaviour
{
    public static RectTransform prefab;
    public static Text countText;
    public static ScrollRect scrollView;
    public static RectTransform content;

    public static List<TerraformingMarsUserItemView> views = new List<TerraformingMarsUserItemView>();

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("TerraformingMarsUserContent") is GameObject gc)
        {
            content = gc.GetComponent<RectTransform>();
        }
        if (GameObject.Find("TerraformingMarsUserItem") is GameObject gri)
        {
            prefab = gri.GetComponent<RectTransform>();
        }
    }

    public static void OnReceivedNewModels(TerraformingMarsUser[] models)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        views.Clear();

        int i = 0;
        foreach (var model in models)
        {
            var instance = Instantiate(prefab.gameObject);
            instance.transform.SetParent(content, false);
            var view = InitializeItemView(instance, model);
            views.Add(view);
            ++i;
        }
    }

    public static TerraformingMarsUserItemView InitializeItemView(GameObject viewGameObject, TerraformingMarsUser user)
    {
        TerraformingMarsUserItemView view = new TerraformingMarsUserItemView(viewGameObject.transform, user);

        view.id.text = user.Name + ": " + user.OuterId;

        return view;
    }

    public class TerraformingMarsUserItemView
    {
        public TextMeshProUGUI id;
        public TerraformingMarsUser user;

        public TerraformingMarsUserItemView(Transform rootView, TerraformingMarsUser ur)
        {
            id = rootView.Find("TerraformingMarsUserItemText").GetComponent<TextMeshProUGUI>();
            user = ur;
        }

        public void OnSelected(bool isDeselect)
        {
            if (id != null)
            {
                id.color = isDeselect ? Color.white : Color.cyan;
            }
        }
    }
}
