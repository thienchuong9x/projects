using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using System.IO;
using System.Xml;
using Dental.Utilities;
using Ionic.Zip;
using System.Text;

public partial class OrderInput : DDVPortalModuleBase
{
    readonly static ILog logger = LogManager.GetLogger(typeof(OrderInput));

        #region Save ViewState
        public List<MasterProsthesis> listMstProsthesis
        {
            get
            {
                if (this.ViewState["listMstProsthesis"] == null)
                {
                    List<MasterProsthesis> list = new List<MasterProsthesis>();
                    try
                    {
                        list = MasterProsthesis.GetProsthesiss(int.Parse(hiddenOfficeCd.Value));
                    }
                    catch { list = new List<MasterProsthesis>(); }
                    this.ViewState["listMstProsthesis"] = list;
                }
                return this.ViewState["listMstProsthesis"] as List<MasterProsthesis>;
            }
        }
        #endregion

        #region "Page Load"

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                
                InitCalendar();
                if (!Page.IsPostBack)
                {
                    InitLanguage(); 

                    this.btnRegister.Attributes.Add("onclick", "javascript:if (Page_ClientValidate()) { if(CheckWarningInput('" + GetResource("MSG_WARNING_INPUT.Text") + "','')) { this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(btnRegister, "").ToString() + ";return true;} else {this.disabled=false; return false; }}");
                    this.btnDelete.Attributes.Add("onclick", "javascript:return ConfirmDeleteTooth('" + GetResource("MSG_CONFIRM_DELETE.Text") + "');");
                    this.btnGoProcess.Attributes.Add("onclick", "javascript:return ConfirmGoPage('" + GetResource("MSG_CONFIRM_GO_PROCESS.Text") + "');");
                    this.btnGoTechPrice.Attributes.Add("onclick", "javascript:return ConfirmGoPage('" + GetResource("MSG_CONFIRM_GO_TECH_PRICE.Text") + "');");
                    //HyperLink1.Visible = false;
                    lBtnDownload.Visible = false;
                    string orderSeq = Request.QueryString["id"];
                 
                    hiddenOfficeCd.Value = GetOffice();
                   
