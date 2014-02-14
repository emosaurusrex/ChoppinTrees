using Framework.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Framework.Controls
{
    public class ExternalLink
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public bool NewWindow { get; set; }
    }
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:LeftPane runat=server></{0}:content>")]
    public class LeftPane : WebControl
    {
        protected override System.Web.UI.HtmlTextWriterTag TagKey
        {
            get
            {
                return System.Web.UI.HtmlTextWriterTag.Div;
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            this.ID = "leftPane";
            this.Controls.Add(TitlePane());
            this.Controls.Add(ExternalLinks());
            this.Controls.Add(Calendar());
            base.Render(writer);
        }

        private Control TitlePane()
        {
            var titlePane = new HtmlGenericControl("div");
            titlePane.ID = "titlePane";
            titlePane.Controls.Add(new HtmlGenericControl("h1")
            {
                InnerText = Properties.Settings.Default.GuildTitle
            });
            return titlePane;
        }

        private Control ExternalLinks()
        {
            var cont = new HtmlGenericControl("div")
            {
                ID = "externalLinks"
            };
            cont.Controls.Add(new HtmlGenericControl("h1")
            {
                InnerText = "external Links"
            });
            var ul = new HtmlCssControl("ul", "content");
            foreach (ExternalLink link in Properties.Settings.Default.ExternalLinks)
            {
                var li = new HtmlGenericControl("li");
                li.Controls.Add(new HtmlAnchor()
                {
                    InnerText = link.Name,
                    HRef = link.URL,
                    Target = link.NewWindow ? "_blank" : "_self"
                });
                ul.Controls.Add(li);
            }
            cont.Controls.Add(ul);
            return cont;
        }
        private Control Calendar()
        {
            var events = new HtmlCssControl("div", "leftPane")
            {
                ID = "events"
            };
            events.Controls.Add(new HtmlGenericControl("h1")
            {
                InnerText = "events"
            });
            events.Controls.Add(new HtmlGenericControl("div")
            {
                ID = "calendar"
            });
            return events;
        }
    }
}
