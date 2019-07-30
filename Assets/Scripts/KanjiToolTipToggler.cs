using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class KanjiToolTipToggler : MonoBehaviour {

    public VRTK_DestinationMarker pointer;
    private KanjiToolTip kanjiToolTip;

    // Start is called before the first frame update
    void OnEnable() {
        pointer = (pointer == null ? GetComponent<VRTK_DestinationMarker>() : pointer);

        if (pointer != null) {
            pointer.DestinationMarkerEnter += DestinationMarkerEnter;
            pointer.DestinationMarkerHover += DestinationMarkerHover;
            pointer.DestinationMarkerExit += DestinationMarkerExit;
            pointer.DestinationMarkerSet += DestinationMarkerSet;
        }
        else {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTKExample_PointerObjectHighlighterActivator", "VRTK_DestinationMarker", "the Controller Alias"));
        }
    }

    protected virtual void OnDisable() {
        if (pointer != null) {
            pointer.DestinationMarkerEnter -= DestinationMarkerEnter;
            pointer.DestinationMarkerHover -= DestinationMarkerHover;
            pointer.DestinationMarkerExit -= DestinationMarkerExit;
            pointer.DestinationMarkerSet -= DestinationMarkerSet;
        }
    }

    protected virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e) {
        Debug.Log("Entered");
        ShowToolTip(e.target);
    }

    private void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e) {
        Debug.Log(e.destinationPosition);
        kanjiToolTip.setDrawLineTo(e.destinationPosition);
    }

    protected virtual void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e) {
        Debug.Log("Exited");
        HideToolTip(e.target);
    }

    protected virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e) {
    }

    protected virtual void ShowToolTip(Transform target) {
        KanjiToolTip tooltip = (target != null ? target.GetComponentInChildren<KanjiToolTip>() : null);
        if (tooltip != null) {
            kanjiToolTip = tooltip;
            tooltip.Show();
        }
    }

    protected virtual void HideToolTip(Transform target) {
        KanjiToolTip tooltip = (target != null ? target.GetComponentInChildren<KanjiToolTip>() : null);
        if (tooltip != null) {
            tooltip.Hide();
        }
    }
}
