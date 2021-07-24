using System;
using System.Linq;
using System.Configuration;

namespace bro
{
    class CommandRunner
    {
        Formatter formatter = new Formatter();
        public void RunShow(ShowCommands options)
        {
            Formatter formatter = new Formatter();
            int number = 5;
            bool Markdown = false;
            bool Ascending = false;

            if (options.Time == "asc")
            {
                Ascending = true;
            }

            if (options.Markdown)
            {
                Markdown = true;
            }

            if (options.Number > 0)
            {
                number = options.Number;
            }

            using (var db = new BroContext())
            {
                if (options.ProjectList)
                {
                    formatter.Heading("Showing Projects");
                    if (options.All)
                    {
                        number = db.Projects.Count();
                    }

                    IQueryable<Project> projects;

                    if(ConfigurationManager.AppSettings.Get("project") == "1")
                    {
                        projects = Ascending ? db.Projects.OrderBy(proj => proj.Created).Take(number) : db.Projects.OrderByDescending(proj => proj.Created).Take(number);
                    } 
                    else
                    {
                        int projectId = Int32.Parse(ConfigurationManager.AppSettings.Get("project"));
                        projects = Ascending ? db.Projects.Where(proj => proj.ProjectID == projectId).OrderBy(proj => proj.Created).Take(number) : db.Projects.Where(proj => proj.ProjectID == projectId).OrderByDescending(proj => proj.Created).Take(number);
                    }
                    
                    formatter.TableProject(projects, Markdown);
                }
                else
                {
                    formatter.Heading("Showing Tasks");
                    if (options.All)
                    {
                        number = db.Tasks.Count();
                    }

                    IQueryable<Task> tasks;

                    if (ConfigurationManager.AppSettings.Get("project") == "1")
                    {
                        tasks = Ascending ? db.Tasks.OrderBy(task => task.Created).Take(number) : db.Tasks.OrderByDescending(task => task.Created).Take(number);
                    } 
                    else
                    {
                        int projectId = Int32.Parse(ConfigurationManager.AppSettings.Get("project"));
                        tasks = Ascending ? db.Tasks.Where(proj => proj.ProjectID == projectId).OrderBy(task => task.Created).Take(number) : db.Tasks.Where(proj => proj.ProjectID == projectId).OrderByDescending(task => task.Created).Take(number);
                    }
                    formatter.TableTask(tasks, Markdown);
                }
            }
        }
        public void RunAdd(AddCommands options)
        {
            string projectName = null;
            string projectDirectory = null;
            if (!string.IsNullOrWhiteSpace(options.Name))
            {
                projectName = options.Name;
                projectDirectory = string.IsNullOrWhiteSpace(options.Directory) ? "" : options.Directory;

                using (var db = new BroContext())
                {
                    db.Add<Project>(new Project { Name = projectName, Directory = projectDirectory });
                    db.SaveChanges();



                    var projects = db.Projects.OrderByDescending(task => task.Created).Take(1);
                    formatter.Heading($"Saved project {projectName}");
                    formatter.TableProject(projects, false);
                }
                return;
            }

            string taskTitle = null;
            string taskBody = null;
            int taskProject;
            if (!string.IsNullOrWhiteSpace(options.Title))
            {
                taskTitle = options.Title;
                taskBody = string.IsNullOrWhiteSpace(options.Body) ? null : options.Body;
                taskProject = Int32.Parse(ConfigurationManager.AppSettings.Get("project"));

                if (options.Project > 1 )
                {
                    taskProject = options.Project;
                }

                Task task = new Task { Title = taskTitle, Body = taskBody, ProjectID = taskProject, Completed = options.Complete };

                using (var db = new BroContext())
                {
                    db.Add<Task>(task);
                    db.SaveChanges();

                    var tasks = db.Tasks.OrderByDescending(task => task.Created).Take(1);
                    formatter.Heading($"Saved task {taskTitle}");
                    formatter.TableTask(tasks, false);
                }
                return;
            }

            if (string.IsNullOrWhiteSpace(options.Name) || string.IsNullOrWhiteSpace(options.Title))
            {
                formatter.Text("Please give title for a task or name for a project to add.");
            }
        }
        public void RunSet(SetCommands options)
        {
            if (options.Id > 0)
            {
                using (var db = new BroContext())
                {
                    Task updates = db.Tasks.Where(task => task.TaskID == options.Id).SingleOrDefault();
                    updates.Completed = !updates.Completed;
                    db.SaveChanges();

                    formatter.Heading($"Updated task \"{updates.Title}\"");
                    if (updates.Completed)
                    {
                        formatter.Text("Task is now complete.");
                    } else
                    {
                        formatter.Text("Task is now incomlete.");
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(options.Project))
            {
                using (var db = new BroContext())
                {
                    var project = db.Projects.Where(project => project.Name == options.Project).ToList<Project>();
                    if (project.Count() == 1)
                    {
                        /*ConfigurationManager.AppSettings.Remove("project");
                        ConfigurationManager.AppSettings.Add("project", project.ElementAt(0).ProjectID.ToString());*/
                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings["project"].Value = project.ElementAt(0).ProjectID.ToString();
                        config.Save(ConfigurationSaveMode.Modified);
                        formatter.Text($"Current project is {project.ElementAt(0).Name}, ID {ConfigurationManager.AppSettings.Get("project")}");
                    }
                }
            }

            if (options.Exit)
            {
                formatter.Text($"Exiting project with ID {ConfigurationManager.AppSettings.Get("project")}");
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["project"].Value = "1";
                config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}
