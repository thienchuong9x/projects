using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dental.Domain
{
    [Serializable]
    public class BillStatementInfo : TrnOrderDetail
    {
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int DentalOfficeCd { get; set; }
        public string PatientNm { get; set; }
        public string BillCd { get; set; }
        public BillStatementInfo()
        {
        }

        public static List<BillStatementInfo> GetBillStatements(BillSearchInfo objSearch)
        {
            if (objSearch.OrderDateFrom == string.Empty)
                objSearch.OrderDateFrom = "1/1/2000";
            if (objSearch.OrderDateTo == string.Empty)
                objSearch.OrderDateTo = "1/1/6000";
            if (objSearch.DeliverDateFrom == string.Empty)
                objSearch.DeliverDateFrom = "1/1/2000";
            if (objSearch.DeliverDateTo == string.Empty)
                objSearch.DeliverDateTo = "1/1/6000";

            var context = new DBContext();

            return (from itemHeader in context.GetTable<TrnOrderHeader>()
                    join
                        itemDetail in context.GetTable<TrnOrderDetail>() on new { itemHeader.OfficeCd, itemHeader.OrderSeq } equals new { itemDetail.OfficeCd, itemDetail.OrderSeq }
                    join
                        masterDentalOffice in context.GetTable<MasterDentalOffice>() on new { itemHeader.OfficeCd, itemHeader.DentalOfficeCd } equals new { masterDentalOffice.OfficeCd, masterDentalOffice.DentalOfficeCd }
                    where
                    (
                        itemHeader.OfficeCd == objSearch.OfficeCd
                        && (string.IsNullOrEmpty(objSearch.DentalOfficeCd) || itemHeader.DentalOfficeCd.ToString() == objSearch.DentalOfficeCd)
                        && (string.IsNullOrEmpty(objSearch.OrderNo) || itemHeader.OrderNo.Contains(objSearch.OrderNo))
                        && (itemDetail.DeliveryStatementNo != null)
                        && ( 
                             (string.IsNullOrEmpty(objSearch.BillStatementNo) && itemDetail.BillStatementNo == null ) 
                         || itemDetail.BillStatementNo == objSearch.BillStatementNo
                           )
                        && (string.IsNullOrEmpty(objSearch.BillCd) || masterDentalOffice.BillCd == Convert.ToInt32(objSearch.BillCd))
                        && (itemDetail.DeliveredDate != null)
                        && (string.IsNullOrEmpty(objSearch.BillMonth) || itemDetail.DeliveredDate.Value.Month == Convert.ToInt32(objSearch.BillMonth))
                        && (string.IsNullOrEmpty(objSearch.BillYear) || itemDetail.DeliveredDate.Value.Year == Convert.ToInt32(objSearch.BillYear))

                        && (Convert.ToDateTime(objSearch.OrderDateFrom).Date <= itemHeader.OrderDate.Date && itemHeader.OrderDate.Date <= Convert.ToDateTime(objSearch.OrderDateTo).Date)
                        && (Convert.ToDateTime(objSearch.DeliverDateFrom).Date <= itemHeader.DeliverDate.Date && itemHeader.DeliverDate.Date <= Convert.ToDateTime(objSearch.DeliverDateTo).Date)
                        &&
                          (
                            (objSearch.UnInsured == true && objSearch.Insured == true)
            || (objSearch.UnInsured == false && objSearch.Insured == false && itemDetail.InsuranceKbn == null)
            || (objSearch.UnInsured == false && objSearch.Insured == true && itemDetail.InsuranceKbn == true)
            || (objSearch.UnInsured == true && objSearch.Insured == false && itemDetail.InsuranceKbn == false)
                          )

                      )
                    orderby itemHeader.OrderSeq, itemDetail.BridgedID, itemDetail.ToothNumber
                    select new BillStatementInfo()
                    {
                        OfficeCd = itemHeader.OfficeCd,
                        OrderSeq = itemDetail.OrderSeq,
                        DetailSeq = itemDetail.DetailSeq,
                        DetailNm = itemDetail.DetailNm,
                        ToothNumber = itemDetail.ToothNumber.Value,
                        ChildFlg = itemDetail.ChildFlg.Value,
                        GapFlg = itemDetail.GapFlg.Value,
                        BridgedID = itemDetail.BridgedID.Value,
                        InsuranceKbn = itemDetail.InsuranceKbn.Value,
                        ProsthesisCd = itemDetail.ProsthesisCd,
                        ProsthesisNm = itemDetail.ProsthesisNm,
                        MaterialCd = itemDetail.MaterialCd.Value,
                        MaterialNm = itemDetail.MaterialNm,
                        Amount = itemDetail.Amount.Value,
                        UnitCd = itemDetail.UnitCd,
                        Price = itemDetail.Price.Value,
                        MaterialPrice = itemDetail.MaterialPrice.Value,
                        ProcessPrice = itemDetail.ProcessPrice.Value,
                        CompleteDate = itemDetail.CompleteDate.Value,
                        DeliveryStatementNo = itemDetail.DeliveryStatementNo,
                        DeliveredDate = itemDetail.DeliveredDate.Value,
                        OrderNo = itemHeader.OrderNo,
                        BillCd = masterDentalOffice.BillCd.ToString(),
                        OrderDate = itemHeader.OrderDate,
                        DentalOfficeCd = itemHeader.DentalOfficeCd

                    }).ToList();
        }

        /* -------------------------------------------------------------------------------------

CREATE PROCEDURE {databaseOwner}{objectQualifier}DDV_GetBillStatementSearch
	@OfficeCd int,
	@OrderNo nvarchar(20),
	@OrderDateFrom nvarchar(20),
	@OrderDateTo nvarchar(20),
	@DeliveredDateFrom nvarchar(20),
	@DeliveredDateTo nvarchar(20),
	@DentalOfficeCd int,
	@BillCd nvarchar(31),
	@TypeUnInsured bit,
	@TypeInsured bit,
	@BillStatementNo nvarchar(20),
	@BillYear nvarchar(4),
	@BillMonth nvarchar(2)
AS                  
BEGIN
SELECT                                   
		hd.[OrderSeq], hd.[OrderNo],dt.[BridgedID], dt.[ToothNumber],[OrderDate],dt.[DeliveredDate],hd.[DentalOfficeCd],dt.[ProsthesisCd],dt.[ProsthesisNm], dt.[Amount], InsuranceKbn, DetailSeq, MaterialCd, MaterialNm, SupplierCd, ProcessPrice,MaterialPrice, BillStatementNo, [PatientLastNm]+' '+[PatientFirstNm] as [PatientNm],[DetailNm] 
	    ,d.[BillCd] 
	FROM DENTAL_TrnOrderHeader as hd (NOLOCK), DENTAL_TrnOrderDetail as dt(NOLOCK)   
	LEFT JOIN dbo.DENTAL_MstDentalOffice d
    on  
    (   
        d.OfficeCd = @OfficeCd
    )      
	WHERE
	
		AND d.DentalOfficeCd = hd.DentalOfficeCd
		AND OrderNo LIKE '%'+ @OrderNo + '%' 
		AND 
		 ( 
		     ( @OrderDateFrom is NULL  AND   @OrderDateTo is NULL) 
		  OR ( @OrderDateFrom is NULL AND  OrderDate <= convert(datetime,@OrderDateTo))
		  OR ( @OrderDateTo is NULL AND  OrderDate >= convert(datetime,@OrderDateFrom))
		  OR ( OrderDate BETWEEN convert(datetime,@OrderDateFrom) and convert(datetime,@OrderDateTo))
		 ) 
		 AND
		  ( 
		     dt.[DeliveredDate] is not NULL
		  )  
		 AND 
		 (
		       @BillYear is NULL 
			or Year(dt.[DeliveredDate]) = @BillYear 
		 )
		 AND 
		 (
		       @BillMonth is NULL 
			or Month(dt.[DeliveredDate]) = @BillMonth
		 )
		 AND 
		 ( 
		     ( @DeliveredDateFrom is NULL  AND   @DeliveredDateTo is NULL) 
		  OR ( @DeliveredDateFrom is NULL AND  dt.DeliveredDate <= convert(datetime,@DeliveredDateTo))
		  OR ( @DeliveredDateTo is NULL AND  dt.DeliveredDate >= convert(datetime,@DeliveredDateFrom))
		  OR ( dt.DeliveredDate BETWEEN convert(datetime,@DeliveredDateFrom) and convert(datetime,@DeliveredDateTo))
		 )        
		 AND 
		  ( @DentalOfficeCd is NULL or  @DentalOfficeCd = hd.DentalOfficeCd)        
		 AND 
		  ( @BillCd is NULL or  @BillCd = d.BillCd
	      )            
		 AND 
		  (
		      (@TypeUnInsured =1 AND @TypeInsured =1)
		    OR(@TypeUnInsured =0 AND @TypeInsured = 0 AND InsuranceKbn is NULL) 
		    OR(@TypeUnInsured =0 AND @TypeInsured =1 AND InsuranceKbn = 1 )
		    OR(@TypeUnInsured =1 AND @TypeInsured =0 AND InsuranceKbn = 0)
		  )
		 AND 
		  ( 
		    dt.DeliveryStatementNo is NOT NULL
		  ) 
		 AND 
		 (  
		       (@BillStatementNo is NULL AND dt.BillStatementNo is NULL)
		     OR  dt.BillStatementNo= @BillStatementNo 
		 ) 
    ORDER BY hd.[OrderSeq],dt.[BridgedID],dt.[ToothNumber]

END
GO
*/


        public static double GetMaterialCarryOver(int officeCd, int billCd, int materialCd)
        {
            var context = new DBContext();

            var firstMaterial =
                   (from header in context.GetTable<TrnBillHeader>()
                    join
                       item in context.GetTable<TrnBillMaterial>() on new { header.OfficeCd, header.BillSeq } equals new { item.OfficeCd, item.BillSeq }
                    orderby item.BillSeq descending
                    where
                    (
                           item.OfficeCd == officeCd
                         && item.MaterialCd == materialCd
                         && header.BillCd == billCd
                         && (item.DelFlg == null || item.DelFlg.Value == false)
                         && (header.DelFlg == null || header.DelFlg.Value == false)
                      )
                    select item).FirstOrDefault();
            if (firstMaterial == null)
                return 0;
            else
                return firstMaterial.MaterialStock;
        }
    }

     /* -------------------------------------------------------------------------------------
/   DDV_BillStatement_GetMaterialCarryOver


CREATE PROCEDURE {databaseOwner}{objectQualifier}DDV_BillStatement_GetMaterialCarryOver 
	@OfficeCd int,
	@BillCd int,
	@MaterialCd int
AS                
SELECT TOP 1
	[{objectQualifier}DENTAL_TrnBillMaterial].MaterialStock as CarryOver
FROM DENTAL_TrnBillMaterial (NOLOCK), DENTAL_TrnBillHeader (NOLOCK)
WHERE 
     [{objectQualifier}DENTAL_TrnBillMaterial].[OfficeCd] = @OfficeCd 
AND  [{objectQualifier}DENTAL_TrnBillHeader].[OfficeCd] = @OfficeCd 
AND  [{objectQualifier}DENTAL_TrnBillMaterial].[BillSeq]= [{objectQualifier}DENTAL_TrnBillHeader].[BillSeq]
AND  [{objectQualifier}DENTAL_TrnBillHeader].[BillCd] = @BillCd
AND  [{objectQualifier}DENTAL_TrnBillMaterial].[MaterialCd] = @MaterialCd
AND  
  ( 
     [{objectQualifier}DENTAL_TrnBillMaterial].[DelFlg] is NULL 
  OR [{objectQualifier}DENTAL_TrnBillMaterial].[DelFlg] = '0'
  )
AND  
  ( 
     [{objectQualifier}DENTAL_TrnBillHeader].[DelFlg] is NULL 
  OR [{objectQualifier}DENTAL_TrnBillHeader].[DelFlg] = '0'
  )
ORDER BY  [{objectQualifier}DENTAL_TrnBillMaterial].[BillSeq] DESC
    
      * /  ------------------------------------------------------------------------------------- */
    public class BillConstantIndex
    {
        public static int index_OrderSeq = 1;
        public static int index_DeliveredDate = 4;
        public static int index_DetailSeq = 13;
        public static int index_Insurance = 9;
        public static int index_MaterialName = 12;
        public static int index_ProcessPrice = 11;
        public static int index_MaterialPrice = 10;
        public static int index_MaterialCd = 14;
        public static int index_SupplierCd = 15;
        public static int index_DentalOfficeCd = 16;
        public static int index_Amount = 18;
        public static int index_BillCd = 19;
        public static int TOTAL_MATERIAL_PER_PAGE = 5;
        public static double RoundSystemPrice(double price, string roundValue)
        {
            double result = price;
            if (roundValue.Equals("ROUNDUP", StringComparison.OrdinalIgnoreCase))
            {
                result = Math.Ceiling(price);
            }
            else if (roundValue.Equals("4down5UP", StringComparison.OrdinalIgnoreCase))
            {
                result = Math.Floor(price + 0.5);
            }
            else
            {
                result = Math.Floor(price);
            }

            return result;
        }
    }

    public class BillSearchInfo
    {
        public int OfficeCd { get; set; }
        public string OrderNo { get; set; }
        public string OrderDateFrom { get; set; }
        public string OrderDateTo { get; set; }
        public string DeliverDateFrom { get; set; }
        public string DeliverDateTo { get; set; }
        public string DentalOfficeCd { get; set; }
        public string BillCd { get; set; }
        public bool Insured { get; set; }
        public bool UnInsured { get; set; }
        public string BillYear { get; set; }
        public string BillMonth { get; set; }
        public string BillStatementNo { get; set; }
    }
    [Serializable]
    public class ReportBillStatement : ICloneable
    {
        public int BillPage { get; set; }
        public int TotalPage { get; set; }
        public string BillStatementNm { get; set; }
        public string BillPostalCd { get; set; }
        public string BillAddress1 { get; set; }
        public string BillAddress2 { get; set; }
        public string BillStatementNo { get; set; }

        public string CompanyNm { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPostalCode { get; set; }
        public string CompanyTel { get; set; }
        public string CompanyFax { get; set; }
        public string ClosingPeriod { get; set; }
        public string BillIssueDate { get; set; }

        public double SumPurChase { get; set; }
        public double LastMonth { get; set; }
        public double Discount { get; set; }
        public double CarryOver { get; set; }
        public double Purchase { get; set; }

        public double Tax { get; set; }
        public double TechPrice { get; set; }
        public double MaterialPrice { get; set; }
        public double BankDeposit { get; set; }

        public string Material { get; set; }
        public int FillMaterial { get; set; }
        public double CarryOverMaterial { get; set; }
        public double AdditionReceiveMaterial { get; set; }
        public double UserAmount { get; set; } //sum(Amount) from TrnStockOut, InOutKubun=21(usage)
        public double RestOfReceivedMaterial { get; set; } //carryover + AdditionReceiveMaterial - UserAmount

        public string BankAccount { get; set; }
        public string BankOwner { get; set; }

        public ReportBillStatement()
        {
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public static ReportBillStatement GetItem(TrnBillHeader billInfo, MasterBill mstBill)
        {
            ReportBillStatement temp = new ReportBillStatement();
            temp.BillStatementNm = mstBill.BillStatementNm;
            temp.BillPostalCd = mstBill.BillPostalCd;
            temp.BillAddress1 = mstBill.BillAddress1;
            temp.BillAddress2 = mstBill.BillAddress2;

            if (mstBill.BankCd!=null)
            {
                MasterBank bankInfo = MasterBank.GetBankMaster(mstBill.OfficeCd, Convert.ToInt32(mstBill.BankCd));
                if (bankInfo != null)
                {
                    temp.BankAccount = bankInfo.BankAccount;
                    temp.BankOwner = bankInfo.AccountOwner;
                }
            }
            temp.BillIssueDate = billInfo.BillIssueDate.ToShortDateString();
            temp.BillStatementNo = billInfo.BillStatementNo;
            temp.ClosingPeriod = billInfo.BillPeriod;
            temp.LastMonth = billInfo.PreviousSumPrice;
            temp.CarryOver = billInfo.CarryOver;
            temp.Purchase = billInfo.CurrentPrice;  // billInfo.CurrentTechPrice + billInfo.CurrentMaterialPrice; 
            temp.Discount = billInfo.CurrentDiscount;
            temp.Tax = billInfo.CurrentTax;

            temp.SumPurChase = billInfo.CurrentSumPrice; // temp.Purchase - temp.Discount + temp.Tax;      //Purchase - Discount + Tax
            temp.TechPrice = billInfo.CurrentTechPrice;
            temp.MaterialPrice = billInfo.CurrentMaterialPrice;
            temp.BankDeposit = 1;
            temp.FillMaterial = 1;
            return temp;
        }
        public static ReportBillStatement GetItem(ReportBillStatement item, TrnBillMaterial materialInfo)
        {
            ReportBillStatement temp = (ReportBillStatement)item.Clone();
            if (materialInfo != null)
            {
                temp.Material = materialInfo.MaterialNm;
                temp.CarryOverMaterial = materialInfo.MaterialCarryOver;
                temp.AdditionReceiveMaterial = materialInfo.MaterialBorrow;
                temp.UserAmount = materialInfo.MaterialUsed;
                temp.RestOfReceivedMaterial = materialInfo.MaterialCarryOver + materialInfo.MaterialBorrow - materialInfo.MaterialUsed;
                temp.FillMaterial = 1;
            }
            else
            {
                temp.FillMaterial = 0;
                temp.BillPage = 1;
                temp.TotalPage = 1;
            }
            return temp;
        }
    }
    public class PdfBillStatementInfo
    {
        public string billStatementNo { get; set; }
        public string pageRange { get; set; }
        public PdfBillStatementInfo(string no, string range)
        {
            billStatementNo = no;
            pageRange = range;
        }
    }
}
