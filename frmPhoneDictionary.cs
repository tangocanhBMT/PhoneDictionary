using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class frmPhoneDictionary : Form
    {

        #region Method
        public frmPhoneDictionary()
        {
            InitializeComponent();
        }

        void creatColumnsForDataGridView()
        {
            var colName = new DataGridViewTextBoxColumn();
            var colPhone = new DataGridViewTextBoxColumn();

            colName.HeaderText = "Full Name";
            colPhone.HeaderText = "Phone Number";

            colName.DataPropertyName = "fullName";
            colPhone.DataPropertyName = "numberPhone";

            //tạo độ rộng cho các cột
            colName.Width = 275;
            colPhone.Width = 275;

            //add 2 cột vào data grid view
            dtgvPhoneDictionary.Columns.AddRange(new DataGridViewColumn[] { colName, colPhone });

        }

        void loadListPhoneDictionary()
        {
            dtgvPhoneDictionary.DataSource = null;
            creatColumnsForDataGridView();
            dtgvPhoneDictionary.DataSource = listPhoneDictionary.Instance.ListNumberPhone;
            dtgvPhoneDictionary.Refresh();
        }
        void clearTextBox()
        {
            foreach (var item in this.Controls)
            {
                TextBox item1 = item as TextBox;
                if (item1 != null)
                {
                    item1.Clear();
                }
            }
        }
        bool checkInput()
        {
            long result;
            if (txtName.Text == "" || txtPhone.Text == "")
            {
                MessageBox.Show("Please enter phone and name");
                return false;
            }
            if (!(long.TryParse(txtPhone.Text, out result))) {
                MessageBox.Show("Please enter valid form of number phone");
                return false;
            }

            if (txtPhone.Text.Length < 10)
            {
                MessageBox.Show("Please enter valid form of number phone");
                return false;
            }
            return true;

        }
        #endregion



        /// <summary>
        /// ///////////////////////////////////////////////////////
        /// </summary>
        #region Event

        int indexChange = -1;
        int indexSearch = -1;
        string statusButton = "";
        private void frmPhoneDictionary_Load(object sender, EventArgs e)
        {
            //readXML();
            loadListPhoneDictionary();
            btnCancel.Enabled = btnSave.Enabled = false;
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!checkInput())
            {
                return;
            }
            string numberPhone = txtPhone.Text;
            string fullName = txtName.Text;
            listPhoneDictionary.Instance.ListNumberPhone.Add(new PhoneDictionary(fullName, numberPhone));
            loadListPhoneDictionary();
        }

        private void dtgvPhoneDictionary_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (statusButton == "search")
            {
                indexSearch = e.RowIndex;
                // indexSearch tính theo bảng mới, nên khi duyệt theo từng phần tử của mảng mới sẽ ra vị trí của phần tử cần sửa
                for (int i = 0; i < listPhoneDictionary.Instance.ListNumberPhone.Count; i++)
                {
                    if (listPhoneDictionary.Instance.ListNumberPhone[i].FullName == dtgvPhoneDictionary.Rows[indexSearch].Cells[0].Value.ToString())
                    {
                        indexChange = i;
                    }
                }
            }
            else
            {
                indexChange = e.RowIndex;
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (indexChange < 0)
            {
                MessageBox.Show("Choose a cell", "alert");
                return;
            }

            txtName.Text = listPhoneDictionary.Instance.ListNumberPhone[indexChange].FullName;
            txtPhone.Text = listPhoneDictionary.Instance.ListNumberPhone[indexChange].NumberPhone;
            btnAdd.Enabled = btnDelete.Enabled = false;
            btnCancel.Enabled = btnSave.Enabled = txtPhone.Enabled = true;


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!checkInput())
            {
                return;
            }
            listPhoneDictionary.Instance.ListNumberPhone[indexChange].FullName = txtName.Text;
            listPhoneDictionary.Instance.ListNumberPhone[indexChange].NumberPhone = txtPhone.Text;
            dtgvPhoneDictionary.Refresh();
            clearTextBox();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (indexChange < 0)
            {
                MessageBox.Show("Choose a row", "Alert");
                return;

            }
            listPhoneDictionary.Instance.ListNumberPhone.RemoveAt(indexChange);


            loadListPhoneDictionary();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearTextBox();
            btnCancel.Enabled = btnSave.Enabled = false;
            btnAdd.Enabled = btnDelete.Enabled = txtPhone.Enabled = true;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            statusButton = "search";
            btnAdd.Enabled = false;
            txtPhone.Enabled = false;
            btnCancel.Enabled = true;

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string search = txtName.Text;
            List<PhoneDictionary> listSearch = new List<PhoneDictionary>();
            foreach (var item in listPhoneDictionary.Instance.ListNumberPhone)
            {
                if (item.FullName.ToLower().Contains(search.ToLower()))
                {
                    listSearch.Add(item);
                }
            }
            dtgvPhoneDictionary.DataSource = null;
            creatColumnsForDataGridView();
            dtgvPhoneDictionary.DataSource = listSearch;
        }



        #endregion

        DataTable dataTableWrite = new DataTable();
        DataSet dataSetWrite = new DataSet();

        DataTable dataTableRead = new DataTable();
        DataSet dataSetRead = new DataSet();

        DataTable makeDataTable()
        {
            // tao bang
            DataTable dataTable = new DataTable();
            // tao cac cot cho bang
            DataColumn colFullName = new DataColumn("FullName");
            DataColumn colPhoneNumber = new DataColumn("PhoneNumber");
            //add cot vo bang
            dataTable.Columns.Add(colFullName);
            dataTable.Columns.Add(colPhoneNumber);

            return dataTable;
        }
        void writeXML()
        {
            dataTableWrite = makeDataTable();
            foreach (var Item in listPhoneDictionary.Instance.ListNumberPhone)
            {
                dataTableWrite.Rows.Add(Item.FullName, Item.NumberPhone);
            }
            dataSetWrite.Tables.Add(dataTableWrite);
            dataSetWrite.WriteXml("data.xml");
        }

        void readXML()
        {
            dataSetRead.ReadXml("data.xml");
            dataTableRead = dataSetRead.Tables[0];
            foreach(DataRow item in dataTableRead.Rows)
            {
                PhoneDictionary newPhoneDictionary = new PhoneDictionary(item);
                listPhoneDictionary.Instance.ListNumberPhone.Add(newPhoneDictionary);
            }
        }

        private void frmPhoneDictionary_FormClosing(object sender, FormClosingEventArgs e)
        {
            writeXML();
        }
    }


}




//save và load data
// DataTable dataTableWrite = new DataTable();
// DataSet dataSetWrite = new DataSet();

//           tạo hàm tạo dataTable
//dataTable creatDataTable(){
//tạo bảng
//DataTable dataTable = new DataTable();
//tạo các cột cho bảng 
// DataColumn = colNumberPhone = new DataColumn("NumberPhone");
//add cột vô bảng
//dataTable.Columns.Add(colNumberPhone);
//retrun dataTable
//}



//                  tạo hàm ghi dữ liệu - chạy khi form closing
//void WriteXML(){
//tạo bảng - duyệt từng phần tử trong list rồi add vổ bảng đó - 
// add bảng đó vô dataset
// dùng dataset để viết xml
//}


//                  tạo hàm đọc dữ liệu - chạy khi form load
// void readXML(){
// đọc xml lên
// tạo bảng
// lấy dữ liệu từ datatable gán vào listphonebook}