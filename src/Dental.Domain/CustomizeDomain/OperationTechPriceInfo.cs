using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Dental.Domain
{
    [Serializable]
    public class OperationTechPriceInfo
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(OperationTechPriceInfo));

        public TrnOrderDetail detail { get; set; }
        public int DetailChange = 0;
        public List<TrnTechPrice> listTechPrice { get; set; }

        public OperationTechPriceInfo()
        {
            listTechPrice = new List<TrnTechPrice>();
            listTechPrice.Add(new TrnTechPrice());
        }
        public static OperationTechPriceInfo getItem(TrnOrderDetail item, List<TrnTechPrice> listTechPriceDB, List<MasterTechPrice> listMstTechPrice)
        {
            OperationTechPriceInfo obj = new OperationTechPriceInfo();
            obj.detail = item;
          
            if (listTechPriceDB != null)
                obj.listTechPrice.AddRange(listTechPriceDB);
            if (obj.listTechPrice.Count == 1)
            {
                logger.Debug(" listTechPriceDB = NULL at ProsthesisCd = " + item.ProsthesisCd);

                List<MasterTechPriceTemplate> listTechPriceTemplate = MasterTechPriceTemplate.GetListTechPriceTemplateByProsthesisCd(item.OfficeCd, item.ProsthesisCd);
                logger.Debug(" listTechPriceTemplate.count = " + listTechPriceTemplate.Count);

                int techDetailNo = 1;
                for (int i = 0; i < listTechPriceTemplate.Count; i++)
                {
                    MasterTechPrice objTech = listMstTechPrice.FirstOrDefault(p => p.TechCd == listTechPriceTemplate[i].TechCd);
                    if (objTech != null)
                    {
                        TrnTechPrice temp = new TrnTechPrice();
                        temp.OfficeCd = item.OfficeCd;
                        temp.DetailSeq = item.DetailSeq;
                        temp.OrderSeq = item.OrderSeq;
                        temp.TechDetailNo = techDetailNo++;
                        temp.TechCd = objTech.TechCd;
                        temp.TechNm = objTech.TechNm;
                        temp.TechPrice = objTech.TechPrice;

                        obj.listTechPrice.Add(temp);
                    }
                }
            }
            return obj;
        }
        public static List<OperationTechPriceInfo> GetOperationTechPriceInfo(int officeCd, double orderSeq, string orderDate, int dentalOfficeCd)
        {
            logger.Debug("Begin GetOperationMaterialInfo dentalOfficeCd = " + dentalOfficeCd);
            List<OperationTechPriceInfo> listOperationProcessInfo = new List<OperationTechPriceInfo>();

            List<TrnOrderDetail> listDetail = TrnOrderDetail.GetTrnOrderDetailList(officeCd, orderSeq);
            List<TrnTechPrice> listTechPrice = TrnTechPrice.GetTrnTechPriceList(officeCd, orderSeq);
            List<MasterTechPrice> listMstTechPrice = MasterTechPrice.GetListMasterTechPriceByOrderDate(officeCd, orderDate, dentalOfficeCd);
            logger.DebugFormat("listDetail.Count = {0} listTechPrice = {1} , listMstTechPrice = {2}", listDetail.Count, listTechPrice.Count, listTechPrice.Count);

            foreach (TrnOrderDetail i in listDetail)
            {
                //Get from here
                List<TrnTechPrice> list = listTechPrice.Where(p => p.DetailSeq == i.DetailSeq).ToList();
                listOperationProcessInfo.Add(OperationTechPriceInfo.getItem(i, list, listMstTechPrice));
            }
            logger.Debug("TOTAL listOperationProcessInfo.Count = " + listOperationProcessInfo.Count);
            return listOperationProcessInfo;
        }
        public static void RegisterOperationTechPrice(List<OperationTechPriceInfo> listInfo, int officeCd, double orderSeq, string accountName)
        {
            logger.Debug("Begin RegisterOperationProcess total listInfo.count = " + listInfo.Count);

            List<TrnTechPrice> listTechPriceDB = TrnTechPrice.GetTrnTechPriceList(officeCd, orderSeq);

            logger.Debug("TOTAL  listTechPriceDB.count = " + listTechPriceDB.Count);


            using (DBContext db = new DBContext())
            {
                using (System.Data.Common.DbTransaction tran = db.UseTransaction())
                {
                    try
                    {
                        foreach (OperationTechPriceInfo info in listInfo)
                        {
                            if (info.DetailChange == 0 || !string.IsNullOrEmpty(info.detail.BillStatementNo))
                                continue;

                            //1.Update Order Detail

                            double processPrice = info.listTechPrice.Where(p => p.TechPrice != null).Sum(p => Convert.ToDouble(p.TechPrice));
                            logger.DebugFormat("1.UPDATE DETAIL  OfficeCd = {0} , OrderSeq = {1} , DetailSeq = {2}  processPrice={3}", officeCd, orderSeq, info.detail.DetailSeq, processPrice);

                            TrnOrderDetail item = TrnOrderDetail.GetDentalOrderDetail(officeCd, orderSeq, info.detail.DetailSeq);
                            item.ProcessPrice = processPrice;
                            item.ModifiedAccount = accountName;
                            item.Update();


                            //2.Insert , Update , Delete TrnTechPrice
                            List<TrnTechPrice> listTempTechPrice = listTechPriceDB.Where(p => p.DetailSeq == info.detail.DetailSeq).ToList();
                            listTechPriceDB.RemoveAll(p => p.DetailSeq == info.detail.DetailSeq);

                            #region GetListProcess Insert,Delete , Update Detail

                            List<TrnTechPrice> listTechPriceInsert = new List<TrnTechPrice>();
                            List<TrnTechPrice> listTechPriceUpdate = new List<TrnTechPrice>();

                            for (int i = 1; i < info.listTechPrice.Count; i++) //ignore first item
                            {
                                info.listTechPrice[i].OfficeCd = officeCd;
                                info.listTechPrice[i].OrderSeq = orderSeq;
                                info.listTechPrice[i].DetailSeq = info.detail.DetailSeq;

                                info.listTechPrice[i].TechDetailNo = i; //Change TechDetail No
                                info.listTechPrice[i].CreateAccount = info.listTechPrice[i].ModifiedAccount = accountName;

                                TrnTechPrice itemUpdate = listTempTechPrice.FirstOrDefault(p => p.TechDetailNo == i);
                                if (itemUpdate != null)
                                {
                                    listTechPriceUpdate.Add(info.listTechPrice[i]);
                                    listTempTechPrice.Remove(itemUpdate);
                                }
                                else //Insert 
                                {
                                    listTechPriceInsert.Add(info.listTechPrice[i]);
                                }
                            }

                            #endregion

                            #region Delete TrnTechPrice

                            logger.Debug("1.Delete TrnTechPrice , listTempProcess =  " + listTempTechPrice.Count);
                            foreach (TrnTechPrice i in listTempTechPrice)
                            {
                                i.Delete();
                            }
                            #endregion

                            #region Update TrnTechPrice
                            logger.Debug("2.Update TrnTechPrice , listUpdate =  " + listTechPriceUpdate.Count);
                            foreach (TrnTechPrice i in listTechPriceUpdate)
                            {
                                i.Update();
                            }
                            #endregion

                            #region Insert TrnTechPrice
                            logger.Debug("3.INSERT TrnTechPrice , listProcessInsert =  " + listTechPriceInsert.Count);
                            foreach (TrnTechPrice i in listTechPriceInsert)
                            {
                                i.OfficeCd = officeCd;
                                i.DetailSeq = info.detail.DetailSeq;
                                i.OrderSeq = info.detail.OrderSeq;

                                i.Insert();
                            }
                            #endregion

                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error RegisterOperationTechPrice Rollback db", ex);
                        tran.Rollback();
                        throw ex;
                    }
                }
            }
          
            logger.Debug("End RegisterOperationProcess sucess");
        }

    }
   
}
