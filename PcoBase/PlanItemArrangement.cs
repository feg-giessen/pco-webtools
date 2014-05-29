namespace PcoBase
{
    public class PlanItemArrangement
    {
        public int id { get; set; }

        public string name { get; set; }

        public object bpm { get; set; }

        public bool has_chords { get; set; }

        public bool has_chord_chart { get; set; }

        public int length { get; set; }

        public string sequence_to_s { get; set; }

        public string meter { get; set; }
    }
}