using System;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.Configuration;
using System.Threading;
using System.Collections;
using JSG.PhotoPropertiesLibrary;

namespace JSG.PhotoPropertiesApp {
	/// <summary>
	/// Summary description for PhotoPropertiesForm.
	/// </summary>
	public class PhotoPropertiesForm : System.Windows.Forms.Form {
		private System.Windows.Forms.OpenFileDialog _openPhotoDialog;
		private System.Windows.Forms.SaveFileDialog _saveXMLFileDialog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MainMenu _mainMenu;
		private System.Windows.Forms.MenuItem _menuItem_File;
		private System.Windows.Forms.MenuItem _menuItem_Analyze;
		private System.Windows.Forms.MenuItem _menuItem_SaveAs;
		private System.Windows.Forms.MenuItem _menuItem_Separator1;
		private System.Windows.Forms.MenuItem _menuItem_Exit;
		private System.Windows.Forms.MenuItem _menuItem_Format;
		private System.Windows.Forms.MenuItem _menuItem_AddXSLT;
		private System.Windows.Forms.MenuItem _menuItem_Help;
		private System.Windows.Forms.MenuItem _menuItem_About;
		private System.Windows.Forms.TextBox _tbOutput;
		private System.Windows.Forms.Splitter _splitter;
		private System.Windows.Forms.Panel _pnlView;
		private System.Windows.Forms.Panel _pnlSubView;
		private System.Windows.Forms.ListView _listView;
		private System.Windows.Forms.ColumnHeader _colHdrTag;
		private System.Windows.Forms.ColumnHeader _colHdrCategory;
		private System.Windows.Forms.ColumnHeader _colHdrName;
		private System.Windows.Forms.ColumnHeader _colHdrValue;
		private System.Windows.Forms.TextBox _viewDescription;

		/// <summary>Define the InitializePhotoPropertiesDelegate delegate signature.</summary>
		private delegate bool InitializePhotoPropertiesDelegate(string xmlFileName);

		/// <summary>The name of this application.</summary>
		private const string APPTITLE = "PhotoProperties";
		/// <summary>The error title for this application.</summary>
		private const string APPERRORTITLE = APPTITLE + " Error";

		/// <summary>An instance of the PhotoProperties class</summary>
		private PhotoProperties _photoProps;
		/// <summary>Has the PhotoProperties instance successfully initialized?</summary>
		private bool _photoPropsInitialized = false;
		/// <summary>An instance of the ResultOptions class defining 
		/// properties used when creating XML output.</summary>
		private ResultOptions _resultOptions;
		/// <summary>The ListViewColumnSorter for the _listView control.</summary>
		private ListViewColumnSorter lvwColumnSorter;
		/// <summary>Was the current analysis successful?</summary>
		private bool _isAnalysisSuccessful = false;


		/// <summary>
		/// PhotoPropertiesForm Constructor.</summary>
		public PhotoPropertiesForm() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this._tbOutput.BackColor = System.Drawing.SystemColors.Info;

			// Create an instance of a ListView column sorter and 
			// assign it to the _listView control.
			lvwColumnSorter = new ListViewColumnSorter();
			this._listView.ListViewItemSorter = lvwColumnSorter;

			_resultOptions = new ResultOptions();
		}

