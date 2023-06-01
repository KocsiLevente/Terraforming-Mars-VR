using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatMessageAdapter : MonoBehaviour
{
    public static RectTransform prefab;
    public static Text countText;
    public static ScrollRect scrollView;
    public static RectTransform content;

    public static List<ChatMessageItemView> views = new List<ChatMessageItemView>();

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("ChatMessageContent") is GameObject gc)
        {
            content = gc.GetComponent<RectTransform>();
        }
        if (GameObject.Find("ChatMessageItem") is GameObject gri)
        {
            prefab = gri.GetComponent<RectTransform>();
        }
    }

    public static void OnReceivedNewModels(ChatMessage[] models)
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

    public static ChatMessageItemView InitializeItemView(GameObject viewGameObject, ChatMessage chat)
    {
        ChatMessageItemView view = new ChatMessageItemView(viewGameObject.transform, chat);

        view.id.text = chat.TimeSent + ": " + chat.Name + ": " + chat.Message;

        return view;
    }

    public class ChatMessageItemView
    {
        public TextMeshProUGUI id;
        public ChatMessage chat;

        public ChatMessageItemView(Transform rootView, ChatMessage cm)
        {
            id = rootView.Find("ChatMessageItemText").GetComponent<TextMeshProUGUI>();
            chat = cm;
        }
    }
}
