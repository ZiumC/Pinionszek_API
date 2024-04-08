namespace Pinionszek_API.Models.DTOs.GetDto.Payments
{
    public class GetGeneralCategoryDto
    {
        public int IdGeneralCategory { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
