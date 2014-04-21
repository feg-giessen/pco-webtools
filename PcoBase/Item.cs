using System.Collections.Generic;

namespace PcoBase
{
    public class Item
    {
        public int id { get; set; }
        public string title { get; set; }
        public int sequence { get; set; }
        public int plan_id { get; set; }
        public string dom_id { get; set; }
        public string type { get; set; }
        public int length { get; set; }
        public string length_formatted { get; set; }
        public string detail { get; set; }
        public int comments_count { get; set; }
        public bool is_preservice { get; set; }
        public bool is_postservice { get; set; }
        public bool is_header { get; set; }
        public List<object> plan_item_medias { get; set; }
        public List<ItemNote> plan_item_notes { get; set; }
        public List<ItemTime> plan_item_times { get; set; }
        public int ccli_print_single { get; set; }
        public int ccli_print_collected { get; set; }
        public int ccli_screen { get; set; }
        public int ccli_custom_arrangement { get; set; }
        public int ccli_recorded { get; set; }
        public int? song_id { get; set; }
        public int? arrangement_id { get; set; }
        public int? key_id { get; set; }
        public string information { get; set; }
        public string description { get; set; }
        public string arrangement_sequence_to_s { get; set; }
        public List<Attachment> attachments { get; set; }
        public Song song { get; set; }
        public PlanItemArrangement arrangement { get; set; }
        public Key key { get; set; }
        public string music_stand_attachment_id { get; set; }
    }
}