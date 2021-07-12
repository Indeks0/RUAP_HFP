using HeartFailurePrediction.WebApi;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace HeartFailurePrediction
{

    public partial class Form1 : Form
    {

        private int Age;
        private int EjectionFraction;
        private int CreatininePhosphokinase;
        private int SerumCreatinine;
        private int SerumSodium;
        private int Anemia;
        private int Diabetes;
        private int HighBloodPressure;
        private int Sex;
        private int Smoking;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbSex.SelectedIndex = 1;
            cbAnemia.SelectedIndex = 1;
            cbDiabetes.SelectedIndex = 1;
            cbHighBloodPressure.SelectedIndex = 1;
            cbSmoking.SelectedIndex = 1;
        }

        private void cbAnemia_SelectedIndexChanged(object sender, EventArgs e)
        {
            Anemia = (cbAnemia.Text == "True") ? 1 : 0;
        }

        private void cbDiabetes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Diabetes = (cbDiabetes.Text == "True") ? 1 : 0;
        }

        private void cbHighBloodPressure_SelectedIndexChanged(object sender, EventArgs e)
        {

            HighBloodPressure = (cbHighBloodPressure.Text == "True") ? 1 : 0;
        }

        private void cbSex_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sex = (cbSex.Text == "Female") ? 0 : 1;
        }

        private void cbSmoking_SelectedIndexChanged(object sender, EventArgs e)
        {
            Smoking = cbSmoking.Text == "True" ? 1 : 0;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SetInputValues();
            StringTable stringtable = CreateStringTable();
            Dictionary<string, StringTable> inputs = CreateDictionary(stringtable);

            var requestResponse = new RequestResponse();
            string result = await requestResponse.InvokeRequestResponseService(inputs);

            List<string> extractedValues = GetValuesFromRequestString(result);
            ShowOutput(extractedValues);
        }

        private static Dictionary<string, StringTable> CreateDictionary(StringTable stringtable)
        {
            return new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            stringtable
                        },
                    };
        }

        private void ShowOutput(List<string> extractedValues)
        {
            label10.Text = null;
            label10.Text = "Person will die from heart failure:\n" + "\t" +
                                extractedValues[0] + "\n" +
                                "\nScored probability:\n" + "\t" +
                                extractedValues[1];
        }

        private StringTable CreateStringTable()
        {
            return new StringTable()
            {
                ColumnNames = new string[] { "age", "anaemia", "creatinine_phosphokinase", "diabetes", 
                    "ejection_fraction", "high_blood_pressure", "platelets", "serum_creatinine", "serum_sodium", 
                    "sex", "smoking", "time", "DEATH_EVENT" },
                Values = new string[,] { { Age.ToString(), Anemia.ToString(), CreatininePhosphokinase.ToString(), Diabetes.ToString(), 
                        EjectionFraction.ToString(), HighBloodPressure.ToString(), "99999", SerumCreatinine.ToString(), SerumSodium.ToString(), 
                        Sex.ToString(), Smoking.ToString(), "99999", "99999" }, { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" }, }
            };
        }

        private void SetInputValues()
        {
            Age = (int)numericAge.Value;
            EjectionFraction = (int)numericEjectionFracture.Value;
            CreatininePhosphokinase = (int)numericCreatininePhosphokinase.Value;
            SerumCreatinine = (int)numericSerumCreatinine.Value;
            SerumSodium = (int)numericSerumSodium.Value;
            Sex = (cbSex.Text == "Female") ? 1 : 0;
            Smoking = cbSmoking.Text == "True" ? 1 : 0;
            HighBloodPressure = (cbHighBloodPressure.Text == "True") ? 1 : 0;
            Diabetes = (cbDiabetes.Text == "True") ? 1 : 0;
            Anemia = (cbAnemia.Text == "True") ? 1 : 0;
        }

        public List<string> GetValuesFromRequestString(string requestString) //gets scored class and it's probability
        {
            var firstIndex = requestString.IndexOf("99999");
            string substring = requestString.Substring(firstIndex + 8);
            string scoredClass = substring[0].Equals('1') ? "True" : "False";  //class
            substring = substring.Substring(4);

            string pattern = @"^\D*(\d+(?:\.\d+)?)";
            Regex rgx = new Regex(pattern);
            var number = rgx.Matches(substring);
            string scoredProbability = number[0].Value; //probability

            int indexNeki = requestString.IndexOf("]]}}}}");
            char kita = requestString[indexNeki - 2];

            List<string> result = new List<string>();
            result.Add(scoredClass);
            result.Add(scoredProbability);

            return result;
        }

    }


    public class StringTable
{
    public string[] ColumnNames { get; set; }
    public string[,] Values { get; set; }
}
}
