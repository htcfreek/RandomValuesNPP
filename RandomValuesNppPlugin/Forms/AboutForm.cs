﻿using Kbg.NppPluginNET;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RandomValuesNppPlugin
{
    public partial class AboutForm : Form
    {
        private readonly ToolTip helperTip = new ToolTip();
        private int ForceEasterEgg = 0;

        public AboutForm()
        {
            InitializeComponent();

            String ver = Main.GetVersion();
            lblTitle.Text += ver;

            // tooltip initialization
            helperTip.SetToolTip(lnkGithub, "Open the RandomValues GitHub page (right-click to copy url)");
            helperTip.SetToolTip(lnkContact, "Send comments or suggestions (right-click to copy address)");

            DisplayEasterEgg();
        }
        private void AboutForm_Load(object sender, EventArgs e)
        {

        }

        private int IsEaster(DateTime dt)
        {
            int month = dt.Month;
            if ((month == 3) || (month == 4)) // always march or april anyway
            {
                int year = dt.Year;
                int g = year % 19;
                int c = year / 100;
                int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
                int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

                int day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
                month = 3;

                if (day > 31)
                {
                    month++;
                    day -= 31;
                }

                // also check for second Easter day
                var dt2 = dt.AddDays(-1);

                // return 1st (sunday) or 2nd (monday)
                if ((month == dt.Month) && (day == dt.Day)) return 1; // 1st, easter sunday
                if ((month == dt2.Month) && (day == dt2.Day)) return 2; // 2nd, easter monday
            }

            return 0; // not Easter day
        }
        private void DisplayEasterEgg()
        {
            // display easter egg icon on certain dates
            DateTime today = DateTime.Now.AddHours(-3); // day 'starts' in the morning and lasts after midnight (especially for new years eve etc.)
            //DateTime today = new DateTime(2021, 12, 31); // testing

            String msg = "";
            String obj = "";
            Image img = RandomValuesNppPlugin.Properties.Resources.easteregg;

            int easter = IsEaster(today);
            if (easter > 0)
            {
                // March/April ?th, varies
                msg = String.Format("Easter {0}day", (easter == 1 ? "Sun" : "Mon"));
                obj = "an Easter egg";
                img = RandomValuesNppPlugin.Properties.Resources.easteregg;
            }
            else
            {
                int daymonth = (today.Month * 100 + today.Day);

                switch (daymonth)
                {
                    case 317: // March 17th
                        msg = "St. Patrick's Day";
                        obj = "a four-leaf clover";
                        img = RandomValuesNppPlugin.Properties.Resources.clover;
                        break;
                    case 1031: // October 31st
                        msg = "Halloween";
                        obj = "a spooky pumpkin";
                        img = RandomValuesNppPlugin.Properties.Resources.pumpkin;
                        break;
                    case 101: // January 1st
                    case 1231: // December 31th
                        msg = "New Year" + (daymonth == 1231 ? "'s Eve" : "");
                        obj = "an oliebol";
                        img = RandomValuesNppPlugin.Properties.Resources.oliebol;
                        break;
                }
            };

            // initialization easter egg
            if (msg != "")
            {
                String tip = String.Format("Happy {0}! You've found {1} ;)", msg, obj);
                helperTip.SetToolTip(picEasterEgg, tip);
                picEasterEgg.Image = img;
                picEasterEgg.Visible = true;
            }
        }
        //private void displayEasterEgg_old()
        //{
        //    // tooltip initialization easter eggs
        //    helperTip.SetToolTip(picClover, "Happy St. Patricks day! You've found a four-leaf clover ;)");
        //    helperTip.SetToolTip(picEasterEgg, "Happy Easter Day! You've found an Easter egg ;)");
        //    helperTip.SetToolTip(picPumpkin, "Happy Halloween! You've found a spooky pumpkin ;)");
        //    helperTip.SetToolTip(picOliebol, "Happy New Year! You've found an oliebol ;)");
        //
        //    // display easter egg icon on certain dates
        //    DateTime today = DateTime.Now.AddHours(-3); // day 'starts' in the morning and lasts after midnight (especially for new years eve etc.)
        //
        //    if (IsEaster(today))
        //    {
        //        picEasterEgg.Visible = true;
        //    }
        //    else
        //    {
        //        int daymonth = (today.Month * 100 + today.Day);
        //
        //        if (daymonth == 317)  picClover.Visible = true;  // March 17th
        //        if (daymonth == 1031) picPumpkin.Visible = true; // October 31st
        //        if (daymonth == 1231) picOliebol.Visible = true; // December 31th
        //        if (daymonth == 101) picOliebol.Visible = true;  // January 1st
        //    };
        //}
        private void OnLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel lbl = (sender as LinkLabel);
            string url = lbl.Text;
            string urlcopy = url;
            if ((string)lbl.Tag == "0")
            {
                url = "https://github.com/BdR76/RandomValuesNPP/";
                urlcopy = url;
            }
            else
            {
                string sub = lblTitle.Text.Replace(" ", "%20");
                url = string.Format("mailto:{0}?subject={1}", url, sub);
            }

            if (e.Button == MouseButtons.Right)
            {
                Clipboard.SetText(urlcopy); // urlcopy is e-mai address without "mailto:.."
            } else {
                // Change the color of the link text by setting LinkVisited to true.
                lbl.LinkVisited = true;

                // Call the Process.Start method to open the default browser with a URL:
                System.Diagnostics.Process.Start(url);
            }
        }

        private void picEasterEgg_Click(object sender, EventArgs e)
        {
            ForceEasterEgg++;
            if ((ForceEasterEgg == 5) && (picEasterEgg.Image == null))
            {
                helperTip.SetToolTip(picEasterEgg, "On certain days in the year you'll find an easter egg here ;)");
                picEasterEgg.Image = RandomValuesNppPlugin.Properties.Resources.easteregg;
            }
        }
    }
}
