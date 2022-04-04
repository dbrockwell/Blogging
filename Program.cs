﻿using System;
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

            string choose = "";

            do{
                Console.WriteLine("1. Display All Blogs");
                Console.WriteLine("2. Add Blog");
                Console.WriteLine("3. Create Post");
                Console.WriteLine("4. Display Posts");
                choose = Console.ReadLine();

                if (choose == "1") {
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

                if (choose == "2") {
                    try
                    {

                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };

                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);

                    }
                        catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }

                if (choose == "3") {
                    try
                    {

                        Console.Write("Enter the blog you want to post to: ");
                        var blogName = Console.ReadLine();

                        var db = new BloggingContext();

                        var blogChoice = db.Blogs.Where(b => b.Name.Contains(blogName, StringComparison.OrdinalIgnoreCase));
                        int blogChoiceNumber = blogChoice.Count();
                        int IDEntery = 0;
        
                        if (blogChoiceNumber > 1) {
                            Console.WriteLine($"There are {blogChoiceNumber} blogs that contains your search");
                            Console.WriteLine("Enter the id of the blog you want to post to:");
                            foreach (var item in blogChoice)
                            {
                                Console.WriteLine(blogChoice);
                            }
                            try {
                                Console.Write("Enter Blog ID: ");
                                IDEntery = Int32.Parse(Console.ReadLine());
                            }
                            catch (Exception) {
                                logger.Error("ID entered was not a number");
                            }
                        }
                        else if (blogChoiceNumber == 1) {
                            var onlyChoice = db.Blogs.First(b => b.Name.Contains(blogName, StringComparison.OrdinalIgnoreCase));
                            Console.WriteLine($"One blog was found with the name of \"{onlyChoice.Name}\"");
                            Console.Write("Would you like to post to this blog (Y/N): ");
                            string wouldContinue = Console.ReadLine();
                            if (wouldContinue.ToUpper() == "Y") {
                                IDEntery = onlyChoice.BlogId;
                            }
                        }
                        else {
                            Console.WriteLine($"No blog found with the name of \"{blogName}\"");
                        }

                        try {
                            var finalBlog = db.Blogs.First(b => b.BlogId == IDEntery);

                            int blogID = finalBlog.BlogId;

                            Console.Write("Enter the title of the post: ");
                            var title = Console.ReadLine();

                            Console.Write("Enter the content of the post: ");
                            var content = Console.ReadLine();

                            var post = new Post { Title = title, Content = content, BlogId = blogID, Blog = finalBlog};

                            db.AddBlog(post);
                            logger.Info("Blog added - {name}", name);
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.Message);
                        }

                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                }
            } while (choose == "1" || choose == "2" || choose == "3" || choose == "4");

            logger.Info("Program ended");
        }
    }
}
