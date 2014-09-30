using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{
    [Serializable]
    [Table(Name = TrnOrderHeaderColumn.TABLE_NAME)]
    public class TrnOrderHeader : BaseDomain<TrnOrderHeader>, ICommonFunctions<TrnOrderHeader>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(TrnOrderHeader));

        #region Private Member
        private int _OfficeCd;
        private double _OrderSeq;
        private string _OrderNo;
        private string _RefOrderNo;
        private System.Nullable<bool> _TrialOrderFlg;
        private System.Nullable<bool> _RemanufactureFlg;
        private System.Nullable<int> _StaffCd;
        private System.DateTime _OrderDate;
        private System.DateTime _DeliverDate;
        private System.Nullable<System.DateTime> _SetDate;
        private int _DentalOfficeCd;
        private string _DentistNm;
        private string _PatientLastNm;
        private string _PatientFirstNm;
        private System.Nullable<int> _PatientSex;
        private System.Nullable<int> _PatientAge;
        private System.Nullable<int> _DeliveryStatement;
        private string _BorrowParts;
        private string _Note;
      
        #endregion

        #region Property Member
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true)]
        public int OfficeCd
        {
            get
            {
                return this._OfficeCd;
            }
            set
            {
                this._OfficeCd = value;
            }
        }
       [ColumnAttribute(Name = BaseDentalOrderKeyColumn.ORDER_SEQ, IsPrimaryKey = true)]
        public double OrderSeq
        {
            get
            {
                return this._OrderSeq;
            }
            set
            {
                this._OrderSeq = value;
            }
        }

        [ColumnAttribute(Name = TrnOrderHeaderColumn.ORDER_NO, CanBeNull = false)]
        public string OrderNo
        {
            get
            {
                return this._OrderNo;
            }
            set
            {
                this._OrderNo = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderHeaderColumn.REF_ORDER_NO)]
        public string RefOrderNo
        {
            get { return _RefOrderNo; }
            set { _RefOrderNo = value; }
        }


        [ColumnAttribute(Name = TrnOrderHeaderColumn.TRIAL_ORDER_FLG, CanBeNull = true)]
        public System.Nullable<bool> TrialOrderFlg
        {
            get
            {
                return this._TrialOrderFlg;
            }
            set
            {
                this._TrialOrderFlg = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderHeaderColumn.REMANUFACTURE_FLG, CanBeNull = true)]
        public System.Nullable<bool> RemanufactureFlg
        {
            get
            {
                return this._RemanufactureFlg;
            }
            set
            {
                this._RemanufactureFlg = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderHeaderColumn.STAFF_CODE, CanBeNull = true)]
        public System.Nullable<int> StaffCd
        {
            get
            {
                return this._StaffCd;
            }
            set
            {
                this._StaffCd = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderHeaderColumn.ORDER_DATE)]
        public System.DateTime OrderDate
        {
            get
            {
                return this._OrderDate;
            }
            set
            {
                this._OrderDate = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderHeaderColumn.DELIVER_DATE)]
        public System.DateTime DeliverDate
        {
            get
            {
                return this._DeliverDate;
            }
            set
            {
                this._DeliverDate = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderHeaderColumn.SET_DATE, CanBeNull = true)]
        public System.Nullable<System.DateTime> SetDate
        {
            get
            {
                return this._SetDate;
            }
            set
            {
                this._SetDate = value;
            }
        }


        [ColumnAttribute(Name = TrnOrderHeaderColumn.DENTAL_OFFICE_CD)]
        public int DentalOfficeCd
        {
            get
            {
                return this._DentalOfficeCd;
            }
            set
            {
                this._DentalOfficeCd = value;
            }
        }

       [ColumnAttribute(Name = TrnOrderHeaderColumn.DENTIST_NM)]
        public string DentistNm
        {
            get
            {
                return this._DentistNm;
            }
            set
            {
                this._DentistNm = value;
            }
        }

       [ColumnAttribute(Name = TrnOrderHeaderColumn.PATIENT_LAST_NM)]
        public string PatientLastNm
        {
            get
            {
                return this._PatientLastNm;
            }
            set
            {
                this._PatientLastNm = value;
            }
        }

        [ColumnAttribute(Name = TrnOrderHeaderColumn.PATIENT_FIRST_NM)]
        public string PatientFirstNm
        {
            get
            {
                return this._PatientFirstNm;
            }
            set
            {
                this._PatientFirstNm = value;
            }
        }

       [ColumnAttribute(Name = TrnOrderHeaderColumn.PATIENT_SEX)]
        public System.Nullable<int> PatientSex
        {
            get
            {
                return this._PatientSex;
            }
            set
            {
                this._PatientSex = value;

            }
        }
       [ColumnAttribute(Name = TrnOrderHeaderColumn.PATIENT_AGE)]
       public System.Nullable<int> PatientAge
       {
           get
           {
               return this._PatientAge;
           }
           set
           {
               this._PatientAge = value;

           }
       }
      

        [ColumnAttribute(Name = TrnOrderHeaderColumn.DELIVERY_STATEMENT)]
        public System.Nullable<int> DeliveryStatement
        {
            get
            {
                return this._DeliveryStatement;
            }
            set
            {
                this._DeliveryStatement = value;
            }
        }

        [ColumnAttribute(Name = TrnOrderHeaderColumn.NOTE)]
        public string Note
        {
            get
            {
                return this._Note;
            }
            set
            {
                this._Note = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderHeaderColumn.BORROW_PARTS)]
        public string BorrowParts
        {
            get
            {
                return this._BorrowParts;
            }
            set
            {
                this._BorrowParts = value;
            }
        }


        [ColumnAttribute(Name = BaseColumn.CREATE_DATE)]
        public System.DateTime CreateDate
        {
            get {return this._CreateDate; }
            set {this._CreateDate = value; }
        }

        [ColumnAttribute(Name = BaseColumn.CREATE_ACCOUNT)]
        public string CreateAccount
        {
            get {return this._CreateAccount; }
            set { this._CreateAccount = value;}
        }

        [ColumnAttribute(Name = BaseColumn.MODIFIED_DATE)]
        public System.DateTime ModifiedDate
        {
            get  { return this._ModifiedDate;}
            set {  this._ModifiedDate = value;   }
        }

        [ColumnAttribute(Name = BaseColumn.MODIFIED_ACCOUNT)]
        public string ModifiedAccount
        {
            get {   return this._ModifiedAccount; }
            set { this._ModifiedAccount = value; }
        }
        #endregion

        #region Common 
        public TrnOrderHeader GetByPrimaryKey()
        {
            Table<TrnOrderHeader> table = GetTable();

            TrnOrderHeader item = table.Single(d => (d.OrderSeq == this.OrderSeq && d.OfficeCd == this.OfficeCd ));
            if (item != null)
                item.Detach<TrnOrderHeader>();

            return item;
        }
        #endregion 

        public static void RegisterOrderInput(TrnOrderHeader header, List<TrnOrderDetail> listDetail)
        {
            logger.DebugFormat("Begin RegisterOrderInput OrderNo = {0} , listDetail.count = {1}", header.OrderNo, listDetail.Count);

            using (DBContext db = new DBContext())
            {
                using (System.Data.Common.DbTransaction tran = db.UseTransaction())
                {
                    try
                    {
                        if (header.OrderSeq == -1)  //Case Insert NEW
                        {
                            header.OrderSeq = GetNextOrderSeq(header.OfficeCd);
                            List<TrnOrderDetail> listDetailDelete = TrnOrderDetail.GetTrnOrderDetailList(header.OfficeCd, header.OrderSeq);
                            foreach (TrnOrderDetail i in listDetailDelete)
                                i.Delete();

                            #region Insert
                            foreach (TrnOrderDetail detail in listDetail)
                            {
                                detail.OfficeCd = header.OfficeCd;
                                detail.OrderSeq = header.OrderSeq;
                                detail.CreateAccount = detail.ModifiedAccount = header.CreateAccount;
                                detail.CreateDate = detail.ModifiedDate = header.CreateDate;
                                detail.Insert();
                            }
                            header.Insert();
                            #endregion Insert
                        }
                        else
                        {
                            header.Update();
                            
                            #region GetList Insert,Delete , Update Detail
                            List<TrnOrderDetail> listDetailDB = TrnOrderDetail.GetTrnOrderDetailList(header.OfficeCd, header.OrderSeq, true);
                            List<TrnOrderDetail> listInsert = new List<TrnOrderDetail>();
                            List<TrnOrderDetail> listUpdate = new List<TrnOrderDetail>();
                            for (int i = listDetail.Count - 1; i >= 0; i--)
                            {
                                listDetail[i].OfficeCd = header.OfficeCd;
                                listDetail[i].OrderSeq = header.OrderSeq;

                                listDetail[i].CreateAccount = listDetail[i].ModifiedAccount = header.CreateAccount;
                                listDetail[i].CreateDate = listDetail[i].ModifiedDate = header.CreateDate;
                                bool bInsert = true;
                                for (int j = listDetailDB.Count - 1; j >= 0; j--)
                                {
                                    if (listDetail[i].OrderSeq == listDetailDB[j].OrderSeq && listDetail[i].DetailSeq == listDetailDB[j].DetailSeq)
                                    {
                                        bInsert = false;
                                        //only update CadOutPutDone = FALSE
                                        //if (listDetailDB[j].CadOutputDone!=null) !string.IsNullOrEmpty(listDetailDB[j].CadOutputDone))
                                        //{
                                        //    if (Convert.ToBoolean(listDetailDB[j].CadOutputDone) == true)
                                        //        listDetail[i].CadOutputDone = true.ToString();


                                        //only update some field in db

                                        #region GetUpdate Field
                                        listDetailDB[j].DisplayOrder = listDetail[i].DisplayOrder;
                                        listDetailDB[j].DetailNm = listDetail[i].DetailNm;
                                        listDetailDB[j].MaterialCd = listDetail[i].MaterialCd;
                                        listDetailDB[j].MaterialNm = listDetail[i].MaterialNm; 
                                        listDetailDB[j].ToothNumber = listDetail[i].ToothNumber;
                                        listDetailDB[j].BridgedID = listDetail[i].BridgedID;
                                        listDetailDB[j].Shape = listDetail[i].Shape;
                                        listDetailDB[j].Shade = listDetail[i].Shade;
                                        listDetailDB[j].AnatomyKit = listDetail[i].AnatomyKit;
                                        listDetailDB[j].Shape = listDetail[i].Shape;
                                        listDetailDB[j].CadOutputDone = listDetail[i].CadOutputDone;

                                        listDetailDB[j].InsuranceKbn = listDetail[i].InsuranceKbn;
                                        listDetailDB[j].ProsthesisCd = listDetail[i].ProsthesisCd;
                                        listDetailDB[j].ProsthesisNm = listDetail[i].ProsthesisNm;

                                        listDetailDB[j].ChildFlg = listDetail[i].ChildFlg;
                                        listDetailDB[j].GapFlg = listDetail[i].GapFlg;
                                        listDetailDB[j].DentureFlg = listDetail[i].DentureFlg;
                                        listDetailDB[j].ModifiedAccount = header.CreateAccount;
                                        #endregion

                                        listUpdate.Add(listDetailDB[j]);

                                      
                                        listDetailDB.RemoveAt(j);
                                        break;
                                    }
                                }
                                if (bInsert)
                                {
                                    listInsert.Add(listDetail[i]);
                                }
                            }
                            #endregion

                            #region Delete
                            foreach (TrnOrderDetail i in listDetailDB)
                            {
                                i.Delete();

                            }
                            #endregion

                            #region Update
                            foreach (TrnOrderDetail i in listUpdate)
                            {
                                i.Update();
                            }
                            #endregion

                            #region Insert
                            foreach (TrnOrderDetail i in listInsert)
                            {
                                i.Insert();
                            }
                            #endregion

                        }

                       
                        tran.Commit();
                      
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Error RegisterOrderInput Rollback db", ex);
                        tran.Rollback();
                        throw ex;
                    }
                }
            }
            logger.Debug("End RegisterOrderInput");
        }
        public static double GetNextOrderSeq(int officeCd)
        {
            var list = GetTable().Where(p => p.OfficeCd == officeCd);
            if (list.Count() == 0)
                return 1;
            else
                return list.Max(p => p.OrderSeq) + 1;
        }
        public static int GetCountOrderNo(int officeCd, int orderSeq, string textOrderNo)        
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.OrderSeq != orderSeq && item.OrderNo == textOrderNo
                    select item).Count(); 
        }

        public static TrnOrderHeader GetTrnOrderHeader(int officeCd, double orderSeq)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.OrderSeq == orderSeq
                    select item).FirstOrDefault();
        }
    }
}
