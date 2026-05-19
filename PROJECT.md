# Problem Tanýmý

RuleWay için bir e-ticaret merchandising (ürün yönetimi / ürün sergileme) yönetim sistemi oluþturmak istiyoruz.
Ana hedefimiz, `"Product"` domain’i içerisinde CRUD (Create, Read, Update, Delete) iþlemlerini kapsayan bir iþ akýþý oluþturmaktýr.

Kendi teknoloji stack’inizi seçebilirsiniz.

---

# Tanýmlar

Bizim için `Product Entity` aþaðýdaki alanlardan oluþmaktadýr:

* Baþlýk (Title)
* Açýklama (Description)
* Kategori (Category – Child Entity / Alt Entity)
* Stok Miktarý (Stock Quantity)

Ýþ süreçlerimizi desteklemek amacýyla REST endpoint’lerinin bulunmasýný tercih ediyoruz.

---

# Product Entity Doðrulama Kurallarý

`Product Entity` için doðrulama kurallarýmýz aþaðýdaki gibidir:

* Baþlýk (`Title`) null veya boþ olamaz ve maksimum 200 karakter sýnýrýna sahip olmalýdýr.
* Bir ürün yalnýzca bir kategoriye sahip olabilir.
* Bir ürünün yayýnda / aktif (`live`) olabilmesi için bir kategoriye sahip olmasý zorunludur.
* Ürünler, kategori seviyesinde tanýmlanan minimum stok miktarýný saðlamalýdýr.
  Stok miktarý bu limitin altýnda olan ürünler yayýnda (`live`) olamaz.

---

# API Gereksinimleri

API’miz bir ürün filtreleme endpoint’i saðlamalýdýr ve aþaðýdaki kriterlere göre ürün filtreleme iþlemini desteklemelidir:

## 1) Anahtar Kelime ile Arama

Bir arama anahtar kelimesi (`search keyword`) gönderilebilmelidir ve bu kelime aþaðýdaki alanlarda sorgulanmalýdýr:

* Title
* Description
* Category Name

---

## 2) Stok Aralýðýna Göre Filtreleme

Minimum (`min`) ve maksimum (`max`) deðerler verilerek stok miktarý aralýðýna göre sorgulama yapýlabilmelidir.

---

# Teslim Beklentisi

Yaptýðýnýz deðiþiklikleri bir Git repository’sine commit etmeniz ve çýktýyý bizimle paylaþmanýz beklenmektedir.
