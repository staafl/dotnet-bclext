﻿
using Fairweather.Service;

namespace Common.Sage
{
    public class Sage_Fields_Nominal_Record { }
    class a { }
    public static partial class Sage_Fields
    {

        public static class InvoiceItem
        {
            // autogenerated: C:\Users\Fairweather\Desktop\sage_strings.pl
            public static readonly Sage_Field
              ADD_DISC_RATE = Double("ADD_DISC_RATE", "Additional Discount Percentage Rate", 8) // Opt
            , BASE_FULL_NET = Double("BASE_FULL_NET", "Full Net Value in Base Currency", 8) // Opt
            , BASE_NET = Double("BASE_NET", "Net Value in Base Currency", 8) // Opt
            , BASE_TAX = Double("BASE_TAX", "Tax Value in Base Currency", 8) // Opt
            , COMMENT_1 = String("COMMENT_1", "Comment 1", 60) // Opt
            , COMMENT_2 = String("COMMENT_2", "Comment 2", 60) // Opt
            , DELIVERY_DATE = Date("DELIVERY_DATE", "Delivery Date", 2) // Opt
            , DEPT_NUMBER = Short("DEPT_NUMBER", "Department Number", 2) // Opt
            , DESCRIPTION = String("DESCRIPTION", "Description", 60) // Opt
            , DISCOUNT_AMOUNT = Double("DISCOUNT_AMOUNT", "Discount Amount", 8) // Opt
            , DISCOUNT_RATE = Double("DISCOUNT_RATE", "Discount Percentage Rate", 8) // Opt
            , EXT_ORDER_REF = String("EXT_ORDER_REF", "External Reference for SOP to INV Link", 30) // Opt

            // , EXT_ORDER_REF_LINE = Integer("EXT_ORDER_REF_LINE", "External Line Reference for SOP to INV Link", 4) // Opt
            , FULL_NET_AMOUNT = Double("FULL_NET_AMOUNT", "Full Net Amount (Before Discount)", 8) // Opt
            , INVOICE_NUMBER = Int("INVOICE_NUMBER", "Invoice Number", 4) // Opt
            , ITEM_NUMBER = Short("ITEM_NUMBER", "Item Number", 2) // Opt
            , JOB_REFERENCE = String("JOB_REFERENCE", "Job Reference", 20) // Opt
            , NET_AMOUNT = Double("NET_AMOUNT", "Net Amount (After Discount)", 8) // Opt
            , NEXT_ITEM = Int("NEXT_ITEM", "Record Number of Next Item", 4) // Opt
            , NOMINAL_CODE = String("NOMINAL_CODE", "Nominal Code", 8) // Opt
            , OFFSET = Int("OFFSET", "Offset of Service Text", 4) // Opt
            , PREV_ITEM = Int("PREV_ITEM", "Record Number of Previous Item", 4) // Opt
            , QTY_ALLOCATED = Double("QTY_ALLOCATED", "Quantity Allocated for this Order", 8) // Opt
            , QTY_DELIVERED = Double("QTY_DELIVERED", "Quantity Previously Delivered for this Order", 8) // Opt
            , QTY_DESPATCH = Double("QTY_DESPATCH", "Quantity to Despatch/Despatched on this Delivery", 8) // Opt
            , QTY_INTRASTAT_CONFIRMED = Double("QTY_INTRASTAT_CONFIRMED", "Quantity Intrastat Confirmed", 8) // Opt
            , QTY_ORDER = Double("QTY_ORDER", "Quantity Ordered", 8) // Opt
            , SERVICE_FILE = String("SERVICE_FILE", "Redundant - Do Not Use", 60) // Opt
            , SERVICE_FILE_SIZE = Int("SERVICE_FILE_SIZE", "Redundant - Do Not Use", 4) // Opt
            , SERVICE_FLAG = SByte("SERVICE_FLAG", "Service Item Flag", 1) // Opt
            , SERVICE_ITEM_LINES = Int("SERVICE_ITEM_LINES", "Redundant - Do Not Use", 4) // Opt
            , STOCK_CODE = String("STOCK_CODE", "Stock Code", 30) // Req
            , TAX_AMOUNT = Double("TAX_AMOUNT", "Tax Amount", 8) // Opt
            , TAX_CODE = Short("TAX_CODE", "Tax Code", 2) // Opt
            , TAX_FLAG = SByte("TAX_FLAG", "", 1) // Opt
            , TAX_RATE = Double("TAX_RATE", "Tax Rate", 8) // Opt - 0..100.0
            , TEXT = String("TEXT", "Service Invoice/Credit Text", 60) // Opt
            , UNIT_OF_SALE = String("UNIT_OF_SALE", "Unit of Sale", 8) // Opt
            , UNIT_PRICE = Double("UNIT_PRICE", "Unit Price", 8) // Opt
            ;
            static public readonly Dict_ro<string, Sage_Field>
            Fields = Make(ADD_DISC_RATE
            , BASE_FULL_NET
            , BASE_NET
            , BASE_TAX
            , COMMENT_1
            , COMMENT_2
            , DELIVERY_DATE
            , DEPT_NUMBER
            , DESCRIPTION
            , DISCOUNT_AMOUNT
            , DISCOUNT_RATE
            , EXT_ORDER_REF
            //, EXT_ORDER_REF_LINE
            , FULL_NET_AMOUNT
            , INVOICE_NUMBER
            , ITEM_NUMBER
            , JOB_REFERENCE
            , NET_AMOUNT
            , NEXT_ITEM
            , NOMINAL_CODE
            , OFFSET
            , PREV_ITEM
            , QTY_ALLOCATED
            , QTY_DELIVERED
            , QTY_DESPATCH
            , QTY_INTRASTAT_CONFIRMED
            , QTY_ORDER
            , SERVICE_FILE
            , SERVICE_FILE_SIZE
            , SERVICE_FLAG
            , SERVICE_ITEM_LINES
            , STOCK_CODE
            , TAX_AMOUNT
            , TAX_CODE
            , TAX_FLAG
            , TAX_RATE
            , TEXT
            , UNIT_OF_SALE
            , UNIT_PRICE
            );


        }
        public static class InvoiceRecord
        {

