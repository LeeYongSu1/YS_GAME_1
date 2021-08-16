using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;
    float _pressedTime = 0;

    public void OnUpdate()
    {
        /*if (EventSystem.current.IsPointerOverGameObject())
            return;*/
        try
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && KeyAction != null)
            {
                KeyAction.Invoke();

            }
        }
        catch (Exception ex)
        {
          
        }

        if (MouseAction != null)
        {

            /*if (Input.GetMouseButtonDown(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Click);
                //MouseAction.Invoke(Define.MouseEvent.PointerUp);
            }
            if (Input.GetMouseButton(0))
            {
                Debug.Log("인보크");
                MouseAction.Invoke(Define.MouseEvent.PointerUp);
                if (Input.GetMouseButtonUp(0))
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
            }*/
            if (Input.GetMouseButton(0))
            {
                if (!_pressed)
                {
                   
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    if (Time.time < _pressedTime + 0.2f)
                    {
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    }
                        
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                _pressed = false;
                _pressedTime = 0;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
