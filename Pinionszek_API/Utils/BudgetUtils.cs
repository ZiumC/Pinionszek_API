using Pinionszek_API.Models.DatabaseModel;

namespace Pinionszek_API.Utils
{
    public enum GeneralCatEnum
    {
        NEEDS,
        WANTS,
        SAVINGS,
        NO_CATEGORY
    }

    public enum PaymentColEnum
    {
        CHARGE,
        REFOUND
    }

    public class BudgetUtils
    {
        private readonly IConfiguration _config;

        public BudgetUtils(IConfiguration config)
        {
            _config = config;
        }

        public decimal GetPaymentsSum(GeneralCatEnum cat, PaymentColEnum col, IEnumerable<Payment> payments)
        {
            int idGeneralCategory = 0;
            bool isParsed = false;

            string appPropertiesPath = "Application:Properties:GeneralCatytegory";
            switch (cat)
            {
                case GeneralCatEnum.NEEDS:
                    string idNeeds = _config[appPropertiesPath + ":IdNeeds"];
                    isParsed = int.TryParse(idNeeds, out idGeneralCategory);
                    break;

                case GeneralCatEnum.WANTS:
                    string idWants = _config[appPropertiesPath + ":IdWants"];
                    isParsed = int.TryParse(idWants, out idGeneralCategory);
                    break;

                case GeneralCatEnum.SAVINGS:
                    string idSavings = _config[appPropertiesPath + ":IdSavings"];
                    isParsed = int.TryParse(idSavings, out idGeneralCategory);
                    break;

                case GeneralCatEnum.NO_CATEGORY:
                    isParsed = true;
                    break;

                default:
                    throw new Exception($"Given category '{cat}' not found in expected values");
            }

            if (!isParsed)
            {
                throw new Exception($"IConfiguration has invalid values to parse id general category. " +
                    $"IConfiguration path: {appPropertiesPath}:{cat}");
            }

            var paymentsData = payments;
            if (cat != GeneralCatEnum.NO_CATEGORY)
            {
                paymentsData = payments
                .Where(p => p.DetailedCategory.GeneralCategory.IsDefault &&
                    p.DetailedCategory.IdGeneralCategory == idGeneralCategory);
            }

            switch (col)
            {
                case PaymentColEnum.CHARGE:
                    return paymentsData.Sum(pbc => pbc.Charge);

                case PaymentColEnum.REFOUND:
                    return paymentsData.Sum(pbc => pbc.Refund);

                default:
                    throw new Exception($"Given collumn '{col}' not found in expected values");
            }
        }

    }
}
