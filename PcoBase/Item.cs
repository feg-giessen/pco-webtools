using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class Item
    {
        [JsonProperty("id")]
		public int Id { get; set; }

        [JsonProperty("title")]
		public string Title { get; set; }

        [JsonProperty("sequence")]
		public int Sequence { get; set; }

        [JsonProperty("plan_id")]
		public int PlanId { get; set; }

        [JsonProperty("dom_id")]
		public string DomId { get; set; }

        [JsonProperty("type")]
		public string Type { get; set; }

        [JsonProperty("length")]
		public int Length { get; set; }

        [JsonProperty("length_formatted")]
		public string LengthFormatted { get; set; }

        [JsonProperty("detail")]
		public string Detail { get; set; }

        [JsonProperty("comments_count")]
		public int CommentsCount { get; set; }

        [JsonProperty("is_preservice")]
		public bool IsPreservice { get; set; }

        [JsonProperty("is_postservice")]
		public bool IsPostservice { get; set; }

        [JsonProperty("is_header")]
		public bool IsHeader { get; set; }

        [JsonProperty("plan_item_medias")]
		public List<object> PlanItemMedias { get; set; }

        [JsonProperty("plan_item_notes")]
		public List<ItemNote> PlanItemNotes { get; set; }

        [JsonProperty("plan_item_times")]
		public List<ItemTime> PlanItemTimes { get; set; }

        [JsonProperty("ccli_print_single")]
		public int CcliPrintSingle { get; set; }

        [JsonProperty("ccli_print_collected")]
		public int CcliPrintCollected { get; set; }

        [JsonProperty("ccli_screen")]
		public int CcliScreen { get; set; }

        [JsonProperty("ccli_custom_arrangement")]
		public int CcliCustomArrangement { get; set; }

        [JsonProperty("ccli_recorded")]
		public int CcliRecorded { get; set; }

        [JsonProperty("song_id")]
		public int? SongId { get; set; }

        [JsonProperty("arrangement_id")]
		public int? ArrangementId { get; set; }

        [JsonProperty("key_id")]
		public int? KeyId { get; set; }

        [JsonProperty("information")]
		public string Information { get; set; }

        [JsonProperty("description")]
		public string Description { get; set; }

        [JsonProperty("arrangement_sequence_to_s")]
		public string ArrangementSequenceToS { get; set; }

        [JsonProperty("attachments")]
		public List<Attachment> Attachments { get; set; }

        [JsonProperty("song")]
		public Song Song { get; set; }

        [JsonProperty("arrangement")]
		public PlanItemArrangement Arrangement { get; set; }

        [JsonProperty("key")]
		public Key Key { get; set; }

        [JsonProperty("music_stand_attachment_id")]
		public string MusicStandAttachmentId { get; set; }
    }
}