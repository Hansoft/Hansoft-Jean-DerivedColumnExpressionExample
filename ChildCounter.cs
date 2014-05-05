using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HPMSdk;
using Hansoft.ObjectWrapper;

namespace Hansoft.Jean.Behavior.DeriveBehavior.Expressions
{
    public class ChildrenCounter
    {
        public static string ChildCount(Task current_task)
        {
            if (!current_task.HasChildren)
                return "";
            int totalCount = 0;
            foreach (Task task in current_task.DeepChildren)
            {
                if (!task.HasChildren)
                {
                    totalCount += 1;
                }
            }
            return totalCount.ToString();
        }
    }
}
