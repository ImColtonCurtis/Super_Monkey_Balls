using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private TMP_InputField clientInput;

    [SerializeField] private TestRelay testRelay;

    private void Awake()
    {
        hostBtn.onClick.AddListener(() =>
        {
            testRelay.CreateRelay();
            //NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() =>
        {
            // NetworkManager.Singleton.StartClient();
            testRelay.JoinRelay(clientInput.text);
        });
    }
}
