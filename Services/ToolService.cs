using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vproker.Models;

namespace vproker.Services
{
    public class ToolService
    {
        public ApplicationDbContext AppContext { get; set; }

        public ILogger<ToolService> Logger { get; set; }

        public ToolService(ILoggerFactory loggerFactory, ApplicationDbContext appContext)
        {
            Logger = loggerFactory.CreateLogger<ToolService>();
            AppContext = appContext;
        }

        public async Task<Tool> Store(Tool tool)
        {
            if (AppContext.Tools.Any(e => e.ID == tool.ID))
            {
                AppContext.Tools.Attach(tool);
                AppContext.Entry(tool).State = EntityState.Modified;
            }
            else
            {
                AppContext.Tools.Add(tool);
            }

            AppContext.SaveChanges();

            return await GetById(tool.ID);
        }

        public async Task<Tool> GetById(string id)
        {
            Tool tool = await AppContext.Tools.SingleOrDefaultAsync(b => b.ID == id);
            return tool;
        }

        public IEnumerable<Tool> GetAll()
        {
            return AppContext.Tools.ToArray<Tool>();
        }

        public Tool GetByName(ClaimsPrincipal user, string name)
        {
            return AppContext.Tools.FirstOrDefault(o => String.Equals(o.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }

        internal static IEnumerable<SelectListItem> GetToolsListItems(IEnumerable<Tool> tools, string selectedId = null)
        {
            var items = tools
                .OrderBy(t => t.Name)
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.ID,
                    Selected = t.ID == selectedId
                }).ToList();
            //items.Add(new SelectListItem() { Text = "Не выбран", Value = null });
            return items;
        }
    }
}
