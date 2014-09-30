using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Dental.Utilities;

namespace Dental.Domain
{
    [Serializable]
    public class OperationProcessInfo
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(OperationProcessInfo));

        public TrnOrderDetail detail; 
        public bool useOutsource { get; set; }     

        public List<CustomProcess> listCustomProcess { get; set; }
        public TrnOutsource OutSource { get; set; }
        public int detailChanged = 0; // 0 -> no change , 1 -> Changed
        public OperationProcessInfo()
        {
            listCustomProcess = new List<CustomProcess>();
            listCustomProcess.Add(new CustomProcess());
        }

        public static OperationProcessInfo getItem(DateTime orderDate, TrnOrderDetail item, List<TrnProcess> listProcess, TrnOutsource objOutSource, objDropDownList dropDownSource)
        {
            OperationProcessInfo obj = new OperationProcessInfo();
            obj.detail = item;
            for (int i = 0; i < listProcess.Count; i++)
            {
                CustomProcess temp = new CustomProcess();
                temp.ProcessCd = listProcess[i].ProcessCd;
                temp.ProcessTime = Common.GetNullableString(listProcess[i].ProcessTime);
                temp.StaffCd = Common.GetNullableString(listProcess[i].StaffCd);
                temp.ID = i + 1;

                temp.ProcessName = CodeName.GetNameFromCode(dropDownSource.listProcess, temp.ProcessCd.ToString());
                temp.StaffNm = CodeName.GetNameFromCode(dropDownSource.listStaff, temp.StaffCd);
                obj.listCustomProcess.Add(temp);
            }

            obj.OutSource = objOutSource; //Set OutSource
            obj.useOutsource = false;
            if (objOutSource == null && obj.listCustomProcess.Count == 1)
            {
                obj.useOutsource = false;
                List<CodeName> listDefaultProcess = CodeName.GetListMstProcessTemplateByProsthesisCd(item.OfficeCd, item.ProsthesisCd);
                logger.Debug("Case get default process template listDefaultProcess.Count = " + listDefaultProcess.Count);
                for (int i = 0; i < listDefaultProcess.Count; i++)
                {
                    CustomProcess temp = new CustomProcess();
                    temp.ProcessCd = Convert.ToInt32(listDefaultProcess[i].Code);
                    temp.ID = i + 1;
                    temp.ProcessName = CodeName.GetNameFromCode(dropDownSource.listProcess, temp.ProcessCd.ToString());
                    obj.listCustomProcess.Add(temp);
                }
            }
            else if (objOutSource != null)
            {
                obj.useOutsource = true;
            }

            logger.Debug(string.Format("DetailNm = {0} ,  obj.listCustomProcess = {1} ", obj.detail.DetailNm, obj.listCustomProcess.Count));
            return obj;
        }

        public static List<OperationProcessInfo> GetOperationProcessInfo(int officeCd, double orderSeq, DateTime orderDate)
        {
            logger.DebugFormat("Begin getOperationProcessInfo officeCd = {0} ,orderSeq = {1}", officeCd, orderSeq);
            List<OperationProcessInfo> listOperationProcessInfo = new List<OperationProcessInfo>();


            List<TrnOrderDetail> listDetail = TrnOrderDetail.GetTrnOrderDetailList(officeCd, orderSeq);
            List<TrnProcess> listProcess = TrnProcess.GetTrnProcessList(officeCd, orderSeq);
            List<TrnOutsource> listTrnOutsource = TrnOutsource.GetTrnOutsourceList(officeCd, orderSeq);

            logger.Debug("listTrnOutsource.COUNT = " + listTrnOutsource.Count);
            objDropDownList dropdownSource = new objDropDownList(officeCd);

            foreach (TrnOrderDetail i in listDetail)
            {
                List<TrnProcess> list = listProcess.Where(p => p.DetailSeq == i.DetailSeq).ToList();

                TrnOutsource objOutSource = listTrnOutsource.Find(p => p.DetailSeq == i.DetailSeq);
                listOperationProcessInfo.Add(OperationProcessInfo.getItem(orderDate, i, list, objOutSource, dropdownSource));
            }
            logger.Debug("TOTAL listOperationProcessInfo COUNT = " + listOperationProcessInfo.Count);
            return listOperationProcessInfo;
        }
   
        public static void OperationProcess_UpdateOperationMstStock(DBContext db , OperationProcessInfo info)
        {
            int outsourceLabCd = 0; //alway 0
            TrnOrderDetail detailItem = TrnOrderDetail.GetDentalOrderDetail(info.detail.OfficeCd, info.detail.OrderSeq, info.detail.DetailSeq);
            TrnOutsource outsourceItem = TrnOutsource.GetTrnOutsource(info.detail.OfficeCd , info.detail.OrderSeq , info.detail.DetailSeq);
            double amountOld = detailItem.Amount == null ? 0 : detailItem.Amount.Value;
            double amount = info.detail.Amount == null ? 0 : info.detail.Amount.Value;
            int inoutKbn = 11; 
            if (amount > amountOld)
            {
                inoutKbn =21;
            }
            if (outsourceItem != null)
            {
                if (info.detail.MaterialCd == null || info.detail.UnitCd == null || info.detail.SupplierCd == null || info.detail.MaterialCd != detailItem.MaterialCd || info.detail.UnitCd != detailItem.UnitCd || outsourceItem.OutsourceCd != outsourceLabCd || info.detail.SupplierCd != detailItem.SupplierCd)
                {
                    logger.Debug("update master stock and TrnStock In out");
                    //Update MstStock Old 
                    if ((amount != 0 || amountOld != 0) && detailItem.MaterialCd != null && detailItem.SupplierCd != null && detailItem.UnitCd != null)
                    {
                        MasterStock stockItem = MasterStock.GetStockMaster(info.detail.OfficeCd, detailItem.MaterialCd.Value, detailItem.SupplierCd.Value, outsourceItem.OutsourceCd, detailItem.UnitCd);
                        if (stockItem != null)
                        {
                            stockItem.Amount = detailItem.Amount == null ? 0 : detailItem.Amount.Value + info.detail.Amount == null ? 0 : info.detail.Amount.Value;
                            stockItem.CreateAccount = stockItem.ModifiedAccount = info.detail.ModifiedAccount;
                            logger.Debug("update master stock1");
                            stockItem.Update();

                        }
                    }
                    // Update StockInout Old
                    if (detailItem.MaterialCd != null && detailItem.SupplierCd != null && detailItem.UnitCd != null)
                    {
                        TrnStockInOut sitem = new TrnStockInOut();
                        sitem.OfficeCd = info.detail.OfficeCd;
                        sitem.OrderSeq = info.detail.OrderSeq;
                        sitem.DetailSeq = info.detail.DetailSeq;
                        sitem.RegisterDate = DateTime.Now;
                        sitem.InOutKbn = 11;
                        sitem.SupplierCd = detailItem.SupplierCd;
                        sitem.OutsourceLabCd = outsourceItem.OutsourceCd;
                        sitem.MaterialCd = detailItem.MaterialCd.Value;
                        sitem.Amount = detailItem.Amount == null ? 0 : detailItem.Amount.Value;
                        sitem.UnitCd = detailItem.UnitCd;
                        sitem.Price = detailItem.Price;
                        sitem.SumPrice = detailItem.MaterialPrice;
                        sitem.CreateAccount = sitem.ModifiedAccount = info.detail.ModifiedAccount;
                        sitem.CreateDate = sitem.ModifiedDate = DateTime.Now;
                        // sitem.Insert();
                        logger.Debug("InsertWithoutKey 1");
                        TrnStockInOut.InsertWithoutKey(db, sitem);

                        amountOld = 0;
                    }
                }
            }

            if (info.detail.Amount != amountOld && info.detail.MaterialCd != null && info.detail.SupplierCd != null && info.detail.UnitCd != null)
            {
                logger.Debug("insert DENTAL_TrnStockInOut  and update mst stock");
                //insert DENTAL_TrnStockInOut 
                TrnStockInOut sitem = new TrnStockInOut();
                sitem.OfficeCd = info.detail.OfficeCd;
                sitem.OrderSeq = info.detail.OrderSeq;
                sitem.DetailSeq = info.detail.DetailSeq;
                sitem.RegisterDate = DateTime.Now;
                sitem.InOutKbn = inoutKbn;
                sitem.SupplierCd = info.detail.SupplierCd;
                sitem.OutsourceLabCd = outsourceLabCd; 
                sitem.MaterialCd = info.detail.MaterialCd.Value;
                sitem.Amount = Math.Abs(amount - amountOld);
                sitem.UnitCd = info.detail.UnitCd;
                sitem.Price = info.detail.Price;
                sitem.SumPrice = info.detail.MaterialPrice;
                sitem.CreateAccount = sitem.ModifiedAccount = info.detail.ModifiedAccount;
                sitem.CreateDate = sitem.ModifiedDate = DateTime.Now;
                //sitem.Insert();

                logger.Debug("InsertWithoutKey 2");
                TrnStockInOut.InsertWithoutKey(db, sitem);

                //Update MSt Stock
                 MasterStock stockItem = MasterStock.GetStockMaster(info.detail.OfficeCd , info.detail.MaterialCd.Value , info.detail.SupplierCd.Value  , outsourceLabCd , info.detail.UnitCd );
                 if(stockItem !=null) 
                 {
                       stockItem.Amount =  stockItem.Amount  - amount + amountOld;
                       stockItem.CreateAccount = stockItem.ModifiedAccount = info.detail.ModifiedAccount;
                       logger.Debug("update master stock2");
                       stockItem.Update();
                 }
            }

            //update detail 
            detailItem.MaterialCd = info.detail.MaterialCd;
            detailItem.MaterialNm = info.detail.MaterialNm; 
            detailItem.SupplierCd = info.detail.SupplierCd; 
            detailItem.Amount = info.detail.Amount; 
            detailItem.UnitCd = info.detail.UnitCd; 
            detailItem.Price = info.detail.Price;
            detailItem.MaterialPrice = info.detail.MaterialPrice; 
            detailItem.ManufactureStaff = info.detail.ManufactureStaff; 
            detailItem.CompleteDate = info.detail.CompleteDate; 
            detailItem.InspectionStaff  = info.detail.InspectionStaff; 
            detailItem.ModifiedAccount = info.detail.ModifiedAccount;

            logger.Debug("update detail");
            detailItem.Update();
           
        }
        public static void RegisterOperationProcess(List<OperationProcessInfo> listInfo, int OfficeCd, double OrderSeq, string modifyAccount)
        {
            logger.Debug("Begin RegisterOperationProcess total listInfo.count = " + listInfo.Count);
           
          
            List<TrnProcess> listProcessDB = TrnProcess.GetTrnProcessList(OfficeCd, OrderSeq);
            List<TrnOutsource> listOutSourceDB = TrnOutsource.GetTrnOutsourceList(OfficeCd, OrderSeq);

            logger.DebugFormat("listProcessDB = {0} , listOutSourceDB = {1}", listProcessDB.Count, listOutSourceDB.Count);

            using (DBContext db = new DBContext())
            {
                using (System.Data.Common.DbTransaction tran = db.UseTransaction())
                {
                    try
                    {   
                        foreach (OperationProcessInfo info in listInfo)
                        {
                            if (info.detailChanged == 0 || !string.IsNullOrEmpty(info.detail.BillStatementNo))
                                continue;

                            info.detail.ModifiedAccount = modifyAccount;

                            logger.DebugFormat(" Loop info.detailSeq  = {0} info.DetailNm = {1}", info.detail.DetailSeq, info.detail.DetailNm);

                            #region Update DETAIL
                            //1.Update Order Detail -> only Case choose
                            logger.Debug("info.useOutsource = " + info.useOutsource);
                            OperationProcess_UpdateOperationMstStock(db , info);
                            
                            logger.Debug("Success Update Detail-MstStock-StockInOut");
                            #endregion

                            //2.Insert , Update , Delete Process
                            List<TrnProcess> listTempProcess = listProcessDB.Where(p => p.DetailSeq == info.detail.DetailSeq).ToList();
                            listProcessDB.RemoveAll(p => p.DetailSeq == info.detail.DetailSeq);

                            TrnOutsource objOutSourceDB = listOutSourceDB.Find(p => p.DetailSeq == info.detail.DetailSeq);
                            listOutSourceDB.RemoveAll(p => p.DetailSeq == info.detail.DetailSeq);

                            logger.DebugFormat("listTempProcess.Count  = {0} , listCustomProcess.Count = {1} ", listTempProcess.Count, info.listCustomProcess.Count);

                            #region GetListProcess Insert,Delete , Update Detail

                            List<TrnProcess> listProcessInsert = new List<TrnProcess>();
                            List<TrnProcess> listProcessUpdate = new List<TrnProcess>();

                            for (int i = 1; i < info.listCustomProcess.Count; i++)
                            {
                                #region GetDental Process
                                TrnProcess temp = new TrnProcess();
                                temp.OfficeCd = OfficeCd;
                                temp.OrderSeq = OrderSeq;
                                temp.DetailSeq = info.detail.DetailSeq;
                                temp.ProcessNo = i;
                                temp.ProcessCd = Convert.ToInt32(info.listCustomProcess[i].ProcessCd);


                                temp.StaffCd = Common.GetNullableInt(info.listCustomProcess[i].StaffCd);
                                temp.ProcessTime = Common.GetNullableInt(info.listCustomProcess[i].ProcessTime);
                                temp.CreateAccount = temp.ModifiedAccount = modifyAccount;

                                #endregion

                                TrnProcess itemProcessUpdate = listTempProcess.FirstOrDefault(p => p.ProcessNo == temp.ProcessNo);
                                if (itemProcessUpdate != null) //case update
                                {
                                    listProcessUpdate.Add(temp);
                                    listTempProcess.Remove(itemProcessUpdate);
                                }
                                else
                                {
                                    listProcessInsert.Add(temp);
                                }
                            }

                            #endregion

                            #region Delete Process

                            logger.Debug("2.1 DeleteTrnProcess , listTempProcess =  " + listTempProcess.Count);
                            foreach (TrnProcess i in listTempProcess)
                            {
                                i.Delete();
                            }
                            #endregion

                            #region Update Process
                            logger.Debug("2.2 Update PROCESS , listUpdate =  " + listProcessUpdate.Count);
                            foreach (TrnProcess process in listProcessUpdate)
                            {
                                process.Update();
                            }
                            #endregion

                            #region Insert Process
                            logger.Debug("2.3.INSERT PROCESS , listProcessInsert =  " + listProcessInsert.Count);
                            foreach (TrnProcess process in listProcessInsert)
                            {
                                process.Insert();
                            }
                            #endregion

                            #region Insert,Update OutSource-Purchase
                            if (!info.useOutsource) //Xoa outsource va purchase
                            {
                                if (info.OutSource != null && info.OutSource.OutsourceCd != -1)
                                {
                                    if (info.OutSource.PurchaseSeq == null)
                                        info.OutSource.Delete();
                                    else
                                    {
                                        //get Pay Money 
                                        TrnPurchase purchaseItem = TrnPurchase.GetTrnPurchase(OfficeCd, info.OutSource.OutsourceCd, info.OutSource.PurchaseSeq.Value);
                                        if (purchaseItem == null)
                                            info.OutSource.Delete();
                                        else
                                        {
                                            if (purchaseItem.PaidMoney == null || purchaseItem.PaidMoney == 0)
                                            {
                                                info.OutSource.Delete();
                                                purchaseItem.Delete();
                                            }
                                        }
                                    }
                                }

                            }
                            else if (info.OutSource != null && info.OutSource.OutsourceCd != -1)
                            {
                                logger.Debug(" 4.Case Insert,Update OutSource-Purchase");
                                info.OutSource.OfficeCd = OfficeCd;
                                info.OutSource.OrderSeq = OrderSeq;
                                info.OutSource.DetailSeq = info.detail.DetailSeq;
                                info.OutSource.CreateAccount = info.OutSource.ModifiedAccount = modifyAccount;

                                double PurchasePrice = (info.detail.MaterialPrice == null) ? 0 : info.detail.MaterialPrice.Value + ((objOutSourceDB == null || objOutSourceDB.TechPrice == null) ? 0 : Convert.ToDouble(objOutSourceDB.TechPrice));

                                TrnPurchase purchaseItem = null;
                                if (info.OutSource.PurchaseSeq != null)
                                {
                                    purchaseItem = TrnPurchase.GetTrnPurchase(OfficeCd, info.OutSource.OutsourceCd, info.OutSource.PurchaseSeq.Value);
                                }
                                if (purchaseItem == null)
                                {
                                    purchaseItem = new TrnPurchase();
                                    purchaseItem.OfficeCd = OfficeCd;
                                    purchaseItem.SupplierOutsourceCd = info.OutSource.OutsourceCd;
                                    purchaseItem.PurchaseSeq = TrnPurchase.GetNextPurchaseSeq(OfficeCd);
                                    info.OutSource.PurchaseSeq = purchaseItem.PurchaseSeq;


                                    purchaseItem.RegularPrice = 0;
                                    purchaseItem.PurchaseFee = 0;

                                    purchaseItem.PurchaseCategory = 2;
                                    purchaseItem.PurchasePrice = PurchasePrice;
                                    purchaseItem.PurchaseItems = info.detail.DetailNm;
                                    purchaseItem.CreateAccount = purchaseItem.ModifiedAccount = modifyAccount;
                                    purchaseItem.Insert();
                                }
                                else
                                {
                                    purchaseItem.PurchaseCategory = 2;
                                    purchaseItem.PurchasePrice = PurchasePrice;
                                    purchaseItem.PurchaseItems = info.detail.DetailNm;
                                    purchaseItem.CreateAccount = purchaseItem.ModifiedAccount = modifyAccount;
                                    //update  
                                    purchaseItem.Update();
                                }
                                if (objOutSourceDB == null)
                                {
                                    logger.Debug("Case objOutSourceDB == null -> insert");
                                    info.OutSource.PurchaseSeq = purchaseItem.PurchaseSeq;
                                    info.OutSource.Insert();
                                  //  purchaseItem.Insert();
                                }
                                else
                                {
                                    logger.Debug("Case objOutSourceDB != null ->Update");
                                    info.OutSource.Update();
                                }
                            }
                            #endregion
                        }
                        logger.Debug("End RegisterOperationProcess");

                        tran.Commit();

                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error RegisterOperationProcess Rollback db", ex);
                        tran.Rollback();
                        throw ex;
                    }
                }
            }
        }

    }
    [Serializable]
    public class CustomProcess
    {
        public int ID { get; set; }
        public int ProcessCd { get; set; }
        public string ProcessName { get; set; }
        public string StaffCd { get; set; }
        public string StaffNm { get; set; }
        public string ProcessTime { get; set; }
        public CustomProcess()
        {
            ID = 0;
            ProcessCd = 0;
            ProcessTime = "0";
        }
        public CustomProcess(int id, int procesCd, string procesNm, string staffCd, string staffNm, string prTime)
        {
            this.ID = id;
            this.ProcessCd = procesCd;
            this.ProcessName = procesNm;
            this.StaffCd = staffCd;
            this.StaffNm = staffNm;
            this.ProcessTime = prTime;
        }
    }
   
    [Serializable]
    public class objDropDownList
    {
        public List<CodeName> listProcess;
        public List<CodeName> listStaff;
        public objDropDownList()
        {
        }
        public objDropDownList(int officeCd)
        {
            listStaff =  CodeName.GetTechnicalStaffListCodeName(officeCd);
            listProcess = CodeName.GetMasterListCodeName("DropDownProcess", officeCd);
        }
    }
   
}