                    FillDropDownSalesManStaffList();
                    FillDropDownSex();
                    if (!string.IsNullOrEmpty(orderSeq))
                    {
                        hiddenOrderSeq.Value = orderSeq;
                        TrnOrderHeader header = TrnOrderHeader.GetTrnOrderHeader(Convert.ToInt32(hiddenOfficeCd.Value), Convert.ToDouble(orderSeq));
                        SetOrderInputItem(header);
                    }
                    else
                    {
                        FillDentalOffice();
                        DropDownDentalOffice.SelectedIndex = 0;
                        DropDownDentalOffice_SelectedIndexChanged();
                        hiddenOrderSeq.Value = "-1";
                        btnGoProcess.Visible = btnGoTechPrice.Visible = false;
                    }
                    InitDetailMasterList();
                    Initiation();
                    RemoveViewState("listMstProsthesis");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Page_Load OrderInput", ex);
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            }
        }


        #endregion

        #region Header Process
        private void InitCalendar()
        {
            hplOrderDate.NavigateUrl = Calendar.InvokePopupCal(TextOrderDate);
            hplDueDate.NavigateUrl = Calendar.InvokePopupCal(TextDueDate);
            hplSetDate.NavigateUrl = Calendar.InvokePopupCal(TextSetDate);
        }
        private void Initiation()
        {
            RemoveViewState("CurrentDetail");

            //Set Lablel from resource 
            SetLabelText(LabelOrderDate, LabelOrderNo, LabelDueDate, LabelDeadLine, LabelSetDate, LabelOfficeCd, LabelDentist, LabelSalesman, LabelPatient, LabelSexAge, LabelComments, LabelBorrowPart, LabelYear, LabelProsthesis,
                LabelMaterial, LabelShade, LabelShape, LabelAnatomyKit, LabelCAD, LabelTrialOrder, LabelRefOrderNo ,LabelRemanufacture, LabelInsurance, LabelDetailNm , LabelToothType);

            SetButtonText(btnSave, btnCancel, btnDelete, btnRegister, btnDetailSave, btnDetailCancel, ButtonSaveBridge, btnAddNoTooth , btnGoProcess, btnGoTechPrice, btnUp , btnDown);
            checkCAD.Text = GetResource("checkCAD.Text");
            checkTrialOrder.Text = GetResource("checkTrialOrder.Text");
            radioInsuranceTrue.Text = GetResource("radioInsuranceTrue.Text");
            radioInsuranceFalse.Text = GetResource("radioInsuranceFalse.Text");
            checkChildTooth.Text = GetResource("checkChildTooth.Text");
            checkGap.Text = GetResource("checkGap.Text");
            checkDenture.Text = GetResource("checkDenture.Text");

            //Set Required
            RequiredTextOrderNo.ErrorMessage = RequiredTextOrderNo.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_FIELD.Text"), LabelOrderNo.Text);
            RequiredUniqueTextOrderNo.ErrorMessage = RequiredUniqueTextOrderNo.ToolTip = GetResource("MSG_EXIST_ORDER_NO.Text");
            RequiredTextOrderDate.ErrorMessage = RequiredTextOrderDate.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_FIELD.Text"), LabelOrderDate.Text);
            RequiredTextDueDate.ErrorMessage = RequiredTextDueDate.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_FIELD.Text"), LabelDueDate.Text);
            RequiredTextOfficeCd.ErrorMessage = RequiredTextOfficeCd.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_FIELD.Text"), LabelOfficeCd.Text);
            RequiredDateTimeTextOrder.ErrorMessage = RequiredDateTimeTextOrder.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_DATE_FIELD.Text"), LabelOrderDate.Text);
            RequiredDateTimeTextDue.ErrorMessage = RequiredDateTimeTextDue.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_DATE_FIELD.Text"), LabelDueDate.Text);
            RequiredDateTimeTextSet.ErrorMessage = RequiredDateTimeTextSet.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_DATE_FIELD.Text"), LabelSetDate.Text);
            RequiredNumberAge.ErrorMessage = RequiredNumberAge.ToolTip =  string.Format(GetResource("MSG_REQUIRED_INPUT_NUMBER_FIELD.Text"), LabelYear.Text); 
            //Set Label from Resource
            List<TrnOrderDetail> list = new List<TrnOrderDetail>();
            if (!string.IsNullOrEmpty(TextOrderNo.Text))
            {
                list = TrnOrderDetail.GetTrnOrderDetailList(Convert.ToInt32(hiddenOfficeCd.Value), Convert.ToDouble(hiddenOrderSeq.Value), false);
            }
            FillDataToGridView(list);
        } 

        protected void gridViewOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
        {
            #region Add header
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Build custom header.
                GridView oGridView = (GridView)sender;
                GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableCell oTableCell = new TableHeaderCell();


                //Add Include in Test
                oTableCell.CssClass = "td_header";
                oTableCell.Width = Unit.Pixel(20);
                oGridViewRow.Cells.Add(oTableCell);

                //Add prosthetic name
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_ORDER_DETAIL_DetailNm.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add prosthetic name
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_ORDER_DETAIL_ProstheticName.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add Material name
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_ORDER_DETAIL_MaterialName.Text");
                oTableCell.CssClass = "td_header";
                oTableCell.Width = Unit.Pixel(50);
                oGridViewRow.Cells.Add(oTableCell);

                //Add Material quantity Amount
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_ORDER_DETAIL_MaterialQuantity.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add ToothNo
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_ORDER_DETAIL_Tooth.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add completion date
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_ORDER_DETAIL_CompletionDate.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add MaterialPrice
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_ORDER_DETAIL_MaterialPrice.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add BridgeID
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_ORDER_DETAIL_BridgeID.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add ProcessPrice
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_ORDER_DETAIL_ProcessPrice.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add DeleveriStateNo
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_ORDER_DETAIL_DeliveryStateNo.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add custom header            
                oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
            }
            #endregion
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeViewTooth.CheckedNodes.Count > 0)
                {
                    TreeNode parentNode = TreeViewTooth.CheckedNodes[0].Parent;
                    Dictionary<TreeNode, int> dic = new Dictionary<TreeNode, int>();
                    for (int i = TreeViewTooth.CheckedNodes.Count - 1; i >= 0; i--)
                    {
                        TreeNode temp = TreeViewTooth.CheckedNodes[i].Parent;
                        if (temp == null)
                        {
                            dic.Add(TreeViewTooth.CheckedNodes[i], 0);
                        }
                        else
                        {
                            if (!dic.Keys.Contains(temp))
                                dic.Add(temp, 0);
                            int totalChild = dic[temp];
                            dic[temp] = ++totalChild;
                        }
                    }
                    foreach (TreeNode node in dic.Keys)
                    {
                        if (node.ChildNodes.Count - dic[node] == 1)
                        {
                            throw new Exception(GetResource("MSG_CANNOT_DELETE_STILL_ONE_ITEM.Text"));
                        }
                    }
                    foreach (TreeNode node in dic.Keys)
                    {
                        if (node.ChildNodes.Count == 0)
                        {
                            TreeViewTooth.Nodes.Remove(node);
                        }
                        else
                        {
                            for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
                            {
                                if (node.ChildNodes[i].Checked)
                                    node.ChildNodes.RemoveAt(i);
                            }
                            if (node.ChildNodes.Count == 0)
                            {
                                TreeViewTooth.Nodes.Remove(node);
                            }
                        }
                    }
                }
                else
                    throw new Exception(GetResource("MSG_NONE_SELECTED_TREE_VIEW_NODE.Text"));
            }
            catch (Exception ex)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            checkCAD.Checked = false;
            var checkedIDs = (from GridViewRow msgRow in gridViewOrderDetail.Rows
                              where ((CheckBox)msgRow.FindControl("Check")).Checked
                              select msgRow).ToList();
            if (checkedIDs.Count > 1)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");");
            }
            else
            {
                InitTreeViewTeeth(checkedIDs.FirstOrDefault());
                ShowOrderHead(false);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RemoveViewState("CurrentDetail", "listMstProsthesis");
            Response.Redirect("OrderList.aspx");
        }
        protected void btnGoTechPrice_Click(object sender, EventArgs e)
        {
            RemoveViewState("CurrentDetail", "listMstProsthesis");
            Response.Redirect("OperationTechPrice.aspx?id=" + hiddenOrderSeq.Value);
        }
        protected void btnGoProcess_Click(object sender, EventArgs e)
        {
            RemoveViewState("CurrentDetail", "listMstProsthesis");
            Response.Redirect("OperationProcess.aspx?id=" + hiddenOrderSeq.Value);
        }
        #endregion

        #region Event
        protected void btnDetailCancel_Click(object sender, EventArgs e)
        {
            hiddenAfterTooth.Value = hiddenBeforeTooth.Value = "";
            RemoveViewState("CurrentDetail");
            if (TreeViewTooth.SelectedNode != null)
            {
                TreeViewTooth.SelectedNode.Selected = false;
            }
            ShowOrderHead(true);
        }

        protected void btnDetailSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeViewTooth.SelectedNode != null)
                {
                    SaveBeforeSelectedNodeChanged(TreeViewTooth.SelectedNode.Value);
                    TreeViewTooth.SelectedNode.Selected = false;
                }
                if (ViewState["CurrentDetail"] != null)
                {
                    List<TrnOrderDetail> listDetail = (List<TrnOrderDetail>)ViewState["CurrentDetail"];
                    //Remove ALL SeqNo not contains in TreeView
                    List<TrnOrderDetail> listFinal = new List<TrnOrderDetail>();
                    int displayOrder = 1;
                    foreach (TreeNode parent in TreeViewTooth.Nodes)
                    {
                        if (parent.ChildNodes.Count == 0)
                        {
                            TrnOrderDetail detail = listDetail.FirstOrDefault(p => p.DetailSeq == Convert.ToInt32(parent.Value));
                            if (detail != null)
                            {
                                detail.DisplayOrder = displayOrder++;
                                listFinal.Add(detail);
                            }
                        }
                        else
                        {
                            foreach (TreeNode child in parent.ChildNodes)
                            {
                                TrnOrderDetail detailChild = listDetail.FirstOrDefault(p => p.DetailSeq == Convert.ToInt32(child.Value));
                                if (detailChild != null)
                                {
                                    detailChild.DisplayOrder = displayOrder++;
                                    listFinal.Add(detailChild);
                                }
                            }
                        }
                    }
                    //we should compare with TreeView 
                    ViewState["CurrentDetail"] = listFinal;
                    FillDataToGridView(listFinal);
                }
                hiddenAfterTooth.Value = hiddenBeforeTooth.Value = "";
                ShowOrderHead(true);
            }
            catch (Exception ex)
            {
                logger.Error("Error btnDetailSave_Click ", ex);
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            }
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TextOrderNo.Text) || string.IsNullOrWhiteSpace(TextOrderDate.Text) || string.IsNullOrWhiteSpace(TextDueDate.Text))
                {
                    return;
                }

                int countOrderNo = TrnOrderHeader.GetCountOrderNo(Convert.ToInt32(hiddenOfficeCd.Value), Convert.ToInt32(hiddenOrderSeq.Value), TextOrderNo.Text);
                if ((hiddenOrderSeq.Value == "-1" && countOrderNo>0) || countOrderNo>=1)
                {
                    RequiredUniqueTextOrderNo.IsValid = false;
                    return;
                }
                if (CheckIllegalChar(TextOrderNo.Text))
                {
                    throw new Exception(GetResource("MSG_ILLEGAL_CHARS.Text"));
                }

                List<TrnOrderDetail> listDetail = GetListTrnOrderDetail();
                //Check Data
                logger.Info("Begin btnRegister_Click - Check input Prosthesis");

                #region Check Prosthesis
                List<CodeName> listMaterial = CodeName.GetListMaterialByOrderDate(Convert.ToInt32(hiddenOfficeCd.Value), TextOrderDate.Text);
                foreach (TrnOrderDetail i in listDetail)
                {
                    if (i.ProsthesisCd == -1)
                    {
                        throw new Exception(string.Format(GetResource("MSG_REQUIRED_INPUT_PROSTHESIS.Text"), i.DetailNm));
                    }
                    if (i.MaterialCd==null)
                    {
                        i.MaterialNm = null;
                    }
                    else if (listMaterial.FirstOrDefault(p => p.Code == i.MaterialCd.ToString()) == null)
                    {
                        i.MaterialCd = null;
                        i.MaterialNm = null;
                        throw new Exception(string.Format(GetResource("MSG_WARN_CHOOSE_MATERIAL.Text"), i.DetailNm));
                    }
                }
                #endregion

                TrnOrderHeader head = GetOrderInputItem();
                head.CreateAccount = head.ModifiedAccount = this.User.Identity.Name;

                //head.DepositCompleteFlg = chkComplete.Checked.ToString();
                logger.Debug("call RegisterOrderInput");
                TrnOrderHeader.RegisterOrderInput(head, listDetail);
                hiddenRegister.Value = "true";

                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_INFO.Text"), GetResource("MSG_UPDATE_SUCESS_Full.Text")) + "\");");

                #region ShowCadCam
                logger.Info("End btnRegister_Click - begin generateCadCam");
                //if checkCAd = false => do not create XML
                //HyperLink1.Visible = false;
                lBtnDownload.Visible = false;
                List<TrnOrderDetail> list = new List<TrnOrderDetail>();
                foreach (GridViewRow r in gridViewOrderDetail.Rows)
                {
                    if (r.Cells[16].Text == "True")
                    {
                        list.Add(GetOrderDetailFromGridView(r));
                    }
                }
                if (list.Count > 0)
                {
                    list = list.Where(l => (l.ToothNumber != null)).ToList();
                    if (list.Count == 0)
                    {
                        TrnOrderDetail dt = new TrnOrderDetail();
                        dt.ToothNumber = null;
                        dt.ProsthesisCd = -1;
                        dt.Shade = null;
                        dt.MaterialCd = null;
                        list.Add(dt);
                    }
                    //HyperLink1.Visible = true;
                    lBtnDownload.Visible = true;
                    HyperLink1.NavigateUrl = generateCadCam(head.OfficeCd, head, list);
                }
                #endregion 

                if (hiddenOrderSeq.Value == "-1")
                    ResetOrderInput();
            }
            catch (Exception ex)
            {
                logger.Error("ERROR btnRegister_Click ", ex);
                btnRegister.Enabled = true;
                this.btnRegister.Attributes.Add("onclick", "javascript:if (Page_ClientValidate()) { if(CheckWarningInput('" + GetResource("MSG_WARNING_INPUT.Text") + "','')) { this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(btnRegister, "").ToString() + ";return true;} else {this.disabled=false; return false; }}");
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            }
        }
        private bool CheckIllegalChar(string text)
        {
            if (text.Contains("|") || text.Contains("\\") || text.Contains("/") || text.Contains(":") || text.Contains("*") || text.Contains("?") || text.Contains("\"") || text.Contains("<") || text.Contains(">"))
                return true;
            return false;
        }
        protected void ButtonSaveBridge_Click(object sender, EventArgs e)
        {
            try
            {
                if (hiddenTooth.Value != "")
                {
                    #region Select New Tooth from Image
                    SaveBeforeSelectedNodeChanged(hiddenAfterTooth.Value);
                    TrnOrderDetail detail = new TrnOrderDetail();
                    detail.DetailSeq = GetDetailSeq();
                    detail.ToothNumber = Common.GetNullableInt(hiddenTooth.Value);
                    int nextToothNumber = GetTotalToothNumberAppear(Common.GetNullableInt(hiddenTooth.Value));
                    string firstInitTooth = (nextToothNumber == 0 ? hiddenTooth.Value : string.Format("{0}({1})", hiddenTooth.Value, (nextToothNumber + 1).ToString()));
                    detail.DetailNm = string.Format(GetResource("NODE_TOOTH_NUMBER.Text"), firstInitTooth);
                    AddTreeNode(null, detail, true);
                    //Add to this Node to ViewState
                    AddViewState(detail);
                    ResetOrderDetail();
                    SetOrderDetail(detail);
                    EnableDetailRegion(true,true);

                    hiddenBeforeTooth.Value = hiddenAfterTooth.Value;
                    hiddenAfterTooth.Value = TreeViewTooth.SelectedNode.Value;
                    #endregion
                }
                else
                {
                    // Combine 
                    if (TreeViewTooth.CheckedNodes.Count == 0)
                    {
                        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_CHECK_AT_LEAST_ONE_ITEM.Text")) + "\");");
                    }
                    else if (TreeViewTooth.CheckedNodes.Count == 1)
                    {
                        #region Case 1 checkbox
                        if (TreeViewTooth.CheckedNodes[0].Parent == null)
                        {
                            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_ITEM_NO_GROUP.Text")) + "\");");
                        }
                        else
                        {
                            if (TreeViewTooth.CheckedNodes[0].Parent.ChildNodes.Count <= 2)
                            {
                                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_CANNOT_DELETE_ONE_ITEM.Text")) + "\");");
                            }
                            else //Remove Nodes but keep Parent
                            {
                                TreeNode temp = TreeViewTooth.CheckedNodes[0];
                                temp.Checked = false;
                                //List<TrnOrderDetail> listDetail = GetListTrnOrderDetail();
                                //TrnOrderDetail item = listDetail.FirstOrDefault(p => p.DetailSeq.ToString() == temp.Value);
                                //if (item != null)
                                //    item.BridgedID = null;
                                TreeViewTooth.Nodes.Add(temp);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region more 2 checked
                        int dept = TreeViewTooth.CheckedNodes[0].Depth;
                        for (int i = TreeViewTooth.CheckedNodes.Count - 1; i > 0; i--)
                        {
                            if (TreeViewTooth.CheckedNodes[i].Depth != dept)
                            {
                                dept = -1; //Error Dept 
                                break;
                            }
                            if (TreeViewTooth.CheckedNodes[0].Parent != TreeViewTooth.CheckedNodes[i].Parent)
                            {
                                dept = -1;
                                break;
                            }
                        }
                        if (dept == -1)
                        {
                            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_CHECK_NODE_IN_SAME_GROUP.Text")) + "\");");
                        }
                        else
                        {
                            List<TrnOrderDetail> listDetail = GetListTrnOrderDetail();
                            if (dept == 0)
                            {
                                hiddenBeforeTooth.Value = hiddenAfterTooth.Value;
                                if (TreeViewTooth.SelectedNode != null)
                                    SaveBeforeSelectedNodeChanged(hiddenBeforeTooth.Value); // save 
                                int nextBridgedID = GetGroupId();
                                TreeNode treeNode = new TreeNode();
                                for (int i = TreeViewTooth.CheckedNodes.Count - 1; i >= 0; i--)
                                {
                                    TrnOrderDetail item = listDetail.FirstOrDefault(p => p.DetailSeq.ToString() == TreeViewTooth.CheckedNodes[i].Value);
                                    item.BridgedID = Common.GetNullableInt(nextBridgedID.ToString());
                                    TreeViewTooth.CheckedNodes[i].Selected = false;
                                    treeNode.ChildNodes.Add(TreeViewTooth.CheckedNodes[i]);
                                }
                                foreach (TreeNode i in treeNode.ChildNodes)
                                {
                                    i.Checked = false;
                                }
                                //add Group Node 
                                treeNode.Text = GetResource("NODE_TOOTH_GROUP.Text") + nextBridgedID.ToString();
                                TreeViewTooth.Nodes.Add(treeNode);
                                treeNode.Selected = true;

                                ResetOrderDetail();
                                txtDetailNm.Text = treeNode.Text;

                                EnableDetailRegion(false,false);
                                hiddenAfterTooth.Value = TreeViewTooth.SelectedNode.Value;
                            }
                            else if (dept == 1)
                            {
                                if (TreeViewTooth.CheckedNodes[0].Parent.ChildNodes.Count - TreeViewTooth.CheckedNodes.Count == 1)
                                {
                                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_CANNOT_REMOVE_TOOTH_IN_BRIDGED.Text")) + "\");");
                                }
                                else
                                {
                                    TreeNode parentNode = TreeViewTooth.CheckedNodes[0].Parent;
                                    for (int i = TreeViewTooth.CheckedNodes.Count - 1; i >= 0; i--)
                                    {
                                        TrnOrderDetail item = listDetail.FirstOrDefault(p => p.DetailSeq.ToString() == TreeViewTooth.CheckedNodes[i].Value);
                                        if (item != null)
                                        {
                                            item.BridgedID = null;
                                            TreeNode temp = TreeViewTooth.CheckedNodes[i];
                                            TreeViewTooth.CheckedNodes[i].Checked = false;
                                            TreeViewTooth.Nodes.Add(temp);
                                        }
                                    }
                                    if (parentNode.ChildNodes.Count == 0)
                                    {
                                        TreeViewTooth.Nodes.Remove(parentNode);
                                    }
                                }
                            }
                        }
                        #endregion

                        TreeViewTooth.ExpandAll();
                    }
                }
                hiddenTooth.Value = "";
            }
            catch (Exception ex)
            {
                logger.Error("Error ButtonSaveBridge_Click ", ex);
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            }
        }
        protected void TreeViewTooth_SelectedNodeChanged(object sender, EventArgs e)
        {
            hiddenBeforeTooth.Value = hiddenAfterTooth.Value;
            SaveBeforeSelectedNodeChanged(hiddenBeforeTooth.Value);
            //Search changed row 
            ResetOrderDetail();
            if (TreeViewTooth.SelectedNode.ChildNodes.Count > 0)
            {
                EnableDetailRegion(false,false);
                btnUp.Enabled = btnDown.Enabled = true;
                txtDetailNm.Text = TreeViewTooth.SelectedNode.Text;
            }
            else
            {
                SetOrderDetail(Convert.ToInt32(TreeViewTooth.SelectedNode.Value));
               // EnableDetailRegion(true);
            }
            hiddenAfterTooth.Value = TreeViewTooth.SelectedNode.Value;
        }

        #endregion
        
        private void InitDetailMasterList()
        {
            FillDropDownMaterialList();
            FillDropDownList(DropDownShape, true);
            FillDropDownList(DropDownShade, true);
            FillDropDownList(DropDownAnatomyKit, true);
        }
       
        private void FillDropDownList(DropDownList dropDown, bool bFirstEmptyItem)
        {
            dropDown.Items.Clear();
            if (bFirstEmptyItem)
                dropDown.Items.Add(new ListItem("", ""));
            List<CodeName> list = CodeName.GetMasterListCodeName(dropDown.ID);
            foreach (CodeName i in list)
            {
                dropDown.Items.Add(new ListItem(i.Name, i.Code));
            }
        }
        private void FillDropDownSalesManStaffList()
        {
            DropDownSalesMan.Items.Add(new ListItem("", ""));

            List<MasterStaff> listStaff = MasterStaff.GetStaffwiOffice( Convert.ToInt32(hiddenOfficeCd.Value)); 
            foreach(MasterStaff i in listStaff) 
            {
                if(i.IsDeleted == false && (i.SalesFlg !=null && i.SalesFlg.Value == true))
                 DropDownSalesMan.Items.Add(new ListItem(i.StaffNm, i.StaffCd.ToString() ));
            }
            //List<CodeName> list = CodeName.GetMasterListCodeName(DropDownSalesMan.ID, Convert.ToInt32(hiddenOfficeCd.Value));
            //foreach (CodeName i in list)
            //{
            //    DropDownSalesMan.Items.Add(new ListItem(i.Name, i.Code));
            //}
        }
        private void FillDropDownMaterialList()
        {
            try
            {
                DropDownMaterial.Items.Clear();
                DropDownMaterial.Items.Add(new ListItem("", ""));
                List<CodeName> list = CodeName.GetListMaterialByOrderDate(Convert.ToInt32(hiddenOfficeCd.Value), TextOrderDate.Text);
                foreach (CodeName i in list)
                {
                    DropDownMaterial.Items.Add(new ListItem(i.Name, i.Code));
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error FillDropDownMaterialList ", ex); 
               // RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            }
        }
        private void FillDropDownSex()
        {
            DropDownSex.Items.Add(new ListItem("", ""));
            DropDownSex.Items.Add(new ListItem(GetResource("SEX_MALE.Text"), "1"));
            DropDownSex.Items.Add(new ListItem(GetResource("SEX_FEMALE.Text"), "2"));
        }
        private void FillDataToGridView(List<TrnOrderDetail> list)
        {
            gridViewOrderDetail.Columns[12].Visible = true;  //MaterialCd
            gridViewOrderDetail.Columns[13].Visible = true;  //Shape
            gridViewOrderDetail.Columns[14].Visible = true;  //Shade
            gridViewOrderDetail.Columns[15].Visible = true;  //Anatomykit
            gridViewOrderDetail.Columns[16].Visible = true;  //CAD outPut Done
            gridViewOrderDetail.Columns[17].Visible = true;  //InsuranceKBN
            gridViewOrderDetail.Columns[18].Visible = true;  //DetailSeq
            gridViewOrderDetail.Columns[19].Visible = true;  //ProsthesisCd
            gridViewOrderDetail.Columns[20].Visible = true;  //ChildFlg
            gridViewOrderDetail.Columns[21].Visible = true;  //GapFlg
            gridViewOrderDetail.Columns[22].Visible = true;  //DentureFlg
            gridViewOrderDetail.Columns[23].Visible = true;  //BillstaementNo
            gridViewOrderDetail.Columns[24].Visible = true;  //OldMaterialCd
            if (list.Count == 0)
            {
                TrnOrderDetail temp = new TrnOrderDetail();
                temp.OrderSeq = -1;
                list.Add(temp);
            }
            gridViewOrderDetail.DataSource = list;
            gridViewOrderDetail.DataBind();

            if (list[0].OrderSeq == -1)
                gridViewOrderDetail.Rows[0].Visible = false;

            gridViewOrderDetail.Columns[1].Visible = false;

            gridViewOrderDetail.Columns[12].Visible = false;  //MaterialCd
            gridViewOrderDetail.Columns[13].Visible = false;  //Shape
            gridViewOrderDetail.Columns[14].Visible = false;  //Shade
            gridViewOrderDetail.Columns[15].Visible = false;  //Anatomykit
            gridViewOrderDetail.Columns[16].Visible = false;  //CAD outPut Done
            gridViewOrderDetail.Columns[17].Visible = false;  //InsuranceKBN
            gridViewOrderDetail.Columns[18].Visible = false;  //DetailSeq
            gridViewOrderDetail.Columns[19].Visible = false;  //ProsthesisCd
            gridViewOrderDetail.Columns[20].Visible = false;  //ChildFlg
            gridViewOrderDetail.Columns[21].Visible = false;  //GapFlg
            gridViewOrderDetail.Columns[22].Visible = false;  //DentureFlg
            if (list[0].OrderSeq == -1)
            {
                list.RemoveAt(0);
            }
        }
        private void ShowOrderHead(bool bShow)
        {
            panelOrderInput.Visible = bShow;
            panelOrderInputDetail.Visible = !bShow;
        }
        private void AddViewState(TrnOrderDetail item)
        {
            if (ViewState["CurrentDetail"] == null)
            {
                List<TrnOrderDetail> listDetail = new List<TrnOrderDetail>();
                listDetail.Add(item);
                ViewState.Add("CurrentDetail", listDetail);
            }
            else
            {

                TrnOrderDetail oldItem = GetOrderDetailFromViewState(item.DetailSeq);
                if (oldItem != null)
                    oldItem = item;
                else
                {
                    List<TrnOrderDetail> listDetail = (List<TrnOrderDetail>)ViewState["CurrentDetail"];
                    listDetail.Add(item);
                }
            }
        }
        private void RemoveViewState(params string[] nameState)
        {
            // CurrentDetail
            foreach (string state in nameState)
            {
                if (ViewState[state] != null)
                {
                    ViewState.Remove(state);
                }
            }
        }

        #region TrnOrderHeader
        private void SetOrderInputItem(TrnOrderHeader header)
        {
            TextOrderNo.Text = header.OrderNo;

            checkTrialOrder.Checked = checkRemanufacture.Checked = false;
            if (header.TrialOrderFlg!=null)
            {
                if (Convert.ToBoolean(header.TrialOrderFlg) == true)
                {
                    checkTrialOrder.Checked = true;
                }
            }
            if (header.RemanufactureFlg!=null)
            {
                if (Convert.ToBoolean(header.RemanufactureFlg) == true)
                {
                    checkRemanufacture.Checked = true;
                }
            }

            TextRefOrderNo.Text = header.RefOrderNo;

            TextOrderDate.Text = header.OrderDate.ToShortDateString();
            TextDueDate.Text = header.DeliverDate.ToShortDateString();

            TextPatientLastNm.Text = header.PatientLastNm;
            TextPatientFirstNm.Text = header.PatientFirstNm;
            TextDentist.Text = header.DentistNm;
            TextDeadLine.Text = DateTime.Now.ToShortDateString();

            //Fill SalesMan first
            TextStaffCd.Text = header.StaffCd.ToString();
            if (header.StaffCd!=null)
            {
                DropDownSalesMan.SelectedValue = TextStaffCd.Text;
            }
            FillDentalOffice();
            TextOfficeCd.Text = header.DentalOfficeCd.ToString();
            GetAutomaticDropDownList(TextOfficeCd, DropDownDentalOffice);
            DropDownDentalOffice_SelectedIndexChanged();

            if (header.SetDate!=null)
            {
                DateTime setDate = Convert.ToDateTime(header.SetDate);
                TextSetDate.Text = setDate.Date.ToShortDateString();
            }
            if (header.PatientSex == 1 || header.PatientSex == 2)
                DropDownSex.SelectedValue = header.PatientSex.ToString();

            if (header.PatientAge != null)
                TextAge.Text = header.PatientAge.ToString();

            TextComments.Text = header.Note;
            TextBorrowPart.Text = header.BorrowParts;

            //if (!string.IsNullOrEmpty(header.DepositCompleteFlg))
            //{
            //    if (Convert.ToBoolean(header.DepositCompleteFlg) == true)
            //        chkComplete.Checked = true;
            //}
        }
        private TrnOrderHeader GetOrderInputItem()
        {

            TrnOrderHeader header = null;
            if (Convert.ToDouble(hiddenOrderSeq.Value) == -1)
            {
                header = TrnOrderHeader.GetTrnOrderHeader(Convert.ToInt32(hiddenOfficeCd.Value), Convert.ToDouble(hiddenOrderSeq.Value));
            }
            else
            {
                header = new TrnOrderHeader();
                header.OfficeCd = Convert.ToInt32(hiddenOfficeCd.Value);
                header.OrderSeq = Convert.ToDouble(hiddenOrderSeq.Value);
            }

            header.OrderNo = TextOrderNo.Text;
            header.RefOrderNo = TextRefOrderNo.Text.Trim() == "" ? null : TextRefOrderNo.Text.Trim(); 
            header.TrialOrderFlg = checkTrialOrder.Checked;
            header.RemanufactureFlg = checkRemanufacture.Checked;
            header.OrderDate = Convert.ToDateTime(TextOrderDate.Text);
            header.DeliverDate = Convert.ToDateTime(TextDueDate.Text);
            header.StaffCd = Common.GetNullableInt(TextStaffCd.Text);
            header.SetDate =  Common.GetNullableDateTime(TextSetDate.Text);
            header.DentalOfficeCd = Convert.ToInt32(TextOfficeCd.Text);
            header.PatientLastNm = TextPatientLastNm.Text.Trim() == "" ? null : TextPatientLastNm.Text.Trim();
            header.PatientFirstNm = TextPatientFirstNm.Text.Trim() == "" ? null : TextPatientFirstNm.Text.Trim(); 
            header.PatientSex = Common.GetNullableInt( DropDownSex.SelectedValue);
            header.PatientAge = Common.GetNullableInt(TextAge.Text);
            header.DentistNm = TextDentist.Text.Trim() == "" ? null : TextDentist.Text.Trim(); 
            header.Note = TextComments.Text.Trim() == "" ? null : TextComments.Text.Trim();
            header.BorrowParts = TextBorrowPart.Text.Trim() == "" ? null : TextBorrowPart.Text.Trim(); 
            
            return header;
        }
        #endregion

        #region TrnOrderDetail
        private void ResetOrderDetail()
        {
            checkCAD.Checked = checkDenture.Checked = checkChildTooth.Checked = checkGap.Checked = false;
            DropDownMaterial.SelectedIndex = 0;
            DropDownShape.SelectedIndex = 0;
            DropDownShade.SelectedIndex = 0;
            DropDownAnatomyKit.SelectedIndex = 0;
            // DropDownProsthesis.SelectedIndex = 0;
            radioInsuranceTrue.Checked = true;

            txtProsthesisAbbNm.Text = txtProsthesisNm.Text = txtProsthesisType.Text = txtDetailNm.Text = "";
        }
        private List<TrnOrderDetail> GetListTrnOrderDetail()
        {
            List<TrnOrderDetail> list = new List<TrnOrderDetail>();
            if (ViewState["CurrentDetail"] != null)
            {
                list = (List<TrnOrderDetail>)ViewState["CurrentDetail"];
            }
            else
            {
                foreach (GridViewRow row in gridViewOrderDetail.Rows)
                {
                    if (row.Visible) 
                    {
                        TrnOrderDetail item = GetOrderDetailFromGridView(row);
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        private TrnOrderDetail GetCurrentOrderDetail()
        {
            TrnOrderDetail item = new TrnOrderDetail();

            item.DetailNm = txtDetailNm.Text;
            if (radioInsuranceFalse.Checked)
                item.InsuranceKbn = false;
            else if (radioInsuranceTrue.Checked)
                item.InsuranceKbn = true;

            MasterProsthesis info = this.listMstProsthesis.Where(p => p.ProsthesisAbbNm == txtProsthesisAbbNm.Text.Trim()).FirstOrDefault();
            if (info != null)
            {
                item.ProsthesisNm = info.ProsthesisNm;
                item.ProsthesisCd = info.ProsthesisCd;
            }
            else
            {
                item.ProsthesisCd = -1; //always Set -1 Temp
            }

            if (DropDownMaterial.SelectedValue != "")
            {
                item.MaterialCd =  Convert.ToInt32(DropDownMaterial.SelectedValue);
                item.MaterialNm = DropDownMaterial.SelectedItem.Text;
            }
            item.Shape = Common.GetNullableInt(DropDownShape.SelectedValue);
            item.Shade = Common.GetNullableInt(DropDownShade.SelectedValue);
            item.AnatomyKit = Common.GetNullableInt(DropDownAnatomyKit.SelectedValue);
      

            item.CadOutputDone = checkCAD.Checked;
            item.ChildFlg = checkChildTooth.Checked;
            item.DentureFlg = checkDenture.Checked;
            item.GapFlg = checkGap.Checked;
            
            return item;

        }
        private TrnOrderDetail GetOrderDetailFromViewState(int DetailSeq)
        {
            TrnOrderDetail item = null;
            if (ViewState["CurrentDetail"] != null)
            {
                try
                {
                    List<TrnOrderDetail> listDetail = (List<TrnOrderDetail>)ViewState["CurrentDetail"];
                    item = listDetail.Find(p => p.DetailSeq == DetailSeq);
                }
                catch
                {
                    item = null;
                }
            }
            return item;
        }
        private TrnOrderDetail GetOrderDetailFromGridView(GridViewRow row)
        {
            TrnOrderDetail item = new TrnOrderDetail();

            if (Common.GetRowString(row.Cells[1].Text) != "")
                item.OrderSeq = Convert.ToInt32(row.Cells[1].Text);

            if (Common.GetRowString(row.Cells[2].Text) != "")
                item.DetailNm = Common.GetRowString(row.Cells[2].Text);

            if (Common.GetRowString(row.Cells[3].Text) != "")
                item.ProsthesisNm = row.Cells[3].Text;

            if (Common.GetRowString(row.Cells[4].Text) != "")
                item.MaterialNm = row.Cells[4].Text;

            if (Common.GetRowString(row.Cells[5].Text) != "")
                item.Amount = Common.GetNullableDouble(row.Cells[5].Text);

            item.ToothNumber = Common.GetNullableInt(Common.GetRowString(row.Cells[6].Text));

            if (Common.GetRowString(row.Cells[7].Text) != "")
                item.CompleteDate = Convert.ToDateTime(row.Cells[7].Text);

            if (Common.GetRowString(row.Cells[8].Text) != "")
                item.MaterialPrice = Convert.ToDouble(row.Cells[8].Text);

            if (Common.GetRowString(row.Cells[9].Text) != "")
                item.BridgedID = Convert.ToInt32(row.Cells[9].Text);

            if (Common.GetRowString(row.Cells[10].Text) != "")
                item.ProcessPrice = Convert.ToDouble(row.Cells[10].Text);
            if (Common.GetRowString(row.Cells[11].Text) != "")
                item.DeliveryStatementNo = row.Cells[11].Text;

            if (Common.GetRowString(row.Cells[12].Text) != "")
                item.MaterialCd =Convert.ToInt32(row.Cells[12].Text);

            if (Common.GetRowString(row.Cells[13].Text) != "")
                item.Shape = Convert.ToInt32(row.Cells[13].Text);

            if (Common.GetRowString(row.Cells[14].Text) != "")
                item.Shade = Convert.ToInt32(row.Cells[14].Text);

            if (Common.GetRowString(row.Cells[15].Text) != "")
                item.AnatomyKit = Convert.ToInt32(row.Cells[15].Text);

            if (Common.GetRowString(row.Cells[16].Text) != "")
                item.CadOutputDone = Convert.ToBoolean(row.Cells[16].Text);

            if (Common.GetRowString(row.Cells[17].Text) != "")
                item.InsuranceKbn = Convert.ToBoolean(row.Cells[17].Text);

            item.DetailSeq = Convert.ToInt32(row.Cells[18].Text);
            if (Common.GetRowString(row.Cells[19].Text) != "")
                item.ProsthesisCd = Convert.ToInt32(row.Cells[19].Text);
            //Tooth Type
            if (Common.GetRowString(row.Cells[20].Text) != "")
                item.ChildFlg = Convert.ToBoolean(row.Cells[20].Text);
            if (Common.GetRowString(row.Cells[21].Text) != "")
                item.GapFlg = Convert.ToBoolean(row.Cells[21].Text);
            if (Common.GetRowString(row.Cells[22].Text) != "")
                item.DentureFlg = Convert.ToBoolean(row.Cells[22].Text);
            if (Common.GetRowString(row.Cells[23].Text) != "")
                item.BillStatementNo = row.Cells[23].Text;
            if (Common.GetRowString(row.Cells[24].Text) != "")
                item.OldMaterialCd = Convert.ToInt32(row.Cells[24].Text);
            else if (hiddenRegister.Value != "")
                item.OldMaterialCd = item.MaterialCd;

            return item;
        }
        private void SetOrderDetail(TrnOrderDetail item)
        {
            //ShowTitleDropDownList(Drop, item.Shape);
            txtDetailNm.Text = item.DetailNm;

            MasterProsthesis info = this.listMstProsthesis.Where(p => p.ProsthesisCd == item.ProsthesisCd).FirstOrDefault();
            if (info != null)
            {
                txtProsthesisAbbNm.Text = info.ProsthesisAbbNm;
                txtProsthesisNm.Text = info.ProsthesisNm;
                txtProsthesisType.Text = info.ProsthesisType;
            }

            if (item.Shape !=null) 
                ShowTitleDropDownList(DropDownShape, item.Shape.ToString());

            if (item.Shade != null) 
                ShowTitleDropDownList(DropDownShade, item.Shade.ToString());

            if (item.MaterialCd!=null)
                ShowTitleDropDownList(DropDownMaterial, item.MaterialCd.ToString());

            if (item.AnatomyKit!=null)
                ShowTitleDropDownList(DropDownAnatomyKit, item.AnatomyKit.ToString());

            if (item.CadOutputDone!=null)
            {
                if (Convert.ToBoolean(item.CadOutputDone) == true)
                    checkCAD.Checked = true;
            }

            if (item.InsuranceKbn!=null)
            {
                if (Convert.ToBoolean(item.InsuranceKbn) == true)
                {
                    radioInsuranceTrue.Checked = true;
                    radioInsuranceFalse.Checked = false;
                }
                else
                {
                    radioInsuranceTrue.Checked = false;
                    radioInsuranceFalse.Checked = true;
                }
            }

            if (item.ChildFlg!=null)
            {
                if (Convert.ToBoolean(item.ChildFlg) == true)
                    checkChildTooth.Checked = true;
            }
            if (item.GapFlg!=null)
            {
                if (Convert.ToBoolean(item.GapFlg) == true)
                    checkGap.Checked = true;
            }
            if (item.DentureFlg!=null)
            {
                if (Convert.ToBoolean(item.DentureFlg) == true)
                    checkDenture.Checked = true;
            }

            EnableDetailRegion(string.IsNullOrEmpty(item.BillStatementNo), item.OldMaterialCd == null); 

        }
        private void ShowTitleDropDownList(DropDownList dropDown, string itemValue)
        {
            try
            {
                ListItem item = dropDown.Items.FindByValue(itemValue);
                if (item != null)
                {
                    dropDown.SelectedValue = itemValue;
                }
                else
                {
                    dropDown.Text = "";
                }
            }
            catch { }
        }

        private void SetOrderDetail(int detailSeq)
        {
            TrnOrderDetail  item = GetOrderDetailFromViewState(detailSeq);
            if (item == null) //Get FromGirdView
            {
                foreach (GridViewRow row in gridViewOrderDetail.Rows)
                {
                    if (row.Cells[18].Text == detailSeq.ToString())  //DetailSeq 18
                    {
                        item = GetOrderDetailFromGridView(row);
                        break;
                    }
                }
            }
            if (item != null)
            {
                SetOrderDetail(item);
            }

        }
        #endregion

        #region Process TreeView
        private void SaveBeforeSelectedNodeChanged(string beforeDetailSeq) //beforeTooth 
        {
            if (beforeDetailSeq.Contains(GetResource("NODE_TOOTH_GROUP.Text")))
                return;
            if (!string.IsNullOrEmpty(beforeDetailSeq))
            {
                TrnOrderDetail newItem = GetCurrentOrderDetail();
                TrnOrderDetail i = null;
                if (ViewState["CurrentDetail"] != null)
                {
                    List<TrnOrderDetail> listDetail = (List<TrnOrderDetail>)ViewState["CurrentDetail"];
                    i = listDetail.FirstOrDefault(p => p.DetailSeq == Convert.ToInt32(beforeDetailSeq));
                    if (i != null)
                    {
                        #region OnlyCopy -> call Shadow Copy
                        i.DetailNm = newItem.DetailNm;
                        TreeNode currentNode = FindTreeNode(i.DetailSeq.ToString());
                        if (currentNode != null)
                        {
                            if (i.ToothNumber!=null)
                            {
                                currentNode.Text = i.ToothNumber + " : " + newItem.DetailNm;
                            }
                            else
                                currentNode.Text = newItem.DetailNm;
                        }
                        i.InsuranceKbn = newItem.InsuranceKbn;
                        i.ProsthesisCd = newItem.ProsthesisCd;
                        i.ProsthesisNm = newItem.ProsthesisNm;
                        i.MaterialCd = newItem.MaterialCd;
                        i.MaterialNm = newItem.MaterialNm;
                        i.Shade = newItem.Shade;
                        i.Shape = newItem.Shape;
                        i.AnatomyKit = newItem.AnatomyKit;
                        i.CadOutputDone = newItem.CadOutputDone;
                        i.ChildFlg = newItem.ChildFlg;
                        i.GapFlg = newItem.GapFlg;
                        i.DentureFlg = newItem.DentureFlg;
                        //Check Contains in GridView 
                        foreach (GridViewRow row in gridViewOrderDetail.Rows)
                        {
                            if (row.Cells[18].Text == i.DetailSeq.ToString())
                            {
                                TrnOrderDetail gridItem = GetOrderDetailFromGridView(row);
                                i.CompleteDate = gridItem.CompleteDate;
                                i.Amount = gridItem.Amount;
                                i.DeliveryStatementNo = gridItem.DeliveryStatementNo;
                                i.ProcessPrice = gridItem.ProcessPrice;
                                i.MaterialPrice = gridItem.MaterialPrice;
                                i.BillStatementNo = gridItem.BillStatementNo;
                                i.OldMaterialCd = gridItem.OldMaterialCd;
                                break;
                            }
                        }
                        #endregion
                    }
                }
                if (i == null)
                {
                    newItem.ToothNumber = null; //this case No tooth
                    newItem.DetailSeq = Convert.ToInt32(beforeDetailSeq);
                    AddViewState(newItem);
                }
            }
        }
        private void CreateTreeView(List<TrnOrderDetail> listDetail)
        {
            TreeViewTooth.Nodes.Clear();
            foreach (TrnOrderDetail detail in listDetail)
            {
                if (detail.BridgedID == null)
                {
                    AddTreeNode(null, detail, false);
                }
                else
                {
                    string groupName = GetResource("NODE_TOOTH_GROUP.Text") + detail.BridgedID;
                    //Find in Tree 
                    TreeNode treeRoot = TreeViewTooth.FindNode(groupName);
                    if (treeRoot == null)
                    {
                        treeRoot = new TreeNode();
                        treeRoot.Text = groupName;
                        treeRoot.Value = groupName;
                        TreeViewTooth.Nodes.Add(treeRoot);
                    }
                    AddTreeNode(treeRoot, detail, false);
                }
            }
            TreeViewTooth.ExpandAll();
        }
        private void InitTreeViewTeeth(GridViewRow rowSelected)
        {
            List<TrnOrderDetail> listDetail = new List<TrnOrderDetail>();
            //Add ViewState
            if (ViewState["CurrentDetail"] == null)
            {
                foreach (GridViewRow row in gridViewOrderDetail.Rows)
                {
                    if (row.Visible)
                    {
                        TrnOrderDetail item = GetOrderDetailFromGridView(row);
                        listDetail.Add(item);
                    }
                }
                ViewState.Add("CurrentDetail", listDetail);
                CreateTreeView(listDetail);
            }
            else
            {
                listDetail = (List<TrnOrderDetail>)(ViewState["CurrentDetail"]);
            }
            if (rowSelected != null)
            {
                //Selected Node
                TreeNode selectedNode = FindTreeNode(rowSelected.Cells[18].Text); //DetailSeq
                if (selectedNode != null)
                {
                    hiddenBeforeTooth.Value = hiddenAfterTooth.Value = selectedNode.Value;
                    selectedNode.Selected = true;
                    TrnOrderDetail item = listDetail.Find(p => p.DetailSeq == Convert.ToInt32(selectedNode.Value));
                    SetOrderDetail(item);
                }
            }
            else
            {
                ResetOrderDetail();
                EnableDetailRegion(false, false);
            }
        }
        private TreeNode FindTreeNode(string pathNode)
        {
            TreeNode findNode = null;
            findNode = TreeViewTooth.FindNode(pathNode);
            if (findNode == null)
            {
                foreach (TreeNode tree in TreeViewTooth.Nodes)
                {
                    if (tree.ChildNodes.Count > 0 && findNode == null)
                    {
                        foreach (TreeNode temp in tree.ChildNodes)
                        {
                            if (temp.Value == pathNode)
                                return temp;
                        }
                    }
                }
            }
            return findNode;
        }
        #endregion

        #region DropDown Change
        protected void TextStaffCd_TextChanged(object sender, EventArgs e)
        {
            GetAutomaticDropDownList(TextStaffCd, DropDownSalesMan);
        }
        protected void DropDownStaff_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextStaffCd.Text = DropDownSalesMan.SelectedItem.Value;
            FillDentalOffice();
        }
        protected void TextOfficeCd_TextChanged(object sender, EventArgs e)
        {
            GetAutomaticDropDownList(TextOfficeCd, DropDownDentalOffice);
            DropDownDentalOffice_SelectedIndexChanged(sender, e);
        }
        protected void DropDownDentalOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownDentalOffice_SelectedIndexChanged();
        }
        private void DropDownDentalOffice_SelectedIndexChanged()
        {
            if (DropDownDentalOffice.SelectedItem != null)
            {
                TextOfficeCd.Text = DropDownDentalOffice.SelectedItem.Value;
                int? staffCd = null;
                if (DropDownSalesMan.SelectedItem.Value != "")
                    staffCd = Convert.ToInt32(DropDownSalesMan.SelectedItem.Value);
                List<CodeName> list = CodeName.GetListCodeNameDentalOfficeMasterByStaff(Convert.ToInt32(hiddenOfficeCd.Value), staffCd);
                CodeName find = list.Find(p => p.Code == TextOfficeCd.Text);
                if (find != null)
                {
                    hiddenTransferDay.Value = find.HiddenValue;
                    SetDealineDate();
                }
            }
        }
        protected void TextDueDate_TextChanged(object sender, EventArgs e)
        {
            SetDealineDate();
        }
        private void SetDealineDate()
        {
            if (string.IsNullOrEmpty(TextDueDate.Text))
            {
                TextDeadLine.Text = "";
            }
            else
            {
                int transferDay = 0;
                if (!string.IsNullOrEmpty(hiddenTransferDay.Value.Trim()))
                    transferDay = Convert.ToInt32(hiddenTransferDay.Value);
                DateTime setDate;
                if (DateTime.TryParse(TextDueDate.Text, out setDate))
                {
                    setDate = setDate.AddDays(-transferDay);
                    TextDeadLine.Text = setDate.ToShortDateString();
                }
            }
        }
        private void FillDentalOffice()
        {
            try
            {
                TextOfficeCd.Text = "";
                DropDownDentalOffice.Items.Clear();
                int? staffCd = null;
                if (DropDownSalesMan.SelectedItem.Value != "")
                    staffCd = Convert.ToInt32(DropDownSalesMan.SelectedItem.Value);
                List<CodeName> listOffice = CodeName.GetListCodeNameDentalOfficeMasterByStaff(Convert.ToInt32(hiddenOfficeCd.Value), staffCd); 
                foreach (CodeName i in listOffice)
                {
                    DropDownDentalOffice.Items.Add(new ListItem(i.Name, i.Code));
                }

                DropDownDentalOffice.SelectedIndex = 0;
                DropDownDentalOffice_SelectedIndexChanged();
            }
            catch (Exception ex)
            {
                logger.Error("Error FillDentalOffice ", ex);
            }
        }
  
        #endregion

        #region CAD_CAM
        protected string generateCadCam(int officeCd, TrnOrderHeader header, List<TrnOrderDetail> list)
        {
            MasterSystem cSys = new MasterSystem();
            MasterItem cItem = new MasterItem();
            var folder = Server.MapPath("~/Portal/XMLOrder");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!Directory.Exists(folder +"\\"+ header.OrderNo))
                Directory.CreateDirectory(folder + "\\" + header.OrderNo);

            if (!Directory.Exists(folder+"\\" + header.OrderNo + "/DWOrder"))
                Directory.CreateDirectory(folder+"\\" + header.OrderNo + "/DWOrder");

          //  StringBuilder output = new StringBuilder();

            string _fileName = folder + "\\" + header.OrderNo + "\\DWOrder\\" + "DWOrder.XML";

            if (File.Exists(_fileName))
            {
                bool isReadOnly = (File.GetAttributes(_fileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                if (isReadOnly)
                    File.SetAttributes(_fileName, File.GetAttributes(_fileName) & ~FileAttributes.ReadOnly);
            }

            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = true;
            XmlWriter writer = XmlWriter.Create(_fileName, ws);            //output, ws);

            writer.WriteStartDocument();
            writer.WriteStartElement("Order");

            writer.WriteAttributeString("xmlns", "xsi", null, MasterSystem.GetSystemMaster("OrderXmlnsXsi").Value);
            //"http://www.w3.org/2001/XMLSchema-instance");
            //writer.WriteAttributeString("xmlns",null, "http://www.dental-wing.com/DWOS"); //cSys.GetValue("OrderXmlns"));
            writer.WriteAttributeString("type", MasterSystem.GetSystemMaster("OrderType").Value);
            writer.WriteAttributeString("version", MasterSystem.GetSystemMaster("OrderVersion").Value);
            //============================================
            writer.WriteComment("Global Information");

            string _OriginalCompany = MasterSystem.GetSystemMaster("CompanyName").Value,
                _OriginalOrderID = header.OrderNo,
                _OriginalCreationDate = header.OrderDate.ToString("yyyy-MM-dd"),   //ToShortDateString(),
                _DentistFullName = header.DentistNm,
                _PatientLastName = header.PatientLastNm,
                _PatientFirstName = header.PatientFirstNm;

            writer.WriteElementString("OriginalCompany", _OriginalCompany);
            writer.WriteElementString("OriginalOrderID", _OriginalOrderID);
            writer.WriteElementString("OriginalCreationDate", _OriginalCreationDate);
            writer.WriteElementString("DentistFullName", _DentistFullName);
            writer.WriteElementString("PatientLastName", _PatientLastName);
            writer.WriteElementString("PatientFirstName", _PatientFirstName);

            string sMaterial = "";
            //============================================
            writer.WriteComment("Clinical Information");

            writer.WriteStartElement("ClinicalModel");
            writer.WriteAttributeString("numbering", MasterSystem.GetSystemMaster("ToothNumber").Value);  //"International");
            writer.WriteComment("brigde between 13-16 with 2 pontics, an arch and an antagonist");
            writer.WriteStartElement("ClinicalPreparationsList");

            foreach (TrnOrderDetail dt in list)
            {
                if (dt.CadOutputDone == true)
                {
                    //Lap lai
                    writer.WriteStartElement("ClinicalPreparation");
                    writer.WriteElementString("ClinicalNumber", dt.ToothNumber.Value.ToString()); //16
                    writer.WriteElementString("SurfaceFile", "");
                    //writer.WriteElementString("ClinicalType", dt.ProsthesisNm); //"STUMP");

                    MasterProsthesis prosInfo = new MasterProsthesis();
                    prosInfo = MasterProsthesis.GetProsthesis(int.Parse(hiddenOfficeCd.Value), dt.ProsthesisCd);
                    writer.WriteElementString("ClinicalType", prosInfo.ProsthesisType);
                    //switch (dt.ProsthesisCd)
                    //{
                    //    case 1:
                    //        writer.WriteElementString("ClinicalType", "STUMP");
                    //        break;
                    //    case 2:
                    //        writer.WriteElementString("ClinicalType", "PONTIC");
                    //        break;
                    //    case 3:
                    //        writer.WriteElementString("ClinicalType", "ARCH");
                    //        break;
                    //    default: 
                    //       writer.WriteElementString("ClinicalType", dt.ProsthesisNm);
                    //      break;

                    //}

                    writer.WriteEndElement();
                    //Het lap lai
                    if (!sMaterial.Contains(dt.MaterialCd + ""))
                        if (sMaterial != "") sMaterial = sMaterial + ";" + dt.MaterialCd;
                        else sMaterial = dt.MaterialCd.ToString();
                    
                }
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            //============================================
            writer.WriteComment("Prosthetic Information");
            writer.WriteStartElement("ProstheticModelsList");
            foreach (string ss in sMaterial.Split(';'))
            {
                // Cho tung loai material
                writer.WriteStartElement("ProstheticModel");
                //if (ss == "1")
                //    writer.WriteElementString("Material", "DWOS Ceramic");
                //if (ss == "2")
                //    writer.WriteElementString("Material", "DWOS Metal");
                //if (ss == "3")
                //    writer.WriteElementString("Material", "DWOS WAX");

                writer.WriteElementString("Material", list.Where(s => (s.MaterialCd == Common.GetNullableInt(ss))).FirstOrDefault().MaterialNm);
                writer.WriteStartElement("ToothProsthesisList");

                foreach (TrnOrderDetail dt in list)
                {
                    if (dt.CadOutputDone == true && Common.GetNullableInt(ss) == dt.MaterialCd)
                    {
                        //Lap lai
                        writer.WriteStartElement("ToothProsthesis");
                        writer.WriteAttributeString("type", MasterItem.GetItemMaster("Form", (int)dt.Shape).ItemValue);     //"FULL_CROWN");
                        writer.WriteElementString("ProsthesisToothNumber", dt.ToothNumber + "");  // "16");
                        //if (ss == "1")
                        //    writer.WriteElementString("ProsthesisToothColor", cItem.GetValue("Shade", dt.Shade + ""));   // "A5");

                        string ProsthesisToothColor = "";
                        if (dt.Shade+"" != string.Empty)
                        {
                            ProsthesisToothColor = MasterItem.GetAll().Where(i=>(i.ItemCathegory=="Shade" && i.ItemNo == dt.Shade)).FirstOrDefault().ItemValue;
                           
                        }
                         writer.WriteElementString("ProsthesisToothColor", ProsthesisToothColor);
                        ///writer.WriteElementString("Material", dt.MaterialNm);   //"DWOS");
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();   //ToothProsthesisList
                writer.WriteEndElement();   //ProstheticModel

                //Ket thuc cho loai Material
            }
            writer.WriteEndElement();   //ProstheticModelsList
            writer.WriteFullEndElement();//Order
            writer.Flush();
            writer.Close();

            //FileSystemUtils.AddToZip(

            //string _fileName = this.PortalSettings.HomeDirectoryMapPath + "\\XMLOrder\\" + header.OrderNo + "\\DWOrder\\" + "DWOrder.XML";
            CreateZipFile(folder + "\\" + header.OrderNo,
                folder + "\\" + header.OrderNo + ".xorder");

            return folder + "/" + header.OrderNo + ".xorder";  // output.ToString();
            
        }

        private void CreateZipFile(string _folderZip, string _ZipFile)
        {
            if (!Directory.Exists(_folderZip))
            {
                return;
            }

            using (ZipFile zip = new ZipFile(Encoding.UTF8))
            {
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.TempFileFolder = Server.MapPath("~/Portal/XMLOrder");
                zip.StatusMessageTextWriter = System.Console.Out;
                zip.AddDirectory(_folderZip); // recurses subdirectories
                zip.Save(_ZipFile);
            }
        }

        #endregion

        protected void TextOrderDate_TextChanged(object sender, EventArgs e)
        {
              FillDropDownMaterialList();
        }

        protected void txtProsthesisAbbNm_TextChanged(object sender, EventArgs e)
        {
            try
            {
                MasterProsthesis info = this.listMstProsthesis.Where(p => p.ProsthesisAbbNm == txtProsthesisAbbNm.Text.Trim()).FirstOrDefault();
                if (info != null)
                {
                    txtProsthesisNm.Text = info.ProsthesisNm;
                    txtProsthesisType.Text = info.ProsthesisType;
                }
                else
                {
                    //string msgError = string.Format(GetResource("MSG_CANNOT_FIND_PROSTHESIS_ABBNM.Text"), txtProsthesisAbbNm.Text);
                    txtProsthesisAbbNm.Text = txtProsthesisNm.Text = txtProsthesisType.Text = string.Empty;
                    //throw new Exception(msgError);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error txtProsthesisAbbNm_TextChanged ", ex); 
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            }
        }

        #region Next AppearTooth
        private int GetDetailSeq()
        {
            int maxValue = 0;
            if (ViewState["CurrentDetail"] != null)
            {
                List<TrnOrderDetail> listDetail = (List<TrnOrderDetail>)ViewState["CurrentDetail"];
                if (listDetail.Count > 0)
                    maxValue = listDetail.Max(p => p.DetailSeq);
            }
            return ++maxValue;
        }
        private int GetGroupId()
        {
            int maxValue = 0;
            if (ViewState["CurrentDetail"] != null)
            {
                List<TrnOrderDetail> listDetail = (List<TrnOrderDetail>)ViewState["CurrentDetail"];
                if (listDetail.Count > 0)
                    maxValue = listDetail.Max(p => p.BridgedID == null ? 0 : Convert.ToInt32(p.BridgedID));
            }
            return ++maxValue;
        }
        private int GetTotalToothNumberAppear(int? toothNumber)
        {
            int total = 0;
            if (ViewState["CurrentDetail"] != null)
            {
                List<TrnOrderDetail> listDetail = (List<TrnOrderDetail>)ViewState["CurrentDetail"];
                if (listDetail.Count > 0)
                    total = listDetail.Where(p => p.ToothNumber == toothNumber).Count();
            }
            return total;
        }
        private void RemoveOrderDetail(int detailSeq)
        {
            List<TrnOrderDetail> listDetail = (List<TrnOrderDetail>)ViewState["CurrentDetail"];
            listDetail.RemoveAll(p => p.DetailSeq == detailSeq);
        }
        #endregion

        protected void btnAddNoTooth_Click(object sender, EventArgs e)
        {
            try
            {
                //Create NoTooth 
                hiddenBeforeTooth.Value = hiddenAfterTooth.Value;
                SaveBeforeSelectedNodeChanged(hiddenAfterTooth.Value);

                TrnOrderDetail detail = new TrnOrderDetail();
                detail.DetailSeq = GetDetailSeq();
                detail.ToothNumber = null;
                detail.DetailNm = string.Format(GetResource("NODE_TOOTH_NUMBER.Text"), string.Empty);

              
                TreeNode treeNode;
                treeNode = new TreeNode();
                treeNode.Value = detail.DetailSeq.ToString();
                treeNode.Text = detail.DetailNm;
                treeNode.Selected = true;
                TreeViewTooth.Nodes.Add(treeNode);
                //Add to this Node to ViewState
                AddViewState(detail);
                ResetOrderDetail();
                txtDetailNm.Text = detail.DetailNm;
                EnableDetailRegion(true, true);
                hiddenAfterTooth.Value = TreeViewTooth.SelectedNode.Value;
            }
            catch (Exception ex)
            {
                logger.Error("Error btnAddNoTooth_Click ", ex);
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR"), ex.Message) + "\");");
            }
        }
        private void AddTreeNode(TreeNode rootNode, TrnOrderDetail detail, bool selected)
        {
            TreeNode newNode = new TreeNode();
            newNode.Value = detail.DetailSeq.ToString();
            newNode.Text = detail.ToothNumber == null ? detail.DetailNm : detail.ToothNumber + " : " + detail.DetailNm;
            newNode.Selected = selected;

            if (!string.IsNullOrEmpty(detail.BillStatementNo))
                newNode.ShowCheckBox = false;

            if (rootNode != null)
                rootNode.ChildNodes.Add(newNode);
            else
                TreeViewTooth.Nodes.Add(newNode);
        }
        private void EnableDetailRegion(bool enable,bool bMaterial)
        {
            txtDetailNm.Enabled = txtProsthesisAbbNm.Enabled = DropDownAnatomyKit.Enabled = DropDownShape.Enabled = DropDownShade.Enabled = radioInsuranceFalse.Enabled = radioInsuranceTrue.Enabled = checkCAD.Enabled = checkChildTooth.Enabled = checkDenture.Enabled = checkGap.Enabled = btnUp.Enabled = btnDown.Enabled = enable;
            DropDownMaterial.Enabled = bMaterial;
        }
        private void ResetOrderInput()
        {
            hiddenRegister.Value = "";
            RemoveViewState("CurrentDetail");
            TextOrderNo.Text = TextRefOrderNo.Text  =  TextOrderDate.Text =  TextDueDate.Text =  TextPatientLastNm.Text = TextPatientLastNm.Text=  TextPatientFirstNm.Text ="";
            TextDentist.Text = TextDeadLine.Text = TextStaffCd.Text = TextOfficeCd.Text = TextSetDate.Text = TextAge.Text = TextComments.Text = TextBorrowPart.Text = "";
            checkRemanufacture.Checked = checkTrialOrder.Checked = false;
            FillDataToGridView(new List<TrnOrderDetail>());
        }

        #region Up,Down Tooth
        protected void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode Node = this.TreeViewTooth.SelectedNode;
                if (Node != null)
                {

                    TreeNode parentNode = this.TreeViewTooth.SelectedNode.Parent;
                    if (parentNode != null)
                    {
                        int index = -1;

                        for (int j = 0; j < parentNode.ChildNodes.Count; j++)
                        {
                            if (Node == parentNode.ChildNodes[j])
                            {
                                index = j;
                                break;
                            }
                        }

                        if (index > 0)
                        {
                            parentNode.ChildNodes.RemoveAt(index);
                            parentNode.ChildNodes.AddAt(index - 1, Node);
                            Node.Selected = true;
                        }
                    }
                    else
                    {
                        int index = -1;

                        for (int i = 0; i < TreeViewTooth.Nodes.Count; i++)
                        {
                            if (Node == TreeViewTooth.Nodes[i])
                            {
                                index = i;
                                break;
                            }
                        }
                        if (index > 0)
                        {

                            TreeViewTooth.Nodes.RemoveAt(index);
                            TreeViewTooth.Nodes.AddAt(index - 1, Node);
                            Node.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error btnUp_Click , Move Node ", ex);
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR"), ex.Message) + "\");");
            }
        }

        protected void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode Node = this.TreeViewTooth.SelectedNode;
                if (Node != null)
                {
                    TreeNode parentNode = this.TreeViewTooth.SelectedNode.Parent;
                    if (parentNode != null)
                    {
                        int index = -1;
                        for (int j = 0; j < parentNode.ChildNodes.Count; j++)
                        {
                         if (Node == parentNode.ChildNodes[j])
                            {
                                index = j;
                                break;
                            }
                        }
                        if (index < parentNode.ChildNodes.Count - 1)
                        {
                            parentNode.ChildNodes.RemoveAt(index);
                            parentNode.ChildNodes.AddAt(index + 1, Node);
                            Node.Selected = true;
                        }
                    }
                    else
                    {
                        int index = -1;
                        for (int i = 0; i < TreeViewTooth.Nodes.Count; i++)
                        {
                            if (Node == TreeViewTooth.Nodes[i])
                            {
                                index = i;
                                break;
                            }
                        }
                        if (index < TreeViewTooth.Nodes.Count - 1)
                        {
                            TreeViewTooth.Nodes.RemoveAt(index);
                            TreeViewTooth.Nodes.AddAt(index + 1, Node);
                            Node.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error btnDown_Click , Move Node ", ex);
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR"), ex.Message) + "\");");
            }
        }
        #endregion 

        #region ChildTooth , Denture 
        protected void checkChildTooth_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if ( checkChildTooth.Checked && TreeViewTooth.SelectedNode != null)
                {
                    List<TrnOrderDetail> listDetail = GetListTrnOrderDetail();
                    TrnOrderDetail item = listDetail.FirstOrDefault(p => p.DetailSeq.ToString() == TreeViewTooth.SelectedNode.Value);
                    if (item != null && item.ToothNumber!=null && (Convert.ToInt32(item.ToothNumber) % 10 > 5))
                    {
                        item.ChildFlg = false;
                        checkChildTooth.Checked = false;
                        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_INFO.Text"), GetResource("MSG_CHILD_TOOTH_NOT_HAPPEN.Text")) + "\");");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error checkChildTooth_CheckedChanged ", ex);
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR"), ex.Message) + "\");");
            }
        }

        protected void checkDenture_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (TreeViewTooth.SelectedNode != null)
                {
                    List<TrnOrderDetail> listDetail = GetListTrnOrderDetail();
                    TrnOrderDetail item = listDetail.FirstOrDefault(p => p.DetailSeq.ToString() == TreeViewTooth.SelectedNode.Value);
                    if (item != null && item.BridgedID!=null)
                    {
                        List<TrnOrderDetail> listBridged = listDetail.Where(p => p.BridgedID == item.BridgedID).ToList();
                        foreach (TrnOrderDetail i in listBridged)
                        {
                            i.DentureFlg = checkDenture.Checked;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("Error checkDenture_CheckedChanged ", ex);
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR"), ex.Message) + "\");");
            }
        }
        #endregion 

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static List<string> SearchProsthesis(string prefixText, int count)
        {
            // TrnOrderHeader.logger.Debug("SearchProsthesis , listProsthesisStatic.Count = " + listProsthesisStatic.Count);
            //if (listProsthesisStatic != null)
            //{
            //    var listSearch = listProsthesisStatic.Where(p => p.ProsthesisAbbNm.Contains(prefixText) || p.ProsthesisCd.ToString().Contains(prefixText)).Select(p => p.ProsthesisAbbNm);
            //    return listSearch.ToList();
            //}
            List<string> list = new List<string>();
            list.Add("test");
            list.Add("test1");
            list.Add("31");
            list.Add("31");
            return list;

        }

        //Download Cad/Cam
        protected void lBtnDownload_Click(object sender, EventArgs e)
        {
            string filePath = HyperLink1.NavigateUrl;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }
}