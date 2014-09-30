using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dental.Domain
{
    public class TechPriceBasedOnTech
    {
        #region Propertise

        public int OfficeCd { get; set; }
        public int TechCd { get; set; }
        public int DentalOfficeCd { get; set; }
        public DateTime StartDate { get; set; }
        public string TechNm { get; set; }
        public string DentalOfficeNm { get; set; }
        public int TechPrice { get; set; }

        #endregion

        #region Method

        public static List<TechPriceBasedOnTech> GetTechPriceByTechCd(int OfficeCd, int TechCd, DateTime StartDate)
        {
            var context = new DBContext();
            var query = from tp in context.GetTable<MasterTechPrice>()
                        from t in context.GetTable<MasterTech>() 
                        from d in context.GetTable<MasterDentalOffice>()
                        where
                        tp.OfficeCd == OfficeCd
                        && tp.TechCd == TechCd
                        && tp.StartDate == StartDate
                        && t.OfficeCd == OfficeCd
                        && d.OfficeCd == OfficeCd
                        && tp.TechCd == t.TechCd
                        && tp.DentalOfficeCd == d.DentalOfficeCd
                        
                        select new TechPriceBasedOnTech
                        {
                            OfficeCd = tp.OfficeCd,
                            TechCd = tp.TechCd,
                            DentalOfficeCd = tp.DentalOfficeCd,
                            StartDate = tp.StartDate,
                            TechNm = t.TechNm,
                            DentalOfficeNm = d.DentalOfficeNm,
                            TechPrice = tp.TechPrice
                        };
            return query.Distinct().ToList();
        }

        #endregion
    }
}