            public static readonly Sage_Field
              ACCOUNT_REF = String("ACCOUNT_REF", "Account Reference", 8)
            , ADDRESS_1 = String("ADDRESS_1", "Customer Address 1", 60)
            , ADDRESS_2 = String("ADDRESS_2", "Customer Address 2", 60)
            , ADDRESS_3 = String("ADDRESS_3", "Customer Address 3", 60)
            , ADDRESS_4 = String("ADDRESS_4", "Customer Address 4", 60)
            , ADDRESS_5 = String("ADDRESS_5", "Customer Address 5", 60)
            , AMOUNT_PREPAID = Double("AMOUNT_PREPAID", "Amount Prepaid", 8)
            , BANK_CODE = String("BANK_CODE", "Bank Posting Code", 8)
            , BASE_TOT_NET = Double("BASE_TOT_NET", "Base Net Amount (Goods but not carriage)", 8)
            , BASE_TOT_TAX = Double("BASE_TOT_TAX", "Base Tax Amount (Goods but not carriage)", 8)
            , BASE_CARR_NET = Double("BASE_CARR_NET", "Base Carriage Net Amount", 8)
            , BASE_CARR_TAX = Double("BASE_CARR_TAX", "Base Carriage Tax Amount", 8)
            , BASE_SETT_DISC_RATE = Double("BASE_SETT_DISC_RATE", "Base Settlement Discount % Rate", 8)
            , BASE_AMOUNT_PAID = Double("BASE_AMOUNT_PAID", "Base Amount Prepaid", 8)
            , CARR_DEPT_NUMBER = Short("CARR_DEPT_NUMBER", "Carriage Department Number", 2)
            , CARR_NET = Double("CARR_NET", "Carriage Net Amount", 8)
            , CARR_NOM_CODE = String("CARR_NOM_CODE", "Carriage Nominal Code", 8)
            , CARR_TAX = Double("CARR_TAX", "Carriage Tax Amount", 8)
            , CARR_TAX_CODE = Short("CARR_TAX_CODE", "Carriage Tax Code", 2)
            , CONSIGNMENT_REF = String("CONSIGNMENT_REF", "Consignment Reference", 30)
            , CONTACT_NAME = String("CONTACT_NAME", "Customer Contact Name", 30)
            , COURIER = Short("COURIER", "Courier Number", 2)
            , CURRENCY = SByte("CURRENCY", "Currency", 1)
            , CURRENCY_USED = SByte("CURRENCY_USED", "Currency Used", 1)
            , CUST_DISC_RATE = Double("CUST_DISC_RATE", "Customer Discount", 8)
            , CUST_ORDER_NUMBER = String("CUST_ORDER_NUMBER", "Customer's Order Number", 30)
            , CUST_TEL_NUMBER = String("CUST_TEL_NUMBER", "Customer's Telephone Number", 30)
            , DEL_ADDRESS_1 = String("DEL_ADDRESS_1", "Delivery Address Line 1", 60)
            , DEL_ADDRESS_2 = String("DEL_ADDRESS_2", "Delivery Address Line 2", 60)
            , DEL_ADDRESS_3 = String("DEL_ADDRESS_3", "Delivery Address Line 3", 60)
            , DEL_ADDRESS_4 = String("DEL_ADDRESS_4", "Delivery Address Line 4", 60)
            , DEL_ADDRESS_5 = String("DEL_ADDRESS_5", "Delivery Address Line 5", 60)
            , DELETED_FLAG = Short("DELETED_FLAG", "Is Deleted", 2)
            , DELIVERY_NAME = String("DELIVERY_NAME", "Delivery Address Name", 60)
            , DISCOUNT_TYPE = SByte("DISCOUNT_TYPE", "Discount Type", 1)
            , EURO_GROSS = Double("EURO_GROSS", "Euro Gross Amount", 8)
            , EURO_RATE = Double("EURO_RATE", "Euro Rate", 8)
            , EXTERNAL_USAGE = Int("EXTERNAL_USAGE", "Number of External Usages", 4)
            , FIRST_ITEM = Int("FIRST_ITEM", "Record Number of First Invoice Item", 4)
            , FOREIGN_GROSS = Double("FOREIGN_GROSS", "Foreign Gross Amount", 8)
            , FOREIGN_RATE = Double("FOREIGN_RATE", "Foreign Rate for Euro Currency", 8)
            , GLOBAL_DEPT_NUMBER = Short("GLOBAL_DEPT_NUMBER", "Global Department Number", 2)
            , GLOBAL_DETAILS = String("GLOBAL_DETAILS", "Global Details", 60)
            , GLOBAL_NOM_CODE = String("GLOBAL_NOM_CODE", "Global Nominal Code", 8)
            , GLOBAL_TAX_CODE = Short("GLOBAL_TAX_CODE", "Global Tax Code", 2)
            , INVOICE_DATE = Date("INVOICE_DATE", "Invoice Date", 2)
            , INVOICE_NUMBER = Int("INVOICE_NUMBER", "Invoice Number", 4)
            , INVOICE_TYPE_CODE = SByte("INVOICE_TYPE_CODE", "Invoice Type Code", 1)
            , ITEMS_NET = Double("ITEMS_NET", "Net Amount (Goods but not carriage)", 8)
            , ITEMS_TAX = Double("ITEMS_TAX", "Tax Amount (Goods but not carriage)", 8)
            , NAME = String("NAME", "Customer Account Name", 60)
            , NOTES_1 = String("NOTES_1", "Notes 1", 60)
            , NOTES_2 = String("NOTES_2", "Notes 2", 60)
            , NOTES_3 = String("NOTES_3", "Notes 3", 60)
            , ORDER_NUMBER = String("ORDER_NUMBER", "Corresponding Order Number", 7)
            , PAYMENT_REF = String("PAYMENT_REF", "Payment Reference", 8)
            , PAYMENT_TYPE = SByte("PAYMENT_TYPE", "Payment Type (SR/SA)", 1)
            , POSTED_CODE = SByte("POSTED_CODE", "Posted Flag - Yes/No", 1)
            , PRINTED_CODE = SByte("PRINTED_CODE", "Printed Flag - Yes/No", 1)
            , SETTLEMENT_DISC_RATE = Double("SETTLEMENT_DISC_RATE", "Settlement Discount % Rate", 8)
            , SETTLEMENT_DUE_DAYS = Short("SETTLEMENT_DUE_DAYS", "Settlement Days", 2)
            , STATUS = SByte("STATUS", "Order Status Code", 1)
            , TAKEN_BY = String("TAKEN_BY", "Order Taken By", 60)
            , TOTAL_BYTES = Int("TOTAL_BYTES", "", 4);


