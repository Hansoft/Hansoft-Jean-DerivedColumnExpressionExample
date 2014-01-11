using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HPMSdk;
using Hansoft.ObjectWrapper;

namespace Hansoft.Jean.Behavior.DeriveBehavior.Expressions
{
    public class Shadowing
    {
        public static string GetShadowValue(Task current_task, string shadowProjectName, string sourceDataBaseIDColumnName, string columnName)
        {
            try
            {
                List<Task> shadowTasks = new List<Task>(HPMUtilities.GetProjects().Find(project => project.Name == shadowProjectName).ProductBacklog.DeepChildren.Cast<Task>());
                Task shadowTask = shadowTasks.Find(shadow => shadow.GetCustomColumnValue(sourceDataBaseIDColumnName).ToInt() == current_task.UniqueID.m_ID);
                if (shadowTask != null)
                    return shadowTask.GetCustomColumnValue(columnName).ToString();
                else
                    return "";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
    }
}
