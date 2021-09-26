using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RepositoryMobileUnitWithList : IRepositoryMobileUnit
    {
        private List<MobileUnit> listOfMobileUnits;

        public RepositoryMobileUnitWithList()
        {
            listOfMobileUnits = new List<MobileUnit>();
        }

        public void Add(MobileUnit mobileUnit)
        {
            if(listOfMobileUnits.Any(x => x.Name == mobileUnit.Name))
                throw new InvalidAddMobileUnitException();
            listOfMobileUnits.Add(mobileUnit);
        }

        public MobileUnit GetMobile(String name)
        {
            return listOfMobileUnits.FirstOrDefault(x => x.Name == name);
        }

        public void Delete(String name)
        {
            listOfMobileUnits.Remove(listOfMobileUnits.FirstOrDefault(x => x.Name == name));
        }

        public bool Exists(String name)
        {
            return listOfMobileUnits.Any(x => x.Name == name);
        }

       public MobileUnit GetNearestAvailable(Location location, List<MobileUnit> list = null)
        {
            if(list == null)
            {
                list = this.listOfMobileUnits;
            }
            double distance = Double.MaxValue;
            MobileUnit nearestMobile = null;
            foreach(var mob in list)
            {
                if(mob.Available)
                { 
                   double thisDistance = location.CalculateDistance(mob.Location);
                    if (thisDistance < distance)
                    {
                        distance = thisDistance;
                        nearestMobile = mob;
                    }
                }
            }
            return nearestMobile;
        }

        ////ver
        //public MobileUnit GetNearestAvailable(Location location, List<MobileUnit> list)
        //{
        //    double distance = Double.MaxValue;
        //    MobileUnit nearestMobile = null;
        //    foreach (var mob in list)
        //    {
        //        if (mob.Available)
        //        {
        //            double thisDistance = location.CalculateDistance(mob.Location);
        //            if (thisDistance < distance)
        //            {
        //                distance = thisDistance;
        //                nearestMobile = mob;
        //            }
        //        }
        //    }
        //    return nearestMobile;
        //}

        public MobileUnit GetFewestEmergencyCallsSolved(Location location, EmergencyCall[] listOfEmergencyCalls, MobileUnit[] aListOfMobileUnits)
        {
            List<MobileUnit> list = new List<MobileUnit>();
            int min = Int32.MaxValue;
            foreach (var mob in aListOfMobileUnits) {
                int number=0;
                if (mob.Available)
                {
                    foreach (var call in listOfEmergencyCalls) {
                        if (call.Mobile.Name.Equals(mob.Name) && call.Solved)
                        {
                            TimeSpan result = DateTime.Now.Subtract(call.SolvedDate);
                            double hours = result.TotalHours;
                            if (hours<=12)
                            {
                                number++;
                            }
                        }
                    }
                    if (number < min)
                    {
                        list.Clear();
                        list.Add(mob);
                        min = number;
                    }
                    else if (number == min) {
                        list.Add(mob);
                    }
                }
            }
            if (list.Count==1)
            {
                return list.First();
            }
            return GetNearestAvailable(location, list);
        }

        public int Count()
        {
            return listOfMobileUnits.Count;
        }

        public void Clear()
        {
            listOfMobileUnits.Clear();
        }

        public String[] GetNames()
        {
            String[] arrayOfNames = new String[listOfMobileUnits.Count];
            int i = 0;
            foreach(var mobile in listOfMobileUnits){
                arrayOfNames[i] = mobile.Name;
                i++;
            }
            return arrayOfNames;
        }

        public void ModifyMobileUnitByName(String oldName, String newName, String newPlate, String newDescription)
        {
            this.GetMobile(oldName).SetPlate(newPlate);
            this.GetMobile(oldName).Description = newDescription;
            this.GetMobile(oldName).Name = newName;
        }

        public void SetCoordinatesToMobile(String name, double longitude, double latitude)
        {
            this.GetMobile(name).Location.SetCoordinates(longitude, latitude);
        }

        public Location GetLocation(String name)
        {
            return this.GetMobile(name).Location;
        }

        public MobileUnit[] GetMobileUnits()
        {
            return listOfMobileUnits.ToArray();
        }

        public bool IsMoreThanOneAvailable()
        {
            int number = 0;
            foreach (var mob in this.listOfMobileUnits)
            {
                if (mob.Available)
                {
                    number++;
                }
            }
            return number > 1;
        }
        
        public void SetMobileAvailable(string nameOfMobile)
        {
            GetMobile(nameOfMobile).Available = true;
        }

        public void SetMobileNotAvailable(string nameOfMobile)
        {
            GetMobile(nameOfMobile).Available = false;
        }        
    }
}
