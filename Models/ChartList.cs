namespace VolgaIT.Models
{
    public class ChartList
    {
        public string type { get; set; }
        public DataLabel data { get; set; }
    }

    public class DataLabel
    {
        public List<String> labels { get; set; }
        public List<ChartData> datasets { get; set; }
    }

    public class ChartData
    {
        public List<Int32> data { get; set; }
        public string borderColor { get; set; }
        public bool fill { get; set; }
    }
}