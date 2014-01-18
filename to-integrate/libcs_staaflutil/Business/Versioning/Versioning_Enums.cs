
namespace Versioning
{
    public enum Sage_Connection_Error
    {
        Unspecified,

        Username_In_Use_Generic,
        Exclusive_Mode,
        Logon_Count_Exceeded,
        Invalid_Credentials,
        Invalid_Version,
        Folder_Does_Not_Exist,
        Invalid_Folder,
        SDO_Not_Registered,
        Unsupported_Version,

        Username_In_Use_Cant_Remove,
    }

    public enum Sage_Error
    {
        sdoNone = 0,
        sdoInvalidArg = 1,
        sdoArchiveException = 2,
        sdoException = 3,
        sdoDaoException = 4,
        sdoDBException = 5,
        sdoFileException = 6,
        sdoMemoryException = 7,
        sdoNotSupportedException = 8,
        sdoOleException = 9,
        sdoOleDispatchException = 10,
        sdoResourceException = 11,
        sdoUserException = 12,
        sdoUnknownException = 13,
        /// The mode specified for the Open method is invalid. 
        sdoInvalidOpenMode = 14,
        /// The Object Name you are specifying is invalid. 
        sdoInvalidName = 15,
        /// You are trying to create an object without a valid connection to the Line 50 data. 
        sdoNotConnected = 16,
        sdoInvalidKey = 17,
        sdoInvalidValue = 18,
        sdoInvalidUse = 19,
        sdoInvalidType = 20,
        sdoLogonFail = 21,
        /// The Password used in the connection is invalid. 
        sdoLogonPassword = 22,
        /// The Logon Name used in the connection is invalid. 
        sdoLogonNameInUse = 23,
        /// Line 50 is in exclusive mode.  This would occur, for example, if Disk Doctor is in use. 
        sdoLogonExclusive = 24,
        /// The data path used in the connection is invalid. 
        sdoBadDataPath = 25,
        /// The Line 50 data is a different version to the SDO. 
        sdoWrongVersion = 26,
        /// The Update method has been called without previously having called the AddNew or Edit methods. 
        sdoBadUpdateMode = 27,
        /// The record cannot be deleted as the account has transactions associated with it. 
        sdoDeleteFailedHasTrans = 28,
        /// The record cannot be deleted for a reason other than having transactions associated i.e. there is a balance on the account. 
        sdoDeleteFailed = 29,
        /// The connection would cause the user count to be exceeded. 
        sdoLogonUserCountExceeded = 30,
        /// The SDO has not been registered on this machine. 
        sdoNotRegistered = 31,
        /// The object you are trying to update is read only - this applies to the AccessData object. 
        sdoReadOnlyAccess = 32,
        /// The Foreign Trader Wizard has not yet been completed - please run Line 50 and complete the Foreign Trader Wizard. 
        sdoForTraNotEnabled = 33,
        /// The Foreign Currency on the account does not match the Foreign Currency on the transaction. 
        sdoCurrencyDoesNotMatch = 34,
        /// You are trying to post a BASE transaction with a Foreign Currency Rate - please clear the Foreign Currency Rate. 
        sdoForeignRateSetOnBase = 35,
        /// Attempting to save an existing object having failed to Load. 
        sdoInvalidSave = 36,
        /// Project status set to not allow postings. 
        sdoInvalidProjectStatus = 37,
        /// Invalid Project. 
        sdoInvalidProject = 38,
        /// Maximum number of project cost codes is 100. 
        sdoMaxCostCodes = 39,
    }

    public enum TransType : sbyte
    {
        sdoNone = 0, // our invention
        sdoSI = 1,
        sdoSC = 2,
        sdoSR = 3,
        sdoSA = 4,
        sdoSD = 5,
        sdoPI = 6,
        sdoPC = 7,
        sdoPP = 8,
        sdoPA = 9,
        sdoPD = 10,
        sdoBP = 11,
        sdoBR = 12,
        sdoCP = 13,
        sdoCR = 14,
        sdoJD = 15,
        sdoJC = 16,
        sdoVP = 17,
        sdoVR = 18,
        sdoCC = 19,
        sdoCD = 20,
        sdoPAI = 21,
        sdoPAO = 22,
        sdoCO = 23,
        sdoSP = 24,
        sdoPR = 25,
        ourInvalid = sdoNone,
        //// ! our own inventions, not part of the sdo
        //ourXR = 0x70,
        //ourXP,
    }
    public enum BankType : sbyte
    {
        /// <summary>
        /// BP & BR
        /// </summary>
        sdoTypeCheque = 1,
        /// <summary>
        /// CP & CR
        /// </summary>
        sdoTypeCash = 2,
        /// <summary>
        /// VP & VR
        /// </summary>
        sdoTypeCredit = 4,
    }
    public enum NominalType : sbyte
    {
        sdoTypeNormal = 1,
        sdoTypeBank = 2,
        sdoTypeControl = 4,
    }
    public enum ItemType : sbyte
    {
        sdoStockItem = 0,
        sdoNonStockItem = 1,
        sdoServiceItem = 2,
    }
    public enum OpenMode
    {
        sdoRead = 0,
        sdoShareCompat = 0,
        sdoReadWrite = 1,
        sdoWrite = 2,
        sdoShareExclusive = 16,
        sdoShareDenyWrite = 32,
        sdoShareDenyRead = 48,
        sdoShareDenyNone = 64,
        sdoDelete = 512,
        sdoCreate = 4096,
        sdoExist = 16384,
    }
    public enum InvoiceType : sbyte
    {
        sdoProductInvoice = 0,
        sdoSopInvoice = 1,
        sdoServiceInvoice = 2,
        sdoProductCredit = 3,
        sdoServiceCredit = 4,
        sdoProductProforma = 5,
        sdoProductQuotation = 6,
        sdoServiceProforma = 7,
        sdoServiceQuotation = 8,
        sdoSopQuote = 9,

