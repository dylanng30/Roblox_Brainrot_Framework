using System.Collections;
using UnityEngine;

namespace RobloxFW.MovementSystem.Player.Components
{
    public class PlayerInput : MonoBehaviour
{
    public Vector2 Movement { get; private set; }
    
    public bool IsPressingSpace { get; private set; }
    
    public bool IsPressingLeftControl { get; private set; }
    public bool IsPressingRightControl { get; private set; }
    
    //Dash
    public bool Dash { get; private set; } = false;
    private bool dashOnCooldown = false;
    
    //Sprint
    public bool Sprint { get; private set; } = false;
    
    
    //ItemBase
    public int HotBarIndex { get; private set; } = 0;
    
    
    private void Update()
    {
        HandleItemInput();
        MovementDirection();
        DashInput();
        SprintInput();
        IsPressingSpace = Input.GetKeyDown(KeyCode.Space);
        IsPressingLeftControl = Input.GetKey(KeyCode.LeftControl);
        IsPressingRightControl = Input.GetKeyDown(KeyCode.RightControl);
    }

    #region ---ItemBase Slot---

    private void HandleItemInput()
    {
        /*for (int i = 0; i <= Constants.HotbarSize ; i++)
        {
            KeyCode key = KeyCode.Alpha0 + i;
            if (Input.GetKeyDown(key))
            {
                HotBarIndex = i - 1;
            }
        }*/
    }

    #endregion

    #region ---Sprint Inputs ---
    private void SprintInput()
    {
        Sprint = Input.GetKey((KeyCode.LeftShift));
    }
    #endregion

    #region ---Dash Inputs ---
    private void DashInput()
    {
        if (dashOnCooldown)
        {
            Debug.Log("[PlayerInput] Dashing or on cooldown");
            return;
        }

        Dash = Input.GetKeyDown(KeyCode.E);
        //Debug.Log($"[PlayerInput] Dashing: {Dash}");
    }

    public void DisableDashAction(float seconds)
    {
        Dash = false;
        StartCoroutine(DisableDashInput(seconds));
    }
    private IEnumerator DisableDashInput(float seconds)
    {
        dashOnCooldown = true;
        yield return new WaitForSeconds(seconds);
        dashOnCooldown = false;
    }
    #endregion

    #region ---MOUSE INPUT---
    private void MovementDirection()
    {
        Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    public bool PressLeftMouse()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool IsDraggingLeftMouse()
    {
        return Input.GetMouseButton(0);
    }

    public bool DropLeftMouse()
    {
        return Input.GetMouseButtonUp(0);
    }
    #endregion
    
}
}