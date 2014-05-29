using Newtonsoft.Json;

namespace PcoBase
{
    public class PlanItemArrangement
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("bpm")]
        public object Bpm { get; set; }

        [JsonProperty("has_chords")]
        public bool HasChords { get; set; }

        [JsonProperty("has_chord_chart")]
        public bool HasChordChart { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("sequence_to_s")]
        public string SequenceToS { get; set; }

        [JsonProperty("meter")]
        public string Meter { get; set; }
    }
}