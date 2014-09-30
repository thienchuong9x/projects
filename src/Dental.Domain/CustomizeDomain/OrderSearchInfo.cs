using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dental.Utilities;

namespace Dental.Domain
{
    public class OrderSearchInfo
    {
        #region Property
        public string OrderNo
        {
            get;
            set;
        }
        public int OfficeCd
        {
            get;
            set;
        }
        public string StaffCd
        {
            get;
            set;
        }
        public string OrderDateFrom
        {
            get;
            set;
        }
        public string OrderDateTo
        {
            get;
            set;
        }

        public string DeliverDateFrom
        {
            get;
            set;
        }

        public string DeliverDateTo
        {
            get;
            set;
        }


        public string DentalOfficeCd
        {
            get;
            set;
        }

        public string DentistNm
        {
            get;
            set;
        }

        public string PatientLastNm
        {
            get;
            set;
        }

        public string PatientFirstNm
        {
            get;
            set;
        }

        public string ProsthesisCd
        {
            get;
            set;
        }

        public string ContractorCd
        {
            get;
            set;
        }

        public Boolean BillCompleteFlg
        {
            get;
            set;
        }

        public Boolean DeliveryCompleteFlg
        {
            get;
            set;
        }

        public Boolean OnlyTrialOrderFlg
        {
            get;
            set;
        }

        public Boolean OnlyReManufactureFlg
        {
            get;
            set;
        }


        public bool DepositCompleteFlg { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
        #endregion 

    }
    public class OrderListInfo
    {
        public double OrderSeq { get; set;  }
        public string OrderNo { get;  set;  }
        public int OfficeCd { get;  set;  }
        public Nullable<int> StaffCd  { get;  set;  }

        public DateTime OrderDate
        {
            get;
            set;
        }

        public string OrderDateStr
        {
            get;
            set;
        }

        public string DeliverDateStr
        {
            get;
            set;
        }

        public string CompleteDate
        {
            get;
            set;
        }

        public DateTime DeliverDate
        {
            get;
            set;
        }

        public DateTime SetDate
        {
            get;
            set;
        }

        public int DentalOfficeCd
        {
            get;
            set;
        }

        public string DentistNm
        {
            get;
            set;
        }

        public string PatientLastNm
        {
            get;
            set;
        }

        public string PatientFirstNm
        {
            get;
            set;
        }

        public Nullable<int> PatientSex
        {
            get;
            set;
        }

        public Nullable<int> PatientAge
        {
            get;
            set;
        }

        public Nullable<int> OutsourceLabCd
        {
            get;
            set;
        }

        public string OutsourceLabStaffNm
        {
            get;
            set;
        }

        public Nullable<double> SumPrice
        {
            get;
            set;
        }

        public Nullable<int> DeliveryStatement
        {
            get;
            set;
        }

        public string Note
        {
            get;
            set;
        }

        public string StaffName { get; set; }
        public string OfficeName { get; set; }
        public string SexName { get; set; }


