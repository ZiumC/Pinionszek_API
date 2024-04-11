using System.ComponentModel.DataAnnotations;

namespace Pinionszek_API.Utils.Attributions
{
    public class BeforeTodayAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            DateTime dateValue = Convert.ToDateTime(value);
            return dateValue >= DateTime.Now;
        }
    }
}
