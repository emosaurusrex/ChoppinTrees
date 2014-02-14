using Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WoWAPI;

namespace ChoppinTrees
{
    public partial class Default : System.Web.UI.Page
    {
        private Dictionary<int, Character> characters = new Dictionary<int, Character>();

        protected void Page_Load(object sender, EventArgs e)
        {
            var task = LoadNews();
            task.Wait();
        }

        private async Task LoadNews()
        {
            using (SqlConnection con = Database.Connect())
            {
                SqlDataReader rs = Database.Query(con, "GetNews");
                while (rs.Read())
                {
                    var control = await CreateNews(rs);
                    this.content.Controls.Add(control);
                }
            }
        }

        private async Task<HtmlControl> CreateNews(SqlDataReader rs)
        {
            var getAuthorTask = GetAuthor(rs.EzInt("CharacterID"),
                rs.EzString("CharacterName"),
                rs.EzString("CharacterRealm"));
            var div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "newsPost");
            div.Controls.Add(new HtmlGenericControl("h1")
            {
                InnerText = rs.EzString("Title")
            });
            var body = new HtmlGenericControl("div");
            var bodyText = new HtmlGenericControl("p")
            {
                InnerHtml = rs.EzString("Body")
            };
            body.Attributes.Add("class", "content");
            body.Controls.Add(bodyText);
            var postInfo = new HtmlGenericControl("div");
            postInfo.Attributes.Add("class", "postInfo");
            var date = rs.EzDate("DatePosted");
            var postTime = new HtmlGenericControl("div")
            {
                InnerText = "Posted " + date.ToShortDateString() + " " + date.ToShortTimeString()
            };
            postInfo.Controls.Add(postTime);
            postTime.Attributes.Add("class", "postTime");
            HtmlControl author = await getAuthorTask;
            postInfo.Controls.AddAt(0, author);
            body.Controls.Add(postInfo);
            div.Controls.Add(body);
            return div;
        }

        private async Task<HtmlControl> GetAuthor(int characterID, string name, string realm)
        {
            Character character;
            if (!characters.ContainsKey(characterID))
            {
                character = await Character.PopAsync(name, realm);
                characters.Add(characterID, character);
            }
            else
            {
                character = characters[characterID];
            }
            var div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "character");
            var charImg = new HtmlImage()
            {
                Src = character.GetAvatarPath()
            };
            charImg.Attributes.Add("class", "rounded charAvatar");
            div.Controls.Add(charImg);
            var links = new HtmlGenericControl("div");
            links.Attributes.Add("class", "linkCont");
            var nameLink = new HtmlAnchor()
            {
               HRef = character.GetArmoryLink(),
               Target = "_blank",
               InnerText = character.Name
            };
            nameLink.Attributes.Add("class", "characterLink");
            nameLink.Attributes.Add("data-class", character.Class.ToString().ToLower());
            links.Controls.Add(nameLink);
            var guild = new HtmlAnchor()
            {
                HRef = character.GuildInfo.GetArmoryLink(),
                Target = "_blank",
                InnerText = "<" + character.GuildInfo.Name + ">"
            };
            guild.Attributes.Add("class", "guildLink");
            links.Controls.Add(guild);
            var specs = new HtmlGenericControl("div");
            specs.Attributes.Add("class", "specs");
            foreach (Talent t in character.Talents)
            {
                var spec = new HtmlGenericControl("span");
                spec.Attributes.Add("class", "spec" + (t.Selected ? " selected" : ""));
                var img = new HtmlImage()
                {
                    Src = t.TalentSpec.GetIconPath()
                };
                img.Attributes.Add("class", "rounded");
                spec.Controls.Add(img);
                spec.Controls.Add(new HtmlGenericControl("span")
                {
                    InnerText = t.TalentSpec.Name
                });
                specs.Controls.Add(spec);
            }
            links.Controls.Add(specs);
            div.Controls.Add(links);
            return div;
        }
    }
}