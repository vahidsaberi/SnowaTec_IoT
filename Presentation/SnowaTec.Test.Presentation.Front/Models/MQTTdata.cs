namespace SnowaTec.Test.Presentation.Front.Models
{
    public class MQTTdata
    {
        public DateTime Date { get; set; }

        public int MessageNumber { get; set; }

        public int Roll { get; set; }

        public int Pitch { get; set; }
        public string? Sensor { get; set; }
    }
}
