using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;

public class Chat : NetworkComponent
{
    public string myMessage;
    public string recentMessage;
    public Text messageBox;
    public InputField inputBox;

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "MSG")
        {
            if (IsServer)
            {
                recentMessage = value;
                SendUpdate("MSG", value);
            }
            if (IsClient)
            {
                recentMessage = value;
                messageBox.text = value;
            }
        }
    }

    public override void NetworkedStart()
    {

    }

    public override IEnumerator SlowUpdate()
    {
        this.transform.position = new Vector3(-5 + Owner * 10, 0, 0);
        if (!IsLocalPlayer)
        {
            inputBox.interactable = false;
            inputBox.gameObject.SetActive(false);
        }
        while (IsServer)
        {
            if (IsDirty)
            {
                SendUpdate("MSG", recentMessage);
                IsDirty = false;

            }
            yield return new WaitForSeconds(.1f);

        }

    }
    public void TextEntry(string message)
    {
        SendCommand("MSG", message);
        IsDirty = true;

    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("Player");
        this.transform.SetParent(tmp.transform);

    }

    // Update is called once per frame
    void Update()
    {

    }
}