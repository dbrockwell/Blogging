using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            Console.WriteLine("1. Display All Blogs");
            Console.WriteLine("2. Add Blog");
            Console.WriteLine("3. Create Post");
            Console.WriteLine("4. Display Posts");
            string choose = Console.ReadLine();

            if (choose == "1") {
                try
                {

                    // Create and save a new Blog
                    Console.Write("Enter a name for a new Blog: ");
                    var name = Console.ReadLine();

                    var blog = new Blog { Name = name };

                    var db = new BloggingContext();
                    db.AddBlog(blog);
                    logger.Info("Blog added - {name}", name);

                    // Display all Blogs from the database
                    var query = db.Blogs.OrderBy(b => b.Name);

                }
                    catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }

            if (choose == "2") {
                try
                {
                    var db = new BloggingContext();
                    var query = db.Blogs.OrderBy(b => b.Name);
                    Console.WriteLine("All blogs in the database:");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.Name);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }

            logger.Info("Program ended");
        }
    }
}
