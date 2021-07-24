using Spectre.Console;
using System.Collections.Generic;
using System.Linq;
using System;
using Humanizer;

namespace bro
{
    class Formatter
    {
        private string[] projectColumns = { "ID", "Name", "Directory", "Created" };
        private string[] taskColumns = { "ID", "Completed", "Name", "Body", "Created", "Project" };
        public void Heading(string text, string options = "bold orangered1")
        {
            var rule = new Rule($"[{options}]{text}[/]\n");
            rule.Alignment = Justify.Left;
            AnsiConsole.Render(rule);
        }

         public void Text(string text, string options = "bold aquamarine1")
        {
            AnsiConsole.Render(new Markup($"[{options}]{text}[/]\n"));
        }

        private string timeHumanizer(DateTime date)
        {
            TimeSpan duration = DateTime.Now - date;
            return duration.Humanize();
        }

        private string[] taskRow(Task item)
        {
            string[] row = { item.TaskID.ToString(), item.Completed.ToString(), item.Title, item.Body != null ? item.Body : "", timeHumanizer(item.Created), item.ProjectID.ToString() };
            return row;
        }

        private string[] projectRow(Project item)
        {
            string[] row = { item.ProjectID.ToString(), item.Name, item.Directory != null ? item.Directory : "", timeHumanizer(item.Created)};
            return row;
        }

        public void TableTask(IQueryable<Task> list, bool markdown)
        {
            Table table = new Table();
            table.AddColumns(taskColumns);
            table.Border = TableBorder.HeavyHead;
            table.BorderColor(Color.Aquamarine1);

            if (markdown)
            {
                table.Border(TableBorder.Markdown);
            }

            foreach (Task item in list)
            {
                string[] row = taskRow(item);
                table.AddRow(row);
            }

            AnsiConsole.Render(table);
        }

        public void TableProject(IQueryable<Project> list, bool markdown)
        {
            Table table = new Table();
            table.AddColumns(projectColumns);
            table.Border = TableBorder.HeavyHead;
            table.BorderColor(Color.Aquamarine1);

            if (markdown)
            {
                table.Border(TableBorder.Markdown);
            }

            foreach (Project item in list)
            {
                string[] row = projectRow(item);
                table.AddRow(row);
            }

            AnsiConsole.Render(table);
        }
    }
}
