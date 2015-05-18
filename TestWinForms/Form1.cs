using HBD.Framework.Data.Comparison;
using HBD.Framework.Data.Excel;
using HBD.Framework.Data.XML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var compareInfo = new CompareInfo()
            {
                PrimaryField = new FieldComparison("ID", "Personal_x005F_x0020_Id"),
            };

            using (var adapter = new ExcelAdapter("Test Data\\officer list 2014 Dec 02.xlsx"))
            {
                compareInfo.TableA = adapter.ConvertToDataTable();
            }

            using (var xmlAdapter = new XMLAdapter("Test Data\\Staff Directory_2014.12.03.xml"))
            {
                compareInfo.TableB = xmlAdapter.ConvertToDataTable();
            }

            compareInfo.PopulateComparisonByColumnNames();

            var result = DataCompareHelper.Compare(compareInfo);
            this.comparisionDataGrids1.DataSource = result;
        }
    }
}
