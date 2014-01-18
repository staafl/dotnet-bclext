using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using System.Linq;
using Common.Sage;
using TransType = Versioning.TransType;
namespace Common.Posting
{
    public abstract class Rule_Base : IRule
    {
        public virtual void Clear() {
        }

        public abstract bool
        Inspect_Line(Data_Line line,
                     int ix,
                     bool is_last,
                out bool skip,
                out IEnumerable<Validation_Error> errors
                );

        public virtual bool
        Are_Valid(IEnumerable<Data_Line> lines) {

            bool _1;
            IEnumerable<Validation_Error> _2;

            foreach (var pair in lines.Ixed().Mark_Last())
                if (!Inspect_Line(pair.First.Second, pair.First.First, pair.Second, out _1, out _2))
                    return false;

            return true;
        }

        #region consts
        public const string STR_INVOICE_TYPE_CODE = "INVOICE_TYPE_CODE";

        public const string STR_ACCOUNT_REF = "ACCOUNT_REF";
        public const string STR_NOMINAL_CODE = "NOMINAL_CODE";
        public const string STR_BANK_CODE = "BANK_CODE";
        public const string STR_PROJECT_REF = "PROJECT_REF";
        public const string STR_COST_CODE = "COST_CODE";
        public const string STR_TYPE = "TYPE";

        public const TransType
        SI = TransType.sdoSI,
        SC = TransType.sdoSC,
        SR = TransType.sdoSR,
        SA = TransType.sdoSA,
        SD = TransType.sdoSD,
        PI = TransType.sdoPI,
        PC = TransType.sdoPC,
        PP = TransType.sdoPP,
        PA = TransType.sdoPA,
        PD = TransType.sdoPD,
        BP = TransType.sdoBP,
        BR = TransType.sdoBR,
        CP = TransType.sdoCP,
        CR = TransType.sdoCR,
        JD = TransType.sdoJD,
        JC = TransType.sdoJC,
        VP = TransType.sdoVP,
        VR = TransType.sdoVR,
        CC = TransType.sdoCC,
        CD = TransType.sdoCD,
        PAI = TransType.sdoPAI,
        PAO = TransType.sdoPAO,
        CO = TransType.sdoCO,
        SP = TransType.sdoSP,
        PR = TransType.sdoPR
            //,
            //XR = TransType.ourXR,
            //XP = TransType.ourXP
        ;
        #endregion

    }
}
