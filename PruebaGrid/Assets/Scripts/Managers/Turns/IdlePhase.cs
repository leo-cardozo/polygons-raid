using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Phases/Idle Phase")]
    public class IdlePhase : Phase
    {
        public override bool IsComplete(SessionManager sm)
        {
            return false;
        }
        public override void OnStartPhase(SessionManager sm)
        {
            base.OnStartPhase(sm);
            Debug.Log("LALALA");
        }

        public virtual void OnEndPhase(SessionManager sm)
        {
            isInit = false;
            forceExit = false;
        }

    }
}
