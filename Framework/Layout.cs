using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;

namespace Framework.Layout
{
    public class HtmlCssControl : HtmlGenericControl
    {
        public HtmlCssControl(string tag, string CssClass)
            : base(tag)
        {
            this.Attributes.Add("class", CssClass);
        }
    }
}