        /// <summary>
        /// Not in sage 11
        /// </summary>
        sdoPopQuote = 10,
        sdoSopProforma = 11,

        /// <summary>
        /// Not in sage 11
        /// </summary>
        sdoPopProforma = 12,
    }

    // Verified for consistency
    public enum LedgerType
    {
        sdoLedgerSales = 256,
        sdoLedgerPurchase = 257,
        sdoLedgerNominal = 258,
        sdoLedgerStockAdj = 259,
        sdoLedgerBank = 260,
        sdoLedgerInvoice = 261,
        sdoLedgerCheck = 262,
        sdoLedgerRepGen = 263,
        sdoLedgerService = 264,
        sdoLedgerCredit = 265,
        sdoLedgerCashBook = 266,
        sdoLedgerSop = 267,
        sdoLedgerPop = 268,
        sdoLedgerStock = 269,
        sdoLedgerDoctor = 270,
        sdoLedgerTranRec = 271,
        sdoLedgerBills = 272,
        sdoLedgerRecur = 273,
        sdoLedgerSetup = 274,
        sdoLedgerControl = 275,
        sdoLedgerInsurance = 276,
        sdoLedgerInvestment = 277,
        sdoledgerDepreciation = 278,
        sdoLedgerServiceCredit = 279,
        sdoLedgerNone = 280,
        sdoLedgerBatchSI = 281,
        sdoLedgerBatchSC = 282,
        sdoLedgerBatchPI = 283,
        sdoLedgerBatchPC = 284,
        sdoLedgerAsset = 285,
        sdoLedgerContact = 286,
        sdoLedgerAdjustment = 287,
        sdoLedgerFinance = 288,
        sdoLedgerAssist = 289,
        sdoLedgerAccStatus = 290,
        sdoLedgerRemittance = 291,
        sdoLedgerPriceList = 292,
        sdoLedgerPrice = 293,
        sdoLedgerCompanyDelivery = 294,
        sdoLedgerSalesDelivery = 295,
        sdoLedgerPurchaseDelivery = 296,
    }

    public enum StockTransType : sbyte
    {
        sdoAI = SageDataObject110.StockTransType.sdoAI,
        sdoAO = SageDataObject110.StockTransType.sdoAO,
        sdoGI = SageDataObject110.StockTransType.sdoGI,
        sdoGO = SageDataObject110.StockTransType.sdoGO,
        sdoMI = SageDataObject110.StockTransType.sdoMI,
        sdoMO = SageDataObject110.StockTransType.sdoMO,
        sdoGR = SageDataObject110.StockTransType.sdoGR,
        sdoDI = SageDataObject110.StockTransType.sdoDI,
        sdoDO = SageDataObject110.StockTransType.sdoDO,
        sdoWO = SageDataObject110.StockTransType.sdoWO,

    }
    /*       Verified for consistency        */
    public enum InvoiceItemType : sbyte
    {
        // {[a-z]+} =.*
        // \1 = SageDataObject110.InvoiceItemType.\1,

        sdoItemStock = SageDataObject110.InvoiceItemType.sdoItemStock,
        sdoItemService = SageDataObject110.InvoiceItemType.sdoItemService,
        sdoItemQtyService = SageDataObject110.InvoiceItemType.sdoItemQtyService,
    }
    public enum ControlTypes : sbyte
    {
        //{[a-z]+} = {:z+}
        //\1 = SageDataObject110.ControlTypes.\1, //\2

        sdoNtNormal = SageDataObject110.ControlTypes.sdoNtNormal, //0,
        sdoNtBank = SageDataObject110.ControlTypes.sdoNtBank, //1,
        sdoNtDebtors = SageDataObject110.ControlTypes.sdoNtDebtors, //2,
        sdoNtCreditors = SageDataObject110.ControlTypes.sdoNtCreditors, //3,
        sdoNtDefaultBank = SageDataObject110.ControlTypes.sdoNtDefaultBank, //4,
        sdoNtSalesTax = SageDataObject110.ControlTypes.sdoNtSalesTax, //5,
        sdoNtPurchaseTax = SageDataObject110.ControlTypes.sdoNtPurchaseTax, //6,
        sdoNtSalesDiscount = SageDataObject110.ControlTypes.sdoNtSalesDiscount, //7,
        sdoNtPurchaseDiscount = SageDataObject110.ControlTypes.sdoNtPurchaseDiscount, //8,
        sdoNtRetainedEarnings = SageDataObject110.ControlTypes.sdoNtRetainedEarnings, //9,
        sdoNtDefaultSales = SageDataObject110.ControlTypes.sdoNtDefaultSales, //10,
        sdoNtAccruals = SageDataObject110.ControlTypes.sdoNtAccruals, //11,
        sdoNtPrepayments = SageDataObject110.ControlTypes.sdoNtPrepayments, //12,
        sdoNtBadDebts = SageDataObject110.ControlTypes.sdoNtBadDebts, //13,
        sdoNtMispostings = SageDataObject110.ControlTypes.sdoNtMispostings, //14,
        sdoNtSuspense = SageDataObject110.ControlTypes.sdoNtSuspense, //15,
        sdoNtDefFinance = SageDataObject110.ControlTypes.sdoNtDefFinance, //16,
    }
}
