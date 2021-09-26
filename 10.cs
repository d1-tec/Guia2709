using System;
using System.Collections.Generic;
using BusinessLogic;
using Domain;
using Exceptions;
using Utils;
using System.Windows.Forms;
using System.Linq;

namespace Controllers
{
    public class CaseController
    {
        private CaseLogic _logic;
        private MobileLogic _logicMobile;

        public CaseController()
        {
            _logic = CaseLogic.GetInstance();
            _logicMobile = MobileLogic.GetInstance();
        }

        public void AddNewCall(string description, string direction, double latitude,double longitude,string urgency,DateTime date)
        {
            Location location = new Location(latitude, longitude);
            Urgency urgencyValue = ConvertStringToUrgency(urgency); 
            EmergencyCall call = EmergencyCall.NowTimeEmergencyCallInstance(location, urgencyValue, direction, description);
            call.Date = date;
            Case newCase = new Case(call);
            _logic.Add(newCase);
        }

        private Urgency ConvertStringToUrgency(string urgency)
        {
            switch (urgency)
            {
                case Constant.PRIORITY_LOW_STRING:
                    return Urgency.Low;
                case Constant.PRIORITY_MEDIUM_STRING:
                    return Urgency.Medium;
                case Constant.PRIORITY_HIGH_STRING:
                    return Urgency.High;
            }
            return Urgency.Low;
        }

        public void LoadCases(ListBox list)
        {
            IEnumerable<Case> cases = _logic.GetUnassignedAndAssignedCases();
            foreach (Case case1 in cases)
            {
                list.Items.Add(case1);
            }
        }

        public void LoadCasesView(ListView list)
        {
            IEnumerable<Case> cases = _logic.GetAllCases();
            if (cases.Count<Case>() == 0)
            {
                throw new EmptyMobileListException(ErrorMessage.UI_MOBILE_LIST_EMPTY);
            }
            foreach (Case case1 in cases)
            {
                string urgency = "";
                string callPosition = "(" + case1.Call.Position.Latitude.ToString() + "," + case1.Call.Position.Longitude.ToString() + ")";
                   
                    if (case1.Call.UrgencyLevel == Urgency.Low)
                        urgency = "BAJA";
                    if (case1.Call.UrgencyLevel == Urgency.Medium)
                        urgency = "MEDIA";
                    if (case1.Call.UrgencyLevel == Urgency.High)
                        urgency = "ALTA";

                ListViewItem listItem = new ListViewItem(case1.State.ToString());
                listItem.SubItems.Add(urgency);
                listItem.SubItems.Add(callPosition);
                listItem.SubItems.Add(case1.Call.Date.ToString());

                if (case1.State == CaseState.Assigned)
                {
                    listItem.SubItems.Add(case1.AssignedMobile.Name);
                    listItem.SubItems.Add(case1.AssignedMobile.Position.Latitude.ToString());
                    listItem.SubItems.Add(case1.AssignDate.ToString(Constant.DATE_FORMAT));
                }
                    
                list.Items.Add(listItem);

            }
        }

        public void LoadAssign(ComboBox comboBox)
        {
            comboBox.DataSource = Enum.GetValues(typeof(AssignType));
        }

        public void CaseResolved(object selectedObject)
        {
            if (!(selectedObject is Case))
            {
                throw new NotExpectedObjectType(ErrorMessage.UI_SELECTED_CASE_EXPECTED);
            }
            Case caseResolved = selectedObject as Case;
            _logic.ResolveCase(caseResolved);
        }

        public void selectAssignType(object selectedItem)
        {
            if((int)selectedItem == 1)
                AssignmentLogicSelector.SelectDefaultAssignment();
            if ((int)selectedItem == 2)
                AssignmentLogicSelector.SelectWaitingTimeAssignment();
            if ((int)selectedItem == 3)
                AssignmentLogicSelector.SelectCasesAttendedAssignment();
        }

        public void selectTimeType(ListBox listBox, int selectedItemTiempo, int selectedTimePor)
        {
            IEnumerable<Mobile> mobiles = _logicMobile.GetAllMobiles();

            if (selectedItemTiempo == 0) //Tiempo medio de asignación
            {
                if(selectedTimePor == 0) //General
                    listBox.Items.Add(_logic.GetAverageAssignationTime() + " Min");

                if (selectedTimePor == 1) //Por Mobile
                    foreach (Mobile mobile in mobiles)
                    {
                            double total = _logic.GetAverageAssignationTimeByMobile(mobile);
                            listBox.Items.Add(mobile.Name + " : " + total + " Min");  
                    }

                if (selectedTimePor == 2) //Por Asignación
                {
                    listBox.Items.Add("Por Defecto : " + _logic.GetAverageAssignationTimeByAssignationType(AssignType.Default) + " Min");
                    listBox.Items.Add("Tiempo de espera : " + _logic.GetAverageAssignationTimeByAssignationType(AssignType.WaitTime) + " Min");
                    listBox.Items.Add("Cantidad de Casos : " + _logic.GetAverageAssignationTimeByAssignationType(AssignType.NumberOfCases) + "Min");
                }
            }

            if (selectedItemTiempo == 1) //Tiempo medio de resolución
            {
                if (selectedTimePor == 0) //General
                    listBox.Items.Add(_logic.GetAverageResolutionTime() + " Min");
                if (selectedTimePor == 1) //Por Mobile
                {
                    foreach (Mobile mobile in mobiles)
                    {
                        double total = _logic.GetAverageResolutionTimeByMobile(mobile);
                        listBox.Items.Add(mobile.Name + " : " + total + " Min");
                    }
                }
                if (selectedTimePor == 2) //Por Asignación
                {
                    listBox.Items.Add("Por Defecto : " + _logic.GetAverageResolutionTimeByAssignationType(AssignType.Default) + " Min");
                    listBox.Items.Add("Tiempo de espera : " + _logic.GetAverageResolutionTimeByAssignationType(AssignType.WaitTime) + " Min");
                    listBox.Items.Add("Cantidad de Casos : " + _logic.GetAverageResolutionTimeByAssignationType(AssignType.NumberOfCases) + " Min");
                }
            }

            if (selectedItemTiempo == 2) //tiempo medio total de asistencia
            {
                if (selectedTimePor == 0) //General
                    listBox.Items.Add(_logic.GetAverageAssistanceTime() + " Min");

                if (selectedTimePor == 1) //Por mobile
                    foreach (Mobile mobile in mobiles)
                    {
                            double total = _logic.GetAverageAssistanceTimeByMobile(mobile);
                            listBox.Items.Add(mobile.Name + ":" + total + " Min");
                    }

                if (selectedTimePor == 2) //Por Asignación
                {
                    listBox.Items.Add("Por Defecto : " + _logic.GetAverageAssistanceTimeByAssignationType(AssignType.Default) + " Min");
                    listBox.Items.Add("Tiempo de espera : " + _logic.GetAverageAssistanceTimeByAssignationType(AssignType.WaitTime) + " Min");
                    listBox.Items.Add("Cantidad de Casos : " + _logic.GetAverageAssistanceTimeByAssignationType(AssignType.NumberOfCases) + " Min");
                }
            }
        }
    }
}

