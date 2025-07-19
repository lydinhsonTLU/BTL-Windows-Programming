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
    public partial class frm_traphong2: Form
    {
        public frm_traphong2()
        {
            InitializeComponent();
        }
        string connectString = connectDb.connectString();

        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();

        private void frm_traphong2_Load(object sender, EventArgs e)
        {
            dataGridView_traphong.ReadOnly = true;
            dataGridView_traphong.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView_traphong.ReadOnly = true;
            dataGridView_traphong.AllowUserToAddRows = false;

            connection = new SqlConnection(connectString);
            connection.Open();

            load_DGV();
        }

        void load_DGV()
        {
            command = connection.CreateCommand();
            command.CommandText = "Select Ma_Phong, CCCD, Ho_Ten, Gia_Phong, Ngay_Thanh_Toan from HoaDon";
            adapter.SelectCommand = command;
            adapter.Fill(table);
            dataGridView_traphong.DataSource = table;
        }

        private void dataGridView_traphong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex >= 0)
                {
                    dataGridView_traphong.ClearSelection();

                    // Chọn dòng được nhấp vào
                    dataGridView_traphong.Rows[e.RowIndex].Selected = true;

                    if (dataGridView_traphong.CurrentRow.Cells[5].Value.ToString() == "Đã thanh toán")
                    {
                        MessageBox.Show("Khách hàng đã thanh toán!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        lbl_txt_phong.Text = dataGridView_traphong.CurrentRow.Cells[0].Value.ToString();
                        lbl_txt_maKH.Text = dataGridView_traphong.CurrentRow.Cells[1].Value.ToString();
                        lbl_txt_SN.Text = dataGridView_traphong.CurrentRow.Cells[2].Value.ToString();
                        lbl_txt_giaphong.Text = dataGridView_traphong.CurrentRow.Cells[3].Value.ToString();
                        dateTimePicker_ngaytra.Text = DateTime.Today.ToString("MM-dd-yyyy");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btn_traphong_Click(object sender, EventArgs e)
        {
            
        }
    }
}
