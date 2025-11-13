using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaSeatBooking.Services
{
    public class SeatService
    {
        private readonly ILocalStorageService _localStorage;

        // Eski tek liste (geriye uyumluluk + "o anki görünür seçim" için)
        public HashSet<string> SelectedSeats { get; private set; } = new();

        // Seçimi direkt değiştirmek için
        public void ReplaceSelectedSeats(IEnumerable<string> seats)
        {
            SelectedSeats = new HashSet<string>(seats);
            if (!string.IsNullOrEmpty(CurrentKey))
                TempSelectionsByShowtime[CurrentKey] = new HashSet<string>(SelectedSeats);
        }

        // Fiyat ve bağlamsal bilgiler
        public decimal UnitPrice { get; set; }
        public string? SelectedMovieTitle { get; set; }
        public DateTime? SelectedShowtime { get; set; }
        public string? SelectedAuditorium { get; set; }

        // Satın alınmış (kalıcı) koltuklar: FilmAdı_yyyyMMddHHmm
        public Dictionary<string, HashSet<string>> reservedByShowtime = new();

        // FilmId_ShowtimeId gibi bir anahtarla geçici seçimler (satın alana kadar)
        public Dictionary<string, HashSet<string>> TempSelectionsByShowtime { get; private set; } = new();

        // O an ekranda olan film+seans anahtarı
        public string? CurrentKey { get; private set; }

        public SeatService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        // Koltuk seçimi toggle (o anki CurrentKey’e yazılır)
        public void ToggleSeat(string seatId)
        {
            if (SelectedSeats.Contains(seatId))
                SelectedSeats.Remove(seatId);
            else
                SelectedSeats.Add(seatId);

            if (!string.IsNullOrEmpty(CurrentKey))
                TempSelectionsByShowtime[CurrentKey] = new HashSet<string>(SelectedSeats);
        }

        // Ekranda hangi seans gösteriliyorsa onu bildir
        // key ör: $"{movie.Id}_{showtimeId}"
        public void SetCurrentKey(string key)
        {
            CurrentKey = key;

            // Bu seans için daha önce seçim varsa onu yükle; yoksa boş set
            if (TempSelectionsByShowtime.TryGetValue(key, out var tempSel))
                SelectedSeats = new HashSet<string>(tempSel);
            else
                SelectedSeats = new HashSet<string>();
        }

        // Dışarıdan bir anahtarın geçici seçimlerini getir (gerekirse)
        public HashSet<string> GetSelectionsFor(string key)
        {
            return TempSelectionsByShowtime.TryGetValue(key, out var sel)
                ? new HashSet<string>(sel)
                : new HashSet<string>();
        }

        //Satın alma işlemi — geçici seçimleri kalıcıya taşır
        // reservedKey ör: $"{SelectedMovieTitle}_{SelectedShowtime:yyyyMMddHHmm}"
        public void CommitPurchase(string reservedKey)
        {
            if (!reservedByShowtime.ContainsKey(reservedKey))
                reservedByShowtime[reservedKey] = new HashSet<string>();

            foreach (var seat in SelectedSeats)
                reservedByShowtime[reservedKey].Add(seat);

            // Bu seansın geçici seçimini temizle
            if (!string.IsNullOrEmpty(CurrentKey))
                TempSelectionsByShowtime.Remove(CurrentKey);
        }

        public void ClearCurrentSelection()
        {
            if (!string.IsNullOrEmpty(CurrentKey))
                TempSelectionsByShowtime.Remove(CurrentKey); // geçici seçimi sil

            SelectedSeats.Clear(); // UI’da özetin boşalması için
        }

        // Verileri localStorage’a kaydet
        public async Task SaveStateAsync()
        {
            await _localStorage.SetItemAsync("selectedSeats", SelectedSeats);
            await _localStorage.SetItemAsync("unitPrice", UnitPrice);
            await _localStorage.SetItemAsync("movieTitle", SelectedMovieTitle);
            await _localStorage.SetItemAsync("showtime", SelectedShowtime);
            await _localStorage.SetItemAsync("auditorium", SelectedAuditorium);
            await _localStorage.SetItemAsync("reservedByShowtime", reservedByShowtime);
            // geçici seçim sözlüğü + currentKey
            await _localStorage.SetItemAsync("tempSelectionsByShowtime", TempSelectionsByShowtime);
            await _localStorage.SetItemAsync("currentKey", CurrentKey);
        }

        // Verileri localStorage’dan geri yükle
        public async Task LoadStateAsync()
        {
            var storedSeats = await _localStorage.GetItemAsync<HashSet<string>>("selectedSeats");
            var storedPrice = await _localStorage.GetItemAsync<decimal>("unitPrice");
            var storedTitle = await _localStorage.GetItemAsync<string>("movieTitle");
            var storedShowtime = await _localStorage.GetItemAsync<DateTime?>("showtime");
            var storedAuditorium = await _localStorage.GetItemAsync<string>("auditorium");
            var storedReserved = await _localStorage.GetItemAsync<Dictionary<string, HashSet<string>>>("reservedByShowtime");
            var storedTemps = await _localStorage.GetItemAsync<Dictionary<string, HashSet<string>>>("tempSelectionsByShowtime");
            var storedCurrentKey = await _localStorage.GetItemAsync<string>("currentKey");

            if (storedSeats is not null)
                SelectedSeats = storedSeats;

            UnitPrice = storedPrice;
            SelectedMovieTitle = storedTitle;
            SelectedShowtime = storedShowtime;
            SelectedAuditorium = storedAuditorium;
            reservedByShowtime = storedReserved ?? new();
            TempSelectionsByShowtime = storedTemps ?? new();
            CurrentKey = storedCurrentKey;

            // CurrentKey varsa o anahtarın selection’ını SelectedSeats’e yansıt
            if (!string.IsNullOrEmpty(CurrentKey) &&
                TempSelectionsByShowtime.TryGetValue(CurrentKey, out var tempSel))
            {
                SelectedSeats = new HashSet<string>(tempSel);
            }
        }

        // Her şeyi sıfırla (kalıcı rezervasyonları silmeyiz)
        public async Task ClearAsync()
        {
            SelectedSeats.Clear();
            UnitPrice = 0;
            SelectedMovieTitle = null;
            SelectedShowtime = null;
            SelectedAuditorium = null;
            await _localStorage.RemoveItemAsync("selectedSeats");
            await _localStorage.RemoveItemAsync("unitPrice");
            await _localStorage.RemoveItemAsync("movieTitle");
            await _localStorage.RemoveItemAsync("showtime");
            await _localStorage.RemoveItemAsync("auditorium");
            // reservedByShowtime'ı SİLMEYİN; satılan koltuklar kalsın.
        }

        // Güvenli JSON okuma metodu
    }
}
