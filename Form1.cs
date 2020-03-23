using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeConverterGUI
{
    public partial class Form1 : Form
    {
        DateTime convertFromDateTime;
        DateTime convertToDateTime = DateTime.Now;
        DateTime newDateTime;
        TimeZoneInfo combo1TZ;
        TimeZoneInfo combo2TZ;
        int offsetA = 0;
        int offsetB = 0;
        bool comboCheck;

        public Form1()
        {
            InitializeComponent();

            // Pull system timezones to use as a datasource for comboboxes.
            var tzCollection = TimeZoneInfo.GetSystemTimeZones();
            var bindingsource1 = new BindingSource();
            bindingsource1.DataSource = tzCollection;

            // Set comboboxes to local time zone.
            foreach (TimeZoneInfo timeZone in tzCollection)
            {
                if (timeZone.Id == TimeZoneInfo.Local.Id)
                {
                    int index = tzCollection.IndexOf(timeZone);
                    
                    // Bind data source to combobox 1.
                    comboBox1.DataSource = bindingsource1.DataSource;
                    comboBox1.SelectedIndex = index;
                    comboCheck = true;

                    // Create new binding contect and bind datasource to combobox 2.
                    comboBox2.BindingContext = new BindingContext();
                    comboBox2.DataSource = bindingsource1.DataSource;
                    comboBox2.SelectedIndex = index;

                    break;
                }
            }

            GetNewDateTime();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCheck)
            {
                GetNewDateTime();
                CheckDiff();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetNewDateTime();
            CheckDiff();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CheckDiff() // Checks the offset between the two selected timezones.
        {
            if (offsetB > offsetA)
            {
                DiffLbl.ForeColor = System.Drawing.Color.Green;
                DiffLbl.Text = "+" + Convert.ToString(offsetB - offsetA) + " Hours";
            } else if (offsetB < offsetA)
            {
                DiffLbl.ForeColor = System.Drawing.Color.Red;
                DiffLbl.Text = Convert.ToString(offsetB - offsetA + " Hours");
            } else
            {
                DiffLbl.Text = "";
            }
        }

        private void timePicker_ValueChanged(object sender, EventArgs e)
        {
            GetNewDateTime();
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            GetNewDateTime();
        }

        private void GetNewDateTime()
        {
            // Update newDateTime variable.
            newDateTime = DateTime.SpecifyKind(datePicker.Value.Date + timePicker.Value.TimeOfDay, DateTimeKind.Unspecified);

            // Get time zone info from first combobox selection.
            combo1TZ = (TimeZoneInfo)comboBox1.SelectedItem;
            offsetA = combo1TZ.BaseUtcOffset.Hours;
            convertFromDateTime = newDateTime;

            if (comboCheck)
            {
                // Get time zone info from second combobox selection.
                combo2TZ = (TimeZoneInfo)comboBox2.SelectedItem;

                //convertToDateTime = TimeZoneInfo.ConvertTimeFromUtc(newDateTime.ToUniversalTime(), combo2TZ);
                offsetB = combo2TZ.BaseUtcOffset.Hours;
                convertToDateTime = TimeZoneInfo.ConvertTime(newDateTime, combo1TZ, combo2TZ);
            }
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            timeLbl1.Text = convertFromDateTime.ToString();
            timeLbl2.Text = convertToDateTime.ToString();
        }
    }
}
