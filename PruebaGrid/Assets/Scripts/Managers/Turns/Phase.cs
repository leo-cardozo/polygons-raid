using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA 
{
    public abstract class Phase : ScriptableObject
    {
        public string phaseName;
        public bool forceExit;
        public abstract bool IsComplete(SessionManager sm);

        [System.NonSerialized]
        protected bool isInit;
        public virtual void OnStartPhase(SessionManager sm)
        {

            if(isInit)
                return;
            isInit = true;

        }
        public virtual void OnEndPhase(SessionManager sm)
        {
            isInit = false;
            forceExit = false;
        }

    }

}