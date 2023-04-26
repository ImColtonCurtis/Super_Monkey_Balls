using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] ButtonSelectionTracker[] mainMenuButtonTrackers, multiplayerMenuButtonTrackers, onlineMenuButtonTrackers;

    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    [SerializeField] CinemachineVirtualCamera preMainVC, mainTitleVC;
    [SerializeField] CinemachineVirtualCamera[] mainMenuHovers, multiplayerMenuHovers, onlineMenuHovers;
    [SerializeField] Image[] mainMenuImages, multiplayerMenuImages, onlineMenuImages;
    [SerializeField] TextMeshProUGUI[] mainMenuTMP, multiplayerMenuTMP, onlineMenuTMP;
    [SerializeField] GameObject[] mainMenuButtonsObj, multiplayerMenuButtonsObj, onlineMenuButtonsObj;

    [SerializeField] Button[] mainMenuButtons, multiplayerMenuButtons, onlineMenuButtons;

    [SerializeField] private TMP_InputField clientInput;
    [SerializeField] private TestRelay testRelay;

    private bool hasBegun;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.UI.Enable();

        //set game obj as selected
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(mainMenuButtonsObj[1], new BaseEventData(eventSystem)); // multiplayer button hover

        #region Main Menu Buttons
        // main menu buttons
        EnableMenuButtons(mainMenuButtons, false); // disable buttons
        mainMenuButtons[0].onClick.AddListener(() => // single player button
        {
            if (hasBegun)
            {
                EnableVC(mainMenuHovers, false); // disable hovers
                EnableMenuButtons(mainMenuButtons, false); // disable buttons
                FadeMenuButtons(mainMenuButtonsObj, mainMenuImages, mainMenuTMP, 40, false); // fade out main menu buttons
            }
        });
        mainMenuButtons[1].onClick.AddListener(() => // multiplayer button
        {
            if (hasBegun)
            {
                EnableVC(mainMenuHovers, false); // disable hovers
                EnableMenuButtons(mainMenuButtons, false); // disable buttons
                EnableMenuButtons(multiplayerMenuButtons, true); // enable buttons
                FadeMenuButtons(mainMenuButtonsObj, mainMenuImages, mainMenuTMP, 40, false); // fade out main menu buttons
                FadeMenuButtons(multiplayerMenuButtonsObj, multiplayerMenuImages, multiplayerMenuTMP, 24, true); // fade in multiplayer buttons

                eventSystem.SetSelectedGameObject(multiplayerMenuButtonsObj[1], new BaseEventData(eventSystem)); // online button hover
            }
        });
        mainMenuButtons[2].onClick.AddListener(() => // options button
        {
            if (hasBegun)
            {
                EnableVC(mainMenuHovers, false); // disable hovers
                EnableMenuButtons(mainMenuButtons, false); // disable buttons
                FadeMenuButtons(mainMenuButtonsObj, mainMenuImages, mainMenuTMP, 40, false); // fade out main menu buttons
            }
        });
        mainMenuButtons[3].onClick.AddListener(() => // exit button
        {
            if (hasBegun)
            {
                EnableVC(mainMenuHovers, false); // disable hovers
                EnableMenuButtons(mainMenuButtons, false); // disable buttons
                FadeMenuButtons(mainMenuButtonsObj, mainMenuImages, mainMenuTMP, 40, false); // fade out main menu buttons
                Application.Quit();
            }
        });
        #endregion

        #region Singleplayer Menu Buttons
        // singleplayer menu buttons
        #endregion

        #region Multiplayer Menu Buttons
        // multiplayer menu buttons
        EnableMenuButtons(multiplayerMenuButtons, false); // disable buttons
        multiplayerMenuButtons[0].onClick.AddListener(() => // split screen button
        {
            if (hasBegun)
            {
                EnableVC(multiplayerMenuHovers, false); // disable hovers
                EnableMenuButtons(multiplayerMenuButtons, false); // disable buttons
                FadeMenuButtons(multiplayerMenuButtonsObj, multiplayerMenuImages, multiplayerMenuTMP, 40, false); // fade out multiplayer menu buttons
            }
        });
        multiplayerMenuButtons[1].onClick.AddListener(() => // online button
        {
            if (hasBegun)
            {
                EnableVC(multiplayerMenuHovers, false); // disable hovers
                EnableMenuButtons(multiplayerMenuButtons, false); // disable buttons
                FadeMenuButtons(multiplayerMenuButtonsObj, multiplayerMenuImages, multiplayerMenuTMP, 40, false); // fade out multiplayer menu buttons

                EnableMenuButtons(onlineMenuButtons, true); // enable buttons

                clientInput.enabled = true; // enable input field
                FadeMenuButtons(onlineMenuButtonsObj, onlineMenuImages, onlineMenuTMP, 24, true); // fade in online menu buttons

                onlineMenuButtonsObj[2].SetActive(true); // enable text input field

                eventSystem.SetSelectedGameObject(onlineMenuButtonsObj[0], new BaseEventData(eventSystem)); // host button hover
            }
        });
        multiplayerMenuButtons[2].onClick.AddListener(() => // back button
        {
            if (hasBegun)
            {
                EnableVC(multiplayerMenuHovers, false); // disable hovers
                EnableMenuButtons(multiplayerMenuButtons, false); // disable buttons
                FadeMenuButtons(multiplayerMenuButtonsObj, multiplayerMenuImages, multiplayerMenuTMP, 40, false); // fade out multiplayer menu buttons

                EnableMenuButtons(mainMenuButtons, true); // enable buttons
                FadeMenuButtons(mainMenuButtonsObj, mainMenuImages, mainMenuTMP, 24, true); // fade in main menu buttons
                eventSystem.SetSelectedGameObject(mainMenuButtonsObj[1], new BaseEventData(eventSystem)); // multiplayer button hover
            }
        });
        #endregion

        #region Online Menu Buttons
        // online menu buttons
        EnableMenuButtons(onlineMenuButtons, false); // disable buttons
        clientInput.enabled = false; // disable input field
        onlineMenuButtons[0].onClick.AddListener(() => // host button
        {
            EnableVC(onlineMenuHovers, false); // disable hovers
            EnableMenuButtons(onlineMenuButtons, false); // disable buttons

            onlineMenuButtonsObj[2].SetActive(false); // disable text input field

            FadeMenuButtons(onlineMenuButtonsObj, onlineMenuImages, onlineMenuTMP, 40, false); // fade out online menu buttons
            testRelay.CreateRelay();
        });
        onlineMenuButtons[1].onClick.AddListener(() => // client button
        {
            EnableVC(onlineMenuHovers, false); // disable hovers
            EnableMenuButtons(onlineMenuButtons, false); // disable buttons

            onlineMenuButtonsObj[2].SetActive(false); // disable text input field

            FadeMenuButtons(onlineMenuButtonsObj, onlineMenuImages, onlineMenuTMP, 40, false); // fade out online menu buttons
            testRelay.JoinRelay(clientInput.text);
        });
        onlineMenuButtons[2].onClick.AddListener(() => // back button
        {
            EnableVC(onlineMenuHovers, false); // disable hovers
            EnableMenuButtons(onlineMenuButtons, false); // disable buttons
            FadeMenuButtons(onlineMenuButtonsObj, onlineMenuImages, onlineMenuTMP, 40, false); // fade out online menu buttons

            clientInput.enabled = false; // disable input field

            onlineMenuButtonsObj[2].SetActive(false); // disable text input field

            EnableMenuButtons(multiplayerMenuButtons, true); // enable buttons
            FadeMenuButtons(multiplayerMenuButtonsObj, multiplayerMenuImages, multiplayerMenuTMP, 24, true); // fade in multiplayer menu buttons
            eventSystem.SetSelectedGameObject(multiplayerMenuButtonsObj[1], new BaseEventData(eventSystem)); // online button hover
        });

        #endregion

        StartCoroutine(BootStrap());
    }

    IEnumerator BootStrap()
    {
        // initalizations
        hasBegun = false;

        #region Main Menu Literature
        // main menu
        foreach (CinemachineVirtualCamera myVC in mainMenuHovers) // disable main menu vc's
        {
            myVC.enabled = false;
        }
        foreach (Image image in mainMenuImages)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
        foreach (TextMeshProUGUI tmp in mainMenuTMP)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0);
        }
        foreach (GameObject obj in mainMenuButtonsObj)
        {
            obj.SetActive(false);
        }
        #endregion

        #region Multiplayer Menu Literature
        // multiplayer menu
        foreach (CinemachineVirtualCamera myVC in multiplayerMenuHovers) // disable multiplayer menu vc's
        {
            myVC.enabled = false;
        }
        foreach (Image image in multiplayerMenuImages)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
        foreach (TextMeshProUGUI tmp in multiplayerMenuTMP)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0);
        }
        foreach (GameObject obj in multiplayerMenuButtonsObj)
        {
            obj.SetActive(false);
        }
        #endregion

        #region Online Menu Literature
        // online menu
        foreach (CinemachineVirtualCamera myVC in onlineMenuHovers) // disable multiplayer menu vc's
        {
            myVC.enabled = false;
        }
        foreach (Image image in onlineMenuImages)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
        foreach (TextMeshProUGUI tmp in onlineMenuTMP)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0);
        }
        foreach (GameObject obj in onlineMenuButtonsObj)
        {
            obj.SetActive(false);
        }

        #endregion

        yield return new WaitForSeconds(0.5f);
        preMainVC.enabled = false; // disaple preMainVC        
    }

    private void OnEnable()
    {
        GlobalNotifier.OnMainMenuButtonSelecttionChanged += MainMenuButtonSelectionHasChanged;
        GlobalNotifier.OnMultiplayerMenuButtonSelectionChanged += MultiplayerMenuButtonSelectionHasChanged;
        GlobalNotifier.OnOnlineMenuButtonSelectionChanged += OnlineMenuButtonSelectionHasChanged;
    }

    private void OnDisable()
    {
        GlobalNotifier.OnMainMenuButtonSelecttionChanged -= MainMenuButtonSelectionHasChanged;
        GlobalNotifier.OnMultiplayerMenuButtonSelectionChanged -= MultiplayerMenuButtonSelectionHasChanged;
        GlobalNotifier.OnOnlineMenuButtonSelectionChanged -= OnlineMenuButtonSelectionHasChanged;
    }

    void MainMenuButtonSelectionHasChanged()
    {
        foreach (CinemachineVirtualCamera myVC in mainMenuHovers)
        {
            myVC.enabled = false;
        }

        foreach (ButtonSelectionTracker myButton in mainMenuButtonTrackers)
        {
            if (myButton.IsSelected)
            {
                Debug.Log(myButton);
                if (myButton == mainMenuButtonTrackers[0]) // single player hover
                {
                    mainMenuHovers[0].enabled = true;
                }
                else if (myButton == mainMenuButtonTrackers[1]) // multiplayer hover
                {
                    mainMenuHovers[1].enabled = true;
                }
                else if (myButton == mainMenuButtonTrackers[2]) // options hover
                {
                    mainMenuHovers[2].enabled = true;
                }
                else if (myButton == mainMenuButtonTrackers[3]) // exit hover
                {
                    mainMenuHovers[3].enabled = true;
                }
            }
        }
    }

    void MultiplayerMenuButtonSelectionHasChanged()
    {
        foreach (CinemachineVirtualCamera myVC in multiplayerMenuHovers)
        {
            myVC.enabled = false;
        }

        foreach (ButtonSelectionTracker myButton in multiplayerMenuButtonTrackers)
        {
            if (myButton.IsSelected)
            {
                Debug.Log(myButton);
                if (myButton == multiplayerMenuButtonTrackers[0]) // split screen hover
                {
                    multiplayerMenuHovers[0].enabled = true;
                }
                else if (myButton == multiplayerMenuButtonTrackers[1]) // online hover
                {
                    multiplayerMenuHovers[1].enabled = true;
                }
                else if (myButton == multiplayerMenuButtonTrackers[2]) // back hover
                {
                    multiplayerMenuHovers[2].enabled = true;
                }
            }
        }
    }

    void OnlineMenuButtonSelectionHasChanged()
    {
        foreach (CinemachineVirtualCamera myVC in onlineMenuHovers)
        {
            myVC.enabled = false;
        }

        foreach (ButtonSelectionTracker myButton in onlineMenuButtonTrackers)
        {
            if (myButton.IsSelected)
            {
                Debug.Log(myButton);
                if (myButton == onlineMenuButtonTrackers[0]) // host hover
                {
                    onlineMenuHovers[0].enabled = true;
                }
                else if (myButton == onlineMenuButtonTrackers[1]) // join hover
                {
                    onlineMenuHovers[1].enabled = true;
                }
                else if (myButton == onlineMenuButtonTrackers[2]) // back hover
                {
                    onlineMenuHovers[2].enabled = true;
                }
            }
        }
    }

    public void Begin(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!hasBegun)
            {
                mainTitleVC.enabled = false; // switch to main menu cam
                mainMenuHovers[1].enabled = true; // multiplayer button hovered cam

                FadeMenuButtons(mainMenuButtonsObj, mainMenuImages, mainMenuTMP, 40, true); // fade main menu buttons in
                StartCoroutine(WaitToEnable());

                Debug.Log("Begin " + context);
                hasBegun = true;
            }
        }
    }

    IEnumerator WaitToEnable()
    {
        yield return new WaitForSeconds(0.5f);
        // enable buttons
        EnableMenuButtons(mainMenuButtons, true);
    }

    void EnableVC(CinemachineVirtualCamera[] virtualCameras, bool enabled)
    {
        // online menu
        foreach (CinemachineVirtualCamera myVC in virtualCameras) // disable multiplayer menu vc's
        {
            myVC.enabled = enabled;
        }
    }

    void FadeMenuButtons(GameObject[] buttonObjs, Image[] buttonImages, TextMeshProUGUI[] buttonTMPs, float animLength, bool fadeDir) // true = fade in
    {
        // enable button objs
        foreach (GameObject obj in buttonObjs)
        {
            obj.SetActive(true);
        }
        // fade main menu button images in
        foreach (Image image in buttonImages)
        {
            StartCoroutine(GlobalFunctions.FadeImage(animLength, image, fadeDir));
        }
        // fade main menu tmps in
        foreach (TextMeshProUGUI tmp in buttonTMPs)
        {
            StartCoroutine(GlobalFunctions.FadeTMP(animLength, tmp, fadeDir));
        }
    }

    void EnableMenuButtons(Button[] myButtons, bool buttonDir)
    {
        foreach (Button button in myButtons)
            button.enabled = buttonDir;
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (hasBegun)
            {

                Debug.Log("Submit " + context);
            }
        }
    }
}
