using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CTF
{
    public class GameSys : MonoBehaviour
    {
        #region FIELDS

        public delegate void FlagResetHandler(GameObject flag);

        public static event FlagResetHandler OnFlagReset;

        public delegate void FlagCapturedHandler(GameObject scorer, string scoreType);

        public static event FlagCapturedHandler OnFlagCaptured;

        #endregion FIELDS

        public static void FlagCaptured(GameObject scorer, string scoreType)
        {
            OnFlagCaptured?.Invoke(scorer, scoreType);
        }
    }
}