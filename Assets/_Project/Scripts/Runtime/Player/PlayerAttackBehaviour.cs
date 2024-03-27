using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CTF
{
    public class PlayerAttackBehaviour : MonoBehaviour
    {
        #region FIELDS

        public Transform bulletSpawnPoint;
        public GameObject bullet;
        public float bulletVelocity = 50;
        private PlayerMovement inputActions;
        private InputAction attackAction;

        #endregion FIELDS

        #region UNITY METHODS

        public void Awake()
        {
            inputActions = new PlayerMovement();
            attackAction = inputActions.Shoot.Shoot;
            attackAction.Enable();
            attackAction.performed += ctx => Attack();
        }

        #endregion UNITY METHODS

        #region METHODS

        public void Attack()
        {
            GameObject bulletInstance = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody bulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = bulletSpawnPoint.forward * bulletVelocity;
        }

        #endregion METHODS
    }
}