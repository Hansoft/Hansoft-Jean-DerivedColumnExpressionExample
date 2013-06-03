using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HPMSdk;
using Hansoft.ObjectWrapper;

namespace Hansoft.Jean.Behavior.DeriveBehavior.Expressions
{
    public class CompletionCounter
    {
        public static string CompletionCount(Task current_task)
        {
            if (!current_task.HasChildren)
                return "";
            int totalCount = 0;
            int completedCount = 0;
            foreach (Task task in current_task.DeepChildren)
            {
                if (!task.HasChildren)
                {
                    totalCount += 1;
                    if ((EHPMTaskStatus)task.Status.Value == EHPMTaskStatus.Completed)
                        completedCount += 1;
                }
            }
            return completedCount.ToString() + "/" + totalCount.ToString();
        }
    }
}
