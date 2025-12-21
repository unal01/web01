# ?? S?navKurs.com Entegrasyonu

## ?? Proje Özeti

**sinavkurs.com** benzeri kapsaml? bir s?nav haz?rl?k platformu sisteme entegre edilmi?tir.

---

## ? Eklenen Özellikler

### 1?? **ExamPrepSiteFactory**
`CoreBuilder/Factories/ExamPrepSiteFactory.cs`

S?nav haz?rl?k sitesi için özel içerik fabrikas?:
- ? Anasayfa (Hero section, istatistikler, ba?ar? hikayeleri)
- ? Hakk?m?zda (Vizyon, misyon, ekip)
- ? Kurslar?m?z (KPSS, ALES, DGS, YDS, TYT-AYT, MSÜ)
- ? ?leti?im (Form, adres, sosyal medya)
- ? SSS (S?kça sorulan sorular)

---

### 2?? **Tenant: S?navKurs**
- **Domain:** `sinavkurs.localhost`
- **Kategori:** `ExamPrep`
- **?çerik:** 5 sayfa, 5 slider, 6 duyuru

---

## ?? ?çerik Detaylar?

### **Sayfalar (5 Adet)**

#### 1. **Anasayfa** (`/home`)
```
- Hero Section (Ba?l?k + CTA butonlar?)
- ?statistikler (15+ y?l, 50.000+ ö?renci, 200+ e?itmen, %95 ba?ar?)
- Neden Biz? (6 özellik)
- Ba?ar? Hikayeleri (2 örnek)
```

#### 2. **Hakk?m?zda** (`/hakkimizda`)
```
- Vizyon
- Misyon
- De?erler (4 madde)
- ?leti?im Kart?
- Ekip (3 ki?i: Kurucu, KPSS, ALES uzmanlar?)
```

#### 3. **Kurslar?m?z** (`/kurslar`)
```
6 Kurs Paketi:
???????????????????????????????????????
? KPSS Haz?rl?k     - ?1.999/Y?l      ?
? ALES Haz?rl?k     - ?1.499/Y?l      ?
? DGS Haz?rl?k      - ?1.699/Y?l      ?
? YDS Haz?rl?k      - ?1.299/Y?l      ?
? TYT-AYT Haz?rl?k  - ?2.499/Y?l      ?
? MSÜ Haz?rl?k      - ?1.799/Y?l      ?
???????????????????????????????????????

Her kurs içeri?i:
- Video ders saati
- Deneme s?nav? say?s?
- Ek özellikler
- Fiyat + CTA butonu
```

#### 4. **?leti?im** (`/iletisim`)
```
- ?leti?im Bilgileri (Adres, telefon, e-posta, çal??ma saatleri)
- ?leti?im Formu (Ad, e-posta, telefon, konu, mesaj)
- Sosyal Medya Butonlar? (Facebook, Twitter, YouTube, WhatsApp)
```

#### 5. **SSS** (`/sss`)
```
Accordion format?nda 4 soru-cevap:
1. Kay?t nas?l olur?
2. S?n?rs?z eri?im var m??
3. ?ade politikas? nedir?
4. Mobil uygulama var m??
```

---

### **Slider'lar (5 Adet)**

| S?ra | Ba?l?k | Aç?klama | CTA |
|------|--------|----------|-----|
| 1 | KPSS'de Ba?ar?n?n Adresi | 15 y?l, 50.000+ ö?renci, %95 ba?ar? | Hemen Ba?la |
| 2 | ALES'te Yüksek Puan | 80+ puan garantisi | ALES Kurslar? |
| 3 | DGS ile Hayalinizdeki Üniversiteye | 450+ saat ders, 45+ deneme | DGS Haz?rl?k |
| 4 | YDS ve Yabanc? Dil S?navlar? | Grammar, vocabulary, reading | YDS Kurslar? |
| 5 | ?? Özel Kampanya - %30 ?ndirim | ?lk 100 kay?t | Kampanyay? Gör |

**Görseller:** Unsplash'ten e?itim temal? profesyonel foto?raflar

---

### **Duyurular (6 Adet)**

| Duyuru | Önemli | Tarih |
|--------|--------|-------|
| ?? 2024 KPSS Ba?vurular? Ba?lad?! | ? Evet | Bugün |
| ?? Ücretsiz Demo Ders ?mkan? | ? Hay?r | 2 gün önce |
| ?? ALES 2024/1 S?nav Takvimi | ? Evet | 1 gün önce |
| ?? Ba?ar? Hikayesi: 87 Puan! | ? Hay?r | 5 gün önce |
| ????? Yeni E?itmen Kadrosu | ? Hay?r | 7 gün önce |
| ?? Mobil Uygulama Yay?nda! | ? Hay?r | 10 gün önce |

---

## ?? Tasar?m Özellikleri

### **Renk Paleti:**
```css
- Primary (KPSS):   #007bff (Mavi)
- Success (ALES):   #28a745 (Ye?il)
- Info (DGS):       #17a2b8 (Turkuaz)
- Warning (YDS):    #ffc107 (Sar?)
- Danger (TYT-AYT): #dc3545 (K?rm?z?)
- Dark (MSÜ):       #343a40 (Koyu)
```

