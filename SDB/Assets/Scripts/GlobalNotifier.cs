using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalNotifier : MonoBehaviour
{
    public delegate void MainMenuButtonSelectionChanged();
    public static event MainMenuButtonSelectionChanged OnMainMenuButtonSelecttionChanged;

    public delegate void MultiplayerMenuButtonSelectionChanged();
    public static event MultiplayerMenuButtonSelectionChanged OnMultiplayerMenuButtonSelectionChanged;

    public delegate void OnlineMenuButtonSelectionChanged();
    public static event OnlineMenuButtonSelectionChanged OnOnlineMenuButtonSelectionChanged;

    public static void OnMainMenuButtonSelectedChanged()
    {
        if (OnMainMenuButtonSelecttionChanged != null)
        {
            OnMainMenuButtonSelecttionChanged();
        }
    }

    public static void OnMultiplayerMenuButtonSelectedChanged()
    {
        if (OnMultiplayerMenuButtonSelectionChanged != null)
        {
            OnMultiplayerMenuButtonSelectionChanged();
        }
    }

    public static void OnOnlineMenuButtonSelectedChanged()
    {
        if (OnOnlineMenuButtonSelectionChanged != null)
        {
            OnOnlineMenuButtonSelectionChanged();
        }
    }
}
