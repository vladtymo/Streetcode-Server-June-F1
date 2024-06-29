namespace Streetcode.BLL.DTO.Streetcode.TextContent
{
    public class RelatedTermDTO
    {
        public int Id { get; set; }
        public string Word { get; set; } = string.Empty;
        public int TermId { get; set; }
        public TermDTO? Term { get; set; }
    }
}
