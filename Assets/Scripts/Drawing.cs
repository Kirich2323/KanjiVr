using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VRTK;
using Tesseract;

public class Drawing : MonoBehaviour
{
    // Start is called before the first frame update
    private VRTK_ControllerEvents controllerEvents;
    private LineRenderer currentLine;
    private Vector3 lastPos;
    private int numVertices = 0;

    void Start()
    {
        controllerEvents = GetComponent<VRTK_ControllerEvents>();
        Debug.Log("Right hand");

        controllerEvents.TriggerPressed += DoTriggerPressed;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position == lastPos)
        {
            return;
        }

        if (controllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerPress))
        {
            currentLine.positionCount = numVertices + 1; //?
            currentLine.SetPosition(numVertices, transform.position);
            numVertices++;
            Debug.Log("pressed");
        }
        else
        {
            Debug.Log("Not pressed");
            Debug.Log(transform.position);
        }
        lastPos = transform.position;
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        GameObject go = new GameObject();
        currentLine = go.AddComponent<LineRenderer>();
        currentLine.startWidth = .02f;
        currentLine.endWidth = .02f;
        //currentLine.material.shader = Shader.Find("Standart");
        //var value = currentLine.material.GetFloat("_Glossiness");
        currentLine.material.SetFloat("_Glossiness", 0.0f);
        currentLine.material.color = Color.red;
        numVertices = 0;
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        //triggerPressed = false;
        //DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "released", e);
    }
}
