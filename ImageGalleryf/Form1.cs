using System;
using System.Collections.Generic;
using System.Drawing;
using C1.Win.C1Tile;
using System.Windows.Forms;
using System.IO;
using C1.Win.C1SplitContainer;


namespace ImageGalleryf
{
    public partial class Form1 : Form
    {
        private TextBox textBox1;

        DataFetcher datafetch = new DataFetcher(); 
        List<ImageItem> imagesList;
        int checkedItems = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(810, 810);
            this.MaximizeBox = false;
            this.ShowIcon = false;
            this.Size = new Size(780, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ImageGallery";

            




            // Using C1 SplitContainer 
            /*  C1SplitContainer split = new C1SplitContainer();
              //create a new panel for the split container
              C1SplitterPanel panel1 = new C1SplitterPanel();
              C1SplitterPanel panel2 = new C1SplitterPanel();
              //add panel1 to the splitcontainer
              split.Panels.Add(panel1);
              split.Panels.Add(panel2);
              split.Dock = DockStyle.Fill;

              panel1.TabIndex = 1;
              panel2.TabIndex = 0;
              panel2.AutoSize = true;
              panel2.AutoScroll = true;


              panel1.Text = "Panel 1";
              panel2.Text = "Panel 2";
              //add the splitcontainer
             // Controls.Add(split);*/


            //////Split container////////
            SplitContainer splitContainer1 = new SplitContainer();

             splitContainer1 = new SplitContainer();
           splitContainer1.Name = "splitContainer1";

            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Orientation = Orientation.Horizontal;
            splitContainer1.SplitterDistance = 40;
            splitContainer1.Margin = new Padding(2, 2, 2, 2);
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Panel1MinSize = 25;
            splitContainer1.Panel2MinSize = 25;
            splitContainer1.Panel1.AutoScroll = true;
            splitContainer1.Panel2.AutoScroll = true;
          

            

            //Adding Tablelayout to the  panel1
            TableLayoutPanel tablelayout = new TableLayoutPanel();
            tablelayout.ColumnCount = 3;
            tablelayout.Dock = DockStyle.Fill;
            tablelayout.RowCount = 1;
            tablelayout.Size = new Size(800, 40);
            tablelayout.Location = new Point(0, 0);
            for(int i = 0; i < tablelayout.ColumnCount; i++)
            {
                if (i == 0) {
                    tablelayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

                }
                else
                {
                    tablelayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.50F));
                }



            }

            splitContainer1.Panel1.Controls.Add(tablelayout);
            

          //Addding panel to the table layout column 2
            Panel panel = new Panel();
            panel.Location = new Point(477, 0);
            panel.Size = new Size(287, 40);
            panel.Dock = DockStyle.Fill;

            panel.Paint += OnSearchPanelPaint;
            tablelayout.Controls.Add(panel,1,0);

            
             //Adding TextBox to the table layout column 2

