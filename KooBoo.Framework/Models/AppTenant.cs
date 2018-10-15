using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooBoo.Framework.Models
{
    public class AppTenant
    {
        public Guid PK { get; set; }
        public string ReferenceDescription { get; set; }
        public string Code { get; set; }
        public string ThemeName { get; set; }
        public string FolderName { get; set; }
        public string ConnectionString { get; set; }
    }
}
