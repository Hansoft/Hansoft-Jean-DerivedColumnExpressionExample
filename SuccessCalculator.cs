using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HPMSdk;
using Hansoft.ObjectWrapper;

namespace Hansoft.Jean.Behavior.DeriveBehavior.Expressions
{
    public class SuccessCalculator
    {
        public static string CalculateSuccessRate(Task current_task)
        {
            long timesRun = 0;
            long timesPassed = 0;
            if(current_task.HasChildren)
            { 
                timesRun = current_task.GetAggregatedCustomColumnValue("Times Run").ToInt();
                if (timesRun == 0)
                    return "";
                timesPassed = current_task.GetAggregatedCustomColumnValue("Times Passed").ToInt();
            }
            else 
            {
                timesRun = current_task.GetCustomColumnValue("Times Run").ToInt();
                if (timesRun == 0)
                    return "";
                timesPassed = current_task.GetCustomColumnValue("Times Passed").ToInt();
            }
            return Math.Round(100 * timesPassed / (double)timesRun, 2) + " %";
        }
    }
}