        public static List<OrderListInfo> GetOrderLists(OrderSearchInfo objSearch)
        {

            //Search from Order Header 
             List<OrderListInfo> list = (from item in TrnOrderHeader.GetTable()
                                         where objSearch.OrderNo.Contains(objSearch.OrderNo) 
                    select new OrderListInfo()
                    {
                        OrderSeq = item.OrderSeq,
                        OrderNo = item.OrderNo,
                        //TrialOrderSeq = item.TrialOrderSeq,
                        //TrialOrderFlg = item.TrialOrderFlg,
                        OfficeCd = item.OfficeCd , 
                        //StaffCd = item.StaffCd ,
                        OrderDate = item.OrderDate , 


                    }).ToList();

             return list;
        }
        public static List<OrderListInfo> GetOrderListSearch(OrderSearchInfo objSearch)
        {
            if (string.IsNullOrWhiteSpace(objSearch.OrderDateFrom))     objSearch.OrderDateFrom = "1/1/2000";
            if (string.IsNullOrWhiteSpace(objSearch.OrderDateTo))       objSearch.OrderDateTo = "1/1/2200";
            if (objSearch.DeliverDateFrom == string.Empty)  objSearch.DeliverDateFrom = "1/1/2000";
            if (objSearch.DeliverDateTo == string.Empty)    objSearch.DeliverDateTo = "1/1/2200";

            DBContext context = new DBContext();
            //Search from Order Header 
            List<OrderListInfo> list = (from item in context.GetTable<TrnOrderHeader>()  
                                        join itemDetail in context.GetTable<TrnOrderDetail>() on new { item.OfficeCd, item.OrderSeq } equals new { itemDetail.OfficeCd, itemDetail.OrderSeq } into  DeTail
                                        from itemDetail in DeTail.DefaultIfEmpty()
                                        from itemOusource in context.GetTable<TrnOutsource>()
                                        where 
                                          (
                                            (item.OfficeCd == objSearch.OfficeCd)
                                         && (objSearch.OrderNo == string.Empty || item.OrderNo.Contains(objSearch.OrderNo))
                                         && (
                                            (string.IsNullOrWhiteSpace(objSearch.OrderDateFrom) && item.OrderDate <= Convert.ToDateTime(objSearch.OrderDateTo))
                                            || (string.IsNullOrWhiteSpace(objSearch.OrderDateTo) && item.OrderDate >= Convert.ToDateTime(objSearch.OrderDateFrom))
                                            || (item.OrderDate >= Convert.ToDateTime(objSearch.OrderDateFrom) && item.OrderDate <= Convert.ToDateTime(objSearch.OrderDateTo))
                                            )

                                         && (
                                            (string.IsNullOrWhiteSpace(objSearch.DeliverDateFrom) && item.DeliverDate <= Convert.ToDateTime(objSearch.DeliverDateTo))
                                            || (string.IsNullOrWhiteSpace(objSearch.DeliverDateTo) && item.DeliverDate >= Convert.ToDateTime(objSearch.DeliverDateFrom))
                                            || (item.DeliverDate >= Convert.ToDateTime(objSearch.DeliverDateFrom) && item.DeliverDate <= Convert.ToDateTime(objSearch.DeliverDateTo))
                                            )

                                         && (objSearch.StaffCd == string.Empty || item.StaffCd.ToString() == objSearch.StaffCd)
                                         && (objSearch.DentalOfficeCd == string.Empty || item.DentalOfficeCd.ToString() == objSearch.DentalOfficeCd)
                                         && (objSearch.PatientLastNm == string.Empty || item.PatientLastNm.Contains(objSearch.PatientLastNm) || item.PatientFirstNm.Contains(objSearch.PatientLastNm))
                                         && (objSearch.OnlyTrialOrderFlg == false || (item.TrialOrderFlg == true && objSearch.OnlyTrialOrderFlg == true))
                                         && (objSearch.OnlyReManufactureFlg == false || (item.RemanufactureFlg == true && objSearch.OnlyReManufactureFlg == true))

                                        && (objSearch.ProsthesisCd == string.Empty || (item.OrderSeq == itemDetail.OrderSeq && itemDetail.ProsthesisCd.ToString() == objSearch.ProsthesisCd && item.OfficeCd == itemDetail.OfficeCd))
                                        && (objSearch.ContractorCd == string.Empty || (item.OrderSeq == itemOusource.OrderSeq && itemOusource.OutsourceCd.ToString() == objSearch.ContractorCd && item.OfficeCd == itemOusource.OfficeCd))
                                        && (objSearch.BillCompleteFlg == true || itemDetail.BillStatementNo == null)
                                        && (objSearch.DeliveryCompleteFlg == true || itemDetail.DeliveryStatementNo == null)
                                      )
                                        
                                        select new OrderListInfo()
                                        {
                                            OrderSeq = item.OrderSeq,
                                            OrderNo = item.OrderNo,
                                            StaffCd =  item.StaffCd,
                                            OrderDate = item.OrderDate,
                                            DeliverDate = item.DeliverDate,
                                            DentalOfficeCd = item.DentalOfficeCd,
                                            DentistNm = item.DentistNm,
                                            PatientLastNm = item.PatientLastNm,
                                            PatientFirstNm  = item.PatientFirstNm,
                                            PatientSex = item.PatientSex,
                                            PatientAge = item.PatientAge,
                                            //TrialOrderSeq = item.TrialOrderSeq,
                                            //TrialOrderFlg = item.TrialOrderFlg,
                                            OfficeCd = item.OfficeCd,                                       

                                        }).Distinct().ToList();

            return list;
        }
    }
}

//ALTER PROCEDURE [dbo].[DDV_GetOrderListsSearch]
//    @OfficeCd int,
//    @OrderNo nvarchar(20),
//    @OrderDateFrom nvarchar(20),
//    @OrderDateTo nvarchar(20),
//    @DeliverDateFrom nvarchar(20),
//    @DeliverDateTo nvarchar(20),
//    @DentalOfficeCd int,
//    @PatientLastNm nvarchar(20),
//    @StaffCd int,
//    @ProsthesisCd int,
//    @ContractorCd int,
//    @BillCompleteFlg  bit,  
//    @DeliveryCompleteFlg bit,
//    @OnlyTrialOrderFlg bit,	
//    @OnlyReManufactureFlg bit 

//AS
//SELECT
//     [OrderSeq],[OrderNo],[StaffCd],[OrderDate],[DeliverDate],[SetDate],[DentalOfficeCd],[DentistNm],[PatientLastNm],[PatientFirstNm],[PatientSex],[PatientAge], [DeliveryStatement],[Note], [PatientLastNm]+' '+[PatientFirstNm] as [PatientNm] 
// FROM DENTAL_TrnOrderHeader as hd (NOLOCK)                
// WHERE (OfficeCd = @OfficeCd) 
//        AND ((@OrderNo is NULL) or 
//            (OrderNo LIKE '%'+ @OrderNo + '%') ) 
//        AND 
//         ( 
//             ( @OrderDateFrom is NULL  AND   @OrderDateTo is NULL) 
//          OR ( @OrderDateFrom is NULL AND  OrderDate <= convert(datetime,@OrderDateTo))
//          OR ( @OrderDateTo is NULL AND  OrderDate >= convert(datetime,@OrderDateFrom))
//          OR ( OrderDate BETWEEN convert(datetime,@OrderDateFrom) and convert(datetime,@OrderDateTo))
//         )  
//         AND 
//         ( 
//             ( @DeliverDateFrom is NULL  AND   @DeliverDateTo is NULL) 
//          OR ( @DeliverDateFrom is NULL AND  DeliverDate <= convert(datetime,@DeliverDateTo))
//          OR ( @DeliverDateTo is NULL AND  DeliverDate >= convert(datetime,@DeliverDateFrom))
//          OR ( DeliverDate BETWEEN convert(datetime,@DeliverDateFrom) and convert(datetime,@DeliverDateTo))
//         )        

//        AND 
//            ( @DentalOfficeCd is NULL or  DentalOfficeCd = @DentalOfficeCd )        
//        AND 
//            ((@PatientLastNm is NULL) or
//            (PatientLastNm LIKE '%'+ @PatientLastNm + '%' OR PatientFirstNm LIKE '%'+ @PatientLastNm + '%'))  
//        AND 
//            ( @StaffCd is NULL or StaffCd = @StaffCd) 
//        AND 
//            (@ProsthesisCd is NULL or  hd.[OrderSeq] IN (SELECT distinct [OrderSeq] FROM [DENTAL_TrnOrderDetail] WHERE ProsthesisCd = @ProsthesisCd )  ) 
//        AND 
//            (@ContractorCd is NULL or hd.[OrderSeq] IN	(SELECT distinct [OrderSeq]  FROM [DENTAL_TrnOutSource] WHERE [OutsourceCd] = @ContractorCd ) ) 

//        AND 
//          (
//            (@BillCompleteFlg =1 ) 
//            OR (@BillCompleteFlg =0 AND (hd.[OrderSeq] IN (SELECT distinct [OrderSeq] FROM [DENTAL_TrnOrderDetail] WHERE  OfficeCd = @OfficeCd AND BillStatementNo is NULL)   OR (   (SELECT count(*) FROM [DENTAL_TrnOrderDetail] WHERE  OfficeCd = @OfficeCd AND OrderSeq=hd.[OrderSeq]) < 1)   )) 
//          )
//        AND 
//          (
//            (@DeliveryCompleteFlg =1 ) 
//            OR (@DeliveryCompleteFlg =0 AND (hd.[OrderSeq] IN (SELECT distinct [OrderSeq] FROM [DENTAL_TrnOrderDetail] WHERE  OfficeCd = @OfficeCd AND  DeliveryStatementNo is NULL)   OR (   (SELECT count(*) FROM [DENTAL_TrnOrderDetail] WHERE  OfficeCd = @OfficeCd AND OrderSeq=hd.[OrderSeq]) < 1)    )) 
//          )

//        AND 
//          (
//            (@OnlyTrialOrderFlg = 1 AND TrialOrderFlg = 1) 
//            OR (@OnlyTrialOrderFlg = 0 ) 
//          )
//        AND 
//          ( 
//            (@OnlyReManufactureFlg = 1 AND RemanufactureFlg = 1 ) 
//            OR (@OnlyReManufactureFlg = 0 )
//          )


//******************************************************************************************



