using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        public void StartAction(IAction action)
        {
            if (currentAction == action) { return; }
            // print("Cancelling action " + action);
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}

