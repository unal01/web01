using CoreBuilder.Factories;
using CoreBuilder.Models;
using System.Threading.Tasks;

namespace CoreBuilder.Services
{
    public class SiteGenerationService
    {
        private readonly EducationSiteFactory _educationFactory;
        private readonly MarketingSiteFactory _marketingFactory;
        private readonly GovernmentSiteFactory _governmentFactory;
        private readonly ExamPrepSiteFactory _examPrepFactory;

        public SiteGenerationService(
            EducationSiteFactory educationFactory, 
            MarketingSiteFactory marketingFactory,
            GovernmentSiteFactory governmentFactory,
            ExamPrepSiteFactory examPrepFactory)
        {
            _educationFactory = educationFactory;
            _marketingFactory = marketingFactory;
            _governmentFactory = governmentFactory;
            _examPrepFactory = examPrepFactory;
        }

        public async Task GenerateDefaultContent(Tenant tenant)
        {
            switch (tenant.Category)
            {
                case "Education":
                    await _educationFactory.CreateDefaultPagesAsync(tenant.Id);
                    break;

                case "Marketing":
                    await _marketingFactory.CreateDefaultPagesAsync(tenant.Id);
                    break;

                case "Government":
                    await _governmentFactory.CreateDefaultPagesAsync(tenant.Id);
                    break;

                case "ExamPrep":
                    await _examPrepFactory.CreateDefaultPagesAsync(tenant.Id);
                    break;

                default:
                    // Tanımsız kategori
                    break;
            }
        }
    }
}
