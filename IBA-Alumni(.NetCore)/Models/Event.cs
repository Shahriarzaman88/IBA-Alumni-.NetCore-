namespace IBA_Alumni_.NetCore_.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string EventHeading { get; set; } = string.Empty;

        public string EventDescription { get; set; } = string.Empty;
        public string EventLocation { get; set; } = string.Empty;
        public DateTime? EventDate { get; set; } 

        public DateTime? EventTime { get; set; }


    }
}