            static public readonly Dict_ro<string, Sage_Field>
            Fields = Make(
               ACCOUNT_REF
             , ADDRESS_1
             , ADDRESS_2
             , ADDRESS_3
             , ADDRESS_4
             , ADDRESS_5
             , AMOUNT_PREPAID
             , BANK_CODE
             , BASE_TOT_NET
             , BASE_TOT_TAX
             , BASE_CARR_NET
             , BASE_CARR_TAX
             , BASE_SETT_DISC_RATE
             , BASE_AMOUNT_PAID
             , CARR_DEPT_NUMBER
             , CARR_NET
             , CARR_NOM_CODE
             , CARR_TAX
             , CARR_TAX_CODE
             , CONSIGNMENT_REF
             , CONTACT_NAME
             , COURIER
             , CURRENCY
             , CURRENCY_USED
             , CUST_DISC_RATE
             , CUST_ORDER_NUMBER
             , CUST_TEL_NUMBER
             , DEL_ADDRESS_1
             , DEL_ADDRESS_2
             , DEL_ADDRESS_3
             , DEL_ADDRESS_4
             , DEL_ADDRESS_5
             , DELETED_FLAG
             , DELIVERY_NAME
             , DISCOUNT_TYPE
             , EURO_GROSS
             , EURO_RATE
             , EXTERNAL_USAGE
             , FIRST_ITEM
             , FOREIGN_GROSS
             , FOREIGN_RATE
             , GLOBAL_DEPT_NUMBER
             , GLOBAL_DETAILS
             , GLOBAL_NOM_CODE
             , GLOBAL_TAX_CODE
             , INVOICE_DATE
             , INVOICE_NUMBER
             , INVOICE_TYPE_CODE
             , ITEMS_NET
             , ITEMS_TAX
             , NAME
             , NOTES_1
             , NOTES_2
             , NOTES_3
             , ORDER_NUMBER
             , PAYMENT_REF
             , PAYMENT_TYPE
             , POSTED_CODE
             , PRINTED_CODE
             , SETTLEMENT_DISC_RATE
             , SETTLEMENT_DUE_DAYS
             , STATUS
             , TAKEN_BY
             , TOTAL_BYTES);

        }
    }
}