using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySharingManager : MonoBehaviour
{

    public GameObject enterSharingRoomButton, sharingRoomNameText, sharingRoomInputField, sendPartyButton;

    NetworkClient networkClient;

    // Start is called before the first frame update
    void Start()
    {
        enterSharingRoomButton.GetComponent<Button>().onClick.AddListener(JoinSharingRoomButtonPressed);
        sendPartyButton.GetComponent<Button>().onClick.AddListener(SendPartyButtonPressed);
        networkClient = GetComponent<NetworkClient>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void JoinSharingRoomButtonPressed()
    {
        Text roomNameText = sharingRoomNameText.GetComponent<Text>();
        //only enter a room if we arent already in one. note* add ability to leave room later
        if(roomNameText.text != "")
        {
            string roomName = sharingRoomInputField.GetComponent<InputField>().text;
            roomNameText.text = roomName;
            networkClient.SendMessageToHost(ClientToServerSignifiers.JoinSharingRoom + "," + roomName);
        }
    }

    private void SendPartyButtonPressed()
    {
        AssignmentPart2.SendCurrentPartyToSharingServer(networkClient);
    }
}
