using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    
    InputManager im;

    public float moveDir { get; private set; } = 0;
    public bool jump { get; private set; } = false;
    public bool crouch { get; private set; } = false;
    public bool action { get; private set; } = false;
    public bool actionUp { get; private set; } = false;

    void Awake() {
        im = new InputManager();

        im.Player.Move.performed += ctx => {
            moveDir = ctx.ReadValue<float>();
        };
        im.Player.Move.canceled += ctx => {
            moveDir = 0;
        };
        im.Player.Jump.performed += ctx => {
            jump = true;
        };
        im.Player.Jump.canceled += ctx => {
            jump = false;
        };
        im.Player.Crouch.performed += ctx => {
            crouch = true;
        };
        im.Player.Crouch.canceled += ctx => {
            crouch = false;
        };
        im.Player.Action.performed += ctx => {
            action = true;
        };
        im.Player.Action.canceled += ctx => {
            action = false;
        };
        im.Player.ActionUp.performed += ctx => {
            actionUp = true;
        };
        im.Player.ActionUp.canceled += ctx => {
            actionUp = false;
        };
    }

    void OnEnable() {
        im.Enable();
    }

    void OnDisable() {
        im.Disable();
    }

}
