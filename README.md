## Cinema Seat Booking

Cinema Seat Booking, hayalî bir sinema zinciri için koltuk seçimi ve rezervasyon deneyimini canlandıran Blazor WebAssembly tabanlı bir demo uygulamadır. Proje, film listesini, seans seçimlerini ve koltuk rezervasyonunu uçtan uca deneyimlemek isteyen geliştiriciler için hazırlanmıştır.

### Neler Sunuyor?

- Filmleri posterleri, süreleri ve seans saatleriyle birlikte görme
- Seans seçimine göre salon planını açıp koltuk seçimi yapma
- Seçilen koltuklara göre anlık fiyat hesabı ve rezervasyon özeti
- Basit bir ödeme/iletişim formu ile rezervasyonu tamamlayıp referans kodu oluşturma
- Blazored Local Storage sayesinde sayfa yenilense bile seçimleri saklama

### Kurulum

1. **Gereksinimler**
   - .NET 8 SDK
   - Tercihen Visual Studio 2022, VS Code veya Rider

2. **Depoyu klonlayın veya indirin**
   ```bash
   git clone <repo-url>
   cd CinemaSeatBooking
   ```

3. **Bağımlılıkları indirin ve uygulamayı başlatın**
   ```bash
   dotnet restore
   dotnet run --project CinemaSeatBooking/CinemaSeatBooking.csproj
   ```

4. Tarayıcınızda `https://localhost:5001` veya konsolda görünen URL’yi açın.

### Proje Yapısı

- `CinemaSeatBooking/Pages` – Ana sayfalar: film listesi (`Movies.razor`), koltuk seçimi (`Seats.razor`), ödeme (`Checkout.razor`)
- `CinemaSeatBooking/Components` – Yeniden kullanılabilir bileşenler, örn. `SeatMap`
- `CinemaSeatBooking/Services` – Film ve koltuk durumunu yöneten servisler
- `CinemaSeatBooking/wwwroot/data/movies.json` – Örnek film ve seans verileri

### Teknolojiler

- **Blazor WebAssembly (.NET 8)**
- **Blazored.LocalStorage** ile istemci tarafı durum yönetimi
- **Bootstrap** tabanlı hafif stiller

### Nasıl Kullanılır?

1. `Filmler` sayfasından bir filmi seçin.
2. Açılan `Koltuk Seçimi` ekranında uygun seansı ve koltukları belirleyin.
3. `Rezervasyonu Tamamla` diyerek iletişim formunu doldurun ve referans kodunuzu alın.
