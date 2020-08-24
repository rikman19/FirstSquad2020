using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TechnomediaLabs;

namespace Zetcil
{

    public class HiddenObjectController : MonoBehaviour
    {

        public bool isEnabled;

        [Header("Main Setting")]
        public int TotalObject;
        [ReadOnly] public int FoundObject;

        [Header("Win Setting")]
        public UnityEvent WinEvent;
        bool isWinEventExecute = false;

        public void InvokeFoundObject()
        {
            FoundObject++;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (FoundObject == TotalObject)
            {
                if (!isWinEventExecute)
                {
                    isWinEventExecute = true;
                    WinEvent.Invoke();
                }
            }
        }
    }
}
