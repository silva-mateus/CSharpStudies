using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        //you may rename the parameters of this method,
        //but do not change their order or type
        public static string BuildFilePathFrom_Refactored(
            DateTime date, Extension extension)
        {
            var day = date.Day;
            var dayOfWeek = date.DayOfWeek;
            var month = date.Month;
            var year = date.Year;
            
            var ext = extension.ToString().ToLower();
            var filePath = $"{dayOfWeek}_{day}_{month}_{year}.{ext}";
            
            return filePath;
            
        }
        
        //do not modify this method!
        public static string BuildFilePathFrom(DateTime d, Extension ex)
        {
            var d1 = d.Day;
            var d2 = d.DayOfWeek;
            var m = d.Month;
            var y = d.Year;
        
            var format = ex.ToString().ToLower();
            var format2 = $"{d2}_{d1}_{m}_{y}.{format}";
        
            return format2;
        }
    }
    
        
    //do not modify this enum!
    public enum Extension
    {
        Txt,
        Json
    }
}
