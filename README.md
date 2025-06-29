# DiziVote

DiziVote, kullanıcıların The Movie Database (TMDb) API'sini kullanarak dizileri aramasına,
popüler dizileri listelemesine ve bu dizilere 1'den 10'a kadar puan vermesine olanak tanıyan bir Windows (WPF) masaüstü uygulamasıdır.
Kullanıcıların verdiği puanlar yerel bir SQLite veritabanında saklanır ve "Puanladıklarım" ekranında görüntülenebilir.

## Özellikler

- **Dizi Arama**
- **Popüler dizileri Listeleme**
- **Detay Görüntüleme**
- **Puan Görüntüleme**
- **Puan Güncelleme ve Silme**
- **Yerel Veritabanı**

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







