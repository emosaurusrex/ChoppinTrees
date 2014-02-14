using Framework;
using Framework.Data;
using Framework.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WoWAPI
{
    [DataContract]
    public class Character
    {
        public int CharacterID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "class")]
        public Classes Class { get; set; }
        [DataMember(Name = "thumbnail")]
        public string Avatar { get; set; }
        [DataMember(Name = "realm")]
        public string Realm { get; set; }
        [DataMember(Name = "talents")]
        public List<Talent> Talents { get; set; }
        public int LootSpec { get; set; }
        [DataMember(Name = "guild")]
        public Guild GuildInfo { get; set; }
        [DataMember(Name = "lastModified")]
        public double LastModifiedAPI { get; set; }

        public Character()
        {
            this.Talents = new List<Talent>();
        }

        public static Character Pop(string name, string realm)
        {
            string json = Request.GetJSON(GetPopURL(name, realm), Constants.Timeout);
            return JSON.Deserialize<Character>(json);
        }

        public static async Task<Character> PopAsync(string name, string realm)
        {
            using (SqlConnection con = Database.Connect())
            {
                SqlDataReader rs = GetCharacterRS(con, name, realm);
                if (!rs.Read())
                {
                    string json = await Request.GetJSONAsync(GetPopURL(name, realm), Constants.Timeout);
                    var character = JSON.Deserialize<Character>(json);
                    UpdateDatabase(character);
                    rs = GetCharacterRS(con, name, realm);
                    if (!rs.Read()) throw new Exception("Could not load character from database.");
                    character.CharacterID = rs.EzInt("CharacterID");
                    if (!rs.EzNull("LootSpec")) character.LootSpec = rs.EzInt("LootSpec");
                    return character;
                }
                try
                {
                    string json = await Request.GetJSONAsync(GetPopURL(name, realm), Constants.Timeout, rs.EzDate("LastUpdated"));
                    var character = JSON.Deserialize<Character>(json);
                    character.CharacterID = rs.EzInt("CharacterID");
                    if(!rs.EzNull("LootSpec")) character.LootSpec = rs.EzInt("LootSpec");
                    Task.Run(() => UpdateDatabase(character));
                    return character;
                }
                catch (WebException x)
                {
                    if (((HttpWebResponse)x.Response).StatusCode != HttpStatusCode.NotModified)
                    {
                        throw x;
                    }
                    return Pop(rs);
                }
            }
        }

        private static SqlDataReader GetCharacterRS(SqlConnection con, string name, string realm)
        {
            return Database.Query(con, "Character.GetCharacter", new List<SqlParameter>()
            {
                new SqlParameter("@name", name),
                new SqlParameter("@realm", realm)
            });
        }

        public static Character Pop(SqlDataReader rs)
        {
            Character character = new Character();
            character.CharacterID = rs.EzInt("CharacterID");
            character.Name = rs.EzString("Name");
            character.Realm = rs.EzString("Realm");
            character.Class = (Classes)rs.EzInt("Class");
            if (!rs.EzNull("Name")) character.Avatar = rs.EzString("Avatar");
            if (!rs.EzNull("LootSpec")) character.LootSpec = rs.EzInt("LootSpec");
            character.Talents = new List<Talent>();
            if (!rs.EzNull("PrimarySpec")) character.Talents.Add(Talent.Pop(rs, true));
            if (!rs.EzNull("SecondarySpec")) character.Talents.Add(Talent.Pop(rs, false));
            if (!rs.EzNull("GuildName"))
            {
                character.GuildInfo = new Guild()
                {
                    Name = rs.EzString("GuildName"),
                    Realm = rs.EzString("Realm")
                };
            }
            return character;
        }
        public static void UpdateDatabase(Character character)
        {
            using (SqlConnection con = Database.Connect())
            {
                var parms = new List<SqlParameter>()
                {
                    new SqlParameter("@name", character.Name),
                    new SqlParameter("@realm", character.Realm),
                    new SqlParameter("@class", character.Class),
                    new SqlParameter("@avatar", character.Avatar),
                    new SqlParameter("@primarySpec", character.Talents.Where(t => t.Selected).First().TalentSpec.Name),
                    new SqlParameter("@secondarySpec", character.Talents.Where(t => !t.Selected).First().TalentSpec.Name)
                };
                if (character.GuildInfo != null)
                {
                    parms.Add(new SqlParameter("@guildName", character.GuildInfo.Name));
                }
                if (character.LootSpec != null)
                {
                    parms.Add(new SqlParameter("@lootSpec", character.LootSpec));
                }
                if (character.LastModifiedAPI != null && character.LastModifiedAPI > 0)
                {
                    parms.Add(new SqlParameter("@lastModified", Date.FromJava(character.LastModifiedAPI)));
                }
                Database.Procedure(con, "Character.UpdateCharacter", parms);
            }
        }

        private static string GetPopURL(string name, string realm)
        {
            return Constants.BaseURL + "api/wow/character/" + realm + "/" + name + "?fields=items,talents,guild";
        }

        public string GetAvatarPath()
        {
            if (string.IsNullOrEmpty(this.Avatar))
            {
                throw new Exception("No avatar has been specified.");
            }
            return Constants.BaseURL + Constants.AvatarPath + this.Avatar;
        }

        public string GetArmoryLink()
        {
            if (string.IsNullOrEmpty(this.Realm))
            {
                throw new Exception("No realm has been specified.");
            }
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new Exception("No name has been specified.");
            }
            return Constants.BaseURL + Constants.ArmoryPath + "character/" + this.Realm + "/" + this.Name + "/" + Constants.ArmoryType;
        }
    }
}
