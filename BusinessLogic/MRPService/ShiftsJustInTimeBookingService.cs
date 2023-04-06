using BusinessLogic.Models;
using DataAcess.Entities;

namespace BusinessLogic.MRPService
{
    //public class ShiftsJustInTimeBookingService
    //{
    //    private readonly DateTime _fromDate;

    //    private readonly Dictionary<string, ManufactorDay> _occupiedManufactoringDays;
    //    private readonly DepartmentEntity _department;

    //    public ShiftsJustInTimeBookingService(DateTime fromDate, DepartmentEntity department)
    //    {
    //        _fromDate = fromDate;
    //        _occupiedManufactoringDays= new();
    //        _department= department;
    //    }

    //    public Shift GetAviableShift(DateTime actualDay)
    //    {
    //        if(_occupiedManufactoringDays.TryGetValue(actualDay.ToShortDateString(), out var manufactorDay))
    //        {
    //            if (manufactorDay.IsFullyOccupied)
    //            {

    //            }
    //        }
    //        for (int i = 0; i < 7; i++)
    //        {
    //            if (workWeekDays.Any(x => x == actualDay.DayOfWeek))
    //            {
    //                return actualDay;
    //            }
    //            actualDay = actualDay.AddDays(-1);
    //        }
    //        throw new Exception("Nieco je zle");
    //    }

    //    public void BookAviableShift(int numberOfMinutes)
    //    {
    //        if(shift.ShiftDurationInMinutes)
    //    }

    //    public void 
    //}
}
