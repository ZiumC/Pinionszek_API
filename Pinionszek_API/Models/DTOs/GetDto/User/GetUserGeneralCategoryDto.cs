namespace Pinionszek_API.Models.DTOs.GetDto.User
{
    public class GetUserGeneralCategoryDto
    {
        public int IdGeneralCategory { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
