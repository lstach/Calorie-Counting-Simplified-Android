using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSXamarin
{

    public class AppMasterDetailPageMasterMenuItem
    {
        public AppMasterDetailPageMasterMenuItem()
        {
            TargetType = typeof(AppMasterDetailPageMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }

        public Type TargetType { get; set; }
    }
}