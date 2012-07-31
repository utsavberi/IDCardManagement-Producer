using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Data.SqlClient;


namespace IDCardManagement
{
    public partial class Form1 : Form
    {
        IDCard idcard;
        string extraTableName;
        public Form1(IDCard idcard, String extraTableName)
        {
            this.extraTableName = extraTableName;
            this.idcard = idcard;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            switch (idcard.dataSourceType)
            {
                case "Microsoft SQL Server Compact 3.5":
                    try
                    {
                        using (SqlCeConnection conn = new SqlCeConnection(idcard.connectionString))
                        {
                            conn.Open();
                            SqlCeCommand cmd = new SqlCeCommand("select * from " + extraTableName + "pic, " + extraTableName + ", " + idcard.tableName + " where " + extraTableName + ".id = " + extraTableName + "pic.id AND " + extraTableName + ".id = " + idcard.tableName + "." + idcard.primaryKey, conn);
                            SqlCeDataReader rdr = cmd.ExecuteReader();
                            int i = 0;
                            while (rdr.Read())
                            {
                                Byte[] byteBLOBData = new Byte[0];
                                byteBLOBData = (Byte[])(rdr["pic"]);
                                MemoryStream stmBLOBData = new MemoryStream(byteBLOBData);
                                PictureBox picBx = new PictureBox();
                                picBx.BackgroundImage = Image.FromStream(stmBLOBData);
                                picBx.BackgroundImageLayout = ImageLayout.Stretch;
                                picBx.Top = (i * 100) + 30;
                                picBx.Height = 100;
                                picBx.Width = 100;
                                groupBox1.Controls.Add(picBx);

                                Label tmp1 = new Label();
                                tmp1.Text = rdr["name"].ToString();
                                tmp1.Top = (i * 100) + 40;
                                tmp1.Left = 120;
                                tmp1.Width = 500;
                                groupBox1.Controls.Add(tmp1);

                                tmp1 = new Label();
                                tmp1.Text = rdr["printtime"].ToString();
                                tmp1.Top = (i * 100) + 60;
                                tmp1.Left = 120;
                                tmp1.Width = 500;
                                groupBox1.Controls.Add(tmp1);

                                Label tmp2 = new Label();
                                tmp2.Text = rdr["machineid"].ToString();
                                tmp2.Top = (i * 100) + 80;
                                tmp2.Left = 120;
                                tmp2.Width = 500;
                                groupBox1.Controls.Add(tmp2);

                                tmp1 = new Label();
                                tmp1.Text = rdr["log"].ToString();
                                tmp1.Top = (i * 100) + 100;
                                tmp1.Left = 120;
                                tmp1.Width = 500;
                                groupBox1.Controls.Add(tmp1);


                                i++;


                            }
                            rdr.Close();
                            cmd = new SqlCeCommand("select count(*) from " + idcard.tableName, conn);
                            enrolledLabel.Text += cmd.ExecuteScalar().ToString();

                            cmd = new SqlCeCommand("select count(*) from " + extraTableName, conn);
                            attendedLabel.Text += cmd.ExecuteScalar().ToString();

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    break;
                case "Microsoft SQL Server":
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(idcard.connectionString))
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand("select * from " + extraTableName + "pic, " + extraTableName + ", " + idcard.tableName + " where " + extraTableName + ".id = " + extraTableName + "pic.id AND " + extraTableName + ".id = " + idcard.tableName + "." + idcard.primaryKey, conn);
                            SqlDataReader rdr = cmd.ExecuteReader();
                            int i = 0;
                            while (rdr.Read())
                            {
                                Byte[] byteBLOBData = new Byte[0];
                                byteBLOBData = (Byte[])(rdr["pic"]);
                                MemoryStream stmBLOBData = new MemoryStream(byteBLOBData);
                                PictureBox picBx = new PictureBox();
                                picBx.BackgroundImage = Image.FromStream(stmBLOBData);
                                picBx.BackgroundImageLayout = ImageLayout.Stretch;
                                picBx.Top = (i * 100) + 30;
                                picBx.Height = 100;
                                picBx.Width = 100;
                                groupBox1.Controls.Add(picBx);

                                Label tmp1 = new Label();
                                tmp1.Text = rdr["name"].ToString();
                                tmp1.Top = (i * 100) + 40;
                                tmp1.Left = 120;
                                tmp1.Width = 500;
                                groupBox1.Controls.Add(tmp1);

                                tmp1 = new Label();
                                tmp1.Text = rdr["printtime"].ToString();
                                tmp1.Top = (i * 100) + 60;
                                tmp1.Left = 120;
                                tmp1.Width = 500;
                                groupBox1.Controls.Add(tmp1);

                                Label tmp2 = new Label();
                                tmp2.Text = rdr["machineid"].ToString();
                                tmp2.Top = (i * 100) + 80;
                                tmp2.Left = 120;
                                tmp2.Width = 500;
                                groupBox1.Controls.Add(tmp2);

                                tmp1 = new Label();
                                tmp1.Text = rdr["log"].ToString();
                                tmp1.Top = (i * 100) + 100;
                                tmp1.Left = 120;
                                tmp1.Width = 500;
                                groupBox1.Controls.Add(tmp1);


                                i++;


                            }
                            rdr.Close();

                            cmd = new SqlCommand("select count(*) from " + idcard.tableName, conn);
                            enrolledLabel.Text += cmd.ExecuteScalar().ToString();

                            cmd = new SqlCommand("select count(*) from " + extraTableName, conn);
                            attendedLabel.Text += cmd.ExecuteScalar().ToString();

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    break;
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog svdlg = new SaveFileDialog();
            if (svdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                Bitmap MemoryImage = new Bitmap(groupBox1.Width, groupBox1.Height);
                Rectangle rect = new Rectangle(0, 0, groupBox1.Width, groupBox1.Height);
                groupBox1.DrawToBitmap(MemoryImage, new Rectangle(0, 0, groupBox1.Width, groupBox1.Height));

                PdfDocument doc = new PdfDocument();
                doc.Pages.Add(new PdfPage());
                XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[0]);
                XImage img = XImage.FromGdiPlusImage(MemoryImage);//XImage.FromFile(source);

                xgr.DrawImage(img, 0, 0);
                doc.Save(svdlg.FileName);
                doc.Close();
            }
        }
    }
}
