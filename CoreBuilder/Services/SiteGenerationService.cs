using CoreBuilder.Factories;
using CoreBuilder.Models;
using System.Threading.Tasks;

namespace CoreBuilder.Services
{
    public class SiteGenerationService
    {
        private readonly EducationSiteFactory _educationFactory;
        private readonly MarketingSiteFactory _marketingFactory;

        public SiteGenerationService(EducationSiteFactory educationFactory, MarketingSiteFactory marketingFactory)
        {
            _educationFactory = educationFactory;
            _marketingFactory = marketingFactory;
        }

        public async Task GenerateDefaultContent(Tenant tenant)
        {
            // DÜZELTİLEN KISIM: tenant.CategoryType yerine tenant.Category kullanıyoruz.
            switch (tenant.Category)
            {
                case "Education":
                    // Eğer Factory metodunuzun adı farklıysa (örn: Create, Build) burayı ona göre düzeltin.
                    // Genelde "CreateDefaultPagesAsync" veya benzeri bir isim kullanmış olabilirsiniz.
                    // Eğer hata alırsanız metodun içini kontrol edin.
                    await _educationFactory.CreateDefaultPagesAsync(tenant.Id);
                    break;

                case "Marketing":
                    await _marketingFactory.CreateDefaultPagesAsync(tenant.Id);
                    break;

                default:
                    // Tanımsız kategori
                    break;
            }
        }
    }
}
