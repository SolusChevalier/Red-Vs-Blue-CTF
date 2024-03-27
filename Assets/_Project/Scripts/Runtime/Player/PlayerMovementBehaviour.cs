using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CTF
{
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        #region FIELDS

        public float speed = 5f;
        public float TurnSpeed = 10f;
        private PlayerMovement inputActions;

        #endregion FIELDS

        #region UNITY METHODS

        public void Awake()
        {
            inputActions = new PlayerMovement();
            inputActions.Movement.MovementInput.performed += ctx => Move(ctx.ReadValue<Vector2>());
        }

        public void OnEnable()
        {
            inputActions.Enable();
        }

        public void OnDisable()
        {
            inputActions.Disable();
        }

        public void Update()
        {
            Vector2 movementInput = inputActions.Movement.MovementInput.ReadValue<Vector2>();
            Move(movementInput);
        }

        #endregion UNITY METHODS

        #region METHODS

        public void Move(Vector2 direction)
        {
            Vector3 movement = transform.forward * direction.y * speed * Time.deltaTime;
            transform.Translate(movement, Space.World);
            float turn = direction.x * TurnSpeed * Time.deltaTime;
            transform.Rotate(0, turn, 0);
        }

        #endregion METHODS
    }
}