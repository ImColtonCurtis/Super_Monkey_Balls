using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkCredentialsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomCode;

    private void OnEnable()
    {
        roomCode.text = PlayerNetwork.publicRoomCode;
    }
}
