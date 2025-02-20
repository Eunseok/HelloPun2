using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    static readonly int Run = Animator.StringToHash("Run");
    public new PhotonView photonView;
    public TMP_Text playerNickname;

    SpriteRenderer sr;
    Rigidbody2D rg;
    Animator anim;

    [Header("Movement")] public float movementSpeed = 2.0f;

    [Header("Chat System")] public GameObject speechBubble;
    public TMP_Text chatText;
    TMP_InputField chatInputField;

    private void Awake()
    {
        // Initialize Player Nickname
        if (photonView.IsMine)
        {
            playerNickname.text = PhotonNetwork.NickName;
            playerNickname.color = Color.white;
            if (Camera.main != null) Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
        }
        else
        {
            playerNickname.text = photonView.Owner.NickName;
            playerNickname.color = Color.red;
        }

        chatInputField = GameObject.Find("Canvas").transform.Find("ChatInput").GetComponent<TMP_InputField>();


        if (chatInputField == null)
        {
            Debug.LogError("Chat Input Field not found!");
        }
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            sr.sortingOrder = 1;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            HandleMovement();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                HandleChatInput();
            }
        }
    }

    private void HandleMovement()
    {
        // Don't allow movement while the chat input field is active
        if (chatInputField.gameObject.activeSelf) return;
        if (GameObject.FindGameObjectWithTag("MiniGame")) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rg.velocity = new Vector2(horizontalInput * movementSpeed, rg.velocity.y);

        if (horizontalInput != 0)
        {
            anim.SetBool(Run, true);
            photonView.RPC("SetSpriteFlipX", RpcTarget.AllBuffered, horizontalInput);
        }
        else
        {
            anim.SetBool(Run, false);
        }
    }

    private void HandleChatInput()
    {
        if (chatInputField.gameObject.activeSelf)
        {
            chatInputField.gameObject.SetActive(false);

            if (chatInputField.text.Length > 0)
            {
                photonView.RPC("ShowChatMessage", RpcTarget.AllBuffered);
                chatInputField.text = string.Empty;

                StopAllCoroutines();
                StartCoroutine(CloseChatBoxAfterDelay(3.0f));
            }
        }
        else
        {
            chatInputField.gameObject.SetActive(true);
            chatInputField.ActivateInputField();
        }
    }

    [PunRPC]
    private void ShowChatMessage()
    {
        speechBubble.SetActive(true);
        chatText.text = chatInputField.text;
    }

    [PunRPC]
    private void CloseChatBox()
    {
        speechBubble.SetActive(false);
    }

    [PunRPC]
    private void SetSpriteFlipX(float direction)
    {
        if (sr)
            sr.flipX = Mathf.Approximately(direction, -1);
    }

    private IEnumerator CloseChatBoxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        photonView.RPC("CloseChatBox", RpcTarget.AllBuffered);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(chatText.text);
        }
        else
        {
            chatText.text = (string)stream.ReceiveNext();
        }
    }
}