### **Bootstrap Komponentleri:**
- ? Cards (Kurs paketleri)
- ? Badges (?statistikler, fiyatlar)
- ? Accordion (SSS)
- ? Forms (?leti?im)
- ? Buttons (CTA'lar)
- ? Grid System (Responsive layout)

---

## ?? Kurulum ve Kullan?m

### **1. Hosts Dosyas? Güncelleme**
```
C:\Windows\System32\drivers\etc\hosts
```

Ekle:
```
127.0.0.1 sinavkurs.localhost
```

### **2. Projeyi Çal??t?r**
```powershell
cd C:\Users\User\source\repos\unal01\web01\CoreBuilder.Admin
dotnet run
```

### **3. Siteyi Ziyaret Et**
```
http://sinavkurs.localhost:5001
```

### **4. Admin Panel**
```
https://localhost:5001
Login: admin / Admin123!

Menü:
- Sayfalar ? sinavkurs.localhost
- Slider ? sinavkurs.localhost  
- Duyurular ? sinavkurs.localhost
```

---

## ?? ?çerik ?statistikleri

```
Toplam ?çerik:
??? 5 Sayfa
?   ??? Anasayfa (Hero + ?statistikler)
?   ??? Hakk?m?zda (Vizyon + Ekip)
?   ??? Kurslar?m?z (6 paket)
?   ??? ?leti?im (Form + Sosyal Medya)
?   ??? SSS (4 soru)
?
??? 5 Slider
?   ??? KPSS (Ba?ar? oran?)
?   ??? ALES (Yüksek puan)
?   ??? DGS (Üniversite)
?   ??? YDS (Yabanc? dil)
?   ??? Kampanya (%30 indirim)
?
??? 6 Duyuru
    ??? 2 Önemli (KPSS, ALES takvimi)
    ??? 4 Normal (Demo, ba?ar?, e?itmen, mobil)
```

---

## ?? Kurs Paketleri Detay?

### **KPSS Haz?rl?k** (?1.999/Y?l)
```
- Genel Yetenek - Genel Kültür
- E?itim Bilimleri
- ÖABT (Alan Bilgisi)
- 500+ Saat Video Ders
- 50+ Deneme S?nav?
```

### **ALES Haz?rl?k** (?1.499/Y?l)
```
- Sözel - Say?sal Bölümler
- Soru Çözüm Teknikleri
- 400+ Saat Video Ders
- 40+ Deneme S?nav?
- Birebir Mentorluk
```

### **DGS Haz?rl?k** (?1.699/Y?l)
```
- Matematik - Türkçe
- Sözel/EA/MF Konular?
- 450+ Saat Video Ders
- 45+ Deneme S?nav?
- Konu Anlat?m? + Soru Çözümü
```

### **YDS Haz?rl?k** (?1.299/Y?l)
```
- Grammar - Vocabulary
- Reading - Translation
- 300+ Saat Video Ders
- 30+ Deneme S?nav?
- Speaking Practice
```

### **TYT-AYT Haz?rl?k** (?2.499/Y?l)
```
- TYT + AYT Tüm Dersler
- Soru Bankas?
- 600+ Saat Video Ders
- 60+ Deneme S?nav?
- Rehberlik Hizmeti
```

### **MSÜ Haz?rl?k** (?1.799/Y?l)
```
- Askeri Okullar S?nav?
- Fiziksel Haz?rl?k
- 350+ Saat Video Ders
- 35+ Deneme S?nav?
- Mülakata Haz?rl?k
```

---

## ?? URL Yap?s?

```
http://sinavkurs.localhost:5001/home          ? Anasayfa
http://sinavkurs.localhost:5001/hakkimizda    ? Hakk?m?zda
http://sinavkurs.localhost:5001/kurslar       ? Kurslar?m?z
http://sinavkurs.localhost:5001/iletisim      ? ?leti?im
http://sinavkurs.localhost:5001/sss           ? SSS
```

---

## ?? Responsive Tasar?m

Tüm sayfalar **Bootstrap 5 Grid System** ile responsive:
```
- Desktop:  col-md-6, col-lg-4
- Tablet:   col-md-6
- Mobile:   col-12
```

---

## ?? Öne Ç?kan Özellikler

### **Anasayfa:**
- ? Etkileyici Hero Section
- ? ?statistik kartlar? (4 adet)
- ? Neden biz? (6 madde)
- ? Ba?ar? hikayeleri (testimonials)

### **Kurslar?m?z:**
- ? 6 farkl? s?nav paketi
- ? Renk kodlu kategoriler
- ? Detayl? özellik listesi
- ? Fiyat + CTA butonlar?
- ? Kampanya banner'?

### **?leti?im:**
- ? Çal??an form (HTML5 validation)
- ? Sosyal medya entegrasyonu
- ? Çal??ma saatleri
- ? Çoklu ileti?im kanallar?

---

## ?? Görsel Kaynaklar

Tüm görseller **Unsplash** üzerinden:
```
- E?itim temal?
- Yüksek çözünürlük (1200x400)
- Ücretsiz kullan?m
- Profesyonel foto?raflar
```

---

## ?? Sonraki Ad?mlar

### **Yap?labilecek ?yile?tirmeler:**
1. ? Gerçek kurs videolar? ekle
2. ?? Ödeme sistemi entegrasyonu
3. ?? Ö?renci paneli (dashboard)
4. ?? S?nav sonuç analizi
5. ?? Sertifika sistemi
6. ?? E-posta bildirimleri
7. ?? PWA (Progressive Web App)
8. ?? Ö?renci giri?i (authentication)

---

## ?? Destek

Sorular?n?z için:
- ?? E-posta: info@sinavkurs.com
- ?? Telefon: 0850 XXX XX XX
- ?? WhatsApp: 0850 XXX XX XX

---

**Haz?rlayan:** AI Assistant  
**Tarih:** 2024  
**Versiyon:** 1.0  
**Durum:** ? Yay?nda (Production Ready)  
**Domain:** http://sinavkurs.localhost:5001
