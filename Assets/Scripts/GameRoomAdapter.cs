using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameRoomAdapter : MonoBehaviour
{
    public static RectTransform prefab;
    public static Text countText;
    public static ScrollRect scrollView;
    public static RectTransform content;

    public static List<GameRoomItemView> views = new List<GameRoomItemView>();

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("GameRoomContent") is GameObject gc) {
            content = gc.GetComponent<RectTransform>();
        }
        if (GameObject.Find("GameRoomItem") is GameObject gri)
        {
            prefab = gri.GetComponent<RectTransform>();
        }
    }

    public static void OnReceivedNewModels(GameRoom[] models)
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

    public static GameRoomItemView InitializeItemView(GameObject viewGameObject, GameRoom room)
    {
        GameRoomItemView view = new GameRoomItemView(viewGameObject.transform, room);

        view.id.text = "Game Room: " + room.Id.ToString();

        return view;
    }

    public class GameRoomItemView
    {
        public TextMeshProUGUI id;
        public GameRoom room;

        public GameRoomItemView(Transform rootView, GameRoom gr)
        {
            id = rootView.Find("GameRoomItemId").GetComponent<TextMeshProUGUI>();
            room = gr;
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
