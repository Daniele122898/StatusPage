using StatusPageAPI.Models.Enums;

namespace StatusPageAPI.Models
{
    public class SpecialNotice
    {
        public Status Status { get; set; }
        public string Notice { get; set; }

        public SpecialNotice(Status status, string notice)
        {
            Status = status;
            Notice = notice;
        }
    }
}