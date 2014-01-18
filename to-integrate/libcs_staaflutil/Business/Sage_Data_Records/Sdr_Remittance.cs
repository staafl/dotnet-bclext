using System;
using System.Collections.Generic;
using System.Linq;
using Fairweather.Service;
using Versioning;

namespace Common
{
    partial class Sage_Logic
    {
        void Remittance(Screen_Type type,
                Postings postings,
                int? payment_no,
                int? discount_no,
                int? pa_no) {

            using (var remitter = new Remitter(this, postings, payment_no, discount_no, pa_no)) {

                if (payment_no.HasValue) {

                    remitter.Remit_Invoices_And_Credit(Settings.Remittance_By_Number);

                    if (discount_no.HasValue)
                        remitter.Remit_PD();


                    remitter.Remit_PP();

                }

                if (pa_no.HasValue )
                    remitter.Remit_PA();

            }



        }




        void Remit_By_Number(Postings postings, ref int index, ref int record_num) {

            throw new NotImplementedException();

        }

        class Remitter : IDisposable
        {
            public Remitter(Sage_Logic sdr,
                            Postings postings,
                            int? payment_no,
                            int? discount_no,
                            int? pa_no) {

                this.postings = postings;
                this.sdr = sdr;
                this.payment_no = payment_no;
                this.discount_no = discount_no;
                this.pa_no = pa_no;

                ws = sdr.Conn.ws;


                // ****************************

                // Todo - 0 or 1?
                index = 0;

                // ****************************



                var oSetupData = ws.Create<SetupData>();
                var oRemittanceIndex = ws.Create<RemittanceIndex>();

                urn = (oSetupData[LAST_CHEQUE].ToInt32() + 1);

                record_num = oRemittanceIndex.Count + 1;



                remittance_fields = Get_Remittance_Data();


                // oRemittanceIndex.MoveLast();
                // RemittanceIndex[RECORD_NUMBER].ToInt32();
            }

            void IDisposable.Dispose() {
                Finish_Remittance();
            }

            // ****************************

            readonly int? discount_no;
            readonly int? pa_no;
            readonly int? payment_no;

            readonly Work_Space ws;
            readonly Postings postings;
            readonly Sage_Logic sdr;
            readonly int urn;
            readonly Dictionary<string, object> remittance_fields;

            int index;
            int record_num;

            bool finished = false;

            // ****************************

            public bool Finish_Remittance() {

                if (finished)
                    return true;

                finished = true;

                var oSetupData = ws.Create<SetupData>();

                oSetupData[LAST_CHEQUE] = urn;

                Try(oSetupData.Write(1));

                return true;

            }


            const string PI = "PI";
            const string PA = "PA";
            const string PP = "PP";
            const string PC = "PC";
            const string PD = "PD";


            // const string T9 = "T9";
            const short T9 = 9;


            public void Remit_Invoices_And_Credit(bool by_number) {

                if (by_number) {

                    Remit_Invoices_Credit_By_Number();

                }
                else {
                    Remit_Invoices();
                    Remit_Credits();
                }
            }

            /// <summary>
            /// Remits the optional Payment on Account which balances the cheque
            /// </summary>
            public void Remit_PA() {

                var amount = postings.Sa_Amount;
                var date = DateTime.Today;

                var dict = new Dictionary<string, object>{
                    {BANK,postings.Bank_Ref},
                    {DATE,postings.Date},
                    {DETAILS,postings.Sa_Details},
                    {GROSS_AMOUNT,0.0},
                    {LASTLINE_FLAG,0},
                    {NET_AMOUNT,amount},
                    {PAYMENT_AMOUNT,-amount},
                    {PAYMENT_DATE,date},
                    {PAYMENT_FLAG,1},
                    {PAYMENT_NO,pa_no.Value},
                    {TAX_CODE,T9},
                    {TRAN_TYPE,PA},
                    {VAT_AMOUNT,0.0},
                };

                Remit(dict);
            }

            /// <summary>
            /// Remits the purchase payment (ie the Receipt)
            /// </summary>
            public void Remit_PP() {

                // var amount = postings.Total_Receipts; // TODO

                var amount = postings.Cheque_Amount;
                var date = DateTime.Today;

                var dict = new Dictionary<string, object>{
                    {BANK,postings.Bank_Ref},
                    {DETAILS,postings.Receipt_Details},
                    {GROSS_AMOUNT,0.0},
                    {LASTLINE_FLAG,0},
                    {NET_AMOUNT,amount},
                    {PAYMENT_AMOUNT,-amount},
                    {PAYMENT_DATE,date},
                    {PAYMENT_FLAG,1},
                    {PAYMENT_NO,payment_no.Value},
                    {TAX_CODE,T9},
                    {TRAN_TYPE,PP},
                    {VAT_AMOUNT,0.0},};


                Remit(dict);

            }

