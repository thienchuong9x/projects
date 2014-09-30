using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dental.Domain
{
   
    public class ProsthesisSearchInfo
    {
        #region Propertise
        public int OfficeCd
        {
            get;
            set;
        }

        public int ProsthesisCd
        {
            get;
            set;
        }

        public string ProsthesisAbbNm
        {
            get;
            set;
        }

        public string ProsthesisNm
        {
            get;
            set;
        }

        public string ProsthesisType
        {
            get;
            set;
        }

        public Nullable<bool> StumpFlg
        {
            get;
            set;
        }

        public int MinimumProcess
        {
            get;
            set;
        }

        public int Process
        {
            get;
            set;
        }

        public int TechPrice
        {
            get;
            set;
        }
        #endregion

        #region Method

        public static List<ProsthesisSearchInfo> GetProsthesisSearch(int OfficeCd, string ProsthesisCd, string ProsthesisNm, Nullable<int> ProcessCd, Nullable<int> TechCd)
        {
            var contex = new DBContext();

            if (ProcessCd == null && TechCd == null)
            {
                return (from mstProsthesis in contex.GetTable<MasterProsthesis>()
                        where
                            mstProsthesis.OfficeCd == OfficeCd
                        && mstProsthesis.ProsthesisCd.ToString().StartsWith(ProsthesisCd)
                        && mstProsthesis.ProsthesisNm.Contains(ProsthesisNm)

                        select new ProsthesisSearchInfo
                        {
                            OfficeCd = OfficeCd,
                            ProsthesisCd = mstProsthesis.ProsthesisCd,
                            ProsthesisAbbNm = mstProsthesis.ProsthesisAbbNm,
                            ProsthesisNm = mstProsthesis.ProsthesisNm,
                            ProsthesisType = mstProsthesis.ProsthesisType,
                            StumpFlg = mstProsthesis.StumpFlg,
                            MinimumProcess = mstProsthesis.MinimumProcess,
                            Process = (from p in contex.GetTable<MasterProcessTemplate>()
                                       where p.ProsthesisCd == mstProsthesis.ProsthesisCd
                                           && p.OfficeCd == OfficeCd
                                       select p.ProcessCd).Count(),
                            TechPrice = (from t in contex.GetTable<MasterTechPriceTemplate>()
                                         where t.ProsthesisCd == mstProsthesis.ProsthesisCd
                                         && t.OfficeCd == OfficeCd
                                         select t.TechCd).Count()

                        }).ToList();
            }

            if (ProcessCd != null && TechCd==null)
            {
                return (from mstProsthesis in contex.GetTable<MasterProsthesis>()
                        from mstProcessTemplete in contex.GetTable<MasterProcessTemplate>()
                        where
                            mstProsthesis.OfficeCd == OfficeCd
                        && mstProsthesis.ProsthesisCd.ToString().StartsWith(ProsthesisCd)
                        && mstProsthesis.ProsthesisNm.Contains(ProsthesisNm)
                        && ((mstProsthesis.ProsthesisCd == mstProcessTemplete.ProsthesisCd && mstProcessTemplete.ProcessCd == ProcessCd))
                        && mstProcessTemplete.OfficeCd == OfficeCd
                        
                        select new ProsthesisSearchInfo
                        {
                            OfficeCd = OfficeCd,
                            ProsthesisCd = mstProsthesis.ProsthesisCd,
                            ProsthesisAbbNm = mstProsthesis.ProsthesisAbbNm,
                            ProsthesisNm = mstProsthesis.ProsthesisNm,
                            ProsthesisType = mstProsthesis.ProsthesisType,
                            StumpFlg = mstProsthesis.StumpFlg,
                            MinimumProcess = mstProsthesis.MinimumProcess,
                            Process = (from p in contex.GetTable<MasterProcessTemplate>()
                                       where p.ProsthesisCd == mstProsthesis.ProsthesisCd
                                           && p.OfficeCd == OfficeCd
                                       select p.ProcessCd).Count(),
                            TechPrice = (from t in contex.GetTable<MasterTechPriceTemplate>()
                                         where t.ProsthesisCd == mstProsthesis.ProsthesisCd
                                         && t.OfficeCd == OfficeCd
                                         select t.TechCd).Count()

                        }).ToList();
            }
            if (TechCd != null && ProcessCd == null)
            {
                return (from mstProsthesis in contex.GetTable<MasterProsthesis>()
                        from mstTechPriceTemplete in contex.GetTable<MasterTechPriceTemplate>()
                        where
                            mstProsthesis.OfficeCd == OfficeCd
                        && mstProsthesis.ProsthesisCd.ToString().StartsWith(ProsthesisCd)
                        && mstProsthesis.ProsthesisNm.Contains(ProsthesisNm)
                        && ((mstProsthesis.ProsthesisCd == mstTechPriceTemplete.ProsthesisCd && mstTechPriceTemplete.TechCd == TechCd))
                        && mstTechPriceTemplete.OfficeCd == OfficeCd
                        select new ProsthesisSearchInfo
                        {
                            OfficeCd = OfficeCd,
                            ProsthesisCd = mstProsthesis.ProsthesisCd,
                            ProsthesisAbbNm = mstProsthesis.ProsthesisAbbNm,
                            ProsthesisNm = mstProsthesis.ProsthesisNm,
                            ProsthesisType = mstProsthesis.ProsthesisType,
                            StumpFlg = mstProsthesis.StumpFlg,
                            MinimumProcess = mstProsthesis.MinimumProcess,
                            Process = (from p in contex.GetTable<MasterProcessTemplate>()
                                       where p.ProsthesisCd == mstProsthesis.ProsthesisCd
                                           && p.OfficeCd == OfficeCd
                                       select p.ProcessCd).Count(),
                            TechPrice = (from t in contex.GetTable<MasterTechPriceTemplate>()
                                         where t.ProsthesisCd == mstProsthesis.ProsthesisCd
                                         && t.OfficeCd == OfficeCd
                                         select t.TechCd).Count()

                        }).ToList();
            }
            if (ProcessCd != null && TechCd != null)
            {
                return (from mstProsthesis in contex.GetTable<MasterProsthesis>()
                        from mstProcessTemplete in contex.GetTable<MasterProcessTemplate>()
                        from mstTechPriceTempltete in contex.GetTable<MasterTechPriceTemplate>()
                        where
                            mstProsthesis.OfficeCd == OfficeCd
                        && mstProsthesis.ProsthesisCd.ToString().StartsWith(ProsthesisCd)
                        && mstProsthesis.ProsthesisNm.Contains(ProsthesisNm)

                        && ((mstProsthesis.ProsthesisCd == mstProcessTemplete.ProsthesisCd && mstProcessTemplete.ProcessCd == ProcessCd) && (mstProsthesis.ProsthesisCd == mstTechPriceTempltete.ProsthesisCd && mstTechPriceTempltete.TechCd == TechCd))
                        && mstProcessTemplete.OfficeCd == OfficeCd
                        && mstTechPriceTempltete.OfficeCd == OfficeCd

                        select new ProsthesisSearchInfo
                        {
                            OfficeCd = OfficeCd,
                            ProsthesisCd = mstProsthesis.ProsthesisCd,
                            ProsthesisAbbNm = mstProsthesis.ProsthesisAbbNm,
                            ProsthesisNm = mstProsthesis.ProsthesisNm,
                            ProsthesisType = mstProsthesis.ProsthesisType,
                            StumpFlg = mstProsthesis.StumpFlg,
                            MinimumProcess = mstProsthesis.MinimumProcess,
                            Process = (from p in contex.GetTable<MasterProcessTemplate>()
                                       where p.ProsthesisCd == mstProsthesis.ProsthesisCd
                                           && p.OfficeCd == OfficeCd
                                       select p.ProcessCd).Count(),
                            TechPrice = (from t in contex.GetTable<MasterTechPriceTemplate>()
                                         where t.ProsthesisCd == mstProsthesis.ProsthesisCd
                                         && t.OfficeCd == OfficeCd
                                         select t.TechCd).Count()

                        }).ToList();
            }
            else
                return null;
        }


        #endregion
    }
    

   
}
