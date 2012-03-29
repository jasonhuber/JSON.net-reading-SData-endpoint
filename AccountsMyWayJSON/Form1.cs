using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace AccountsMyWayJSON
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string URL = string.Format("http://{1}/sdata/slx/dynamic/-/{0}?format=json",
            "Accounts", "localhost:3333");
            CredentialCache myCache = new CredentialCache();
            myCache.Add(new Uri(URL), "Basic",
                new NetworkCredential("Admin", "ll@@kers123"));
            WebRequest wreq = System.Net.WebRequest.Create(URL);
            wreq.Credentials = myCache;
            WebResponse wresp = wreq.GetResponse();
            StreamReader sr = new StreamReader(wresp.GetResponseStream());
            string jsonresp = sr.ReadToEnd();
            
            AccountsMyWayJSON.Account j = JsonConvert.DeserializeObject<AccountsMyWayJSON.Account>(jsonresp);
            int total = 0;
            foreach (Dictionary<string,object> item in j.resources)
            {
                if ((item["Employees"]!=null))
                {
                    total += int.Parse(item["Employees"].ToString());
                }
            }
            j.totalEmployeesinResults = total;
            List<Dictionary<string,object>> r = new List<Dictionary<string,object>>(0);
            j.resources = r;
            rtbDisplay.Text =  JsonConvert.SerializeObject(j);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string URL = string.Format("http://{1}/sdata/slx/dynamic/-/{0}?format=json",
         "Accounts", "localhost:3333");
            CredentialCache myCache = new CredentialCache();
            myCache.Add(new Uri(URL), "Basic",
                new NetworkCredential("Admin", "ll@@kers123"));
            WebRequest wreq = System.Net.WebRequest.Create(URL);
            wreq.Credentials = myCache;
            WebResponse wresp = wreq.GetResponse();
            StreamReader sr = new StreamReader(wresp.GetResponseStream());
            string jsonresp = sr.ReadToEnd();

            AccountsMyWayJSON.Account j = JsonConvert.DeserializeObject<AccountsMyWayJSON.Account>(jsonresp);
            int total = 0;
            //this is really the difference with this example. 
            //I need a place to hold the new resources temporarily...
            List<Dictionary<string,object>> newresources = new List<Dictionary<string,object>>();

            foreach (Dictionary<string, object> item in j.resources)
            {
                if ((item["AccountName"] != null) && item["AccountName"].ToString().StartsWith("A"))
                {
                    Dictionary<string, object> newkeyvalue = new Dictionary<string, object>(0);
                    newkeyvalue.Add("AccountName", item["AccountName"]);

                    newresources.Add(newkeyvalue);
                }
            }
            //then I replace the .resources in the original object. Simple. 
            j.resources = newresources;
            rtbDisplay.Text = JsonConvert.SerializeObject(j);
        }
    }
    public class Account
    {
        [JsonProperty(PropertyName = "$descriptor")]
        public string descriptor { get; set; }
        [JsonProperty(PropertyName = "$url")]
        public string url { get; set; }
        [JsonProperty(PropertyName = "$totalResults")]
        public string totalResults { get; set; }
        [JsonProperty(PropertyName = "$startIndex")]
        public string startIndex { get; set; }
        [JsonProperty(PropertyName = "$itemsPerPage")]
        public string itemsPerPage { get; set; }
        [JsonProperty(PropertyName = "$next")]
        public string next { get; set; }
        [JsonProperty(PropertyName = "$schema")]
        public string schema { get; set; }
        [JsonProperty(PropertyName = "$first")]
        public string first { get; set; }
        [JsonProperty(PropertyName = "$last")]
        public string last { get; set; }
        [JsonProperty(PropertyName = "$template")]
        public string template { get; set; }
        [JsonProperty(PropertyName = "$post")]
        public string post { get; set; }
        [JsonProperty(PropertyName = "$service")]
        public string service { get; set; }
        [JsonProperty(PropertyName = "$resources")]
        public List<Dictionary<string, object>> resources { get; set; }
        //this is the new property I want added to the feed (outer most JSON object):
        public int totalEmployeesinResults { get; set; }
    }
}
