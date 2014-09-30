using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;
using System.Collections;

namespace Dental.Domain
{
    [Serializable]
    [Table(Name = TrnOrderDetailColumn.TABLE_NAME)]
    public class TrnOrderDetail : BaseDomain<TrnOrderDetail>, ICommonFunctions<TrnOrderDetail>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(TrnOrderDetail));

        #region Private Member
        private int _OfficeCd;
        private double _OrderSeq;
        private int _DetailSeq;
        private string _DetailNm;

        private int _DisplayOrder;
        private System.Nullable<int> _ToothNumber;
        private System.Nullable<bool> _ChildFlg;
        private System.Nullable<bool> _GapFlg;
        private System.Nullable<bool> _DentureFlg;
        private System.Nullable<int> _BridgedID;
        private System.Nullable<bool> _InsuranceKbn;

        private int _ProsthesisCd;

        private string _ProsthesisNm;

        private System.Nullable<int> _MaterialCd;
        private string _MaterialNm;
        public System.Nullable<int> OldMaterialCd { get; set; }

        private System.Nullable<int> _SupplierCd;
        private System.Nullable<double> _Amount;


        private string _UnitCd;

        private System.Nullable<int> _Shape;

        private System.Nullable<int> _Shade;

        private System.Nullable<int> _AnatomyKit;

        private System.Nullable<bool> _CadOutputDone;

        private System.Nullable<double> _Price;

        private System.Nullable<double> _MaterialPrice;

        private System.Nullable<double> _ProcessPrice;

        private System.Nullable<int> _ManufactureStaff;


        private System.Nullable<System.DateTime> _CompleteDate;

        private System.Nullable<int> _InspectionStaff;



        private string _DeliveryStatementNo;

        private System.Nullable<System.DateTime> _DeliveredDate;



        private string _BillStatementNo;


        public string UnitNm { get; set; }
        #endregion

        #region Public Property
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
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.ORDER_SEQ, CanBeNull = false, IsPrimaryKey = true)]
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

        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.DETAIL_SEQ, CanBeNull = false, IsPrimaryKey = true)]
        public int DetailSeq
        {
            get { return _DetailSeq; }
            set { _DetailSeq = value; }
        }

        [ColumnAttribute(Name = TrnOrderDetailColumn.DETAIL_NM)]
        public string DetailNm
        {
            get { return _DetailNm; }
            set { _DetailNm = value; }
        }


        [ColumnAttribute(Name = TrnOrderDetailColumn.DISPLAY_ORDER)]
        public int DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
        }

        [ColumnAttribute(Name = TrnOrderDetailColumn.TOOTH_NUMBER, CanBeNull = true)]
        public System.Nullable<int> ToothNumber
        {
            get { return _ToothNumber; }
            set { _ToothNumber = value; }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.CHILD_FLG, CanBeNull = true)]
        public System.Nullable<bool> ChildFlg
        {
            get { return _ChildFlg; }
            set { _ChildFlg = value; }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.GAP_FLG, CanBeNull = true)]
        public System.Nullable<bool> GapFlg
        {
            get { return _GapFlg; }
            set { _GapFlg = value; }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.DENTURE_FLG, CanBeNull = true)]
        public System.Nullable<bool> DentureFlg
        {
            get { return _DentureFlg; }
            set { _DentureFlg = value; }
        }


        [ColumnAttribute(Name = TrnOrderDetailColumn.BRIDGED_ID)]
        public System.Nullable<int> BridgedID
        {
            get
            {
                return this._BridgedID;
            }
            set
            {
                this._BridgedID = value;
            }
        }

        [ColumnAttribute(Name = TrnOrderDetailColumn.INSURANCE_KBN)]
        public System.Nullable<bool> InsuranceKbn
        {
            get
            {
                return this._InsuranceKbn;
            }
            set
            {
                this._InsuranceKbn = value;
            }
        }

        [ColumnAttribute(Name = TrnOrderDetailColumn.PROSTHESIS_CD, CanBeNull = false)]
        public int ProsthesisCd
        {
            get
            {
                return this._ProsthesisCd;
            }
            set
            {
                this._ProsthesisCd = value;
            }
        }

        [ColumnAttribute(Name = TrnOrderDetailColumn.PROSTHESIS_NM)]
        public string ProsthesisNm
        {
            get
            {
                return this._ProsthesisNm;
            }
            set
            {
                this._ProsthesisNm = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.MATERIAL_CD)]
        public System.Nullable<int> MaterialCd
        {
            get
            {
                return this._MaterialCd;
            }
            set
            {
                this._MaterialCd = value;
            }
        }

        [ColumnAttribute(Name = TrnOrderDetailColumn.MATERIAL_NM)]
        public string MaterialNm
        {
            get
            {
                return this._MaterialNm;
            }
            set
            {
                this._MaterialNm = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.SUPPLIER_CD)]
        public System.Nullable<int> SupplierCd
        {
            get { return _SupplierCd; }
            set { _SupplierCd = value; }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.AMOUNT)]
        public System.Nullable<double> Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }



        [ColumnAttribute(Name = TrnOrderDetailColumn.UNIT_CD)]
        public string UnitCd
        {
            get
            {
                return this._UnitCd;
            }
            set
            {
                this._UnitCd = value;

            }
        }

        [ColumnAttribute(Name = TrnOrderDetailColumn.SHAPE)]
        public System.Nullable<int> Shape
        {
            get
            {
                return this._Shape;
            }
            set
            {
                this._Shape = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.SHADE)]
        public System.Nullable<int> Shade
        {
            get
            {
                return this._Shade;
            }
            set
            {
                this._Shade = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.ANATOMY_KIT)]
        public System.Nullable<int> AnatomyKit
        {
            get
            {
                return this._AnatomyKit;
            }
            set
            {
                this._AnatomyKit = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.CAD_OUTPUT_DONE)]
        public System.Nullable<bool> CadOutputDone
        {
            get
            {
                return this._CadOutputDone;
            }
            set
            {
                this._CadOutputDone = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.PRICE)]
        public System.Nullable<double> Price
        {
            get
            {
                return this._Price;
            }
            set
            {
                this._Price = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.MATERIAL_PRICE)]
        public System.Nullable<double> MaterialPrice
        {
            get
            {
                return this._MaterialPrice;
            }
            set
            {
                this._MaterialPrice = value;
            }
        }

        [ColumnAttribute(Name = TrnOrderDetailColumn.PROCESS_PRICE)]
        public System.Nullable<double> ProcessPrice
        {
            get
            {
                return this._ProcessPrice;
            }
            set
            {

                this._ProcessPrice = value;

            }
        }

        [ColumnAttribute(Name = TrnOrderDetailColumn.MANUFACTURE_STAFF)]
        public System.Nullable<int> ManufactureStaff
        {
            get { return _ManufactureStaff; }
            set { _ManufactureStaff = value; }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.INSPECTION_STAFF)]
        public System.Nullable<int> InspectionStaff
        {
            get { return _InspectionStaff; }
            set { _InspectionStaff = value; }
        }

        [ColumnAttribute(Name = TrnOrderDetailColumn.DELIVERY_STATEMENT)]
        public string DeliveryStatementNo
        {
            get
            {
                return this._DeliveryStatementNo;
            }
            set
            {
                this._DeliveryStatementNo = value;
            }
        }


        [ColumnAttribute(Name = TrnOrderDetailColumn.COMPLETE_DATE)]
        public System.Nullable<System.DateTime> CompleteDate
        {
            get
            {
                return this._CompleteDate;
            }
            set
            {
                this._CompleteDate = value;
            }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.DELIVERED_DATE)]
        public System.Nullable<System.DateTime> DeliveredDate
        {
            get { return _DeliveredDate; }
            set { _DeliveredDate = value; }
        }
        [ColumnAttribute(Name = TrnOrderDetailColumn.BILL_STATEMENT_NO)]
        public string BillStatementNo
        {
            get { return _BillStatementNo; }
            set { _BillStatementNo = value; }
        }

        [ColumnAttribute(Name = BaseColumn.CREATE_DATE)]
        public System.DateTime CreateDate
        {
            get { return this._CreateDate; }
            set { this._CreateDate = value; }
        }

        [ColumnAttribute(Name = BaseColumn.CREATE_ACCOUNT)]
        public string CreateAccount
        {
            get { return this._CreateAccount; }
            set { this._CreateAccount = value; }
        }

        [ColumnAttribute(Name = BaseColumn.MODIFIED_DATE)]
        public System.DateTime ModifiedDate
        {
            get { return this._ModifiedDate; }
            set { this._ModifiedDate = value; }
        }

        [ColumnAttribute(Name = BaseColumn.MODIFIED_ACCOUNT)]
        public string ModifiedAccount
        {
            get { return this._ModifiedAccount; }
            set { this._ModifiedAccount = value; }
        }

        #endregion

        #region ICommonFunctions
        public TrnOrderDetail GetByPrimaryKey()
        {
            Table<TrnOrderDetail> table = GetTable();
            TrnOrderDetail item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.OrderSeq == this.OrderSeq &&  d.DetailSeq  == this.DetailSeq));
            if (item != null)
                item.Detach<TrnOrderDetail>();
            return item;
        }
        #endregion

        #region Method
        public static List<TrnOrderDetail> GetTrnOrderDetailList(int OfficeCd, double OrderSeq)
        {
            logger.Debug("Begin GetListDentalOrderDetail , OrderSeq = " + OrderSeq);
            var result = from item in GetTable()
                         where item.OrderSeq == OrderSeq && item.OfficeCd == OfficeCd
                         orderby item.DisplayOrder 
                         select item;
            return result.ToList();

        }
        public static List<TrnOrderDetail> GetTrnOrderDetailList(int officeCd, double orderSeq, bool bGetCadOutput)
        {

            List<TrnOrderDetail> result = TrnOrderDetail.GetTrnOrderDetailList(officeCd, orderSeq);
            if (bGetCadOutput == true)
            {
                //foreach (TrnOrderDetail i in result)
                //{
                //    //if (!string.IsNullOrEmpty(i.CompleteDate))
                //     //   i.CompleteDate = Convert.ToDateTime(i.CompleteDate).ToShortDateString();
                //}
            }
            else
            {
                foreach (TrnOrderDetail i in result)
                {
                    i.CadOutputDone = false;
                    //if (!string.IsNullOrEmpty(i.CompleteDate))
                    //i.CompleteDate = Convert.ToDateTime(i.CompleteDate).ToShortDateString();
                }
            }
            return result;
        }

        public static TrnOrderDetail GetDentalOrderDetail(int officeCd, double orderSeq, int detailSeq)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.OrderSeq == orderSeq && item.DetailSeq == detailSeq
                    select item).FirstOrDefault();
        }

        #endregion
    }
}
