using Pinionszek_API.Models.DTOs.GetDto.Payments;

namespace Pinionszek_API.Models.DTOs.GetDto.User
{
    public class GetUserCategoryDto
    {
        public int IdDetailedCategory { get; set; }
        public string Name { get; set; }
        public GetGeneralCategoryDto GeneralCategory { get; set; }
    }
}
