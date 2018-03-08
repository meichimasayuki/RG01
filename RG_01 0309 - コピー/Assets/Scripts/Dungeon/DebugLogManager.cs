using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogManager : MonoBehaviour
{
    [SerializeField] private GameObject debugLogObject = null;
    [SerializeField] private Text textUI = null;

    private bool isActive = false;

    private void Awake()
    {
        Application.logMessageReceived += OnLogMessage;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived += OnLogMessage;
    }

    public void DebugLogSwitch()
    {
        isActive = !isActive;
        debugLogObject.SetActive(isActive);
    }

    private void OnLogMessage(string i_logText, string i_stackTrace, LogType i_type)
    {
        if (string.IsNullOrEmpty(i_logText))
        {
            return;
        }

        if (!string.IsNullOrEmpty(i_stackTrace))
        {
            switch (i_type)
            {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    i_logText += System.Environment.NewLine + i_stackTrace;
                    break;
                default:
                    break;
            }
        }

        switch (i_type)
        {
            case LogType.Error:
            case LogType.Assert:
            case LogType.Exception:
                i_logText = string.Format("<color=red>{0}</color>", i_logText);
                break;
            case LogType.Warning:
                i_logText = string.Format("<color=yellow>{0}</color>", i_logText);
                break;
            default:
                break;
        }

        textUI.text += i_logText + System.Environment.NewLine;
    }
}