            /// <summary>
            /// Remits the Discount transaction
            /// </summary>
            public void Remit_PD() {


                var date1 = DateTime.Today;
                var date2 = DateTime.Today;
                var amount = postings.Total_Discounts;

                var dict = new Dictionary<string, object> {
                    {BANK, postings.Bank_Ref},
                    {DETAILS, postings.Discount_Details},
                    {DATE, date1},
                    {GROSS_AMOUNT, 0.0},
                    {LASTLINE_FLAG, 0},
                    {NET_AMOUNT, 0},
                    {PAYMENT_AMOUNT, amount},
                    {PAYMENT_DATE, date2},
                    {PAYMENT_FLAG, 0},
                    {PAYMENT_NO, discount_no.Value},
                    {TRAN_TYPE, PD},
                    {VAT_AMOUNT, 0.0},};

                Remit(dict);

            }


            // ****************************

            //void Remit_Invoices_Credit_By_Number() {

            //    var usage_alloc = postings.Usage_Allocations;
            //    var receipt_alloc = postings.Receipt_Allocations;


            //    var usages = usage_alloc.Triples(_ => true, cad => cad.Source.Value);
            //    var receipts = receipt_alloc.Triples(_ => false, cad => cad.Target);

            //    var to_remit = usages.Concat(receipts).lst();

            //    // Sort by relevant transaction number
            //    to_remit.Sort(triple => triple.Third);

            //    foreach (var triple in to_remit) {

            //        var cad = triple.First;
            //        var is_credit = triple.Second;
            //        var unique = triple.Third;

            //        //(amount > 0.0).tiff();

            //        if (is_credit)
            //            Remit_Credit(unique, cad.Amount, cad.Type);
            //        else
            //            Remit_Invoice(unique, cad.Amount);

            //    }

            //}

            void Remit_Invoices_Credit_By_Number() {

                var usage_alloc = postings.Usage_Allocations;
                var invoice_alloc = Receipts_And_Discounts_By_Target();


                var usage_data = usage_alloc.Select(cad =>
                    Triple.Make(cad.Source.Value,
                                (Allocation_Type?)cad.Type,
                                cad.Amount));

                var invoice_data = invoice_alloc.Select(pair =>
                    Triple.Make(pair.First,
                                (Allocation_Type?)null,
                                pair.Second));

                var to_remit = usage_data.Concat(invoice_data).lst();

                // Sort by relevant transaction number
                to_remit.Sort(triple => triple.First);

                foreach (var triple in to_remit) {

                    var unique = triple.First;
                    var null_type = triple.Second;
                    var amount = triple.Third;

                    var is_credit = null_type.HasValue;

                    (amount > 0.0m).tiff();

                    if (is_credit)
                        Remit_Credit(unique, amount, null_type.Value);
                    else
                        Remit_Invoice(unique, amount);

                }

            }

            IEnumerable<Pair<int, decimal>>
            Receipts_And_Discounts_By_Target() {

                var receipt_alloc = postings.Receipt_Allocations;
                var discount_alloc = postings.Discount_Allocations;


                // (Credit Reference, Amount)
                var ret = from cad in receipt_alloc.Concat(discount_alloc)
                          group cad by cad.Target
                              into g
                              select Pair.Make(g.Key, g.Sum(cad => cad.Amount));


                return ret;

            }

            void Remit_Credits() {

                var usages = postings.Usage_Allocations;

                foreach (var cad in usages)
                    Remit_Credit(cad.Source.Value, cad.Amount, cad.Type);

            }

            void Remit_Invoices() {

                var pairs = Receipts_And_Discounts_By_Target();

                foreach (var pair in pairs)
                    Remit_Invoice(pair.First, pair.Second);

            }

            // ****************************


            void Remit_Credit(int source, decimal amount, Allocation_Type type) {

                (amount > 0.0m).tiff();

                var str_type = types[type];

                SplitData split;
                HeaderData header;

                Get_Header_And_Split(source, out header, out split);

                var dict = new Dictionary<string, object> {
                        {BANK, postings.Bank_Ref},
                        {DATE, split[DATE]},
                        {DETAILS, split[DETAILS]},
                        {GROSS_AMOUNT, split[NET_AMOUNT]},
                        {INV_REF, header[INV_REF]},
                        {LASTLINE_FLAG, 0},
                        {NET_AMOUNT, split[NET_AMOUNT]}, // or 0?
                        {PAYMENT_AMOUNT, amount},
                        {PAYMENT_DATE, DateTime.Today},
                        {PAYMENT_FLAG, 0},
                        {PAYMENT_NO, source},
                        //{TAX_CODE, STR_Na}, TODO
                        {TRAN_TYPE, str_type},
                        {VAT_AMOUNT, split[TAX_AMOUNT]},};


                Remit(dict);

            }