        //AND 
        //    (@ProsthesisCd is NULL or  hd.[OrderSeq] IN (SELECT distinct [OrderSeq] FROM [DENTAL_TrnOrderDetail] WHERE ProsthesisCd = @ProsthesisCd )  ) 
        //AND 
        //    (@ContractorCd is NULL or hd.[OrderSeq] IN	(SELECT distinct [OrderSeq]  FROM [DENTAL_TrnOutSource] WHERE [OutsourceCd] = @ContractorCd ) ) 

        //AND 
        //  (
        //    (@BillCompleteFlg =1 ) 
        //    OR (@BillCompleteFlg =0 AND (hd.[OrderSeq] IN (SELECT distinct [OrderSeq] FROM [DENTAL_TrnOrderDetail] WHERE  OfficeCd = @OfficeCd AND BillStatementNo is NULL)   OR (   (SELECT count(*) FROM [DENTAL_TrnOrderDetail] WHERE  OfficeCd = @OfficeCd AND OrderSeq=hd.[OrderSeq]) < 1)   )) 
        //  )
        //AND 
        //  (
        //    (@DeliveryCompleteFlg =1 ) 
        //    OR (@DeliveryCompleteFlg =0 AND (hd.[OrderSeq] IN (SELECT distinct [OrderSeq] FROM [DENTAL_TrnOrderDetail] WHERE  OfficeCd = @OfficeCd AND  DeliveryStatementNo is NULL)   OR (   (SELECT count(*) FROM [DENTAL_TrnOrderDetail] WHERE  OfficeCd = @OfficeCd AND OrderSeq=hd.[OrderSeq]) < 1)    )) 
        //  )