            TextBox txtbox = new TextBox();
            txtbox.Name = "_searchBox";
            txtbox.BorderStyle = BorderStyle.None;
            txtbox.Dock = DockStyle.Fill;
            txtbox.Location = new Point(16, 9);
            txtbox.Size = new Size(244, 16);
            txtbox.Text = "Search Image";
            tablelayout.Controls.Add(txtbox, 1, 0);
            txtbox.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left);
            txtbox.DoubleClick += OnTextbox;
            panel.Controls.Add(txtbox);
            
           

           // Add panel to 3rd column of TableLayout

            Panel panelsearch = new Panel();
            panelsearch.Location = new Point(479, 12);
            panelsearch.Margin = new Padding(2, 12, 45, 12);
            panelsearch.Size = new Size(40, 16);
            panelsearch.TabIndex = 1;
            panelsearch.Paint += OnSearchPanelPaint; 
            tablelayout.Controls.Add(panelsearch, 2, 0);

            // Add Picture Box Control to this panel

            PictureBox picbox = new PictureBox();
            picbox.Name = "_search";
            picbox.Dock = DockStyle.Left;
            picbox.Location = new Point(0, 0);
            picbox.Margin = new Padding(0, 0, 0, 0);
            picbox.Size = new Size(40, 16);
            picbox.SizeMode = PictureBoxSizeMode.Zoom;
            picbox.Image = Properties.Resources.searchicon;
            picbox.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left);
            panelsearch.Controls.Add(picbox);
            panelsearch.Click += OnSearchClick;


            // Picture Box For PDF Export
            PictureBox picbox2 = new PictureBox();
            picbox2.Name = "_exportImage";
            picbox2.Location = new Point(29, 3);
            picbox2.Size = new Size(135, 28);
            picbox2.SizeMode = PictureBoxSizeMode.StretchImage;
            picbox2.Visible = false;
            picbox2.Dock = DockStyle.Top;
            picbox2.Image = Properties.Resources.exportpdf;
            splitContainer1.Panel2.Controls.Add(picbox2);
            picbox2.Click += OnExportClick;
            picbox2.Paint += OnExportImagePaint;


            //Adding tile Control
            C1TileControl tileControl = new C1TileControl();
            tileControl.Name = "_imageTileControl";
            tileControl.AllowRearranging = true;
            tileControl.CellHeight = 78;
            tileControl.CellSpacing = 11;          
            tileControl.CellWidth = 78;
            tileControl.Dock = DockStyle.Fill;
            tileControl.Size = new Size(764, 718);
            tileControl.SurfacePadding = new Padding(12, 4, 12, 4);
            tileControl.SwipeDistance = 20;
            tileControl.SwipeRearrangeDistance = 98;
            tileControl.Paint += OnTileControlPaint;
            tileControl.TileChecked += OnTileChecked;
            tileControl.TileUnchecked += OnTileUnchecked;
           splitContainer1.Panel2.Controls.Add(tileControl);

            //PDf Control
            C1.C1Pdf.C1PdfDocument imagePdfDocument = new C1.C1Pdf.C1PdfDocument();

            //StatusStrip Control

            StatusStrip statusStrip = new StatusStrip();
            statusStrip.Dock = DockStyle.Bottom;
            statusStrip.Visible = false;
            
            //Progress Bar

            ToolStripProgressBar toolStripProgressBar = new ToolStripProgressBar();
            toolStripProgressBar.Style = ProgressBarStyle.Marquee;
            this.Controls.Add(splitContainer1);


            //Save image in local Resources
            this.textBox1 = new TextBox();
            this.textBox1.Visible = false;
            this.textBox1.AcceptsReturn = true;
            this.textBox1.AcceptsTab = true;
            this.textBox1.Dock = DockStyle.Fill;
            this.textBox1.Multiline = true;
            this.textBox1.ScrollBars = ScrollBars.Vertical;
            this.textBox1.Click += button1_Click;


        }




        //Your Event Here
        //Adding Tiles

        private void AddTiles(List<ImageItem> imageList)
        {

            _imageTileControl.Groups[0].Tiles.Clear();

            foreach (var imageitem in imageList)
            {
                Tile tile = new Tile(); tile.HorizontalSize = 2;
                tile.VerticalSize = 2;
                _imageTileControl.Groups[0].Tiles.Add(tile);
                Image img = Image.FromStream(new
                MemoryStream(imageitem.Base64));

                Template tl = new Template();
                ImageElement ie = new ImageElement();
                ie.ImageLayout = ForeImageLayout.Stretch;
                tl.Elements.Add(ie);
                tile.Template = tl;
                tile.Image = img;

            }

        }


        //Exporting Image in pdf
       
        private void OnExportClick(object sender, EventArgs e)
        {
            List<Image> images = new List<Image>();
            foreach (Tile tile in _imageTileControl.Groups[0].Tiles)
            {
                if (tile.Checked)
                {
                    images.Add(tile.Image);

                }
            }

            ConvertToPdf(images);
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "pdf";
            saveFile.Filter = "PDF files (*.pdf)|*.pdf*";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {

                imagePdfDocument.Save(saveFile.FileName);

            }
        }
       

        // Converting Image into Pdf
        private void ConvertToPdf(List<Image> images)
        {
            RectangleF rect = imagePdfDocument.PageRectangle;
            bool firstPage = true;
            foreach (var selectedimg in images)
            {
                if (!firstPage)
                {
                    imagePdfDocument.NewPage();
                }
                firstPage = false;

                rect.Inflate(-72, -72); 
                imagePdfDocument.DrawImage(selectedimg, rect);

            }

        }


        //Draw Border or give Look and feel to the Export Image button
        private void OnExportImagePaint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(_exportImage.Location.X,
            _exportImage.Location.Y, _exportImage.Width, _exportImage.Height);
            r.X -= 29;
            r.Y -= 3;
            r.Width--;
            r.Height--;
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawRectangle(p, r);
            e.Graphics.DrawLine(p, new Point(0, 43), new
                Point(this.Width, 43));

        }

        //If the Tile Is Checked
        private void OnTileChecked(object sender, TileEventArgs e)
        {
            checkedItems++;
            _exportImage.Visible = true;
            textBox1.Visible = true;

        }

        //UnChecked Tile
        private void OnTileUnchecked(object sender, TileEventArgs e)
        {
            checkedItems--;
            _exportImage.Visible = checkedItems > 0;


        }


        //Designing the Tile 
        private void OnTileControlPaint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawLine(p, 0, 43, 800, 43);
        }


        //When Clicked on  Search
        private async void OnSearchClick(object sender, EventArgs e)
        {
            statusStrip1.Visible = true;
            try
            {
                imagesList = await
               datafetch.GetImageData(_searchBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please Enter correct imagename");
                throw ex;
                
            }
            AddTiles(imagesList);
            statusStrip1.Visible = false;
        }


        //When Double Clicked on Search Strip
        private void OnTextbox(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Images";
            ofd.Multiselect = true;
            ofd.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" +
             "All files (*.*)|*.*";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                foreach (String file in ofd.FileNames)
                {
                    try
                    {
                        PictureBox imageControl = new PictureBox();
                        imageControl.Height = 400;
                        imageControl.Width = 400;

                        Image.GetThumbnailImageAbort myCallback =
                                new Image.GetThumbnailImageAbort(ThumbnailCallback);
                        Bitmap myBitmap = new Bitmap(file);
                        Image myThumbnail = myBitmap.GetThumbnailImage(300, 300,
                            myCallback, IntPtr.Zero);
                        imageControl.Image = myThumbnail;


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }

        }

        //Displaying Image Thumbnail
        public bool ThumbnailCallback()
        {
            return false;
        }


        //Saving image in the local File System( In this case: Images)
        private void button1_Click(object sender,EventArgs e)
        {

            try
            {
                File.Copy(textBox1.Text, Path.Combine(@"Images\", Path.GetFileName(textBox1.Text)), true);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Plaease make Images folder");
                throw ex;
            }

          
        }
        
        
        // Designing the Search Panel
        private void OnSearchPanelPaint(object sender, PaintEventArgs e)
        {
           
            try
            {
              TextBox txt =  (TextBox)this.Controls.Find("_searchBox",true)[0];
                Rectangle r =txt.Bounds;
                r.Inflate(3, 3);
                Pen p = new Pen(Color.LightGray);
                e.Graphics.DrawRectangle(p, r);

            }
            catch(NullReferenceException ex)
            {
                MessageBox.Show("String is null");
                throw ex;
            }

        }


    }
}