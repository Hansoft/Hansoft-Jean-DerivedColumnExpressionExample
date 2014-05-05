using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HPMSdk;
using Hansoft.ObjectWrapper;

namespace Hansoft.Jean.Behavior.DeriveBehavior.Expressions
{
    public class AggregatePassCounter
    {
        public static string PassCount(Task current_task)
        {
            if (!current_task.HasChildren)
                return "";
            int passCount = 0;
            foreach (Task task in current_task.DeepChildren)
            {
                if (!task.HasChildren)
                {
                    string lastRun = task.GetCustomColumnValue("Last Run").ToString();
                    string lastPass = task.GetCustomColumnValue("Latest Pass").ToString();

                    if (lastRun != "" && lastRun == lastPass)
                        passCount += 1;
                }
            }
            return passCount.ToString();
        }
    }
}
