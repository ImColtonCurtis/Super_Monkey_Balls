using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : NetworkBehaviour
{
    //[SerializeField] private Transform spawnedObjectPrefab;
    //private Transform spawnedObjectTransform;
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    [SerializeField] private Transform camRotGrav;
    [SerializeField] private CinemachineFreeLook cinemachineFreeLookCam;
    private float gravityMod = 0f;
    private Vector2 prevGravity = Vector2.zero;
    private int prevXInput, prevYInput;
    const float downGravity = -29f, topSpeed = 14.5f;
    public static string publicRoomCode;
    [SerializeField] Transform playerSphereCol;
    private Vector3 startingLocation = new Vector3(0,0.5f,0);
    [SerializeField] Rigidbody myRB;

    [SerializeField] GameObject RippleCamera;

    [SerializeField] ParticleSystem ripple;
    [SerializeField] private float VelocityXZ, VelocityY;
    private Vector3 PlayerPos;
    [SerializeField] Transform ballBody;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int = 56,
            _bool = true,
            message = "Hi friend.",
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("Player Joined");
        if (!IsOwner)
        {
            cinemachineFreeLookCam.Priority -= 1; // so only owners cam is visible
            cinemachineFreeLookCam.enabled = false;
            return;
        }
        LoadLevel("OnlineLobby"); // load online lobby
        if (IsServer)
            StartCoroutine(HoldOnSecond());

        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + " " + newValue._int + " " + newValue._bool + " " + newValue.message);
        };
    }

    IEnumerator HoldOnSecond()
    {
        Debug.Log("PlayerCount: " + NetworkManager.ConnectedClients.Count);
        yield return new WaitForSeconds(5);
        if (SceneManager.GetActiveScene().name == "OnlineLobby")
            StartCoroutine(HoldOnSecond());
    }

    void LoadLevel(string sceneToJoin)
    {
        // load online lobby
        if (IsServer && !string.IsNullOrEmpty(sceneToJoin))
        {
            var status = NetworkManager.SceneManager.LoadScene(sceneToJoin, LoadSceneMode.Single);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load lobby " + $"with a {nameof(SceneEventProgressStatus)}: {status}");
                return;
            }
            RespawnObject(startingLocation);
        }
    }

    void RespawnObject(Vector3 spawnLocation)
    {
        myRB.velocity = Vector3.zero;
        myRB.angularVelocity = Vector3.zero;
        playerSphereCol.transform.position = spawnLocation;
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        // Get Player Input
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        // Get Camera Normalized Directional Vectors
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        // Create direction-relative input vectors
        Vector3 forwardRelativeVerticalInput = inputVector.y*forward;
        Vector3 rightRelativeForizontalInput = inputVector.x*right;

        // Create and apply camera relative movement
        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + 
            rightRelativeForizontalInput;

        // Gravity Modifier
        float acceleration = topSpeed / 5;
        float decceleration = topSpeed / 5;
        if (Vector3.Magnitude(inputVector) == 0)
        {
            if (gravityMod > 0)
                gravityMod -= decceleration;
            else
                gravityMod = 0;
        }
        else if (gravityMod < topSpeed)
            gravityMod += acceleration;

        // Change Gravity
        Vector2 newGravity = new Vector3(inputVector.x, inputVector.y);
        newGravity = new Vector3(cameraRelativeMovement.x, cameraRelativeMovement.z);
        if (newGravity.x != 0 || newGravity.y != 0)
            prevGravity = newGravity;
        Physics.gravity = new Vector3(newGravity.x*gravityMod, downGravity, newGravity.y*gravityMod);

        // Camera Controls
        //float camMod = 3f;
        //camRotGrav.eulerAngles = new Vector3(-prevGravity.y*gravityMod* camMod, 0, prevGravity.x*gravityMod* camMod);

        // Fix Ball Rotation

        // Fix Skybox Rotation

        // Gather previous inputs for comparison's sake
        prevXInput = Mathf.RoundToInt(inputVector.x);
        prevYInput = Mathf.RoundToInt(inputVector.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSphereCol.position.y < -30)
            RespawnObject(startingLocation);

        VelocityXZ = Vector3.Distance(new Vector3(ballBody.position.x, 0, ballBody.position.z), new Vector3(PlayerPos.x, 0, PlayerPos.z));
        VelocityY = Vector3.Distance(new Vector3(0, ballBody.position.y, 0), new Vector3(0, PlayerPos.y, 0));
        PlayerPos = ballBody.position;

        RippleCamera.transform.position = ballBody.position + Vector3.up * 10;
        RippleCamera.transform.eulerAngles = new Vector3(90, 0, 0);
        if (IsOwner)
            Shader.SetGlobalVector("_Player", ballBody.position);

        /*
        if (!IsOwner) return;

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
            TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } });
            
            randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false,
                message = "All your base are belong to us!",
            };            
        }
        
        if (Keyboard.current.yKey.wasPressedThisFrame)
        { 
            spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
            Destroy(spawnedObjectPrefab.gameObject);
        }        
        
        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4 && VelocityY > 0.03f)
        {
            CreateRipple(-180, 180, 3, 2, 0.5f, 2);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 4 && VelocityXZ > 0.0075f && Time.renderedFrameCount % 3 == 0)
        {
            GameObject empt = new GameObject();
            Transform forwardTransform = empt.transform;
            forwardTransform.rotation = Quaternion.LookRotation(myRB.velocity, Vector3.up);

            int y = (int)forwardTransform.eulerAngles.y;
            CreateRipple(y-90, y+90, 3, 5, 0.5f, 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 4 && VelocityY > 0.03f)
        {
            CreateRipple(-180, 180, 3, 2, 0.5f, 2);
        }

        if (other.tag == "LoadLevel")
            LoadLevel(other.name); // load new stage
    }

    void CreateRipple(int Start, int End, int Delta, float Speed, float Size, float Lifetime)
    {
        ripple.transform.eulerAngles = Vector3.zero;
        Vector3 forward = ripple.transform.eulerAngles;
        forward.y = Start;
        ripple.transform.eulerAngles = forward;

        GameObject empt = new GameObject();
        Transform forwardTransform = empt.transform;
        forwardTransform.rotation = Quaternion.LookRotation(myRB.velocity, Vector3.up);

        for (int i = Start; i < End; i += Delta)
        {
            ripple.Emit(ballBody.position + forwardTransform.transform.forward * 0.1f, ripple.transform.forward*Speed, Size, Lifetime, Color.white);
            ripple.transform.eulerAngles += Vector3.up * 3;
        }
    }

    // RPC (function must end in "ServerRpc")
    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRPC " + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId);
    }

    // RPC (function must end in "ClientRpc")
    [ClientRpc]
    private void TestClientRpc(string message)
    {
        publicRoomCode = message;
    }
}
