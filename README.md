# .Net-Core-Entity-Framework-Exercises

## SQLite Provider

**Yeni Core Console projesi oluşturma:**

`dotnet new console`

**SQLite provider:**

`dotnet add package Microsoft.EntityFrameworkCore.Sqlite`

**.Net Core araçları için:**

`dotnet tool install --global dotnet-ef`

**Araç kontrolü için:**

`dotnet ef -h`

**SQLite database için:**

https://sqlitebrowser.org/

**Entity Sınıfları:**

```csharp
    public class Articles
    {
        //Primary Key (Id veya <typne_name>Id yani ArticlesId)
        /*opsiyonel;
        [Key]
        public int YaziId {get; set;}
        */
        public int Id { get; set; }
        [MaxLength(150)] //maksimium 150 karakter olsun
        [Required] //zorunlu alan
        //using System.ComponentModel.DataAnnotations;
        public string Title { get; set; }
        public string Text {get; set;}
    }

    public class Category
    {
        public int CategoryId {get; set;}
        public string Name {get; set;}
    }
```

**Context Sınıfı:**

```csharp
 public class BlogContext:DbContext 
 //using Microsoft.EntityFrameworkCore;
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories {get; set;}

        //Veritabanı Ayarları
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blog.db"); 
			//veritabanını belirttik
        }
    }
```

**Veritabanı Oluşturma:**

Migration İşlemi : 
-	 Önce Entity Framework Core tasarım bileşenini eklememiz lazım:

	`dotnet add package Microsoft.EntityFrameworkCore.Design`
	
- veritabanımızı oluşturalım adım 1 - (migration oluşturma işlemi):

	`dotnet ef migrations add IlkDatabase`
	* oluşturulanları silmek için: `dotnet ef migrations remove`
	
	sonuç : Migrations klasörü altında 20201102174708_IlkDatabase.cs dosyamızı oluşturdu
	
- veritabanımızı oluşturalım adım 2 - (migrationın uygulanması)

	`dotnet ef database update`
	
Bu veritabanı dosyasını SQLiteBrowser ile görüntüleyebiliriz.

