# Employees_Test Veritabanı Kurulumu

Bu SQL script, Microsoft SQL Server üzerinde **çalışan personel yönetim sistemleri** için örnek bir veritabanı yapısını oluşturur. Veritabanı adı: `Employees_Test`

## 🔧 Kurulum Adımları

1. Microsoft SQL Server Management Studio (SSMS) uygulamasını açın.
2. Yeni bir sorgu penceresi açın.
3. Bu dosyadaki SQL komutlarını pencereye yapıştırın.
4. F5 tuşuna basarak çalıştırın veya "Execute" butonuna tıklayın.

## 💽 Oluşturulan Tablolar

- `Employees_Test`: Personel bilgilerini içerir (isim, pozisyon, maaş).
- `Attendance`: Personel yoklamalarını içerir (tarih, durum, geç/gelmedi).

## 🔗 İlişkiler

- `Attendance.EmployeeID`, `Employees_Test.EmployeeID` alanına `FOREIGN KEY` olarak bağlanır.
- `Attendance.Status` alanı yalnızca şu değerleri kabul eder: `'Late'`, `'Absent'`, `'Present'`.

## ⚠️ Dikkat Edilmesi Gerekenler

- **Virüs programları**, bazı durumlarda SQL Server'ın veri dosyalarına (`.mdf` / `.ldf`) erişimini engelleyebilir.
  - Eğer hata alırsanız, virüs koruma yazılımınızı geçici olarak devre dışı bırakmanız gerekebilir.
  - Özellikle veri dosyalarının bulunduğu klasöre (örnek: `C:\Program Files\Microsoft SQL Server\...`) erişim izni verilmelidir.

- Full-Text Search özelliği sisteminizde kurulu değilse aşağıdaki satır çalışmaz ama kod otomatik kontrol içerdiğinden hata vermez:
  ```sql
  EXEC [Employees_Test].[dbo].[sp_fulltext_database] @action = 'enable'
