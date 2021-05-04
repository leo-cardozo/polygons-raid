using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA 
{
    [CreateAssetMenu(menuName = "Phases/Place Units")]
    public class PlaceUnits : Phase
    {
        public override bool IsComplete(SessionManager sm)
        {
            return true;
        }

        public override void OnStartPhase(SessionManager sm)
        {
            base.OnStartPhase(sm);
            PlaceUnitsOnGrid(sm);
        }

        public override void OnEndPhase(SessionManager sm)
        {

        }

        void PlaceUnitsOnGrid(SessionManager sm) 
        {

            GridCharacter[] units = sm.gridObject.GetComponentsInChildren<GridCharacter>();

            foreach (GridCharacter u in units)
            {
                Node n = sm.gridManager.GetNode(u.transform.position);
                if(n != null)
                {
                    u.transform.position = n.worldPosition;
                }
            }

        }
    }

}
