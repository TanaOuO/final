using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1111410021final
{
    public partial class Form1 : Form
    {
        private byte[] currentPhotoBytes = null;
        private string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;User ID=sa;Password=sa123";
        private DataSet ds = new DataSet();
        private BindingManagerBase bmEmployees;
        public Form1()
        {
            InitializeComponent();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadEmployeeData();
            LoadTerritoriesComboBox();
        }

        private void LoadEmployeeData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    
                    string sqlEmp = "SELECT EmployeeID, LastName, FirstName, Title, BirthDate, HireDate, ReportsTo, PhotoPath, Photo,Address,City,Region,Postalcode,Country,HomePhone,Extension,Notes,TitleOfCourtesy FROM Employees";
                    SqlDataAdapter daEmp = new SqlDataAdapter(sqlEmp, conn);

                    
                    if (ds.Tables["Employees"] != null) ds.Tables["Employees"].Clear();
                    daEmp.Fill(ds, "Employees");

                    
                    txtEmployeeID.DataBindings.Clear();
                    txtLastName.DataBindings.Clear();
                    txtFirstName.DataBindings.Clear();
                    txtTitle.DataBindings.Clear();
                    txtReportsTo.DataBindings.Clear();
                    txtPhotoPath.DataBindings.Clear();
                    txtBirthDate.DataBindings.Clear();
                    txtHireDate.DataBindings.Clear();
                    txtAddress.DataBindings.Clear();
                    txtCountry.DataBindings.Clear();
                    txtRegion.DataBindings.Clear();
                    txtExtension.DataBindings.Clear();
                    txtHomePhone.DataBindings.Clear();
                    txtNotes.DataBindings.Clear();
                    txtTitleOfCourtesy.DataBindings.Clear();
                    txtCity.DataBindings.Clear();
                    txtPostalcode.DataBindings.Clear();

                  
                    txtLastName.DataBindings.Add("Text", ds, "Employees.LastName");
                    txtFirstName.DataBindings.Add("Text", ds, "Employees.FirstName");
                    txtTitle.DataBindings.Add("Text", ds, "Employees.Title");
                    txtReportsTo.DataBindings.Add("Text", ds, "Employees.ReportsTo");
                    txtPhotoPath.DataBindings.Add("Text", ds, "Employees.PhotoPath");
                    txtEmployeeID.DataBindings.Add("Text", ds, "Employees.EmployeeID");
                    txtBirthDate.DataBindings.Add("Text", ds, "Employees.BirthDate", true, DataSourceUpdateMode.OnPropertyChanged, "", "yyyy-MM-dd");
                    txtHireDate.DataBindings.Add("Text", ds, "Employees.HireDate", true, DataSourceUpdateMode.OnPropertyChanged, "", "yyyy-MM-dd");
                    txtAddress.DataBindings.Add("Text", ds, "Employees.Address");
                    txtCountry.DataBindings.Add("Text", ds, "Employees.City");
                    txtRegion.DataBindings.Add("Text", ds, "Employees.Region");
                    txtExtension.DataBindings.Add("Text", ds, "Employees.Postalcode");
                    txtHomePhone.DataBindings.Add("Text", ds, "Employees.HomePhone");
                    txtNotes.DataBindings.Add("Text", ds, "Employees.Notes");
                    txtTitleOfCourtesy.DataBindings.Add("Text", ds, "Employees.TitleOfCourtesy");
                    txtCity.DataBindings.Add("Text", ds, "Employees.Country");
                    txtPostalcode.DataBindings.Add("Text", ds, "Employees.Extension");

                   
                    bmEmployees = this.BindingContext[ds, "Employees"];

                   
                    bmEmployees.PositionChanged -= BmEmployees_PositionChanged;
                    bmEmployees.PositionChanged += BmEmployees_PositionChanged;

                    
                    UpdateSubData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("資料庫連線失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BmEmployees_PositionChanged(object sender, EventArgs e)
        {
            UpdateSubData();
        }

        private void UpdateSubData()
        {
            if (bmEmployees == null || bmEmployees.Position < 0 || ds.Tables["Employees"].Rows.Count == 0) return;

           
            DataRowView currentCtrl = (DataRowView)bmEmployees.Current;

            
            if (currentCtrl["EmployeeID"] == DBNull.Value)
            {
                pictureBox1.Image = null;
                dgvTerritories.DataSource = null; 
                return; 
            }

       
            int empId = Convert.ToInt32(currentCtrl["EmployeeID"]);

            
            if (currentCtrl["Photo"] != DBNull.Value)
            {
                byte[] photoBytes = (byte[])currentCtrl["Photo"];
                int offset = (photoBytes.Length > 78 && photoBytes[0] == 21 && photoBytes[1] == 28) ? 78 : 0;

                using (MemoryStream ms = new MemoryStream(photoBytes, offset, photoBytes.Length - offset))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox1.Image = null;
            }

            
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sqlTerritory = @"
            SELECT ET.TerritoryID AS [業務區代碼], T.TerritoryDescription AS [業務區名稱]
            FROM EmployeeTerritories ET
            JOIN Territories T ON ET.TerritoryID = T.TerritoryID
            WHERE ET.EmployeeID = @EmpID";

                SqlCommand cmd = new SqlCommand(sqlTerritory, conn);
                cmd.Parameters.AddWithValue("@EmpID", empId);

                SqlDataAdapter daTerr = new SqlDataAdapter(cmd);
                DataTable dtTerr = new DataTable();
                daTerr.Fill(dtTerr);

                dgvTerritories.DataSource = dtTerr;
            }
        }



        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            bmEmployees.Position = 0;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (bmEmployees.Position < bmEmployees.Count - 1) bmEmployees.Position++;
        }

        private void btnPrior_Click(object sender, EventArgs e)
        {
            if (bmEmployees.Position > 0) bmEmployees.Position--;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bmEmployees.Position = bmEmployees.Count - 1;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
           
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "圖片檔案|*.jpg;*.jpeg;*.png;*.bmp"; 

            if (ofd.ShowDialog() == DialogResult.OK)
            {
               
                pictureBox1.Image = Image.FromFile(ofd.FileName);

                currentPhotoBytes = File.ReadAllBytes(ofd.FileName);

                txtPhotoPath.Text = ofd.FileName;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (currentPhotoBytes == null)
            {
                MessageBox.Show("請先點選「瀏覽...」選擇一張照片！", "提示");
                return;
            }

            DataRowView currentCtrl = (DataRowView)bmEmployees.Current;
            int empId = Convert.ToInt32(currentCtrl["EmployeeID"]);

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    string sql = "UPDATE Employees SET Photo = @Photo, PhotoPath = @PhotoPath WHERE EmployeeID = @EmpID";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Photo", currentPhotoBytes);
                    cmd.Parameters.AddWithValue("@PhotoPath", txtPhotoPath.Text);
                    cmd.Parameters.AddWithValue("@EmpID", empId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("照片上傳成功！", "成功");
                }
                LoadEmployeeData(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("上傳失敗：" + ex.Message);
            }
        }

        private void btnNew_Click_Click(object sender, EventArgs e)
        {
            bmEmployees.AddNew();     
            pictureBox1.Image = null;
            currentPhotoBytes = null; 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                MessageBox.Show("請至少輸入員工姓氏 (LastName)！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                
                bmEmployees.EndCurrentEdit();

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    
                    string sqlInsert = @"
                INSERT INTO Employees (
                    LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, 
                    Address, City, Region, PostalCode, Country, HomePhone, Extension, 
                    Notes, ReportsTo, PhotoPath, Photo
                ) 
                VALUES (
                    @LastName, @FirstName, @Title, @TitleOfCourtesy, @BirthDate, @HireDate, 
                    @Address, @City, @Region, @PostalCode, @Country, @HomePhone, @Extension, 
                    @Notes, @ReportsTo, @PhotoPath, @Photo
                )";

                    SqlCommand cmd = new SqlCommand(sqlInsert, conn);

                   
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);

                    cmd.Parameters.AddWithValue("@Title", string.IsNullOrEmpty(txtTitle.Text) ? (object)DBNull.Value : txtTitle.Text);
                    cmd.Parameters.AddWithValue("@TitleOfCourtesy", string.IsNullOrEmpty(txtTitleOfCourtesy.Text) ? (object)DBNull.Value : txtTitleOfCourtesy.Text);
                    cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(txtAddress.Text) ? (object)DBNull.Value : txtAddress.Text);
                    cmd.Parameters.AddWithValue("@City", string.IsNullOrEmpty(txtCity.Text) ? (object)DBNull.Value : txtCity.Text);
                    cmd.Parameters.AddWithValue("@Region", string.IsNullOrEmpty(txtRegion.Text) ? (object)DBNull.Value : txtRegion.Text);
                    cmd.Parameters.AddWithValue("@PostalCode", string.IsNullOrEmpty(txtPostalcode.Text) ? (object)DBNull.Value : txtPostalcode.Text);
                    cmd.Parameters.AddWithValue("@Country", string.IsNullOrEmpty(txtCountry.Text) ? (object)DBNull.Value : txtCountry.Text);
                    cmd.Parameters.AddWithValue("@HomePhone", string.IsNullOrEmpty(txtHomePhone.Text) ? (object)DBNull.Value : txtHomePhone.Text);
                    cmd.Parameters.AddWithValue("@Extension", string.IsNullOrEmpty(txtExtension.Text) ? (object)DBNull.Value : txtExtension.Text);
                    cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(txtNotes.Text) ? (object)DBNull.Value : txtNotes.Text);
                    cmd.Parameters.AddWithValue("@PhotoPath", string.IsNullOrEmpty(txtPhotoPath.Text) ? (object)DBNull.Value : txtPhotoPath.Text);


                    cmd.Parameters.AddWithValue("@ReportsTo", string.IsNullOrEmpty(txtReportsTo.Text) ? (object)DBNull.Value : Convert.ToInt32(txtReportsTo.Text));

                    
                    cmd.Parameters.AddWithValue("@BirthDate", string.IsNullOrEmpty(txtBirthDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtBirthDate.Text));
                    cmd.Parameters.AddWithValue("@HireDate", string.IsNullOrEmpty(txtHireDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtHireDate.Text));

                 
                    cmd.Parameters.AddWithValue("@Photo", currentPhotoBytes == null ? (object)DBNull.Value : currentPhotoBytes);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("新員工資料新增成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadEmployeeData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("存檔失敗：" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bmEmployees.CancelCurrentEdit(); 
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
           
            if (bmEmployees == null || bmEmployees.Position < 0) return;

            
            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                MessageBox.Show("員工姓氏 (LastName) 不能為空！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                
                bmEmployees.EndCurrentEdit();

               
                DataRowView currentCtrl = (DataRowView)bmEmployees.Current;
                int empId = Convert.ToInt32(currentCtrl["EmployeeID"]);

                using (SqlConnection conn = new SqlConnection(connString))
                {
                   
                    string sqlUpdate = @"
                UPDATE Employees 
                SET LastName = @LastName, 
                    FirstName = @FirstName, 
                    Title = @Title, 
                    TitleOfCourtesy = @TitleOfCourtesy, 
                    BirthDate = @BirthDate, 
                    HireDate = @HireDate, 
                    Address = @Address, 
                    City = @City, 
                    Region = @Region, 
                    PostalCode = @PostalCode, 
                    Country = @Country, 
                    HomePhone = @HomePhone, 
                    Extension = @Extension, 
                    Notes = @Notes, 
                    ReportsTo = @ReportsTo, 
                    PhotoPath = @PhotoPath
                WHERE EmployeeID = @EmpID";

                    SqlCommand cmd = new SqlCommand(sqlUpdate, conn);

                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);

                    cmd.Parameters.AddWithValue("@Title", string.IsNullOrEmpty(txtTitle.Text) ? (object)DBNull.Value : txtTitle.Text);
                    cmd.Parameters.AddWithValue("@TitleOfCourtesy", string.IsNullOrEmpty(txtTitleOfCourtesy.Text) ? (object)DBNull.Value : txtTitleOfCourtesy.Text);
                    cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(txtAddress.Text) ? (object)DBNull.Value : txtAddress.Text);
                    cmd.Parameters.AddWithValue("@City", string.IsNullOrEmpty(txtCity.Text) ? (object)DBNull.Value : txtCity.Text);
                    cmd.Parameters.AddWithValue("@Region", string.IsNullOrEmpty(txtRegion.Text) ? (object)DBNull.Value : txtRegion.Text);
                    cmd.Parameters.AddWithValue("@PostalCode", string.IsNullOrEmpty(txtPostalcode.Text) ? (object)DBNull.Value : txtPostalcode.Text);
                    cmd.Parameters.AddWithValue("@Country", string.IsNullOrEmpty(txtCountry.Text) ? (object)DBNull.Value : txtCountry.Text);
                    cmd.Parameters.AddWithValue("@HomePhone", string.IsNullOrEmpty(txtHomePhone.Text) ? (object)DBNull.Value : txtHomePhone.Text);
                    cmd.Parameters.AddWithValue("@Extension", string.IsNullOrEmpty(txtExtension.Text) ? (object)DBNull.Value : txtExtension.Text);
                    cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(txtNotes.Text) ? (object)DBNull.Value : txtNotes.Text);
                    cmd.Parameters.AddWithValue("@PhotoPath", string.IsNullOrEmpty(txtPhotoPath.Text) ? (object)DBNull.Value : txtPhotoPath.Text);

                    cmd.Parameters.AddWithValue("@ReportsTo", string.IsNullOrEmpty(txtReportsTo.Text) ? (object)DBNull.Value : Convert.ToInt32(txtReportsTo.Text));


                    cmd.Parameters.AddWithValue("@BirthDate", string.IsNullOrEmpty(txtBirthDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtBirthDate.Text));
                    cmd.Parameters.AddWithValue("@HireDate", string.IsNullOrEmpty(txtHireDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtHireDate.Text));


                    cmd.Parameters.AddWithValue("@EmpID", empId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("員工資料已順利更新！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadEmployeeData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新失敗：" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTerritoriesComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {

                string sql = "SELECT TerritoryID, TerritoryDescription FROM Territories";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cbxTerritories.DataSource = dt;
                cbxTerritories.DisplayMember = "TerritoryDescription"; 
                cbxTerritories.ValueMember = "TerritoryID";            
            }
        }

        private void btnAddTerritory_Click(object sender, EventArgs e)
        {
            if (cbxTerritories.SelectedValue == null) return;

            string selectedTerrID = cbxTerritories.SelectedValue.ToString();

            DataRowView currentCtrl = (DataRowView)bmEmployees.Current;
            int empId = Convert.ToInt32(currentCtrl["EmployeeID"]);

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    string sql = "INSERT INTO EmployeeTerritories (EmployeeID, TerritoryID) VALUES (@EmpID, @TerrID)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@EmpID", empId);
                    cmd.Parameters.AddWithValue("@TerrID", selectedTerrID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("責任區新增成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                UpdateSubData();
            }
            catch (SqlException ex)
            {

                if (ex.Number == 2627)
                    MessageBox.Show("這名員工已經負責此區域了，請選擇其他區域！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("發生錯誤：" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveTerritory_Click(object sender, EventArgs e)
        {
            if (dgvTerritories.CurrentRow == null)
            {
                MessageBox.Show("請先在下方的表格中，點選要移除的責任區！", "提示");
                return;
            }

            string selectedTerrID = dgvTerritories.CurrentRow.Cells["業務區代碼"].Value.ToString();

            DataRowView currentCtrl = (DataRowView)bmEmployees.Current;
            int empId = Convert.ToInt32(currentCtrl["EmployeeID"]);

            DialogResult result = MessageBox.Show($"確定要解除該員工與區域代碼 [{selectedTerrID}] 的負責關係嗎？", "確認移除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string sql = "DELETE FROM EmployeeTerritories WHERE EmployeeID = @EmpID AND TerritoryID = @TerrID";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@EmpID", empId);
                        cmd.Parameters.AddWithValue("@TerrID", selectedTerrID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    UpdateSubData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("移除失敗：" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
