using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SQLite_Provider
{

    //Context Sınıfı
    public class BlogContext:DbContext //using Microsoft.EntityFrameworkCore;
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories {get; set;}

        //Veritabanı Ayararı
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blog.db"); //veritabanını belirttik , connection string
        }
    }


    //Entity Sınıfları
    public class Article
    {
        //Primary Key (Id veya <typne_name>Id yani ArticlesId)
        /*opsiyonel;
        [Key]
        public int YaziId {get; set;}
        */
        public int Id { get; set; }
        [MaxLength(150)] //maksimium 150 karakter olsun
        [Required] //zorunlu alan
        //using System.ComponentModel.DataAnnotations; diye eklememiz lazım [] özellikleri için
        public string Title { get; set; }
        public string Text {get; set;}
    }

    public class Category
    {
        public int CategoryId {get; set;}
        public string Name {get; set;}
    }

    class Program
    {
        static void Main(string[] args)
        {
            //yaziEkle();
            //yazilariEkle();
            //yaziyiGetirId(1);
            //yazilariGetir();
            //yaziGuncelle();
            //yaziGuncelle2();
            //yaziGuncelle3();
            //yaziSil(3);
            //yaziSil2();
            yaziSil3();
        }


        // Veritabanına Kayıt ekleme
        static void yaziEkle()
        {
            using(var db = new BlogContext())
            {
                var article = new Article {Title="Js",Text="Js çok yönlü bir dildir"};
                //tek bir article bilgisi ekleyebiliriz
                db.Articles.Add(article);
                db.SaveChanges(); //bekleyen değişiklikler veritabanına aktarılır
                Console.WriteLine("veriler eklendi");
            }
        }

        static void yazilariEkle()
        {
            using(var db = new BlogContext())
            {
                var articles = new List<Article> //using System.Collections.Generic;
                {
                    new Article {Title="Mysql", Text="Mysql bir veritabanı yönetim sistemidir."},
                    new Article {Title = "PostgreSql", Text="PostgreSql bir veritabanı yönetim sistemidir."}
                };
                /*sırayla tüm yazıları veritabanına foreach ile ekleyebiliriz
                foreach(var article in articles)
                {
                    db.Articles.Add(article);
                }
                db.SaveChanges();
                */
                db.Articles.AddRange(articles); //AddRange bir koleksiyon ekler
                db.SaveChanges();
                Console.WriteLine("veriler eklendi");
            }

        }

        //Veritabanından Kayıt Seçme
        static void yazilariGetir()
        {
            using(var icerik = new BlogContext())
            {
                //tüm verileri seçme
                var articles = icerik.Articles.ToList(); //using System.Linq;
                /*  
                var articles = icerik
                .Articles
                .Select(article => new {
                    article.Title
                })
                .ToList();  bize sadece yazıların başlık bilgileri gelir (select ile sağladım , kolon filtrelemesi)
                */

                //verilerin başlık ve içeriklerini ekrana yazdıralım
                foreach(var article in articles)
                {
                    Console.WriteLine($"title: {article.Title} text: {article.Text}");
                }

            }
        }

        static void yaziyiGetirId(int id)
        {
            using(var context = new BlogContext())
            {
                var article = context
                .Articles
                .Where(article => article.Id == id) // and ve or operatörleri de kullanılabilir
                .FirstOrDefault(); //bulduğu değeri gönderir veya null değer gönderir (bulamazsa)

                //verinin ekrana bastırılması
                Console.WriteLine($"title: {article.Title} text: {article.Text}");
            }
        }

        //Veritabanından Kayıt Güncelleme
        static void yaziGuncelle()
        {
            using(var db = new BlogContext())
            {
                //change tracking
                var a = db
                .Articles
                //.AsNoTracking()
                .Where(i => i.Id == 1)
                .FirstOrDefault(); //id'si 1 olan var mı diye bakıyoruz yoksa null gelecek
                if (a != null) //a null değilse demek ki obje var
                {
                    a.Text += " Çokça kullanılmaktadır.";
                    db.SaveChanges();
                    Console.WriteLine("Güncelleme yapıldı");
                }
            }
        }

        static void yaziGuncelle2()
        {
            using(var db = new BlogContext())
            {
                var entity = new Article(){Id=1};
                db.Articles.Attach(entity);
                entity.Text = "Js çok yönlü bir dildir.";
                db.SaveChanges();
                Console.WriteLine("Güncelleme yapıldı");
            }
        }

        static void yaziGuncelle3()
        {
              using(var db = new BlogContext())
            {
                //change tracking
                var a = db
                .Articles
                //.AsNoTracking()
                .Where(i => i.Id == 1)
                .FirstOrDefault(); //id'si 1 olan var mı diye bakıyoruz yoksa null gelecek
                if (a != null) //a null değilse demek ki obje var
                {
                    a.Text += " Çokça kullanılmaktadır. 2";
                    db.Articles.Update(a);
                    db.SaveChanges();
                    Console.WriteLine("Güncelleme yapıldı");
                }
            }
        }

        //Veritabanından Kayıt Silme
        static void yaziSil(int id)
        {
            using(var db = new BlogContext())
            {
                var a = db
                .Articles
                .FirstOrDefault(i => i.Id == id); //Where yazmadan ifadeti FirstorDefault içerisine de yazabiliriz.

                if(a != null)
                {
                    //db.Remove()
                    db.Articles.Remove(a);
                    db.SaveChanges();
                    Console.WriteLine("Yazı silindi");
                }
            }
        }

        static void yaziSil2()
        {
            using(var db = new BlogContext())
            {
                var a = new Article(){Id=2};
                db.Articles.Remove(a);
                db.SaveChanges();
                Console.WriteLine("Yazı silindi");
            }
        }

        static void yaziSil3()
        {
            using(var db = new BlogContext())
            {
                var a = new Article(){Id=2};
                db.Entry(a).State = EntityState.Deleted;
                db.SaveChanges();
                Console.WriteLine("Yazı silindi");
            }
        }

    }
}
