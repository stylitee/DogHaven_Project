using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doghavenCapstone
{
    public class HomeFlyOutFlyoutMenuItem
    {
        public HomeFlyOutFlyoutMenuItem()
        {
            TargetType = typeof(HomeFlyOutFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}