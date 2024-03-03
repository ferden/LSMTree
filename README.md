LSM Tree ve SSTable Tabanlı Key-Value Veri Depolama Sistemi
Bu proje, Log-Structured Merge-tree (LSM Tree) ve Sorted String Table (SSTable) mantığına dayanarak tasarlanmış bir key-value veri depolama sistemidir. .NET Core ile geliştirilmiştir ve temel CRUD (Create, Read, Update, Delete) işlemlerini destekler. Türkiye'nin 81 ilini key-value çiftleri olarak kullanarak basit bir veri seti üzerinde çalışır.

Özellikler
CRUD İşlemleri: Veriler üzerinde ekleme, okuma, güncelleme ve silme işlemleri yapabilir.
MemTable: Hafızada geçici olarak veri saklar.
SSTable: Disk üzerinde veri saklar ve verileri CSV formatında tutar.
Compaction: Henüz implemente edilmemiş olsa da, veri depolama sistemi compaction işlemlerini desteklemek üzere tasarlanmıştır.
Kurulum
Projeyi lokal ortamınızda çalıştırmak için aşağıdaki adımları takip edin:

Bu repoyu klonlayın veya indirin.
.NET Core yüklü değilse, resmi .NET Core sayfasından .NET Core SDK'yı indirip kurun.
Komut satırı/terminal üzerinden projenin bulunduğu klasöre gidin.
dotnet build komutu ile projeyi derleyin.
dotnet run komutu ile uygulamayı çalıştırın.
Kullanım
Uygulama çalıştırıldığında, kullanılabilir komutlar ekranda gösterilir. Desteklenen komutlar:

add [key] [value]: Yeni bir key-value çifti ekler veya mevcut bir key'in değerini günceller.
update [key] [value]: Mevcut bir key'in değerini günceller.
delete [key]: Belirtilen key'i ve ilişkili değeri siler.
get [key]: Belirtilen key'in değerini getirir.
exit: Uygulamadan çıkar.
Katkıda Bulunma
Bu projeye katkıda bulunmak istiyorsanız, lütfen şu adımları takip edin:

Projeyi forklayın ve kendi GitHub hesabınıza klonlayın.
Yeni bir branch oluşturun (git checkout -b feature/AmazingFeature).
Değişikliklerinizi commit edin (git commit -m 'Add some AmazingFeature').
Branch'inizi GitHub'a push edin (git push origin feature/AmazingFeature).
Bir Pull Request oluşturun.
Lisans
Bu proje MIT lisansı altında lisanslanmıştır. Daha fazla bilgi için LICENSE dosyasına bakın.
