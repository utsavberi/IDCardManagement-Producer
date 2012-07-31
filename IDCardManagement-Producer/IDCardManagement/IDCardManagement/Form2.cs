using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WinFormCharpWebCam;

namespace IDCardManagement
{
    public partial class Form2 : Form
    {
        public int Y { get; set; }
        public int X { get; set; }
        private IDCard idcard;
        Panel panel1;
        string extraTableName;
        Panel pictureContainerPanel;
        String filename;

        public Form2()
        {
            InitializeComponent();
            webcamStatus = 0;
        }

        //called when double-click on .idc file
        public Form2(string fileopen)
        {
            InitializeComponent();
         
            if (fileopen != null)
            {
                filename = fileopen;
                openLoadFile();
            }
        }

        //called when "file->new" 
        public Form2(IDCard idcard)
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            this.idcard = idcard;
            Form2_LoadFile();
        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {

            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }

        private void Form2_Load(object o, EventArgs e)
        {

            toolStripStatusLabel1.Text = "Click on Open to start working...";
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                fontToolStripComboBox.Items.Add(font.Name);
            }


        }

        SqlCeDataAdapter sqlCeAdapter;
        SqlDataAdapter sqlAdapter;
        DataTable dTable;
        BindingSource bSource;
        private void loadDataGrid(String str)
        {
            switch (idcard.dataSourceType)
            {
                case "Microsoft SQL Server Compact 3.5":
                    try
                    {
                        SqlCeConnection c = new SqlCeConnection(str);
                        {
                            c.Open();
                            sqlCeAdapter = new SqlCeDataAdapter("SELECT * FROM " + idcard.tableName, c);
                            {
                                dTable = new DataTable();
                                sqlCeAdapter.Fill(dTable);
                                SqlCeCommandBuilder sqlCommand = new SqlCeCommandBuilder(sqlCeAdapter);
                                sqlCeAdapter.InsertCommand = sqlCommand.GetInsertCommand();
                                sqlCeAdapter.UpdateCommand = sqlCommand.GetUpdateCommand();
                                sqlCeAdapter.DeleteCommand = sqlCommand.GetDeleteCommand();
                                sqlCeAdapter.InsertCommand.Connection = c;

                                //sqlAdapter.InsertCommand = new SqlCeCommand("insert into "+idcard.tableName+" values ");
                                bSource = new BindingSource();
                                bSource.DataSource = dTable;
                                dataGridView1.DataSource = bSource;
                                //dataGridView1.DataSource = t;
                            }
                        }
                    }
                    catch (Exception ex) { MessageBox.Show("Invalid file format :" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    break;
                case "Microsoft SQL Server":
                    string cntu="";
                    try
                    {
                        SqlConnection c = new SqlConnection(str);
                        {
                            c.Open();
                             cntu = "SELECT * FROM " + idcard.tableName;
                            sqlAdapter = new SqlDataAdapter(cntu, c);
                            {
                                dTable = new DataTable();
                                sqlAdapter.Fill(dTable);
                                SqlCommandBuilder sqlCommand = new SqlCommandBuilder(sqlAdapter);
                                sqlAdapter.InsertCommand = sqlCommand.GetInsertCommand();
                                sqlAdapter.UpdateCommand = sqlCommand.GetUpdateCommand();
                                sqlAdapter.DeleteCommand = sqlCommand.GetDeleteCommand();
                                sqlAdapter.InsertCommand.Connection = c;

                                //sqlAdapter.InsertCommand = new SqlCeCommand("insert into "+idcard.tableName+" values ");
                                bSource = new BindingSource();
                                bSource.DataSource = dTable;
                                dataGridView1.DataSource = bSource;
                                //dataGridView1.DataSource = t;
                            }
                        }
                    }
                    catch (Exception ex) { MessageBox.Show("Invalid file format : " + ex.Message +"    "+cntu, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    break;
            }


        }

        private void Form2_LoadFile()
        {
            webcamStatus = 0;
            webcamstarted = 0;
            selectFieldComboBox.DataSource = idcard.fields;
            SetDoubleBuffered(panel1);
            if (idcard.backgroundImage != null) panel1.BackgroundImage = idcard.backgroundImage;
            toolStripStatusLabel1.Text = "Select Records on the right to fill form..";//"Right Click to add Fields...";
            titleLbl.Text = idcard.title;
            titleLbl.MouseDown += tmplbl_MouseDown;
            ControlMover.Init(titleLbl);
            // if (pictureBox1 != null) //ControlMover.Init(pictureBox1);
            panel1.Visible = true;
            loadDataGrid(idcard.connectionString);
            enableItems();

            panel1.Size = new Size(idcard.dimensions.Width * 10, idcard.dimensions.Height * 10);
            panel1.Left = ((this.Width - panel1.Width - dataGridView1.Width) / 2) - 20;
            panel1.Top = (this.Height - panel1.Height) / 2;
            rectangleShape1.Visible = true;
            rectangleShape1.SetBounds(panel1.Left + 5, panel1.Top + 5, panel1.Width, panel1.Height);
            //contextMenuStrip1.Items.Clear();
            //foreach (String str in idcard.selectedFields)
            //{
            //    ToolStripItem tmp = contextMenuStrip1.Items.Add(str);
            //    tmp.Click += tmpToolStripItem_Click;
            //}
            titleLbl.Tag = "notext";

            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is Label)//(string)ctl.Tag !="notext")
                {
                    //if ((string)ctl.Tag != "notext")
                    {
                        Label tmp = new Label();
                        tmp.Tag = ctl.Text;
                        tmp.Left = ctl.Left + ctl.Width + 10;
                        tmp.BackColor = Color.Beige;
                        tmp.Top = ctl.Top;
                        tmp.MouseDown += tmplbl_MouseDown;
                        ControlMover.Init(tmp);
                        panel1.Controls.Add(tmp);
                        tmp.AutoSize = true;
                        // MessageBox.Show("name:"+ctl.Name+" tag :"+ctl.Tag);
                    }
                    //else { MessageBox.Show("found him"); }
                }
            }

        }

        private void enableItems()
        {
            printToolStripButton.Enabled = true;
            webcamToolStripButton.Enabled = true;
            saveToolStripButton.Enabled = true;
            selectFieldComboBox.Enabled = true;
            reportsToolStripButton.Enabled = true;
        }

        //open
        private void openToolStripButton_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                openLoadFile();
            }
        }