            void Remit_Invoice(int target, decimal amount) {

                SplitData split;
                HeaderData header;

                Get_Header_And_Split(target, out header, out split);

                var payment_date = DateTime.Today;

                var dict = new Dictionary<string, object> {
                        {BANK, postings.Bank_Ref},
                        {DATE, split[DATE]},
                        {DETAILS, split[DETAILS]},
                        {GROSS_AMOUNT, split[NET_AMOUNT]},
                        {INV_REF, header[INV_REF]},
                        {LASTLINE_FLAG, 0},
                        {NET_AMOUNT, split[NET_AMOUNT]}, // or 0?
                        {PAYMENT_AMOUNT, -amount}, // Note well
                        {PAYMENT_DATE, payment_date},
                        {PAYMENT_FLAG, 0},
                        {PAYMENT_NO, target},
                        //{TAX_CODE, STR_Na}, TODO
                        {TRAN_TYPE, PI},
                        {VAT_AMOUNT, split[TAX_AMOUNT]},};


                Remit(dict);

            }


            Dictionary<Allocation_Type, string>
            types = new Dictionary<Allocation_Type, string>
            {{Allocation_Type.SC_To_SI,PC},
            {Allocation_Type.SA_To_SI,PA},
            {Allocation_Type.SR_To_SI,PP},
            {Allocation_Type.SD_To_SI,PI},};


            // ****************************


            void Remit(Dictionary<string, object> data) {

                finished.tift<InvalidOperationException>();

                var oRemittanceData = ws.Create<RemittanceData>();

                oRemittanceData.Fill(remittance_fields);
                oRemittanceData.Fill(data);

                // TODO: Verify!
                oRemittanceData[INDEX] = Pad_Index(index);
                oRemittanceData[URN] = Pad_Urn(urn);

                Try(oRemittanceData.Write(record_num));

                Index(urn, record_num);

                ++record_num;
                ++index;

            }

            void Index(int urn, int record_num) {

                finished.tift<InvalidOperationException>();

                //var oRemittanceIndex = ws.Create<RemittanceIndex>();
                //Try(oRemittanceIndex.Open(OpenMode.sdoReadWrite));

                ////Try(RemittanceIndex.Add());
                //oRemittanceIndex[RECORD_NUMBER] = (short)record_num;
                //oRemittanceIndex[URN] = css_URN;

                //Try(oRemittanceIndex.Write(record_num));

                var oRemittanceIndex = ws.Create<RemittanceIndex>();

                oRemittanceIndex[RECORD_NUMBER] = record_num;
                oRemittanceIndex[URN] = Pad_Urn(urn);

                Try(oRemittanceIndex.Write(record_num));

            }

            void Try(bool cond) {
                sdr.Try(cond);
            }


            Dictionary<string, object>
            Get_Remittance_Data() {

                var oPurchaseRecord = ws.Create<PurchaseRecord>();

                var account_ref = postings.Account_Ref;

                oPurchaseRecord[ACCOUNT_REF] = account_ref;
                oPurchaseRecord.Find(false).tiff();

                var ret = new Dictionary<string, object>() { 
                       {ACCOUNT_REF, account_ref},
                       {ADDRESS_1, oPurchaseRecord[ADDRESS_1]},
                       {ADDRESS_2, oPurchaseRecord[ADDRESS_2]},
                       {ADDRESS_3, oPurchaseRecord[ADDRESS_3]},
                       {ADDRESS_4, oPurchaseRecord[ADDRESS_4]},
                       {ADDRESS_5, oPurchaseRecord[ADDRESS_5]},
                       {FAX, oPurchaseRecord[FAX]},
                       {NAME, oPurchaseRecord[NAME]},
                       {PRINTED_FLAG, 0},
                       {TELEPHONE, oPurchaseRecord[TELEPHONE]},                    
                       {URN, Pad_Urn(urn)},
                    };

                return ret;
            }

            void Get_Header_And_Split(int usage_ref,
                                      out HeaderData HeaderData,
                                      out SplitData SplitData) {

                SplitData = ws.Create<SplitData>();

                SplitData.Read(usage_ref).tiff();

                HeaderData = ws.Create<HeaderData>();

                int header = SplitData[HEADER_NUMBER].ToInt32();

                HeaderData.Read(header).tiff();

            }


            static string Pad_Urn(int urn) {

                // TODO: pad to 7 or 8?
                return urn.ToString().PadLeft(7, '0');

            }

            static string Pad_Index(int index) {

                // TODO: pad to 7 or 8?
                return index.ToString().PadLeft(8, '0');


            }





        }





        // ****************************










    }
}
