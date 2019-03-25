using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour, IInputHandler, ISourceStateHandler, IManipulationHandler
{
    [Range(0.01f, 1.0f)] public float PositionLerpSpeed = 0.4f;
    [Range(0.01f, 1.0f)] public float RotationLerpSpeed = 0.4f;
    public float DistanceScale = 8f;

    private bool IsDraggingEnabled = true;
    private bool isDragging;

    private Vector3 manipulationEventData;
    private Vector3 manipulationDelta;

    private IInputSource currentInputSource;
    private uint currentInputSourceId;

    public void OnInputDown(InputEventData eventData)
    {
        if (IsDraggingEnabled && !isDragging &&
            eventData.InputSource.SupportsInputInfo(eventData.SourceId,
            inputInfo: SupportedInputInfo.PointerPosition))
        {
            InputManager.Instance.PushModalInputHandler(gameObject);
            isDragging = true;
            currentInputSource = eventData.InputSource;
            currentInputSourceId = eventData.SourceId;
        }
        else { return; }
    }

    public void OnInputUp(InputEventData eventData)
    {
        if (currentInputSource != null && eventData.SourceId == currentInputSourceId
            && isDragging)
        {
            InputManager.Instance.PopModalInputHandler();
            isDragging = false;
            currentInputSource = null;
        }
    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        Debug.LogFormat("Cancel Scale: Sources: {0} \r\n" +
    "CummulativeDelta: {1} {2} {3} ",
        eventData.InputSource, eventData.CumulativeDelta.x,
        eventData.CumulativeDelta.y, eventData.CumulativeDelta.z);
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        Debug.LogFormat("Complete Move: Sources: {0} \r\n" +
    "CummulativeDelta: {1} {2} {3} ",
        eventData.InputSource, eventData.CumulativeDelta.x,
        eventData.CumulativeDelta.y, eventData.CumulativeDelta.z);
        manipulationDelta = Vector3.zero;
        manipulationEventData = Vector3.zero;
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        Debug.LogFormat("Start Move: Sources: {0} \r\n" +
            "CummulativeDelta: {1} {2} {3} ",
                eventData.InputSource, eventData.CumulativeDelta.x,
                eventData.CumulativeDelta.y, eventData.CumulativeDelta.z);
        manipulationEventData = eventData.CumulativeDelta;
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        Vector3 delta = eventData.CumulativeDelta - manipulationEventData;
        manipulationDelta = delta * DistanceScale;
        manipulationEventData = eventData.CumulativeDelta;
    }

    public void OnSourceDetected(SourceStateEventData eventData)
    {

    }

    public void OnSourceLost(SourceStateEventData eventData)
    {
        if (currentInputSource != null && eventData.SourceId == currentInputSourceId
            && isDragging)
        {
            InputManager.Instance.PopModalInputHandler();
            isDragging = false;
            currentInputSource = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion currentRot = gameObject.transform.rotation;
        if (IsDraggingEnabled && isDragging) {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, 
                                            gameObject.transform.position + manipulationDelta, 
                                            PositionLerpSpeed);
        }
    }

}
