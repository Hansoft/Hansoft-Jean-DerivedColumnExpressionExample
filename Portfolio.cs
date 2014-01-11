using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HPMSdk;
using Hansoft.ObjectWrapper;

namespace Hansoft.Jean.Behavior.DeriveBehavior.Expressions
{
    public class Portfolio
    {

        private class ReleaseDateComparer : IComparer<Release>
        {
            public int Compare(Release x, Release y)
            {
                return (x.Date.Ticks - y.Date.Ticks)>0?1:-1;
            }
        }

        public static string NextTollGate(Task current_task)
        {
            if (current_task.LinkedTasks.Count == 0)
                return "";
            Task other = current_task.LinkedTasks[0];
            if (other is SubProject && other.Parent is Schedule && other.Name == "Tollgates")
            {
                List<Release> tollgates = new List<Release>((other.Children.FindAll(r => r is Release)).Cast<Release>());
                if (tollgates.Count == 0)
                    return "No tollgates found";
                tollgates.Sort(new ReleaseDateComparer());
                foreach (Release tg in tollgates)
                {
                    bool notCompleted = true;
                    if (tg.ProductBacklogItemsInSchedule.Count > 0 || tg.ScheduledTasks.Count > 0 || tg.Sprints.Count > 0)
                        notCompleted = tg.ProductBacklogItemsInSchedule.Exists(p => (EHPMTaskStatus)p.AggregatedStatus.Value != EHPMTaskStatus.Completed) ||
                                     tg.ScheduledTasks.Exists(p => (EHPMTaskStatus)p.AggregatedStatus.Value != EHPMTaskStatus.Completed) ||
                                     tg.Sprints.Exists(p => (EHPMTaskStatus)p.AggregatedStatus.Value != EHPMTaskStatus.Completed);
                    if (notCompleted)
                    {
                            return tg.Name + " " + tg.Date.ToShortDateString();
                        /*
                         * TBD Need to fix calculation of duration properly for ScheduledTasks and ProductBacklogItemsINSchedule for this to work right
                        int total = tg.ScheduledTasks.Sum(t => t.Duration) + tg.ProductBacklogItemsInSchedule.Sum(t => t.Duration);
                        if (total == 0)
                        int completed = tg.ScheduledTasks.FindAll(t => (EHPMTaskStatus)t.Status.Value == EHPMTaskStatus.Completed).Sum(t => t.Duration);
                        double percentComplete = completed / total * 100;
                        return string.Format("[{0:F1}%] ", tg.PercentComplete) + tg.Name + " " + tg.Date.ToShortDateString();
                        */
                    }
                }
                return "All completed";
            }
            else
                return "No tollgates found";
        }

        public static string TotalCostToDate(Task current_task)
        {
            long costToDate = current_task.GetCustomColumnValue("HW Actual").ToInt() + current_task.GetCustomColumnValue("SW Actual").ToInt() + current_task.GetCustomColumnValue("Work to date").ToInt() * current_task.GetCustomColumnValue("Work cost/h").ToInt();
            return costToDate.ToString();
        }

        public static string AggregatedTimeSpent(Task current_task, string columnName)
        {
            if (current_task.HasChildren)
            {
                HPMTaskCustomSummaryValue[] summaries = SessionManager.Session.TaskRefGetSummary(current_task.UniqueID).m_CustomSummaryValues;
                foreach (HPMTaskCustomSummaryValue summary in summaries)
                {
                    if (summary.m_Hash == current_task.ProjectView.GetCustomColumn(columnName).m_Hash)
                        return summary.m_FloatValue.ToString();
                }
            }
            return current_task.GetCustomColumnValue(columnName).ToString();
        }

        public static string WorkToDate(Task current_task, string columnName)
        {
            if (current_task.LinkedTasks.Count == 0)
                return "";
            Task other = current_task.LinkedTasks[0];
            if (other is SubProject && other.Parent is Schedule && other.Name == "Tollgates")
            {
                long workToDate = 0;
                foreach (Task task in other.ProjectView.Children)
                {
                    workToDate += task.GetCustomColumnValue(columnName).ToInt();
                }
                return workToDate.ToString();
            }
            else
                return "0";
        }
    }
}
