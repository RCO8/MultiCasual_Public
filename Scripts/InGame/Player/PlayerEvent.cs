using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEvent : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<int> OnActionEvent;

    private void CallMoveEvent(Vector2 dir)
    {
        OnMoveEvent?.Invoke(dir);
    }
    private void CallActionEvent(int index)
    {
        OnActionEvent?.Invoke(index);
    }

    #region CallEventInput
    public void InputMoveEvent(InputAction.CallbackContext context)
    {
        Vector2 dir = Vector2.zero;
        if (context.phase == InputActionPhase.Performed)
            dir = context.ReadValue<Vector2>();
        CallMoveEvent(dir);
    }
    public void InputActionEvent(InputAction.CallbackContext context)
    {
        int value = 0;
        switch(context.control.path)
        {
            case "/Keyboard/z":
                value = 1;
                break;
            case "/Keyboard/x":
                value = 2;
                break;
            case "/Keyboard/c":
                value = 3;
                break;
        }
        CallActionEvent(value);
    }
    #endregion
}