        void openLoadFile()
        {
            this.Text = filename + " - IDCard Producer";

            ArrayList fields = new ArrayList();
            ArrayList selectedFields = new ArrayList();

            Image backgroundImage = null;
            string connectionString = "", tableName = "", title = "", dataSourceType = "", primaryKey = "";

            Size dimensions = new Size();

            panel1.Controls.Clear();
            panel1.ContextMenuStrip = contextMenuStrip1;

            try
            {
                using (XmlTextReader reader = new XmlTextReader(filename))
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            #region
                            switch (reader.Name)
                            {
                                case "label":
                                    Label tmp = new Label();
                                    tmp.Text = reader.GetAttribute("text");
                                    tmp.Top = Convert.ToInt32(reader.GetAttribute("top"));
                                    tmp.Left = Convert.ToInt32(reader.GetAttribute("left"));
                                    panel1.Controls.Add(tmp);
                                    tmp.MouseDown += tmplbl_MouseDown;
                                    ControlMover.Init(tmp);
                                    tmp.AutoSize = true;
                                    tmp.Font = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(reader.GetAttribute("font"));
                                    tmp.BackColor = Color.FromArgb(Convert.ToInt32(reader.GetAttribute("backcolor")));
                                    tmp.ForeColor = Color.FromArgb(Convert.ToInt32(reader.GetAttribute("forecolor")));
                                    break;
                                case "IDpictureBox":
                                    pictureContainerPanel = new Panel();
                                    pictureContainerPanel.Tag = "IDpictureBox";
                                    pictureContainerPanel.BackgroundImage = global::IDCardManagement.Properties.Resources.avatar;
                                    pictureContainerPanel.BackgroundImageLayout = ImageLayout.Stretch;
                                    pictureContainerPanel.Left = Convert.ToInt32(reader.GetAttribute("left"));
                                    pictureContainerPanel.Top = Convert.ToInt32(reader.GetAttribute("top"));
                                    pictureContainerPanel.Height = Convert.ToInt32(reader.GetAttribute("height"));
                                    pictureContainerPanel.Width = Convert.ToInt32(reader.GetAttribute("width"));
                                    panel1.Controls.Add(pictureContainerPanel);
                                    break;

                                case "idCard":
                                    dimensions.Height = Convert.ToInt32(reader.GetAttribute("height"));
                                    dimensions.Width = Convert.ToInt32(reader.GetAttribute("width"));
                                    String base64String;
                                    if ((base64String = reader.GetAttribute("backgroundImage")) != null)
                                    {
                                        byte[] imageBytes = Convert.FromBase64String(base64String);
                                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                        // Convert byte[] to Image
                                        ms.Write(imageBytes, 0, imageBytes.Length);
                                        backgroundImage = Image.FromStream(ms, true);


                                    }
                                    title = reader.GetAttribute("title");
                                    tableName = reader.GetAttribute("tableName");
                                    connectionString = reader.GetAttribute("connectionString");
                                    dataSourceType = reader.GetAttribute("dataSourceType");
                                    primaryKey = reader.GetAttribute("primaryKey");
                                    extraTableName = reader.GetAttribute("extraTableName");
                                    break;
                                case "field":
                                    fields.Add(reader.ReadString());
                                    break;
                                case "selectedField":
                                    selectedFields.Add(reader.ReadString());
                                    break;

                            }

                            #endregion
                        }
                    }
                idcard = new IDCard(connectionString, dataSourceType, tableName, primaryKey, dimensions, backgroundImage, fields, selectedFields, title);
                Form2_LoadFile();
            }
            catch (Exception ex) { MessageBox.Show("Invalid file format" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }

        private void tmpToolStripItem_Click(object sender, EventArgs e)
        {

            ToolStripItem clickedItem = (ToolStripItem)sender;
            Label tmp = new Label();
            tmp.BackColor = Color.Transparent;
            tmp.Text = clickedItem.Text;
            tmp.Left = X;
            tmp.Top = Y;
            tmp.AutoSize = true;
            tmp.MouseDown += tmplbl_MouseDown;
            ControlMover.Init(tmp);
            panel1.Controls.Add(tmp);
        }

        private void tmplbl_MouseDown(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Control)
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; }
                    if (ctl is PictureBox) { ((PictureBox)ctl).BorderStyle = BorderStyle.None; }
                }
            Label tmp = sender as Label;
            tmp.BorderStyle = BorderStyle.FixedSingle;
            fontToolStripComboBox.Text = tmp.Font.FontFamily.Name;
            fontSizeToolStripComboBox.Text = ((int)tmp.Font.Size).ToString();
            toolStripButton1.BackColor = tmp.ForeColor;
            toolStripButton2.BackColor = tmp.BackColor;

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                X = Cursor.Position.X - panel1.Left;
                Y = Cursor.Position.Y - panel1.Top - 30;
                Console.WriteLine(X + "lkkn :" + Y);
            }

        }

        private void panel1_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls) { if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; } }

        }

        private void Form2_Click(object sender, EventArgs e)
        {
            if (panel1 != null)
                foreach (Control ctl in panel1.Controls) { if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; } }

        }

        private void fontToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is Label) if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle) ((Label)ctl).Font = new Font(fontToolStripComboBox.Text, ctl.Font.Size);
            }
        }

        private void fontSizeToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is Label)
                {
                    if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                        ((Label)ctl).Font = new Font(ctl.Font.FontFamily.ToString(), Convert.ToInt32(fontSizeToolStripComboBox.Text));
                }
            }

        }

        private void backcolorToolStripButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is Label)
                    {
                        if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                            ((Label)ctl).BackColor = colorDialog1.Color;
                    }
                }

        }

        //private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    pictureBox1.BorderStyle = BorderStyle.FixedSingle;
        //    foreach (Control ctl in panel1.Controls) { if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; } }

        //}

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is Label)
                {
                    if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                        panel1.Controls.Remove(ctl);

                }
            }

        }

        private void forecolorToolStripButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is Label)
                    {
                        if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                            ((Label)ctl).ForeColor = colorDialog1.Color;
                    }
                }

        }

        private byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert, System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] Ret;
            //try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    Ret = ms.ToArray();
                }
            }
            //catch (Exception ex) { Console.WriteLine(ex.Message); ; }
            return Ret;
        }

        //save
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {

            //using (SqlCeConnection con = new SqlCeConnection(idcard.connectionString))
            //{

            //    string tmp = "if not exists (select * from sysobjects where name='" + idcard.tableName + "extra' and xtype='U')";
            //    con.Open();
            //    try{
            //        using (SqlCeCommand cmd = new SqlCeCommand("  create table " + idcard.tableName + "extra ( pic1 varbinary(8000) )", con))
            //    {
            //        cmd.ExecuteNonQuery();
            //    }}
            //    catch(SqlCeException ex)
            //    {
            //        Console.WriteLine("myerror :"+ex.Message);
            //    }

            //    try
            //    {
            //        using (SqlCeCommand cmd2 = new SqlCeCommand("Insert into " + idcard.tableName + "extra (pic1) Values (@Pic)", con))
            //        {

            //            cmd2.Parameters.Add("Pic", SqlDbType.Image, 0).Value =
            //                ConvertImageToByteArray(pictureBox1.BackgroundImage, System.Drawing.Imaging.ImageFormat.Jpeg);
            //            cmd2.ExecuteNonQuery();

            //        }
            //    }
            //    catch (SqlCeException ex)
            //    {
            //        Console.WriteLine("myerror2 :" + ex.Message);
            //    }

            //    using (SqlCeCommand cmd3 = new SqlCeCommand("select * from "+idcard.tableName+"extra ",con)) {
            //        SqlCeDataReader reader= cmd3.ExecuteReader();
            //        reader.Read();
            //        byte[] imageBytes = (byte[])(reader["pic1"]);
            //        MemoryStream ms = new MemoryStream(imageBytes,0,imageBytes.Length);//, );
            //        // Convert byte[] to Image
            //        ms.Write(imageBytes, 0, imageBytes.Length);
            //        panel1.BackgroundImage  = Image.FromStream(ms, true);
            //        ms.Close();
            //        //panel1.BackgroundImage = Image.FromStream(new by);//((byte[])reader["pic1"]));
            //    }
            //} 
            #region
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //    using (XmlWriter wrt = XmlWriter.Create(saveFileDialog1.FileName))
            //    {
            //        this.Text = openFileDialog1.FileName + " - IDCard Producer";
            //        wrt.WriteStartDocument();
            //        wrt.WriteStartElement("panel");
            //        foreach (Control ctl in this.panel1.Controls)
            //        {
            //            if (ctl is Label)
            //            {
            //                wrt.WriteStartElement("label");
            //                wrt.WriteAttributeString("text", ((Label)ctl).Text);
            //                wrt.WriteAttributeString("top", ((Label)ctl).Top.ToString());
            //                wrt.WriteAttributeString("left", ((Label)ctl).Left.ToString());
            //                wrt.WriteAttributeString("backcolor", ((Label)ctl).BackColor.ToArgb().ToString()); //Color.FromArgb(int);
            //                wrt.WriteAttributeString("forecolor", ((Label)ctl).ForeColor.ToArgb().ToString());
            //                wrt.WriteAttributeString("font", TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(((Label)ctl).Font)); //Font font = (Font)converter.ConvertFromString(fontString);
            //                wrt.WriteEndElement();

            //            }
            //            if (ctl is PictureBox)
            //            {
            //                wrt.WriteStartElement("pictureBox");
            //                wrt.WriteAttributeString("left", ((PictureBox)ctl).Left.ToString());
            //                wrt.WriteAttributeString("top", ((PictureBox)ctl).Top.ToString());
            //                wrt.WriteAttributeString("height", ((PictureBox)ctl).Height.ToString());
            //                wrt.WriteAttributeString("width", ((PictureBox)ctl).Width.ToString());
            //                wrt.WriteEndElement();
            //            }

            //        }

            //        wrt.WriteStartElement("idCard");
            //        wrt.WriteAttributeString("connectionString", idcard.connectionString);
            //        wrt.WriteAttributeString("height", idcard.dimensions.Height.ToString());
            //        wrt.WriteAttributeString("width", idcard.dimensions.Width.ToString());
            //        wrt.WriteAttributeString("tableName", idcard.tableName);
            //        wrt.WriteAttributeString("title", label1.Text);//idcard.title
            //        string imagebase64String;
            //        using (MemoryStream ms = new MemoryStream())
            //        {
            //            idcard.backgroundImage.Save(ms, idcard.backgroundImage.RawFormat);
            //            byte[] imageBytes = ms.ToArray();
            //            imagebase64String = Convert.ToBase64String(imageBytes);
            //        }
            //        wrt.WriteAttributeString("backgroundImage", imagebase64String);
            //        foreach (string str in idcard.fields)
            //        {

            //            wrt.WriteElementString("field", str);

            //            wrt.WriteElementString("field", str);

            //        }
            //        foreach (string str in idcard.selectedFields)
            //        {
            //            wrt.WriteElementString("selectedField", str);
            //        }
            //        wrt.WriteEndElement();//idcard
            //        wrt.WriteEndElement();//panel
            //        wrt.WriteEndDocument();
            //    }
            #endregion
        }

        //new
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            // Form1 frm = new Form1(this);
            //Hide();
            //frm.Show();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is Label)
                    {
                        if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                            ((Label)ctl).Font = fontDialog1.Font;
                    }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewCellCollection dcc = dataGridView1.SelectedRows[0].Cells;
            foreach (DataGridViewCell dc in dcc)
                foreach (Control ctl in panel1.Controls)
                    if (ctl is Label && ctl.Tag != null)
                        if (dc.OwningColumn.HeaderCell.Value.ToString() == (ctl as Label).Tag.ToString())
                            (ctl as Label).Text = dc.Value.ToString();

        }

        //Printing
        Bitmap MemoryImage;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Rectangle pagearea = e.PageBounds;
            //e.Graphics.DrawImage(MemoryImage, (pagearea.Width / 2) - (panel1.Width / 2), panel1.Location.Y);
            e.Graphics.DrawImage(MemoryImage, 0, 0);

        }

        public void GetPrintArea(Panel pnl)
        {
            MemoryImage = new Bitmap(pnl.Width, pnl.Height);
            Rectangle rect = new Rectangle(0, 0, pnl.Width, pnl.Height);
            pnl.DrawToBitmap(MemoryImage, new Rectangle(0, 0, pnl.Width, pnl.Height));
        }
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    e.Graphics.DrawImage(MemoryImage, 0, 0);
        //    base.OnPaint(e);
        //}

        public Boolean Print()
        {

            GetPrintArea(panel1);
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK) { printDocument1.Print(); return true; }
            else return false;
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            if (Print() == true)
            {
                int i = getIndexOf(idcard.primaryKey, dataGridView1);
                string id = "";
                try
                {
                     id = dataGridView1.SelectedRows[0].Cells[i].Value.ToString();
                }
                catch (Exception x)
                {
                    MessageBox.Show("Please select a record on the left before printing..!!");
                    return;
                }
                string log = "";
                string oldprinttime = "";


                string cnstr = "";
                switch (idcard.dataSourceType)
                {
                    case "Microsoft SQL Server Compact 3.5":
                        using (SqlCeConnection con = new SqlCeConnection(idcard.connectionString))
                        {
                            try
                            {
                                con.Open();
                                cnstr = "select * from " + extraTableName + " where " + idcard.primaryKey + " = '" + id + "'";
                                using (SqlCeCommand cmd1 = new SqlCeCommand(cnstr, con))
                                {

                                    SqlCeDataReader rdr = cmd1.ExecuteReader();



                                    if (rdr.Read() == false)
                                    {

                                        using (SqlCeCommand cmd2 = new SqlCeCommand("Insert into " + extraTableName + "  Values (@id,@printtime,@machineid,@log,@oldprinttime)", con))
                                        {

                                            //cmd2.Parameters.Add("Pic", SqlDbType.Image, 0).Value = ConvertImageToByteArray(pictureContainerPanel.BackgroundImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            cmd2.Parameters.Add("id", SqlDbType.NVarChar).Value = id;
                                            cmd2.Parameters.Add("printtime", SqlDbType.NVarChar).Value = DateTime.Now.ToString();
                                            cmd2.Parameters.Add("machineid", SqlDbType.NVarChar).Value = Environment.MachineName + " : " + Environment.UserDomainName + " : " + Environment.UserName;
                                            cmd2.Parameters.Add("log", SqlDbType.NVarChar).Value = "";
                                            cmd2.Parameters.Add("oldprinttime", SqlDbType.NVarChar).Value = "";
                                            cmd2.ExecuteNonQuery();

                                        }
                                        using (SqlCeCommand cmd2 = new SqlCeCommand("Insert into " + extraTableName + "pic  Values (@id,@Pic)", con))
                                        {

                                            cmd2.Parameters.Add("Pic", SqlDbType.Image, 0).Value = ConvertImageToByteArray(pictureContainerPanel.BackgroundImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            cmd2.Parameters.Add("id", SqlDbType.NVarChar).Value = id;
                                            //cmd2.Parameters.Add("printtime", SqlDbType.NVarChar).Value = DateTime.Now.ToString();
                                            //cmd2.Parameters.Add("machineid", SqlDbType.NVarChar).Value = Environment.MachineName;
                                            //cmd2.Parameters.Add("log", SqlDbType.NVarChar).Value = "";
                                            //cmd2.Parameters.Add("oldprinttime", SqlDbType.NVarChar).Value = "";
                                            cmd2.ExecuteNonQuery();

                                        }
                                    }
                                    else
                                    {
                                        oldprinttime = rdr["printtime"].ToString();
                                        InputBox ib = new InputBox();
                                        if (ib.ShowDialog() == DialogResult.OK)
                                            log = ib.value;
                                        //InputBox.show("Enter reason for re-print..");
                                        //log = InputBox.value;
                                        cnstr = "update " + extraTableName + "  set printtime = '" + DateTime.Now.ToString() + "' , log = '" + log + "' ,  oldprinttime = '" + oldprinttime + "' where id = '" + id + "'";
                                        using (SqlCeCommand cmd3 = new SqlCeCommand(cnstr, con))
                                        {
                                            cmd3.ExecuteNonQuery();
                                        }
                                        using (SqlCeCommand cmd2 = new SqlCeCommand("update " + extraTableName + "pic set pic = @Pic where id = @id", con))
                                        {

                                            cmd2.Parameters.Add("Pic", SqlDbType.Image, 0).Value = ConvertImageToByteArray(pictureContainerPanel.BackgroundImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            cmd2.Parameters.Add("id", SqlDbType.NVarChar).Value = id;
                                            //cmd2.Parameters.Add("printtime", SqlDbType.NVarChar).Value = DateTime.Now.ToString();
                                            //cmd2.Parameters.Add("machineid", SqlDbType.NVarChar).Value = Environment.MachineName;
                                            //cmd2.Parameters.Add("log", SqlDbType.NVarChar).Value = "";
                                            //cmd2.Parameters.Add("oldprinttime", SqlDbType.NVarChar).Value = "";
                                            cmd2.ExecuteNonQuery();

                                        }


                                    }

                                }
                                con.Close();
                            }

                            catch (SqlCeException ex)
                            {
                                MessageBox.Show("myerror2 :" + ex.Message + "   " + cnstr);
                                Console.WriteLine("myerror2 :" + ex.Message + "   " + cnstr);
                            }

                        }
                        break;
                    case "Microsoft SQL Server":
                        using (SqlConnection con = new SqlConnection(idcard.connectionString))
                        {
                            try
                            {
                                con.Open();
                                cnstr = "select * from " + extraTableName + " where " + idcard.primaryKey + " = '" + id + "'";
                                using (SqlCommand cmd11 = new SqlCommand(cnstr, con))
                                {

                                    SqlDataReader rdr = cmd11.ExecuteReader();



                                    if (rdr.Read() == false)
                                    {
                                        rdr.Close(); cmd11.Dispose();
                                        using (SqlCommand cmd22 = new SqlCommand("Insert into " + extraTableName + "  Values (@id,@printtime,@machineid,@log,@oldprinttime)", con))
                                        {

                                            //cmd2.Parameters.Add("Pic", SqlDbType.Image, 0).Value = ConvertImageToByteArray(pictureContainerPanel.BackgroundImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            cmd22.Parameters.Add("id", SqlDbType.NVarChar).Value = id;
                                            cmd22.Parameters.Add("printtime", SqlDbType.NVarChar).Value = DateTime.Now.ToString();
                                            cmd22.Parameters.Add("machineid", SqlDbType.NVarChar).Value = Environment.MachineName + " : " + Environment.UserDomainName + " : " + Environment.UserName;
                                            cmd22.Parameters.Add("log", SqlDbType.NVarChar).Value = "";
                                            cmd22.Parameters.Add("oldprinttime", SqlDbType.NVarChar).Value = "";
                                            cmd22.ExecuteNonQuery();

                                        }
                                        using (SqlCommand cmd222 = new SqlCommand("Insert into " + extraTableName + "pic  Values (@id,@Pic)", con))
                                        {

                                            cmd222.Parameters.Add("Pic", SqlDbType.Image, 0).Value = ConvertImageToByteArray(pictureContainerPanel.BackgroundImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            cmd222.Parameters.Add("id", SqlDbType.NVarChar).Value = id;
                                            //cmd2.Parameters.Add("printtime", SqlDbType.NVarChar).Value = DateTime.Now.ToString();
                                            //cmd2.Parameters.Add("machineid", SqlDbType.NVarChar).Value = Environment.MachineName;
                                            //cmd2.Parameters.Add("log", SqlDbType.NVarChar).Value = "";
                                            //cmd2.Parameters.Add("oldprinttime", SqlDbType.NVarChar).Value = "";
                                            cmd222.ExecuteNonQuery();

                                        }
                                    }
                                    else
                                    {
                                        oldprinttime = rdr["printtime"].ToString();
                                        InputBox ib = new InputBox();
                                        if (ib.ShowDialog() == DialogResult.OK)
                                            log = ib.value;
                                        //InputBox.show("Enter reason for re-print..");
                                        //log = InputBox.value;
                                        cnstr = "update " + extraTableName + "  set printtime = '" + DateTime.Now.ToString() + "' , log = '" + log + "' ,  oldprinttime = '" + oldprinttime + "' where id = '" + id + "'";
                                        using (SqlCommand cmd32 = new SqlCommand(cnstr, con))
                                        {
                                            cmd32.ExecuteNonQuery();
                                        }
                                        using (SqlCommand cmd21 = new SqlCommand("update " + extraTableName + "pic set pic = @Pic where id = @id", con))
                                        {

                                            cmd21.Parameters.Add("Pic", SqlDbType.Image, 0).Value = ConvertImageToByteArray(pictureContainerPanel.BackgroundImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            cmd21.Parameters.Add("id", SqlDbType.NVarChar).Value = id;
                                            //cmd21.Parameters.Add("printtime", SqlDbType.NVarChar).Value = DateTime.Now.ToString();
                                            //cmd21.Parameters.Add("machineid", SqlDbType.NVarChar).Value = Environment.MachineName;
                                            //cmd21.Parameters.Add("log", SqlDbType.NVarChar).Value = "";
                                            //cmd21.Parameters.Add("oldprinttime", SqlDbType.NVarChar).Value = "";
                                            cmd21.ExecuteNonQuery();

                                        }

                                    }

                                }
                                con.Close();
                            }

                            catch (SqlException ex)
                            {
                                MessageBox.Show("myerror2 :" + ex.Message + "   " + cnstr);
                                Console.WriteLine("myerror2 :" + ex.Message + "   " + cnstr);
                            }

                        }
                        break;
                }


            }
        }

        private int getIndexOf(string columnName, DataGridView dataGridView)
        {
            foreach (DataGridViewColumn col in dataGridView.Columns)
                if (col.HeaderText.ToLower().Trim() == columnName.ToLower().Trim())
                {
                    return dataGridView.Columns.IndexOf(col);
                }

            return -1;
        }

        WebCam webcam;
        int webcamStatus;
        int webcamstarted;
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (pictureContainerPanel != null)
                if (webcamStatus == 0)
                {
                    if (webcamstarted == 0)
                    {
                        webcam = new WebCam();
                        webcam.InitializeWebCam(ref pictureContainerPanel);
                        webcam.Start();
                        webcamstarted = 1;
                    }
                    else if (webcamstarted == 1)
                    {
                        webcam.Continue();
                    }
                    webcamStatus = 1;
                    toolStripStatusLabel1.Text = "Click on 'Capture Image' button again to capture image";

                }
                else
                {
                    webcam.Stop(); webcamStatus = 0;

                }
        }

        private void selectFieldComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            queryTxt.Enabled = true;
        }

        private void queryTxt_TextChanged(object sender, EventArgs e)
        {
            //int r = bSource.Find("Name", "qdwdwd");//("select * from " + idcard.tableName + "where Name='qdwdwd'");//customersBindingSource.Find("ContactName", contactName);
            if (queryTxt.Text.Length > 0)
                bSource.Filter = "CONVERT(" + selectFieldComboBox.Text + ", 'System.String')  like '%" + queryTxt.Text.ToString() + "%'";//"Name = 'qdwdwd'";
            //bSource.IndexOf(r);
            else bSource.Filter = null;// selectFieldComboBox.Text + " like '%%'";

            //dataGridView1.s
            //SqlCeCommandBuilder sqlCommand = new SqlCeCommandBuilder(sqlAdapter);
            //sqlAdapter.SelectCommand.Connection = new SqlCeConnection(idcard.connectionString);
            ////new SqlCeCommand("select * from "+idcard.tableName+"where Name='qdwdwd'");
            //sqlAdapter.Fill(dTable);
        }

        private void dataGridView1_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                switch (idcard.dataSourceType)
                {
                    case "Microsoft SQL Server Compact 3.5":
                        sqlCeAdapter.Update(dTable);
                        break;
                    case "Microsoft SQL Server":
                        sqlAdapter.Update(dTable);
                        break;
                }
            }
            catch (Exception x) { }
        }

        private void queryTxt_Click(object sender, EventArgs e)
        {
            if (queryTxt.Text == "Enter query...")
                queryTxt.SelectAll();


        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            Form1 frm= new Form1(idcard,extraTableName);
            frm.Show();
        }






    }
}
