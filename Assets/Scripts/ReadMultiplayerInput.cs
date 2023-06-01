using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadMultiplayerInput : MonoBehaviour
{
    public static string inputUserName = "";
    public static string inputChatMessage = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadStringInputUserName(string s)
    {
        inputUserName = s;
        Debug.Log(inputUserName);
    }

    public void ReadStringInputChatMessage(string s)
    {
        inputChatMessage = s;
        Debug.Log(inputChatMessage);
    }
}
