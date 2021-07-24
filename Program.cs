using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;
using CommandLine;
using Spectre.Console;

namespace bro
{
    class Program
    {
        public static List<Task> tasks = new List<Task>();
        static void Main(string[] args)
        {
            
            /*using (var db = new BroContext())
            {
                Formatter formatter = new Formatter();
                formatter.Heading("Database path");
                formatter.Text($"{db.DbPath}");


                Console.WriteLine("Inserting a new default task");
                db.Add(new Task { Completed = false, Title = "task false datetime", ProjectID = Int32.Parse(ConfigurationManager.AppSettings.Get("defaultProject")) });
                db.SaveChanges();

                Console.WriteLine("Inserting a new project");
                db.Add(new Project { Name = "Test" });
                db.SaveChanges();

                // Create
                Console.WriteLine("Inserting a new task");
                db.Add(new Task { Title = "task 1", ProjectID = 2 });
                db.SaveChanges();

                Console.WriteLine("Querying for a task");
                var tasks = db.Tasks.OrderBy(task => task.Created);

                foreach (Task task in tasks)
                {
                    Console.WriteLine(task.Title);
                }

            }*/
            CommandRunner cm = new CommandRunner();

            Parser.Default.ParseArguments<SetCommands, AddCommands, ShowCommands>(args)
            .WithParsed<SetCommands>(options => cm.RunSet(options))
            .WithParsed<AddCommands>(options => cm.RunAdd(options))
            .WithParsed<ShowCommands>(options => cm.RunShow(options));
        }        
    }
}
