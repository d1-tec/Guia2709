using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class Statistics
    {
        public static TimeSpan AverageAssignationTime(IRepositoryEmergencyCall repositoryEmergencyCalls, EmergencyCall.AssignModes oneMode = EmergencyCall.AssignModes.None, String nameOfMobile = null)
        {
            TimeSpan meanTime = new TimeSpan();
            EmergencyCall[] calls = repositoryEmergencyCalls.GetEmergencyCalls();
            int count = 0;
            foreach (var call in calls)
            {
                if (call.Mobile != null)
                {
                    if (oneMode == EmergencyCall.AssignModes.None && nameOfMobile == null)
                    {
                        meanTime = meanTime + (call.AssignationDate - call.CreationDate);
                        count++;
                    }
                    else
                    {
                        if (call.AssignMode == oneMode && nameOfMobile == null)
                        {
                            meanTime = meanTime + (call.AssignationDate - call.CreationDate);
                            count++;
                        }
                        if (oneMode == EmergencyCall.AssignModes.None && call.Mobile.Name == nameOfMobile)
                        {
                            meanTime = meanTime + (call.AssignationDate - call.CreationDate);
                            count++;
                        }
                        if (call.AssignMode == oneMode && nameOfMobile != null && call.Mobile.Name == nameOfMobile)
                        {
                            meanTime = meanTime + (call.AssignationDate - call.CreationDate);
                            count++;
                        }
                    }
                }
            }
            TimeSpan result = new TimeSpan(0, 0, 0);
            if (count != 0)
            {
                result = TimeSpan.FromMilliseconds(meanTime.TotalMilliseconds / count);
            }
            return result;
        }

        public static TimeSpan AverageSolvedTime(IRepositoryEmergencyCall repositoryEmergencyCalls, EmergencyCall.AssignModes oneMode = EmergencyCall.AssignModes.None, String nameOfMobile = null)
        {
            TimeSpan meanTime = new TimeSpan();
            EmergencyCall[] calls = repositoryEmergencyCalls.GetEmergencyCalls();
            int count = 0;
            foreach (var call in calls)
            {
                if (call.Solved)
                {
                    if (oneMode == EmergencyCall.AssignModes.None && nameOfMobile == null)
                    {
                        meanTime = meanTime + (call.SolvedDate - call.AssignationDate);
                        count++;
                    }
                    else
                    {
                        if (call.AssignMode == oneMode && nameOfMobile == null)
                        {
                            meanTime = meanTime + (call.SolvedDate - call.AssignationDate);
                            count++;
                        }
                        if (oneMode == EmergencyCall.AssignModes.None && call.Mobile.Name == nameOfMobile)
                        {
                            meanTime = meanTime + (call.SolvedDate - call.AssignationDate);
                            count++;
                        }
                        if (call.AssignMode == oneMode && nameOfMobile != null && call.Mobile.Name == nameOfMobile)
                        {
                            meanTime = meanTime + (call.SolvedDate - call.AssignationDate);
                            count++;
                        }
                    }
                }
            }
            TimeSpan result = new TimeSpan(0, 0, 0);
            if (count != 0)
            {
                result = TimeSpan.FromMilliseconds(meanTime.TotalMilliseconds / count);
            }
            return result;
        }

        public static TimeSpan AverageTotalSolvedTime(IRepositoryEmergencyCall repositoryEmergencyCalls, EmergencyCall.AssignModes oneMode = EmergencyCall.AssignModes.None, String nameOfMobile = null)
        {
            TimeSpan meanTime = new TimeSpan();
            EmergencyCall[] calls = repositoryEmergencyCalls.GetEmergencyCalls();
            int count = 0;
            foreach (var call in calls)
            {
                if (call.Solved)
                {
                    if (oneMode == EmergencyCall.AssignModes.None && nameOfMobile == null)
                    {
                        meanTime = meanTime + (call.SolvedDate - call.CreationDate);
                        count++;
                    }
                    else
                    {
                        if (call.AssignMode == oneMode && nameOfMobile == null)
                        {
                            meanTime = meanTime + (call.SolvedDate - call.CreationDate);
                            count++;
                        }
                        if (oneMode == EmergencyCall.AssignModes.None && call.Mobile.Name == nameOfMobile)
                        {
                            meanTime = meanTime + (call.SolvedDate - call.CreationDate);
                            count++;
                        }
                        if (call.AssignMode == oneMode && nameOfMobile != null && call.Mobile.Name == nameOfMobile)
                        {
                            meanTime = meanTime + (call.SolvedDate - call.CreationDate);
                            count++;
                        }
                    }
                }
            }
            TimeSpan result = new TimeSpan(0, 0, 0);
            if (count != 0)
            {
                result = TimeSpan.FromMilliseconds(meanTime.TotalMilliseconds / count);
            }
            return result;
        }
    }
}
