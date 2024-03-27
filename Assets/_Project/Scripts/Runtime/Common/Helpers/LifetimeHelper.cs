using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class LifetimeHelper : MonoBehaviour
    {
        #region FIELDS

        public float Lifetime = 5f;

        #endregion

        #region METHODS

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(Lifetime);
            Destroy(gameObject);
        }

        #endregion
    }