                                         //&& (
                                         //   (string.IsNullOrWhiteSpace(objSearch.OrderDateFrom) && string.IsNullOrWhiteSpace(objSearch.OrderDateTo))
                                         //   || (objSearch.OrderDateFrom == string.Empty && item.OrderDate <= Convert.ToDateTime(objSearch.OrderDateTo))
                                         //   || (objSearch.OrderDateTo == string.Empty && item.OrderDate >= Convert.ToDateTime(objSearch.OrderDateFrom))
                                         //   || (item.OrderDate >= Convert.ToDateTime(objSearch.OrderDateFrom) && item.OrderDate <= Convert.ToDateTime(objSearch.OrderDateTo))
                                         //   )

                                         //&& (
                                         //   (string.IsNullOrWhiteSpace(objSearch.DeliverDateFrom) && string.IsNullOrWhiteSpace(objSearch.DeliverDateTo))
                                         //   || (objSearch.DeliverDateFrom == string.Empty && item.DeliverDate <= Convert.ToDateTime(objSearch.DeliverDateTo))
                                         //   || (objSearch.DeliverDateTo == string.Empty && item.DeliverDate >= Convert.ToDateTime(objSearch.DeliverDateFrom))
                                         //   || (item.DeliverDate >= Convert.ToDateTime(objSearch.DeliverDateFrom) && item.DeliverDate <= Convert.ToDateTime(objSearch.DeliverDateTo))
                                         //   )