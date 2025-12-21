using CoreBuilder.Data;
using CoreBuilder.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBuilder.Factories
{
    /// <summary>
    /// YÖK.gov.tr tarz? resmi kurum siteleri için içerik fabrikas?
    /// </summary>
    public class GovernmentSiteFactory
    {
        private readonly AppDbContext _context;

        public GovernmentSiteFactory(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateDefaultPagesAsync(Guid tenantId)
        {
            var pages = new List<Page>
            {
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Anasayfa",
                    Slug = "home",
                    Content = @"
                        <div class='hero-section'>
                            <h1>Yüksekö?retim Bilgi Sistemi</h1>
                            <p class='lead'>Türkiye'deki tüm üniversiteler ve yüksekö?retim kurumlar? hakk?nda güncel bilgiler</p>
                        </div>
                        <div class='row mt-4'>
                            <div class='col-md-4'>
                                <div class='card'>
                                    <div class='card-body'>
                                        <h3>?? ?statistikler</h3>
                                        <p>209 Üniversite</p>
                                        <p>8.5M Ö?renci</p>
                                        <p>185K Akademisyen</p>
                                    </div>
                                </div>
                            </div>
                            <div class='col-md-4'>
                                <div class='card'>
                                    <div class='card-body'>
                                        <h3>??? Üniversiteler</h3>
                                        <p>Devlet Üniversiteleri</p>
                                        <p>Vak?f Üniversiteleri</p>
                                        <p>Yurtd??? Programlar?</p>
                                    </div>
                                </div>
                            </div>
                            <div class='col-md-4'>
                                <div class='card'>
                                    <div class='card-body'>
                                        <h3>?? Mevzuat</h3>
                                        <p>Kanunlar ve Yönetmelikler</p>
                                        <p>Genelgeler</p>
                                        <p>Yönergeler</p>
                                    </div>
                                </div>
                            </div>
                        </div>",
                    IsPublished = true
                },
                
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Kurumsal",
                    Slug = "kurumsal",
                    Content = @"
                        <h1>Kurumsal Bilgiler</h1>
                        <h2>Misyonumuz</h2>
                        <p>Yüksekö?retim sisteminin planlanmas?, organizasyonu, yönetimi ve denetimi konular?nda koordinasyonu sa?lamak.</p>
                        
                        <h2>Vizyonumuz</h2>
                        <p>Dünya standartlar?nda kaliteli, eri?ilebilir ve sürdürülebilir bir yüksekö?retim sistemi olu?turmak.</p>
                        
                        <h2>Tarihçe</h2>
                        <p>1981 y?l?nda 2547 say?l? Yüksekö?retim Kanunu ile kurulmu?tur.</p>
                        
                        <h2>Organizasyon ?emas?</h2>
                        <ul>
                            <li>Ba?kanl?k</li>
                            <li>Genel Sekreterlik</li>
                            <li>Daire Ba?kanl?klar?</li>
                            <li>Dan??ma Kurullar?</li>
                        </ul>",
                    IsPublished = true
                },
                
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Üniversiteler",
                    Slug = "universiteler",
                    Content = @"
                        <h1>Türkiye'deki Üniversiteler</h1>
                        
                        <div class='alert alert-info'>
                            <strong>Toplam:</strong> 209 Üniversite (129 Devlet + 80 Vak?f)
                        </div>
                        
                        <h2>Devlet Üniversiteleri</h2>
                        <table class='table table-striped'>
                            <thead>
                                <tr>
                                    <th>Üniversite Ad?</th>
                                    <th>?ehir</th>
                                    <th>Kurulu?</th>
                                    <th>Ö?renci</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>?stanbul Üniversitesi</td>
                                    <td>?stanbul</td>
                                    <td>1453</td>
                                    <td>120.000</td>
                                </tr>
                                <tr>
                                    <td>Ankara Üniversitesi</td>
                                    <td>Ankara</td>
                                    <td>1946</td>
                                    <td>75.000</td>
                                </tr>
                                <tr>
                                    <td>Ege Üniversitesi</td>
                                    <td>?zmir</td>
                                    <td>1955</td>
                                    <td>68.000</td>
                                </tr>
                                <tr>
                                    <td>Hacettepe Üniversitesi</td>
                                    <td>Ankara</td>
                                    <td>1967</td>
                                    <td>52.000</td>
                                </tr>
                                <tr>
                                    <td>ODTÜ</td>
                                    <td>Ankara</td>
                                    <td>1956</td>
                                    <td>32.000</td>
                                </tr>
                            </tbody>
                        </table>
                        
                        <h2>Vak?f Üniversiteleri</h2>
                        <table class='table table-striped'>
                            <thead>
                                <tr>
                                    <th>Üniversite Ad?</th>
                                    <th>?ehir</th>
                                    <th>Kurulu?</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Koç Üniversitesi</td>
                                    <td>?stanbul</td>
                                    <td>1993</td>
                                </tr>
                                <tr>
                                    <td>Sabanc? Üniversitesi</td>
                                    <td>?stanbul</td>
                                    <td>1994</td>
                                </tr>
                                <tr>
                                    <td>Bilkent Üniversitesi</td>
                                    <td>Ankara</td>
                                    <td>1984</td>
                                </tr>
                            </tbody>
                        </table>",
                    IsPublished = true
                },
                
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "?statistikler",
                    Slug = "istatistikler",
                    Content = @"
                        <h1>Yüksekö?retim ?statistikleri</h1>
                        
                        <div class='row'>
                            <div class='col-md-6'>
                                <div class='card mb-4'>
                                    <div class='card-header bg-primary text-white'>
                                        <h4>Ö?renci Say?lar? (2024)</h4>
                                    </div>
                                    <div class='card-body'>
                                        <ul class='list-group list-group-flush'>
                                            <li class='list-group-item d-flex justify-content-between'>
                                                <span>Önlisans</span>
                                                <strong>2.850.000</strong>
                                            </li>
                                            <li class='list-group-item d-flex justify-content-between'>
                                                <span>Lisans</span>
                                                <strong>4.920.000</strong>
                                            </li>
                                            <li class='list-group-item d-flex justify-content-between'>
                                                <span>Yüksek Lisans</span>
                                                <strong>580.000</strong>
                                            </li>
                                            <li class='list-group-item d-flex justify-content-between'>
                                                <span>Doktora</span>
                                                <strong>150.000</strong>
                                            </li>
                                            <li class='list-group-item d-flex justify-content-between bg-light'>
                                                <span><strong>TOPLAM</strong></span>
                                                <strong>8.500.000</strong>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            
                            <div class='col-md-6'>
                                <div class='card mb-4'>
                                    <div class='card-header bg-success text-white'>
                                        <h4>Akademik Personel</h4>
                                    </div>
                                    <div class='card-body'>
                                        <ul class='list-group list-group-flush'>
                                            <li class='list-group-item d-flex justify-content-between'>
                                                <span>Profesör</span>
                                                <strong>28.500</strong>
                                            </li>
                                            <li class='list-group-item d-flex justify-content-between'>
                                                <span>Doçent</span>
                                                <strong>18.200</strong>
                                            </li>
                                            <li class='list-group-item d-flex justify-content-between'>
                                                <span>Dr. Ö?r. Üyesi</span>
                                                <strong>45.800</strong>
                                            </li>
                                            <li class='list-group-item d-flex justify-content-between'>
                                                <span>Ara?t?rma Görevlisi</span>
                                                <strong>92.500</strong>
                                            </li>
                                            <li class='list-group-item d-flex justify-content-between bg-light'>
                                                <span><strong>TOPLAM</strong></span>
                                                <strong>185.000</strong>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class='card'>
                            <div class='card-header bg-info text-white'>
                                <h4>Fakülte Da??l?m?</h4>
                            </div>
                            <div class='card-body'>
                                <table class='table'>
                                    <tr>
                                        <td>Mühendislik Fakülteleri</td>
                                        <td class='text-end'><strong>1.250.000 Ö?renci</strong></td>
                                    </tr>
                                    <tr>
                                        <td>??BF (?ktisat ??letme)</td>
                                        <td class='text-end'><strong>1.180.000 Ö?renci</strong></td>
                                    </tr>
                                    <tr>
                                        <td>T?p Fakülteleri</td>
                                        <td class='text-end'><strong>95.000 Ö?renci</strong></td>
                                    </tr>
                                    <tr>
                                        <td>Hukuk Fakülteleri</td>
                                        <td class='text-end'><strong>145.000 Ö?renci</strong></td>
                                    </tr>
                                    <tr>
                                        <td>E?itim Fakülteleri</td>
                                        <td class='text-end'><strong>420.000 Ö?renci</strong></td>
                                    </tr>
                                </table>
                            </div>
                        </div>",
                    IsPublished = true
                },
                
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "Mevzuat",
                    Slug = "mevzuat",
                    Content = @"
                        <h1>Yasal Mevzuat</h1>
                        
                        <h2>?? Kanunlar</h2>
                        <div class='list-group mb-4'>
                            <a href='#' class='list-group-item list-group-item-action'>
                                <div class='d-flex w-100 justify-content-between'>
                                    <h5 class='mb-1'>2547 Say?l? Yüksekö?retim Kanunu</h5>
                                    <small>04.11.1981</small>
                                </div>
                                <p class='mb-1'>Yüksekö?retim kurumlar?n?n kurulu?, i?leyi? ve görevlerini düzenleyen temel kanun.</p>
                            </a>
                            <a href='#' class='list-group-item list-group-item-action'>
                                <div class='d-flex w-100 justify-content-between'>
                                    <h5 class='mb-1'>2914 Say?l? Yüksek Ö?retim Personel Kanunu</h5>
                                    <small>12.10.1983</small>
                                </div>
                                <p class='mb-1'>Yüksekö?retim kurumlar? personel hükümlerini içerir.</p>
                            </a>
                        </div>
                        
                        <h2>?? Yönetmelikler</h2>
                        <ul class='list-group mb-4'>
                            <li class='list-group-item'>Lisansüstü E?itim ve Ö?retim Yönetmeli?i</li>
                            <li class='list-group-item'>Önlisans ve Lisans Düzeyindeki Programlar Aras? Geçi? Yönetmeli?i</li>
                            <li class='list-group-item'>Yüksekö?retim Kurumlar? Ö?renci Disiplin Yönetmeli?i</li>
                            <li class='list-group-item'>Ö?retim Üyesi Yeti?tirme Program? Yönetmeli?i</li>
                        </ul>
                        
                        <h2>?? Genelgeler</h2>
                        <div class='table-responsive'>
                            <table class='table table-hover'>
                                <thead class='table-light'>
                                    <tr>
                                        <th>Tarih</th>
                                        <th>Konu</th>
                                        <th>Durum</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>15.01.2024</td>
                                        <td>2024 Bahar Dönemi Aç?k ve Uzaktan Ö?retim Hakk?nda</td>
                                        <td><span class='badge bg-success'>Yürürlükte</span></td>
                                    </tr>
                                    <tr>
                                        <td>20.12.2023</td>
                                        <td>Yabanc? Uyruklu Ö?renci Kay?tlar?</td>
                                        <td><span class='badge bg-success'>Yürürlükte</span></td>
                                    </tr>
                                    <tr>
                                        <td>10.11.2023</td>
                                        <td>Akademik Takvim De?i?iklikleri</td>
                                        <td><span class='badge bg-warning'>Güncellendi</span></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>",
                    IsPublished = true
                },
                
                new Page
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Title = "?leti?im",
                    Slug = "iletisim",
                    Content = @"
                        <h1>?leti?im Bilgileri</h1>
                        
                        <div class='row'>
                            <div class='col-md-6'>
                                <div class='card'>
                                    <div class='card-body'>
                                        <h4>?? Adres</h4>
                                        <p>
                                            Yüksekö?retim Kurulu Ba?kanl???<br>
                                            Bilkent Plaza B1 Blok<br>
                                            06800 Bilkent / ANKARA
                                        </p>
                                        
                                        <h4>?? Telefon</h4>
                                        <p>0 (312) 298 70 00</p>
                                        
                                        <h4>?? Faks</h4>
                                        <p>0 (312) 266 47 59</p>
                                        
                                        <h4>?? E-posta</h4>
                                        <p>iletisim@yok.gov.tr</p>
                                    </div>
                                </div>
                            </div>
                            
                            <div class='col-md-6'>
                                <div class='card'>
                                    <div class='card-body'>
                                        <h4>?? Çal??ma Saatleri</h4>
                                        <p>Hafta ?çi: 08:30 - 17:30<br>Cumartesi-Pazar: Kapal?</p>
                                        
                                        <h4>?? Ba?vuru Sistemleri</h4>
                                        <ul>
                                            <li>e-Ba?vuru Sistemi</li>
                                            <li>C?MER (?leti?im Merkezi)</li>
                                            <li>Bilgi Edinme Hakk?</li>
                                        </ul>
                                        
                                        <h4>?? Sosyal Medya</h4>
                                        <p>
                                            <a href='#' class='btn btn-sm btn-primary me-2'>Twitter</a>
                                            <a href='#' class='btn btn-sm btn-info me-2'>LinkedIn</a>
                                            <a href='#' class='btn btn-sm btn-danger'>YouTube</a>
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>",
                    IsPublished = true
                }
            };

            _context.Pages.AddRange(pages);
            await _context.SaveChangesAsync();
        }
    }
}
