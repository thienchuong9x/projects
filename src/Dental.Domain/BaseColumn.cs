using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dental.Domain
{
    public class BaseColumn
    {
        public BaseColumn() { }

        #region Common column of all table

        public const string CREATE_ACCOUNT = "CreateAccount";
        public const string CREATE_DATE = "CreateDate";
        public const string MODIFIED_ACCOUNT = "ModifiedAccount";
        public const string MODIFIED_DATE = "ModifiedDate";

        #endregion
    }
    public class BaseDentalOrderKeyColumn
    {
        public const string OFFICE_CD = "OfficeCd";
        public const string ORDER_SEQ = "OrderSeq";
        public const string DETAIL_SEQ = "DetailSeq";
    }


    #region "Master Table"
    public class MasterBillColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstBill";

        public const string OFFICE_CD = "OfficeCd";
        public const string BILL_CD = "BillCd";
        public const string BILL_NM = "BillNm";
        public const string BILL_STATEMENT_NM = "BillStatementNm";
        public const string BILL_POSTAL_CODE = "BillPostalCd";
        public const string BILL_ADDRESS1 = "BillAddress1";
        public const string BILL_ADDRESS2 = "BillAddress2";
        public const string BILL_TEL = "BillTEL";
        public const string BILL_FAX = "BillFAX";
        public const string BILL_CONTACTPERSON = "BillContactPerson";
        public const string CREDIT_LIMIT = "CreditLimit";
        public const string BILL_FLG = "BillFlg";
        public const string BILL_CLOSING_DAY = "BillClosingDay";
        public const string BANK_CD = "BankCd";
        public const string SUPPLIER_CD = "SupplierCd";
    }
    public class MasterBankColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstBank";

        public const string OFFICE_CD = "OfficeCd";
        public const string BANK_CD = "BankCd";
        public const string BANK_ACCOUNT = "BankAccount";
        public const string ACCOUNT_OWNER = "AccountOwner";
        public const string FOR_RECEIVE_FLG = "ForReceiveFlg";
        public const string FOR_PAY_FLG = "ForpayFlg";
    }
    public class MasterBillStatementNoColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstBillStatementNo";

        public const string OFFICE_CD = "OfficeCd";
        public const string BILL_CD = "BillCd";
        public const string YEAR = "Year";
        public const string BLANCH_NO = "BlanchNo";
    }
    public class MasterDeliveryStatementNoColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstDeliveryStatementNo";

        public const string OFFICE_CD = "OfficeCd";
        public const string DENTAL_OFFICE_CD = "DentalOfficeCd";
        public const string YEAR = "Year";
        public const string BLANCH_NO = "BlanchNo";
    }
    public class MasterDentalOfficeColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstDentalOffice";

        public const string OFFICE_CD = "DentalOfficeCd";
        public const string OFFICE_NAME = "DentalOfficeNm";
        public const string OFFICE_ABB_NAME = "DentalOfficeAbbNm";
        public const string OFFICE_STAFF_CD = "StaffCd";
        public const string OFFICE_POSTAL_CODE = "DentalOfficePostalCd";
        public const string OFFICE_ADDRESS1 = "DentalOfficeAddress1";
        public const string OFFICE_ADDRESS2 = "DentalOfficeAddress2";
        public const string OFFICE_TEL = "DentalOfficeTEL";
        public const string OFFICE_FAX = "DentalOfficeFAX";
        public const string TRANSFER_DAY = "TransferDays";
        public const string BILL_CD = "BillCd";
    }
    public class MasterInventoryColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstInventory";

        public const string MATERIAL_CD = "MaterialCd";
        public const string SUPPLIER_CD = "SupplierCd";
        public const string AMOUNT = "Amount";
        public const string AMOUNT_UNIT_CD = "AmountUnitCd";
        public const string FRACTION = "Fraction";
        public const string FRACTION_UNIT_CD = "FractionUnitCd";
    }
    public class MasterItemColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstItem";

        public const string ITEM_CATHEGORY = "ItemCathegory";
        public const string ITEM_NO = "ItemNo";
        public const string ITEM_NM = "ItemNm";
        public const string ITEM_VALUE = "ItemValue";
        public const string VIEW_ORDER = "ViewOrder";
        public const string IS_DELETED = "IsDeleted";
    }
    public class MasterMaterialColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstMaterial";

        public const string OFFICE_CD = "OfficeCd";
        public const string MATERIAL_CD = "MaterialCd";
        public const string MATERIAL_NM = "MaterialNm";
        public const string MATERIAL_CAD_NM = "MaterialCADNm";
        public const string PRODUCT_MAKER = "ProductMaker";
        public const string PRODUCT_CD = "ProductCd";
        public const string PRODUCT_NM = "ProductNm";
        public const string LENT_PRICE = "LentPrice";
        public const string SHADE_FLG = "ShadeFlg";
        public const string APPLY_DATE = "ApplyDate";
        public const string TERMINATE_DATE = "TerminateDate";

    }
    public class MasterMaterialPriceColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstMaterialPrice";

        public const string OFFICE_CD = "OfficeCd";
        public const string MATERIAL_CD = "MaterialCd";
        public const string SUPPLIER_CD = "SupplierCd";
        public const string START_DATE = "StartDate";
        public const string END_DATE = "EndDate";
        public const string PRICE_NM = "PriceNm";
        public const string PRICE = "Price";
    }
    public class MasterOfficeColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstOffice";

        public const string OFFICE_CD = "OfficeCd";
        public const string OFFICE_NAME = "OfficeNm";
        public const string OFFICE_POSTAL_CODE = "OfficePostalCd";
        public const string OFFICE_ADDRESS1 = "OfficeAddress1";
        public const string OFFICE_ADDRESS2 = "OfficeAddress2";
        public const string OFFICE_TEL = "OfficeTEL";
        public const string OFFICE_FAX = "OfficeFAX";
        public const string IS_DELETED = "IsDeleted";
    }
    public class MasterOutsourceLabColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstOutsourceLab";

        public const string OFFICE_CD = "OfficeCd";
        public const string OUTSOURCE_CD = "OutsourceCd";
        public const string OUTSOURCE_NM = "OutsourceNm";
        public const string OUTSOURCE_POSTAL_CODE = "OutsourcePostalCd";
        public const string OUTSOURCE_ADDRESS1 = "OutsourceAddress1";
        public const string OUTSOURCE_ADDRESS2 = "OutsourceAddress2";
        public const string OUTSOURCE_TEL = "OutsourceTEL";
        public const string OUTSOURCE_FAX = "OutsourceFAX";
        public const string OUTSOURCE_CONTACT_PERSON = "OutsourceContactPerson";
    }
    public class MasterProcessColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstProcess";

        public const string OFFICE_CD = "OfficeCd";
        public const string PROCESS_CD = "ProcessCd";
        public const string PROCESS_NM = "ProcessNm";
        public const string IS_DELETED = "IsDeleted";
    }
    public class MasterProcessTemplateColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstProcessTemplate";

        public const string OFFICE_CD = "OfficeCd";
        public const string PROSTHESIS_CD = "ProsthesisCd";
        public const string PROCESS_CD = "ProcessCd";
        public const string DISPLAY_ORDER = "DisplayOrder";
    }
    public class MasterProsthesisColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstProsthesis";

        public const string OFFICE_CD = "OfficeCd";
        public const string PROSTHESIS_CD = "ProsthesisCd";
        public const string PROSTHESIS_ABB_NM = "ProsthesisAbbNm";
        public const string PROSTHESIS_NM = "ProsthesisNm";
        public const string PROSTHESIS_TYPE = "ProsthesisType";
        public const string STUMP_FLG = "StumpFlg";
        public const string MINIUM_PROCESS = "MinimumProcess";

    }
    public class MasterStaffColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstStaff";
        public const string STAFF_CODE = "StaffCd";
        public const string STAFF_NAME = "StaffNm";
        public const string STAFF_KANA_NAME = "StaffNmKana";
        public const string STAFF_SECTION_CODE = "SectionCd";
        public const string STAFF_SALE_FLG = "SalesFlg";
        public const string STAFF_TECH_FLG = "TechFlg";
        public const string STAFF_IS_DELETED = "IsDeleted";
        public const string STAFF_USER_ID = "UserId";
    }
    public class MasterStockColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstStock";

        public const string OFFICE_CD = "OfficeCd";
        public const string MATERIAL_CD = "MaterialCd";
        public const string SUPPLIER_CD = "SupplierCd";
        public const string OUTSOURCE_LAB_CD = "OutsourceLabCd";
        public const string UNIT_CD = "UnitCd";
        public const string AMOUNT = "Amount";
    }
    public class MasterSupplierColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstSupplier";

        public const string OFFICE_CD = "OfficeCd";
        public const string SUPPLIER_CD = "SupplierCd";
        public const string SUPPLIER_NM = "SupplierNm";
        public const string SUPPLIER_ABB_NM = "SupplierAbbNm";
        public const string SUPPLIER_POSTAL_CODE = "SupplierPostalCd";
        public const string SUPPLIER_ADDRESS1 = "SupplierAddress1";
        public const string SUPPLIER_ADDRESS2 = "SupplierAddress2";
        public const string SUPPLIER_TEL = "SupplierTEL";
        public const string SUPPLIER_FAX = "SupplierFAX";
        public const string SUPPLIER_STAFF = "SupplierStaff";
    }
    public class MasterSystemColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstSystem";

        public const string PARAMETER = "Parameter";
        public const string VALUE = "Value";
    }
    public class MasterTaxColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstTax";

        public const string TAX_CD = "TaxCd";
        public const string TAX_RATE = "TaxRate";
        public const string START_DATE = "StartDate";
        public const string END_DATE = "EndDate";
        public const string ROUND_FRACTION = "RoundFraction";
    }
    public class MasterTechColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstTech";

        public const string OFFICE_CD = "OfficeCd";
        public const string TECH_CD = "TechCd";
        public const string START_DATE = "StartDate";
        public const string END_DATE = "EndDate";
        public const string TECH_NM = "TechNm";
        public const string STANDARD_TECH_PRICE = "StandardTechPrice";
        public const string EDITABLE = "Editable";
        public const string TECH_GROUP = "TechGroup";
    }
    public class MasterTechPriceColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstTechPrice";

        public const string OFFICE_CD = "OfficeCd";
        public const string TECH_CD = "TechCd";
        public const string START_DATE = "StartDate";
        public const string DENTAL_OFFICE_CD = "DentalOfficeCd";
        public const string TECH_PRICE = "TechPrice";
    }
    public class MasterTechPriceTemplateColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstTechPriceTemplate";

        public const string OFFICE_CD = "OfficeCd";
        public const string PROSTHESIS_CD = "ProsthesisCd";
        public const string TECH_CD = "TechCd";
        public const string DISPLAY_ORDER = "DisplayOrder";
    }
    public class MasterUnitColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_MstUnit";

        public const string OFFICE_CD = "OfficeCd";
        public const string UNIT_CD = "UnitCd";
        public const string MATERIAL_CD = "MaterialCd";
        public const string UNIT_NM = "UnitNm";
        public const string PRIORITY = "Priority";
        public const string AMOUNT_BY_MINIMUM_UNIT = "AmountByMinimumUnit";
        public const string UNIT_NOTE = "UnitNote";
        public const string APPLY_DATE = "ApplyDate";
        public const string TERMINATE_DATE = "TerminateDate";

    }
    public class MasterRolesColumn : BaseColumn
    {
        public const string TABLE_NAME = "Roles";

        public const string ROLE_ID = "RoleID";
        public const string PORTAL_ID ="PortalID";
        public const string ROLE_NAME ="RoleName";
        public const string DESCRIPTION = "Description";
        public const string SERVICE_FEE = "ServiceFee";
        public const string BILLING_FREQUENCY = "BillingFrequency";
        public const string TRIAL_PERIOD = "TrialPeriod";
        public const string TRIAL_FREQUENCY = "TrialFrequency";
        public const string BILLING_PERIOD = "BillingPeriod";
        public const string TRIAL_FEE = "TrialFee";
        public const string IS_PUBLIC = "IsPublic";
        public const string AUTO_ASSIGNMENT = "AutoAssignment";
        public const string ROLE_GROUP_ID = "RoleGroupID";
        public const string RSVP_CODE = "RSVPCode";
        public const string ICON_FILE = "IconFile";
        public const string CREATED_BY_USER_ID = "CreatedByUserID";
        public const string CREATED_ON_DATE = "CreatedOnDate";
        public const string LAST_MODIFIED_BY_USER_ID = "LastModifiedByUserID";
        public const string LAST_MODIFIED_ON_DATE = "LastModifiedOnDate";
        public const string STATUS = "Status";
        public const string SECURITY_MODE = "SecurityMode";
        public const string IS_SYSTEM_ROLE = "IsSystemRole";
    }
    public class MasterUserRolesColumn : BaseColumn
    {
        public const string TABLE_NAME = "UserRoles";

        public const string USER_ROLE_ID = "UserRoleID";
        public const string USER_ID = "UserID";
        public const string ROLE_ID = "RoleID";
        public const string EXPIRY_DATE = "ExpiryDate";
        public const string IS_TRIAL_USED = "IsTrialUsed";
        public const string EFFECTIVE_DATE = "EffectiveDate";
        public const string CREATED_BY_USER_ID = "CreatedByUserID";
        public const string CREATED_ON_DATE = "CreatedOnDate";
        public const string LAST_MODIFIED_BY_USER_ID = "LastModifiedByUserID";
        public const string LAST_MODIFIED_ON_DATE = "LastModifiedOnDate";
        public const string STATUS = "Status";
        public const string IS_OWNER = "IsOwner";
    }

    public class MasterUsersColumn : BaseColumn
    {
        public const string TABLE_NAME = "Users";
                
        public const string USER_ID = "UserID";
        public const string USERNAME = "Username";
        public const string FIRST_NAME = "FirstName";
        public const string LAST_NAME = "LastName";
        public const string IS_SUPER_USER = "IsSuperUser";
        public const string AFFILIATE_ID = "AffiliateId";
        public const string EMAIL = "Email";
        public const string DISPLAY_NAME = "DisplayName";
        public const string UPDATE_PASSWORD = "UpdatePassword";
        public const string LAST_IP_ADDRESS = "LastIPAddress";
        public const string IS_DELETED = "IsDeleted";
        public const string CREATED_BY_USER_ID = "CreatedByUserID";
        public const string CREATED_ON_DATE = "CreatedOnDate";
        public const string LAST_MODIFIED_BY_USER_ID = "LastModifiedByUserID";
        public const string LAST_MODIFIED_ON_DATE = "LastModifiedOnDate";
    }
    public class PermissionColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_Permission";

        public const string STAFF_CD = "StaffCd";
        public const string OFFICE_CD = "OfficeCd";
        public const string PERMISSION = "Permission";
        public const string DEL_FLG = "DelFlg";        
    }

    #endregion    
   
    #region "Trn Table"
    public class TrnBillColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnBill";

        public const string BILL_SEQ = "BillSeq";
        public const string DETAIL_NO = "BillDetailNO";
        public const string DETAIL_NM = "BillDetailNm";
        public const string STATEMENT_NO = "BillStatamentNo";
        public const string DETAIL_PRICE = "BillDetailPrice";
        public const string SUM_PRICE = "BillSumPrice";


        public const string BILL_DATE = "BillDate";
        public const string BILL_CD = "BillCd";
        public const string BILL_NM = "BillNm";
        public const string DENTAL_OFFICE_CD = "DentalOfficeCd";
        public const string DENTAL_OFFICE_NM = "DentalOfficeNm";
        public const string PAID_MONEY = "PaidMoney";

        public const string PAY_COMPLETE_FLG = "PayCompleteFlg";
        public const string LAST_PAY_DATE = "LastPayDate";
        public const string NOTE = "Note";
    }
    public class TrnBillHeaderColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnBillHeader";

        public const string BILL_SEQ = "BillSeq";
        public const string BILL_STATEMENT_NO = "BillStatementNo";
        public const string BILL_ISSUE_DATE = "BillIssueDate";
        public const string BILL_YEAR = "BillYear";
        public const string BILL_MONTH = "BillMonth";
        public const string BILL_PERIOD = "BillPeriod";
        public const string BILL_CD = "BillCd";
        public const string BILL_NM = "BillNm";

        public const string PREVIOUS_SUM_PRICE = "PreviousSumPrice";
        public const string CARRY_OVER = "CarryOver";
        public const string CURRENT_TECH_PRICE = "CurrentTechPrice";
        public const string CURRENT_MATERIAL_PRICE = "CurrentMaterialPrice";
        public const string CURRENT_INSURED_PRICE = "CurrentInsuredPrice";
        public const string CURRENT_UNINSURED_PRICE = "CurrentUnInsuredPrice";

        public const string CURRENT_PRICE = "CurrentPrice";
        public const string CURRENT_DISCOUNT = "CurrentDiscount";
        public const string CURRENT_TAX = "CurrentTax";
        public const string CURRENT_SUM_PRICE = "CurrentSumPrice";
        public const string CURRENT_PAID_MONEY = "CurrentPaidMoney";
     
        public const string LAST_PAY_DATE = "LastPayDate";
        public const string DELETE_FLAG = "DelFlg";
        public const string PAY_COMPLETE_FLG = "PayCompleteFlg";
        public const string NOTE = "Note";
     
    }
    public class TrnBillMaterialColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnBillMaterial";

        public const string BILL_SEQ = "BillSeq";
        public const string MATERIAL_CD= "MaterialCd";
        public const string MATERIAL_NM = "MaterialNm";
        public const string MATERIAL_CARRY_OVER = "MaterialCarryOver";
        public const string MATERIAL_BORROW = "MaterialBorrow";
        public const string MATERIAL_USED = "MaterialUsed";
        public const string MATERIAL_STOCK = "MaterialStock";
        public const string DEL_FLG = "DelFlg";
    }
    public class TrnBillMoneyColumn : BaseColumn
    {

        public const string TABLE_NAME = "DENTAL_TrnBillMoney";

        public const string BILL_SEQ = "BillSeq";
        public const string PAY_DATE = "PayDateTime";
        public const string BILL_CD = "BillCd";
        public const string PAID_MONEY = "PaidMoney";
        public const string BALANCE = "Balance";
 
    }
    public class TrnMoneyReceiveColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnMoneyReceive";

        public const string PAY_DATE = "PayDate";
        public const string BILL_CD = "BillCd";
        public const string BILL_NM = "BillNm";
        public const string PAY_AMOUNT = "PayAmount";
        public const string SUBTRACT_FEE = "SubtractFee";
        public const string SUM_PAY_AMOUNT = "SumPayAmount";
        public const string UNCHARGE_PAY = "UnchargerPay";
        public const string BANK_CD = "BankCd";
        public const string BILL_STATEMENT_NO = "BillStatementNo";
        public const string NOTE = "Note";
    }
    public class TrnMoneyReceiveHistoryColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnMoneyReceive";
    }    
    public class TrnOrderDetailColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnOrderDetail";

        public const string DETAIL_NM = "DetailNm";
        public const string DISPLAY_ORDER = "DisplayOrder";
        public const string BRIDGED_ID = "BridgedID";
        public const string TOOTH_NUMBER = "ToothNumber";

        public const string CHILD_FLG = "ChildFlg";
        public const string GAP_FLG = "GapFlg";
        public const string DENTURE_FLG = "DentureFlg";


        public const string INSURANCE_KBN = "InsuranceKbn";
        public const string PROSTHESIS_CD = "ProsthesisCd";
        public const string PROSTHESIS_NM = "ProsthesisNm";
        public const string MATERIAL_CD = "MaterialCd";
        public const string MATERIAL_NM = "MaterialNm";
        public const string SUPPLIER_CD = "SupplierCd";
        public const string AMOUNT = "Amount";
        public const string UNIT_CD = "UnitCd";
        public const string SHAPE = "Shape";
        public const string SHADE = "Shade";
        public const string ANATOMY_KIT = "AnatomyKit";
        public const string CAD_OUTPUT_DONE = "CadOutputDone";
        public const string PRICE = "Price";
        public const string MATERIAL_PRICE = "MaterialPrice";
        public const string PROCESS_PRICE = "ProcessPrice";
        public const string MANUFACTURE_STAFF = "ManufactureStaff";
        public const string INSPECTION_STAFF = "InspectionStaff";


        public const string STAFF_CODE = "StaffCd";
        public const string STAFF_NM = "StaffNm";
        public const string DELIVERY_STATEMENT = "DeliveryStatementNo";
        public const string COMPLETE_DATE = "CompleteDate";

        public const string DELIVERED_DATE = "DeliveredDate";
        public const string BILL_STATEMENT_NO = "BillStatementNo";

    }
    public class TrnOrderHeaderColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnOrderHeader";

        public const string ORDER_NO = "OrderNo";
        public const string REF_ORDER_NO = "RefOrderNo";
        public const string TRIAL_ORDER_FLG = "TrialOrderFlg";
        public const string REMANUFACTURE_FLG = "RemanufactureFlg";
        public const string OFFICE_CODE = "OfficeCd";
        public const string STAFF_CODE = "StaffCd";
        public const string ORDER_DATE = "OrderDate";
        public const string DELIVER_DATE = "DeliverDate";
        public const string SET_DATE = "SetDate";
        public const string DENTAL_OFFICE_CD = "DentalOfficeCd";
        public const string DENTIST_NM = "DentistNm";
        public const string PATIENT_LAST_NM = "PatientLastNm";
        public const string PATIENT_FIRST_NM = "PatientFirstNm";
        public const string PATIENT_SEX = "PatientSex";
        public const string PATIENT_AGE = "PatientAge";

        public const string DELIVERY_STATEMENT = "DeliveryStatement";
        public const string BORROW_PARTS = "BorrowParts";
        public const string NOTE = "Note";
    }
    public class TrnOutsourceColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnOutsource";

        public const string OUTSOURCE_CD = "OutsourceCd";
        public const string OUTSOURCE_DATE = "OutsourceDate";
        public const string RECEIVE_ESTIMATE_DATE = "ReceiveEstimateDate";
        public const string RECEIVE_DATE = "ReceiveDate";
        public const string TECH_PRICE = "TechPrice";
        public const string PURCHASE_SEQ = "PurchaseSeq";

    }
    public class TrnPayColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnPay";

        public const string SUPPLIER_OUTSOURCE_CD = "SupplierOutsourceCd";
        public const string PURCHARSE_SEQ = "PurchaseSeq";
        public const string PAY_DATE = "PayDate";
        public const string PAY_PRICE = "PayPrice";
        public const string PAY_FEE = "PayFee";
        public const string BANK_CD = "BankCd";
    }
    public class TrnPayHistoryColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnPayHistory";
    }
    public class TrnProcessColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnProcess";

        public const string PROCESS_NO = "ProcessNo";
        public const string PROCESS_CD = "ProcessCd";
        public const string STAFF_CD = "StaffCd";
        public const string WORKTIME = "ProcessTime";
    }
    public class TrnPurchaseColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnPurchase";

        public const string OFFICE_CD = "OfficeCd";
        public const string SUPPLIER_OUTSOURCE_CD = "SupplierOutsourceCd";
        public const string PURCHASE_SEQ = "PurchaseSeq";
        public const string PURCHASE_DATE = "PurchaseDate";
        public const string PURCHASE_CATEGORY = "PurchaseCategory";
        public const string PURCHASE_ITEMS = "PurchaseItems";
        public const string PURCHASE_PRICE = "PurchasePrice";
        public const string REGULAR_PRICE = "RegularPrice";
        public const string PURCHASE_FEE = "PurchaseFee";
        public const string PAID_MONEY = "PaidMoney";
        public const string BALANCE = "Balance";
        public const string NOTE = "Note";
    }
    public class TrnStockInOutColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnStockInOut";

        public const string REGISTER_DATE = "RegisterDate";
        public const string IN_OUT_KBN = "InOutKbn";
        public const string SUPPLIER_CD = "SupplierCd";
        public const string OUTSOURCE_LAB_CD = "OutsourceLabCd";
        public const string MATERIAL_CD = "MaterialCd";
        public const string AMOUNT = "Amount";
        public const string UNIT_CD = "UnitCd";
        public const string PRICE = "Price";
        public const string SUM_PRICE = "SumPrice";
        public const string COMMENT = "Comment";
    }
    public class TrnTechPriceColumn : BaseColumn
    {
        public const string TABLE_NAME = "DENTAL_TrnTechPrice";

        public const string OFFICE_CD = "OfficeCd";
        public const string ORDER_SEQ = "OrderSeq";
        public const string DETAIL_SEQ = "DetailSeq";
        public const string TECH_DETAIL_NO = "TechDetailNo";
        public const string TECH_CD = "TechCd";
        public const string TECH_NM = "TechNm";
        public const string TECH_PRICE = "TechPrice";
    }  
    #endregion
}
