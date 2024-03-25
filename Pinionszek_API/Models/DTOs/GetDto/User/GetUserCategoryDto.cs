namespace Pinionszek_API.Models.DTOs.GetDto.User
{
    public class GetUserCategoryDto
    {
        public int IdDetailedCategory { get; set; }
        public string Name { get; set; }
        public GetUserGeneralCategoryDto GeneralCategory { get; set; }
    }
}