		/// <summary>Clean up any resources being used.</summary>
		protected override void Dispose( bool disposing ) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PhotoPropertiesForm));
			this._openPhotoDialog = new System.Windows.Forms.OpenFileDialog();
			this._saveXMLFileDialog = new System.Windows.Forms.SaveFileDialog();
			this._mainMenu = new System.Windows.Forms.MainMenu();
			this._menuItem_File = new System.Windows.Forms.MenuItem();
			this._menuItem_Analyze = new System.Windows.Forms.MenuItem();
			this._menuItem_SaveAs = new System.Windows.Forms.MenuItem();
			this._menuItem_Separator1 = new System.Windows.Forms.MenuItem();
			this._menuItem_Exit = new System.Windows.Forms.MenuItem();
			this._menuItem_Format = new System.Windows.Forms.MenuItem();
			this._menuItem_AddXSLT = new System.Windows.Forms.MenuItem();
			this._menuItem_Help = new System.Windows.Forms.MenuItem();
			this._menuItem_About = new System.Windows.Forms.MenuItem();
			this._tbOutput = new System.Windows.Forms.TextBox();
			this._splitter = new System.Windows.Forms.Splitter();
			this._pnlView = new System.Windows.Forms.Panel();
			this._pnlSubView = new System.Windows.Forms.Panel();
			this._listView = new System.Windows.Forms.ListView();
			this._colHdrTag = new System.Windows.Forms.ColumnHeader();
			this._colHdrCategory = new System.Windows.Forms.ColumnHeader();
			this._colHdrName = new System.Windows.Forms.ColumnHeader();
			this._colHdrValue = new System.Windows.Forms.ColumnHeader();
			this._viewDescription = new System.Windows.Forms.TextBox();
			this._pnlView.SuspendLayout();
			this._pnlSubView.SuspendLayout();
			this.SuspendLayout();
			// 
			// _openPhotoDialog
			// 
			this._openPhotoDialog.Filter = "Image Files (*.jpg,*.jpeg,*.png,*.tif,*.tiff)|*.jpg;*.jpeg;*.png;*.tif;*.tiff|All Files (*.*)" +
				"|*.*";
			// 
			// _saveXMLFileDialog
			// 
			this._saveXMLFileDialog.FileName = "XmlOut1";
			this._saveXMLFileDialog.Filter = "XML Files (*.xml)|*.xml;|All Files (*.*)|";
			// 
			// _mainMenu
			// 
			this._mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this._menuItem_File,
																					  this._menuItem_Format,
																					  this._menuItem_Help});
			// 
			// _menuItem_File
			// 
			this._menuItem_File.Index = 0;
			this._menuItem_File.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this._menuItem_Analyze,
																						   this._menuItem_SaveAs,
																						   this._menuItem_Separator1,
																						   this._menuItem_Exit});
			this._menuItem_File.Text = "&File";
			this._menuItem_File.Popup += new System.EventHandler(this._menuItem_File_Popup);
			// 
			// _menuItem_Analyze
			// 
			this._menuItem_Analyze.Enabled = false;
			this._menuItem_Analyze.Index = 0;
			this._menuItem_Analyze.Text = "Analyze &Image File...";
			this._menuItem_Analyze.Click += new System.EventHandler(this._menuItem_Analyze_Click);
			// 
			// _menuItem_SaveAs
			// 
			this._menuItem_SaveAs.Enabled = false;
			this._menuItem_SaveAs.Index = 1;
			this._menuItem_SaveAs.Text = "Save &As...";
			this._menuItem_SaveAs.Click += new System.EventHandler(this._menuItem_SaveAs_Click);
			// 
			// _menuItem_Separator1
			// 
			this._menuItem_Separator1.Index = 2;
			this._menuItem_Separator1.Text = "-";
			// 
			// _menuItem_Exit
			// 
			this._menuItem_Exit.Index = 3;
			this._menuItem_Exit.Text = "E&xit";
			this._menuItem_Exit.Click += new System.EventHandler(this._menuItem_Exit_Click);
			// 
			// _menuItem_Format
			// 
			this._menuItem_Format.Index = 1;
			this._menuItem_Format.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							 this._menuItem_AddXSLT});
			this._menuItem_Format.Text = "For&mat";
			// 
			// _menuItem_AddXSLT
			// 
			this._menuItem_AddXSLT.Checked = false;
			this._menuItem_AddXSLT.Index = 0;
			this._menuItem_AddXSLT.Text = "&Add Output XLST";
			this._menuItem_AddXSLT.Click += new System.EventHandler(this.DefaultMenuItemClick);
			// 
			// _menuItem_Help
			// 
			this._menuItem_Help.Index = 2;
			this._menuItem_Help.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this._menuItem_About});
			this._menuItem_Help.Text = "&Help";
			// 
			// _menuItem_About
			// 
			this._menuItem_About.Index = 0;
			this._menuItem_About.Text = "&About...";
			this._menuItem_About.Click += new System.EventHandler(this._menuItem_About_Click);
			// 
			// _tbOutput
			// 
			this._tbOutput.Dock = System.Windows.Forms.DockStyle.Top;
			this._tbOutput.Enabled = false;
			this._tbOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this._tbOutput.Multiline = true;
			this._tbOutput.Name = "_tbOutput";
			this._tbOutput.ReadOnly = true;
			this._tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this._tbOutput.Size = new System.Drawing.Size(592, 120);
			this._tbOutput.TabIndex = 0;
			this._tbOutput.Text = "\r\nInitializing...";
			this._tbOutput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// _splitter
			// 
			this._splitter.BackColor = System.Drawing.SystemColors.Control;
			this._splitter.Dock = System.Windows.Forms.DockStyle.Top;
			this._splitter.Location = new System.Drawing.Point(0, 120);
			this._splitter.Name = "_splitter";
			this._splitter.Size = new System.Drawing.Size(592, 3);
			this._splitter.TabIndex = 1;
			this._splitter.TabStop = false;
			// 
			// _pnlView
			// 
			this._pnlView.BackColor = System.Drawing.SystemColors.Control;
			this._pnlView.Controls.AddRange(new System.Windows.Forms.Control[] {
																				   this._pnlSubView,
																				   this._viewDescription});
			this._pnlView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pnlView.Location = new System.Drawing.Point(0, 123);
			this._pnlView.Name = "_pnlView";
			this._pnlView.Size = new System.Drawing.Size(592, 350);
			this._pnlView.TabIndex = 2;
			// 
			// _pnlSubView
			// 
			this._pnlSubView.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this._listView});
			this._pnlSubView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pnlSubView.Name = "_pnlSubView";
			this._pnlSubView.Size = new System.Drawing.Size(592, 310);
			this._pnlSubView.TabIndex = 0;
			// 
			// _listView
			// 
			this._listView.AllowColumnReorder = true;
			this._listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this._colHdrTag,
																						this._colHdrCategory,
																						this._colHdrName,
																						this._colHdrValue});
			this._listView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._listView.FullRowSelect = true;
			this._listView.GridLines = true;
			this._listView.MultiSelect = false;
			this._listView.Name = "_listView";
			this._listView.Size = new System.Drawing.Size(592, 310);
			this._listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this._listView.TabIndex = 0;
			this._listView.View = System.Windows.Forms.View.Details;
			this._listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this._listView_ColumnClick);
			this._listView.SelectedIndexChanged += new System.EventHandler(this._listView_SelectedIndexChanged);
			// 
			// _colHdrTag
			// 
			this._colHdrTag.Text = "Tag";
			// 
			// _colHdrCategory
			// 
			this._colHdrCategory.Text = "Category";
			this._colHdrCategory.Width = 100;
			// 
			// _colHdrName
			// 
			this._colHdrName.Text = "Name";
			this._colHdrName.Width = 175;
			// 
			// _colHdrValue
			// 
			this._colHdrValue.Text = "Value";
			this._colHdrValue.Width = 250;
			// 
			// _viewDescription
			// 
			this._viewDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._viewDescription.Location = new System.Drawing.Point(0, 310);
			this._viewDescription.Multiline = true;
			this._viewDescription.Name = "_viewDescription";
			this._viewDescription.ReadOnly = true;
			this._viewDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._viewDescription.Size = new System.Drawing.Size(592, 40);
			this._viewDescription.TabIndex = 1;
			this._viewDescription.Text = "";
			// 
			// PhotoPropertiesForm
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(592, 473);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._pnlView,
																		  this._splitter,
																		  this._tbOutput});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this._mainMenu;
			this.MinimumSize = new System.Drawing.Size(488, 120);
			this.Name = "PhotoPropertiesForm";
			this.Text = "PhotoProperties";
			this.Load += new System.EventHandler(this.PhotoPropertiesForm_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.PhotoPropertiesForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.PhotoPropertiesForm_DragEnter);
			this._pnlView.ResumeLayout(false);
			this._pnlSubView.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main() {
			Application.Run(new PhotoPropertiesForm());
		}

		/// <summary>
		/// Initializes any photo properties values</summary>
		private void PhotoPropertiesForm_Load(object sender, System.EventArgs e) {

			// Get the configuration settings (from the <app>.exe.config file)
			string initXmlFile = ConfigurationSettings.AppSettings["InitializationXmlFile"];
			_resultOptions.XMLNamespace = ConfigurationSettings.AppSettings["OutputXmlNamespace"];
			_resultOptions.XSLTransform = ConfigurationSettings.AppSettings["OutputXSLTransform"];

			InitializePhotoProperties(initXmlFile);
		}

		/// <summary>
		/// Initializes the PhotoProperties tag properties using asynchronous 
		/// method invocation of the initialization.</summary>
		private void InitializePhotoProperties(string initXmlFile) {
			// Create an instance of the PhotoProperties
			_photoProps = new PhotoProperties();

			// Use the asynchronous method invocation of the initialization delegate
			// to initialize the photo properties in a worker thread.
			InitializePhotoPropertiesDelegate initDelegate = 
				new InitializePhotoPropertiesDelegate(_photoProps.Initialize);
			AsyncCallback callback = new AsyncCallback(InitializePhotoPropertiesCompleted);
			initDelegate.BeginInvoke(initXmlFile, callback, initDelegate);
		}

		/// <summary>
		/// The callback method that is automatically called 
		/// when the async invoke has completed.</summary>
		/// <remarks>This function will be called on the pool thread.</remarks>
		public void InitializePhotoPropertiesCompleted(IAsyncResult call) {
			InitializePhotoPropertiesDelegate init = (InitializePhotoPropertiesDelegate)call.AsyncState;
			bool isInitialized = init.EndInvoke(call);

			if (isInitialized) {
				// Raise the continue loading 'event'
				BeginInvoke(new MethodInvoker(ContinueLoadingEvent));
			}
			else {
				string errMessage = _photoProps.GetInitializeErrorMessage();
				if (errMessage == String.Empty)
					errMessage = "An unknown error occurred.";

				MessageBox.Show(errMessage,
					APPERRORTITLE,
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error); 

				// close the application
				this.Close();
			}
		}

		/// <summary>
		/// This 'event' is raised when the initialization of the 
		/// PhotoProperties tag properties completed successfully.</summary>
		/// <remarks>It will be called on the UI thread.</remarks>
		private void ContinueLoadingEvent() {
			// reset the controls
			this._tbOutput.Text = "";
			this._tbOutput.Font = TextBox.DefaultFont;
			this._tbOutput.TextAlign = HorizontalAlignment.Left;
			this._tbOutput.Enabled = true;

			_photoPropsInitialized = true;
		}

		/// <summary>
		/// Checks/unchecks the menuitem (sender).</summary>
		private void DefaultMenuItemClick(Object sender, EventArgs e) {
			MenuItem mnu = (MenuItem)sender;
			mnu.Checked = !mnu.Checked;
		}

		/// <summary>
		/// Analyzes a given image file.</summary>
		/// <param name="fileName">A valid image file.</param>
		/// <returns>True if the analysis was completed without any errors.</returns>
		private bool AnalyzeImageFile(string fileName) {
			bool isAnalyzed = false;
			try {
				_photoProps.Analyze(fileName);
				isAnalyzed = true;
			}
			catch (InvalidOperationException ex) {
				MessageBox.Show(ex.Message,
					"Analyze Image Exception",
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error); 
			}
			return isAnalyzed;
		}

		/// <summary>
		/// Displays a newly created XML Output of the image file analysis.</summary>
		/// <remarks>The output can be modified through various selections 
		/// of the menu formatting options.</remarks>
		private void ViewAnalysis() {
			// Set the output properties
			_resultOptions.IncludeXSLT = this._menuItem_AddXSLT.Checked;

			// Create the XML output of the analysis in a memory stream
			MemoryStream memStream = new MemoryStream();
			_photoProps.WriteXml(memStream, _resultOptions);

			// If the stream is closed, create a new stream 
			// with the saved buffer
			if (memStream.CanRead == false) {
				byte[] buf = memStream.GetBuffer();
				memStream = new MemoryStream(buf);
			}

			// Load the stream into an XmlDocument
			XmlDocument doc = new XmlDocument();
			doc.PreserveWhitespace = true;
			XmlTextReader reader = new XmlTextReader(memStream);
			doc.Load(reader);  

			// Display the XML output
			_tbOutput.Text = doc.InnerXml;

			// For example the output would include tagDatum elements such as:
			//    <tagDatum id="33434" category="EXIF">
			//      <name>ExposureTime</name>
			//      <description>Exposure time, measured in seconds.</description>
			//      <value>1/4</value>
			//    </tagDatum>

			// Display all the tagDatum nodes in the following four columns:
			//		Tag, Category, Name, and Value.
			// The specific description is shown when a particular tagDatum (row) is selected.
			XmlNodeList elemList = doc.GetElementsByTagName("tagDatum");
			for (int i = 0; i < elemList.Count; i++) {
				XmlNode elem = elemList[i];
				XmlAttributeCollection attrColl = elem.Attributes;
				string tdId = attrColl["id"].Value;
				if (tdId == null || tdId.Length == 0)
					continue;
				string tdCategory = attrColl["category"].Value;
				string tdName = null;
				string tdValue = null;
				string tdDescrip = null;
				string tdPPValue = null;
				for (int j = 0; j < elem.ChildNodes.Count; j++) {
					switch (elem.ChildNodes[j].Name) {
						case "name":
							tdName = elem.ChildNodes[j].InnerXml;
							break;
						case "description":
							tdDescrip = elem.ChildNodes[j].InnerXml;
							break;
						case "value":
							tdValue = elem.ChildNodes[j].InnerXml;
							break;
						case "prettyPrintValue":
							tdPPValue = elem.ChildNodes[j].InnerXml;
							break;
					}
				}
				// View the Id in Hex format
				string tagId = "0x" + Convert.ToInt32(tdId).ToString("X4");
				ListViewItem lvi = _listView.Items.Add(tagId);
				lvi.Tag = tdDescrip;
				// Show the category (or "-")
				if (tdCategory != null && tdCategory != String.Empty)
					_listView.Items[i].SubItems.Add(tdCategory);
				else
					_listView.Items[i].SubItems.Add("-");
				// Show the name
				if (tdName != null)
					_listView.Items[i].SubItems.Add(tdName);
				// Show the result:
				//   If available, show the prettyprint value
				//   Else show the value,
				//   Else show "-".
				if (tdPPValue != null)
					_listView.Items[i].SubItems.Add(tdPPValue);
				else if (tdValue != null)
					_listView.Items[i].SubItems.Add(tdValue);
				else
					_listView.Items[i].SubItems.Add("-");
			}
		}

		/// <summary>
		/// Saves a newly created XML Output of the image file analysis.</summary>
		/// <remarks>The output can be modified through various selections 
		/// of the menu formatting options.</remarks>
		/// <param name="fileName"></param>
		private void SaveAnalysis(string fileName) {
			// Set the output properties
			_resultOptions.IncludeXSLT = this._menuItem_AddXSLT.Checked;
			// Create the XML output
			_photoProps.WriteXml(fileName, _resultOptions);
		}

		/// <summary>
		/// Analyzes an image file.
		/// If successful, the result is displayed.</summary>
		private void AnalyzeAndReport(string fileName) {
			try {
				// Set the window title
				this.Text = PhotoPropertiesForm.APPTITLE 
					+ " - " 
					+ Path.GetFileName(fileName);

				// Change the cursor to wait cursor
				Cursor.Current = Cursors.WaitCursor;

				// Clear the text box
				_tbOutput.Clear();

				// Clear the list view
				_listView.Items.Clear();

				// Analyze the image file and view the XML results.
				_isAnalysisSuccessful = AnalyzeImageFile(fileName);
				if (_isAnalysisSuccessful)
					ViewAnalysis();
			}
			catch (System.Exception ex) {
				MessageBox.Show(ex.Message,
					"Analyze Image Exception",
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error); 
			}
			finally {
				// Change the Cursor back to arrow
				Cursor.Current = Cursors.Default;
			}
		}

		/// <summary>
		/// Analyzes a user-selected image file.
		/// If successful, the result is displayed.</summary>
		private void _menuItem_Analyze_Click(object sender, System.EventArgs e) {
			OpenFileDialog ofd = _openPhotoDialog;
			DialogResult dlgResult = ofd.ShowDialog();
			if (DialogResult.OK == dlgResult)
				AnalyzeAndReport(ofd.FileName);
		}

		/// <summary>
		/// Saves the current analysis in a user-selected file.</summary>
		private void _menuItem_SaveAs_Click(object sender, System.EventArgs e) {
			if (_isAnalysisSuccessful == false)
				return;

			SaveFileDialog sfd = this._saveXMLFileDialog;
			// default to the image filename (w/o extension)
			sfd.FileName = Path.GetFileNameWithoutExtension(_photoProps.ImageFileName);
			DialogResult dlgResult = sfd.ShowDialog();
			if (DialogResult.OK == dlgResult) {
				try {
					// Change the cursor to wait cursor
					Cursor.Current = Cursors.WaitCursor;

					// Save the analysis
					SaveAnalysis(sfd.FileName);
				}
				catch (System.Exception ex) {
					Console.WriteLine(ex.ToString());
					MessageBox.Show(ex.Message,
						"Save Analysis Exception",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error); 
				}
				finally {
					// Change the Cursor back to arrow
					Cursor.Current = Cursors.Default;
				}
			}
		}

		/// <summary>
		/// Exits the application.</summary>
		private void _menuItem_Exit_Click(object sender, System.EventArgs e) {
			this.Close();
		}

		/// <summary>
		/// Sets the enable status of the file menu's subitems.</summary>
		private void _menuItem_File_Popup(object sender, System.EventArgs e) {
			// Enabled after the photo properties was initialized.
			this._menuItem_Analyze.Enabled = this._photoPropsInitialized;
			// Enabled after an analysis was completed.
			this._menuItem_SaveAs.Enabled = _isAnalysisSuccessful;
		}

		/// <summary>
		/// Occurs when the selected item in the list view control changes.</summary>
		private void _listView_SelectedIndexChanged(object sender, System.EventArgs e) {
			if (this._listView.SelectedItems.Count > 0) {
				ListViewItem lvi = this._listView.SelectedItems[0];
				_viewDescription.Text = (string)lvi.Tag;
			}
		}

		/// <summary>
		/// Occurs when a column header is clicked.</summary>
		private void _listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e) {
			// Determine if clicked column is already the column that is being sorted.
			if (e.Column == lvwColumnSorter.SortColumn) {
				// Reverse the current sort direction for this column.
				if (lvwColumnSorter.Order == SortOrder.Ascending) {
					lvwColumnSorter.Order = SortOrder.Descending;
				}
				else {
					lvwColumnSorter.Order = SortOrder.Ascending;
				}
			}
			else {
				// Set the column number that is to be sorted; default to ascending.
				lvwColumnSorter.SortColumn = e.Column;
				lvwColumnSorter.Order = SortOrder.Ascending;
			}

			// Perform the sort with these new sort options.
			this._listView.Sort();
		}

		/// <summary>
		/// Occurs when one or more files are dragged to the application.</summary>
		private void PhotoPropertiesForm_DragEnter(object sender, System.Windows.Forms.DragEventArgs e) {
			// only accept files
			if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
				e.Effect = DragDropEffects.Copy;
		}

		/// <summary>
		/// Occurs when the files are dropped on the application.
		/// The application only accepts a single file; 
		/// if more than one file is dropped, a warning message is displayed.
		/// </summary>
		private void PhotoPropertiesForm_DragDrop(object sender, System.Windows.Forms.DragEventArgs e) {
			// Save the filename.
			// If more than one file was dragged, only accept the first
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files.Length > 1)
				MessageBox.Show("Only one file may be dropped on the application.",
					APPERRORTITLE,
					MessageBoxButtons.OK, 
					MessageBoxIcon.Warning);
			else
				AnalyzeAndReport(files[0]);
		}

		/// <summary>Displays the About box.</summary>
		private void _menuItem_About_Click(object sender, System.EventArgs e) {
			string title = String.Empty;
			string descrip = String.Empty;
			string copyright = String.Empty;
			string version = String.Empty;

			Assembly assembly = Assembly.GetExecutingAssembly();

			// Get the title, description and company
			object[] attributes = assembly.GetCustomAttributes(true);   
			foreach (object attribute in attributes) {
				if (attribute is AssemblyTitleAttribute)
					title = ((AssemblyTitleAttribute)attribute).Title;
				if (attribute is AssemblyDescriptionAttribute)
					descrip = ((AssemblyDescriptionAttribute)attribute).Description;
				if (attribute is AssemblyCopyrightAttribute)
					copyright = ((AssemblyCopyrightAttribute)attribute).Copyright;
			}
			version = String.Format("{0}.{1}", 
				assembly.GetName().Version.Major,
				assembly.GetName().Version.Minor);

			string about = title +
				Environment.NewLine +
				descrip +
				Environment.NewLine +
				Environment.NewLine +
				"Version " + version +
				Environment.NewLine +
				copyright;
			MessageBox.Show(about, "About " + title);
		}
	}
}
