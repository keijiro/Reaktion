using UnityEngine;
using System.Collections;

public class MessageReceiver : MonoBehaviour
{
    void OnReaktorInput (float level)
    {
        Debug.Log (level);
    }

    void OnReaktorTrigger ()
    {
        Debug.Log ("Bang!");
    }
}
