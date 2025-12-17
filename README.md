# DiziVote

DiziVote, kullanıcıların The Movie Database (TMDb) API'sini kullanarak dizileri aramasına,
popüler dizileri listelemesine ve bu dizilere 1'den 10'a kadar puan vermesine olanak tanıyan bir Windows (WPF) masaüstü uygulamasıdır.
Kullanıcıların verdiği puanlar yerel bir SQLite veritabanında saklanır ve "Puanladıklarım" ekranında görüntülenebilir.

## Özellikler

- **Dizi Arama**
  <img width="1900" height="1000" alt="Ekran görüntüsü 2025-06-29 213613" src="https://github.com/user-attachments/assets/783d6677-fb2c-4ff8-ab0c-6ebe01627888" />
- **Popüler dizileri Listeleme**
  <img width="1908" height="1000" alt="Ekran görüntüsü 2025-06-29 212241" src="https://github.com/user-attachments/assets/8f7ff911-8ff4-4774-b8c0-166762499ee5" />
- **Detay Görüntüleme**
  <img width="1900" height="1000" alt="Ekran görüntüsü 2025-06-29 212811" src="https://github.com/user-attachments/assets/569cbc8e-cd25-4b75-9c66-f3f5cec24077" />
- **Puan Görüntüleme**
  <img width="1900" height="1000" alt="Ekran görüntüsü 2025-06-29 212905" src="https://github.com/user-attachments/assets/5c978efc-754d-48eb-abce-0f16791efc2b" />
  <img width="1900" height="1000" alt="Ekran görüntüsü 2025-06-29 213029" src="https://github.com/user-attachments/assets/882b2f85-bd25-41db-9ab9-8f874b66c9e1" />
- **Puan Güncelleme ve Silme**
  <img width="900" height="500" alt="Ekran görüntüsü 2025-06-29 214105" src="https://github.com/user-attachments/assets/a6dd8dcc-4777-489e-9210-f37e846a9ca1" />
  <img width="900" height="500" alt="Ekran görüntüsü 2025-06-29 213839" src="https://github.com/user-attachments/assets/b9d907a7-5ed6-4ae1-ad4e-9343ec856e27" />
- **Yerel Veritabanı**
  <img width="1900" height="1000" alt="Ekran görüntüsü 2025-06-29 222726" src="https://github.com/user-attachments/assets/d512ce77-2690-45f5-a841-db2f3ce00530" />

## Teknolojiler

- **Platform:** .NET ve C#
- **Arayüz:** WPF (Windows Presentation Foundation)
- **Tasarım Deseni:** MVVM (Model-View-ViewModel)
- **API:** [The Movie Database (TMDb) API](https://www.themoviedb.org/documentation/api)
- **Veritabanı:** SQLite, Entity Framework Core
- **Ek Kütüphaneler:** Newtonsoft.Json, Microsoft.Extensions.Configuration

## Kod Yapısı
MVVM tasarım modeline göre organize edilmiştir.

### ` Views `: Kullanıcıların etkileşime girdiği pencerelerin bulunduğu dosya
- ` MaiWindow.xaml `: Uygulamanın ana penceresidir. Dizi arama, popüler dizileri listeleme ve dizilerle etkileşime girme işlevleri burada.
- ` MyRatingsView.xaml `: Kullanıcının daha önceden puanladığı dizileri görebildiği ve bu dizilerle etkileşime geçebildiği penceredir.
- ` App.xaml `: Uygulamanın başlangıç noktasıdır. Uygulama kaynakları burada tanımlanmıştır.
- ` App.xaml.cs `: Bu kodda uygulma ilk çalıştığında yapılandırma dosyalarının okunmasını sağlar.


### ` ViewModels `: Pencerelerin arkasındaki mantığı yönetir. View'daki buton tıklamalarını yakalar ve gerekli işlemleri veya sonuçları yansıtmak için tanımlanmış properties leri kullanır.
- ` MainViewModel.cs `:` MainWindow ` un veri bağlamıdır. Dizi arama, popüler dizileri gösterme, dizi seçme ve puanlama komutlarını içerir.
- ` MyRatingsViewModel.cs `:` MyRatingsView ` in veri bağlamıdır. Veritabanındaki puanlanmış dizileri yükler, seçilen dizilerin puanını değiştirme veya silme kommutlarını içerir.


### ` Models `: Burada uygulamanın temel yapıları olan basit sınıflar var.
- ` TVShow.cs `: TMDb API den gelen dizi hakkındaki bilgileri tutar. (Id, İsim, Özet, AfişURL)
- ` RetedTVShow.cs `: Kullanıcının veritabanında saklanan halidir. ` TVShow.cs ` den ayrı olarak kullanıcı puanını da içerir.


### ` Services `: API ve Veritabanıyla iletişim kuran sınıflarımız burada bulunur.
- ` TMDbService.cs `: TMDb API si ile tüm iletişimi buradaki kodlar ile sağladım. API anahtarını yapılandırma dosyasından okur.
- ` DatabaseService.cs `: Yerel SQLite veritabanı ile olan tüm CRUD işlemlerini buradaki kodlarla sağladım.


### ` Data `: Burada veritabanı ile doğrudan ilgili sınıfımız var.
- ` DiziVoteDBContext.cs `: Entity Framework Core' un veritabanı oturumunu temsil eden sınıfımızdır. Hangi modelin veritabanında hangi tabloya karşılık geleceğini ve veritabanı bağlantısının nasıl kurulacağını tanımladığımız yerdir.


### ` Converters `: Burada WPF'ın veri bağlama işlemlerini kolaylaştırmak için oluşturduğum sınıflar mevcuttur. Bazı veriler View - ViewModel arasında uyumlu olmadığı için ihtiyaç duydum.
- ` RatingToBrushMultiConverter.cs `: Dizinin puanına ve butonun kendi değerine bakarak puanlama butonlarının rengini ayarlayan bir sınıftır.
- ` NullToBooleanConverter.cs `: Bir nesnenin null olup olmamasına göre true veya false değeri döndürür. Bir dizinin seçilip seçilmediğini kontrol için bu sınıfı oluşturdum.
- ` StringToVisibilityConverter.cs `: Bir metnin boş olup olmasına göre arayüz elemanının görünürlüğünü ayarlayan sınıfımız.


### ` Migrations `: Bu klasör Entity Framework Core tarafından otomatik oluşturuldu. Mantık olarak veritabanı modelinde oluşan bir değişikliğin veritabanı şemasında nasıl uygulanacağını tanımlayan koddur. Senkronizasyonu sağlar.







