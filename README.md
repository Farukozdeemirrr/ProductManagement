# Product Management Case Açıklaması

Projede `Product` entity için aşağıdaki alanlar oluşturulmuştur:

```text
Id
Title
Description
Category
StockQuantity
```

`Category` entity için ise aşağıdaki alanlar oluşturulmuştur:

```text
Id
Name
MinStockQuantity
ParentId
Slug
```

Bu entity’lere ek olarak sistemde **Soft Delete** ve **Auditable** alanları da bulunmaktadır.

Soft Delete ve Auditable kapsamındaki alanlar sayesinde kayıtların oluşturulma, güncellenme, silinme ve aktiflik durumları takip edilebilmektedir.
Projede Infrastructure'den ayrı olarak oluşturulan Repository katmanı bilinçli bir davranıştır. Bu katman, projenin daha temiz ve geliştirilebilir çalışmalara uyum sağlanabileceğini gösteren bir yapı kurulabileceğini göstermek için yapılmıştır.
---

## Product Entity Doğrulama Kuralları

Task içerisinde belirtildiği üzere `Product` entity için aşağıdaki iş kuralları uygulanmıştır:

* `Title` alanı `null` veya boş olamaz.
* `Title` alanı maksimum `200` karakter uzunluğunda olabilir.
* Bir ürün yalnızca bir kategoriye sahip olabilir.
* Bir ürünün yayında / aktif (`live`) olabilmesi için bir kategoriye sahip olması zorunludur.
* Ürünler, kategori seviyesinde tanımlanan minimum stok miktarını sağlamalıdır.
* Stok miktarı, ilgili kategorinin minimum stok miktarının altında olan ürünler yayında (`live`) olamaz.

Bu kurallar hem domain seviyesinde hem de uygulama akışı içerisinde kontrol edilmiştir.

---

## Product Filtreleme İş Akışı

Task içerisinde istendiği üzere ürünler için filtreleme endpoint’i oluşturulmuştur.

Bu endpoint aşağıdaki kriterlere göre ürün filtreleme işlemini desteklemektedir.

---

### 1. Anahtar Kelime ile Arama

Bir arama anahtar kelimesi (`search keyword`) gönderilebilmektedir.

Gönderilen anahtar kelime aşağıdaki alanlarda sorgulanmaktadır:

```text
Title
Description
Category Name
```

Bu sayede kullanıcı ürün başlığı, ürün açıklaması veya kategori adına göre ürün araması yapabilmektedir.

---

### 2. Stok Aralığına Göre Filtreleme

Ürünler stok miktarına göre minimum ve maksimum değerler verilerek filtrelenebilmektedir.

Desteklenen filtreleme alanları:

```text
Minimum Stock Quantity
Maximum Stock Quantity
```

Bu sayede belirli bir stok aralığında bulunan ürünler listelenebilmektedir.

---

## Test Edilen İş Süreçleri

Taskta belirtilen aşağıdaki iş süreçleri test edilmiştir:

* Product CRUD işlemleri
* Category CRUD işlemleri
* Product title doğrulama kuralları
* Product-category ilişkisi
* Product live durumuna geçiş kuralları
* Category minimum stok miktarı kontrolü
* Keyword bazlı ürün arama
* Stok aralığına göre ürün filtreleme
* Soft Delete akışı
* Aktif / pasif ürün yönetimi

Yapılan testler sonucunda akışın taskta beklenildiği üzere istenilen şekilde çalıştığı doğrulanmıştır.

---

## Soft Delete Yapısı

Projede Soft Delete yapısı bilinçli olarak `ToggleActiveStatusAsync` şeklinde isimlendirilmiştir.

Bu yapı, kayıtların fiziksel olarak veritabanından silinmesi yerine aktif / pasif duruma alınmasını sağlamaktadır.

Ürünler:

```text
Aktiften pasife alınabilir.
Pasiften tekrar aktife çekilebilir.
```

Bu durum frontend tarafında aşağıdaki gibi gösterilebilir:

```text
Arşivlendi
Silindi
Pasif
Aktif değil
```

Şu anlık projede **Hard Delete** yapısı bulunmamaktadır.

Bu tercih sayesinde veri kaybı yaşanmadan ürünlerin sistemdeki geçmişi korunabilmektedir.

---

## ToggleLiveStatusAsync İş Akışı

`ToggleLiveStatusAsync`, ürünün yayına alınma veya yayından kaldırılma sürecini yöneten iş akışıdır.

Bir ürünün `IsLive = true` durumuna getirilebilmesi için aşağıdaki şartlar sağlanmalıdır:

```text
Ürün bir kategoriye sahip olmalıdır.
Ürünün stok miktarı, bağlı olduğu kategorinin minimum stok miktarından fazla veya ona eşit olmalıdır.
```

Yani ürün stok miktarı, kategori bazında tanımlanan minimum stok miktarını sağlıyorsa ve ürün bir kategoriye bağlıysa ürün yayına alınabilir.
ToggleActiveStatusAsync Ürünün sistemdeki aktif pasifliğini gösterirken ToggleLiveStatusAsync ürünün canlı ortamda sunulup sunulmadığını belirtmektedir.Bu iki durum birbirinden iyi ayırt edilmelidir.
---

## Live Ürünlerde Stok Güncelleme Davranışı

Eğer ürün `IsLive = true` durumundayken ürünün stok miktarı güncellenir ve yeni stok miktarı, kategori için tanımlanan minimum stok değerinin altına düşerse ürünün `IsLive` değeri otomatik olarak `false` değerine güncellenir.

Bu davranış bilinçli olarak bu şekilde tasarlanmıştır.

Bunun sebebi, stok miktarının yalnızca admin tarafından değil, client veya farklı iş süreçleri tarafından da düşürülebileceğinin göz önünde bulundurulmasıdır.

Bu nedenle sistem daha esnek olacak şekilde tasarlanmıştır.

Örneğin:

```text
Category.MinStockQuantity = 10
Product.StockQuantity = 15
Product.IsLive = true
```

Bu ürün yayındadır.

Ancak stok miktarı daha sonra şu şekilde güncellenirse:

```text
Product.StockQuantity = 5
```

Bu durumda ürün artık kategori minimum stok şartını sağlamadığı için otomatik olarak:

```text
Product.IsLive = false
```

durumuna çekilir.

---

## Live Ürünlerde Kategori Güncelleme Davranışı

Eğer ürün `IsLive = true` durumundayken ürünün kategorisi `null` bırakılmaya çalışılırsa sistem hata fırlatmaktadır.

Bu davranış, stok miktarı güncellemesindeki otomatik `IsLive = false` davranışından farklı tasarlanmıştır.

Bunun sebebi, kategori bilgisinin ürünün temel iş kuralı açısından zorunlu olmasıdır.

Kullanıcının bu işlem hakkında bilinçli hareket etmesi ve ürünün kategori bağlantısını bilerek yönetmesi beklenmektedir.

Bu nedenle kategori bilgisinin kaldırılması durumunda sistem otomatik pasife alma yerine hata fırlatır.

Bu yaklaşım ile ürünün kategori ilişkisi bilinçli ve kontrollü bir iş akışıyla yönetilmiş olur.

---

## Genel Sonuç

Bu proje kapsamında `Product` domain’i için CRUD işlemlerini kapsayan temel ürün yönetimi akışı oluşturulmuştur.

Taskta belirtilen validation ve business rule gereksinimleri uygulanmıştır.


