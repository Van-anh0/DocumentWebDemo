using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentLayer;
using DocumentLayer.BaseObjectMethods;
using DocumentLayer.MasterSystem;
namespace DocumentWebDemo.pages
{
    public partial class DocumentUI : Page
    {
        public List<Document> ls = new List<Document>();


        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsLogin)
            //  Response.Redirect("Login_Page.aspx");

            if (!Page.IsPostBack)
            {
                this.hPageIndex.Value = "0";
                this.dlPageNumber.Text = Global.g_PageSize;
                this.LoadTimKiem(0);
                this.LoadEditButton();
                this.LoadProductType();
            }

            this.LoadPhanTrang();

        }

        private void LoadProductType()
        {
            this.drProductType.Items.Clear();
            this.drProductType.DataSource = DocumentManagement.Instance.documentFunc.Select_All();
            this.drProductType.DataTextField = "Title";
            this.drProductType.DataValueField = "ID";
            this.DataBind();

        }

        protected void btXoa_Click(object sender, EventArgs e)
        {
            string selected = Request.Form["cbID"];
            if (selected != null && selected.Trim().Length > 0)
            {
                List<string> list = selected.Split(',').ToList();
                DocumentManagement.Instance.documentFunc.Delete_IDs(list);
            }
            this.LoadTimKiem(int.Parse(this.hPageIndex.Value));

        }

        protected void btThemMoi_Click(object sender, EventArgs e)
        {
            this.pnControls.Visible = true;
            this.pnTable.Visible = false;
            this.pnPhanTrang.Visible = false;
            this.Label1.Text = "";
        }

        protected void btLuu_Click(object sender, EventArgs e)
        {
            Document obj = this.GetValue();
            DocumentManagement.Instance.documentFunc.InsertUpdate(obj);

        }

        private Document GetValue()
        {
            Document obj = new Document();
            obj.ID = this.txtID.Value.Length > 0 ? int.Parse(this.txtID.Value) : -1;
            obj.Title = this.txtName.Value;
            obj.DocumentTypeID = int.Parse(this.txtPrice.Value);
            obj.Path = this.txtDescription.Value;

            return obj;
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            /*
            try
            {
                if (this.fuFileUpload.FileName.Length > 0)
                {
                    string path = Server.MapPath("/ExcelFiles");
                    this.fuFileUpload.SaveAs(path + "/xenoitinh.xlsx");
                    List<NguoiDung> list = DataController.NguoiDung_ImportExcelFile(path + "/xenoitinh.xlsx");
                    if (list != null && list.Count > 0)
                    {
                        DataController.NguoiDung_Import(list);
                        this.Label1.Text = "Import thành công.";
                    }
                    else
                        this.Label1.Text = "Danh sach rong.";
                }
            }
            catch { }
            */
        }

        protected void btThoat_Click(object sender, EventArgs e)
        {
            this.pnControls.Visible = false;
            this.pnTable.Visible = true;
            this.pnPhanTrang.Visible = true;
            this.Label1.Text = "";
            this.LoadTimKiem(int.Parse(this.hPageIndex.Value));
        }

        protected void btXoaTrang_Click(object sender, EventArgs e)
        {
            this.txtID.Value = "";
            this.txtPrice.Value = "";
            this.txtName.Value = "";
            this.txtDescription.Value = "";

        }

        protected void btTim_Click(object sender, EventArgs e)
        {
            this.pnControls.Visible = false;
            this.pnTable.Visible = true;
            this.pnPhanTrang.Visible = true;
            this.Label1.Text = "";

            this.hPageIndex.Value = "0";
            this.LoadTimKiem(0);
            this.LoadPhanTrang();
        }

        protected void btPhanTrang_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int PageIndex = int.Parse(this.hPageIndex.Value);
            switch (btn.ID)
            {
                case "btTruoc":
                    PageIndex = int.Parse(this.hPageIndex.Value);
                    PageIndex = (PageIndex > 0) ? PageIndex - 1 : 0;
                    this.hPageIndex.Value = PageIndex.ToString();
                    break;
                case "btSau":
                    int PageSize = int.Parse(this.dlPageNumber.SelectedValue);
                    int TotalRows = int.Parse(hTotalRows.Value);
                    PageIndex = ((PageIndex + 1) * PageSize < TotalRows) ? PageIndex + 1 : PageIndex;
                    break;
                default:
                    PageIndex = int.Parse(btn.Text) - 1;
                    break;
            }
            this.hPageIndex.Value = PageIndex.ToString();
            this.LoadTimKiem(PageIndex);
            this.UpdatePanel1.Update();
        }

        protected void dlPageNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            int PageIndex = int.Parse(this.hPageIndex.Value);
            this.LoadTimKiem(PageIndex);
            Global.g_PageSize = this.dlPageNumber.SelectedValue;

        }
        #endregion

        #region Methods

        private void LoadEditButton()
        {
            try
            {
                int idEdit = int.Parse(Request.QueryString["idedit"]);
                this.pnTable.Visible = false;
                this.pnPhanTrang.Visible = false;
                this.pnControls.Visible = true;
                Document obj = DocumentManagement.Instance.documentFunc.Select_ID(idEdit);
                if (obj != null)
                {
                    this.txtID.Value = obj.ID.ToString();
                    this.txtPrice.Value = obj.DocumentTypeID.ToString();
                    this.txtName.Value = obj.Title;
                    this.txtDescription.Value = obj.Content;

                }
            }
            catch { }

        }

        private void LoadPhanTrang()
        {
            try
            {
                int TotalRows = int.Parse(this.hTotalRows.Value);
                int PageSize = int.Parse(this.dlPageNumber.SelectedValue);
                int count = TotalRows / PageSize;
                if (TotalRows % PageSize > 0)
                    count++;
                if (count > 20)
                    count = 20;
                this.pnButton.Controls.Clear();
                for (int i = 0; i < count; i++)
                {
                    Button bt = new Button()
                    {
                        ID = "bt" + i,
                        Text = (i + 1).ToString()
                    };
                    bt.Attributes.Add("runat", "server");
                    bt.Click += new EventHandler(this.btPhanTrang_Click);
                    bt.CssClass = "btn btn-dark";
                    this.pnButton.Controls.Add(bt);

                }

            }
            catch { }
        }
        private void LoadTimKiem(int pIndex)
        {
            int PageSize = int.Parse(this.dlPageNumber.SelectedValue);
            int TotalRows = 0;
            this.ls = DocumentManagement.Instance.documentFunc.Find_KeyWord(this.txtKeyword.Value, PageSize, pIndex, out TotalRows);
            this.hTotalRows.Value = TotalRows.ToString();
            if (ls == null || ls.Count == 0)
            {
                this.pnTable.Visible = false;
                this.pnPhanTrang.Visible = false;
                this.Label1.Text = "Không tìm thấy dữ liệu.";
            }
        }
        #endregion
    }
}