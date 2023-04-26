using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionTracker : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool IsSelected { get; private set; } = false;

    [SerializeField] bool isMainMenuButton, isMultiplayerMenuButton, isOnlineMenuButton;

    public void OnSelect(BaseEventData eventData)
    {
        IsSelected = true;
        if (isMainMenuButton)
            GlobalNotifier.OnMainMenuButtonSelectedChanged();
        else if (isMultiplayerMenuButton)
            GlobalNotifier.OnMultiplayerMenuButtonSelectedChanged();
        else if (isOnlineMenuButton)
            GlobalNotifier.OnOnlineMenuButtonSelectedChanged();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        IsSelected = false;
    }
}
