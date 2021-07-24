using System;
using CommandLine;

namespace bro
{
    [Verb("set", HelpText = "Set a project for the session or toggle task completion.")]
    class SetCommands
    {
        [Option('t', "task-id", HelpText = "ID of the task to toggle completion.")]
        public int Id { get; set; }

        [Option('p', "project", HelpText = "Set project for the session.")]
        public string Project { get; set; }

        [Option('e', "exit", HelpText = "Exit the project session.")]
        public bool Exit{ get; set; }
    }

    [Verb("add", HelpText="Add a new task.")]
    class AddCommands
    {
        [Option('t', "title", HelpText = "Title of the task.")]
        public string Title { get; set; }

        [Option('b', "body", Required = false, HelpText = "Give a body to the task.")]
        public string Body { get; set; }

        [Option('c', "complete", Required = false, HelpText = "Set task as completed.")]
        public bool Complete { get; set; }

        [Option('p', "project", Required = false, HelpText = "Project ID to add task too.")]
        public int Project { get; set; }

        [Option('n', "name", Required = false, HelpText = "Name of the project.")]
        public string Name { get; set; }

        [Option('d', "directory", HelpText = "Directory of the project.")]
        public string Directory { get; set; }
    }

    [Verb("show", isDefault:true, HelpText="Display the tasks")]
    class ShowCommands
    {
        [Option('n', "number", HelpText = "Displays specified number of tasks.")]
        public int Number { get; set; }

        [Option('a', "all", HelpText = "Displays all the remaining tasks.")]
        public bool All { get; set; }

        [Option('t', "time", Default ="desc", HelpText = "Sort by when the task was created. 'asc' | 'desc'.")]
        public string Time { get; set; }

        [Option('m', "markdown", HelpText = "Output table as markdown.")]
        public bool Markdown { get; set; }

        [Option('p', "projects", HelpText = "List all the projects.")]
        public bool ProjectList { get; set; }
    }

}
