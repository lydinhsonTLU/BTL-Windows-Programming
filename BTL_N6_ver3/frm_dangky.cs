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
    public partial class frm_dangky: Form
    {
        //private DataTable table = new DataTable();
        string conn = connectDb.connectString();
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public frm_dangky()
        {
            InitializeComponent();
        }

        //button thoát
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //sự kiện đăng ký
        private void btn_dangky_Click(object sender, EventArgs e)
        {
            string ten = txtten.Text.Trim();
            string tk = txttk.Text.Trim();
            string mk = txtmk.Text.Trim();
            string email = txtemail.Text.Trim();


            command = connection.CreateCommand();
            string query = "Select *from DangNhap where TenTaiKhoan = '" + tk + "' ";
            command.CommandText = query;
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);

            if (ten == ""|| tk==""||mk==""||email=="")
            {
                MessageBox.Show("Hãy nhập đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }else
            {
                if (table.Rows.Count == 1)
                {
                    MessageBox.Show("Tài khoản đã tồn tại!");
                    return;
                } else {
                    DialogResult r = MessageBox.Show("Bạn sẽ sử dụng tài khoản và mật khẩu này để ĐĂNG NHẬP!", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (r == DialogResult.Yes)
                    {
                        string query1 = "insert into DangNhap values (N'" + ten + "', '" + tk + "', '" + mk + "', '" + email + "')";


                        command = connection.CreateCommand();
                        command.CommandText = query1;
                        command.ExecuteNonQuery();

                        txtten.Text = txttk.Text = txtmk.Text = txtemail.Text = "";

                        MessageBox.Show("Đăng ký thành công! Hãy quay lại để ĐĂNG NHẬP!", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }else
                    {
                        return;
                    }
                }
                    
            }


        }

        //
        private void frm_dangky_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(conn);
            connection.Open();

            btn_thoat.Cursor = Cursors.Hand;
            btn_dangky.Cursor = Cursors.Hand;
        }
    }
}
