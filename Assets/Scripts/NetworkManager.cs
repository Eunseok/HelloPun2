using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")] public TMP_Text titleText;
    public TMP_InputField nicknameInputField;
    public GameObject loginPanel;
    public GameObject connectingPanel;
    public GameObject connectedPanel;

    private const string DefaultRoomName = "Lobby";

    private void Awake()
    {
        // Set screen resolution and configure Photon network settings
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    private IEnumerator Start()
    {
        // Start Photon connection
        connectingPanel.SetActive(true);
        yield return PhotonNetwork.ConnectUsingSettings();

        if (PhotonNetwork.IsConnected)
        {
            connectingPanel.SetActive(false);
            connectedPanel.SetActive(true);
            Debug.Log("Connected to Photon successfully.");
        }
        else
        {
            Application.Quit(); 
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");

        if (PhotonNetwork.CountOfRooms == 0)
        {
            PhotonNetwork.CreateRoom(DefaultRoomName);
            Debug.Log("Lobby room created.");
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("Joined a random room.");
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
        loginPanel.SetActive(false);

        // 플레이어 생성 (0, 0, 0)
        PhotonNetwork.Instantiate("PlayerPrefab", Vector2.zero, Quaternion.identity);
    }

    public void OnLoginButtonClick()
    {
        string nickname = nicknameInputField.text.Trim();

        if (nickname.Length > 1)
        {
            PhotonNetwork.NickName = nickname;
            PhotonNetwork.JoinLobby();

            connectingPanel.SetActive(true);
            connectedPanel.SetActive(false);
        }
        else
        {
            titleText.text = "닉네임은 2글자 이상 입력하세요!";
        }
    }

    private void Update()
    {
        if (loginPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            OnLoginButtonClick();
        }
    }
}