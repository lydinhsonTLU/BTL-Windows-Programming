using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BTL_N6_ver3
{
    public partial class frm_quen: Form
    {
        string connectString = connectDb.connectString();
        SqlConnection connection;
        
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public frm_quen()
        {
            InitializeComponent();
        }

        private void btnQuaylai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_quen_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectString);
            connection.Open();

            btnQuaylai.Cursor = Cursors.Hand;
            btnxacnhan.Cursor = Cursors.Hand;
        }


        //sự kiện xác nhận
        private void btnxacnhan_Click(object sender, EventArgs e)
        {
            string email = txtemail.Text.Trim();
            string ten = txtten.Text.Trim();
            string tk, mk;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(ten))
            {
                MessageBox.Show("Điền đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable table = new DataTable();
            string query = "SELECT TenTaiKhoan, MatKhau FROM DangNhap WHERE HoTen = @HoTen AND Email = @Email";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@HoTen", ten);
                command.Parameters.AddWithValue("@Email", email);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                }
            }

            // Kiểm tra dữ liệu trả về
            if (table.Rows.Count > 0)
            {
                tk = table.Rows[0]["TenTaiKhoan"].ToString();
                mk = table.Rows[0]["MatKhau"].ToString();

                MessageBox.Show($"Tài khoản của bạn: {tk}\nMật khẩu: {mk}", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtten.Text = txtemail.Text = "";
            }
            else
            {
                MessageBox.Show("Không tìm thấy tài khoản nào", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
