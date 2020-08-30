using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Common.Constant
{
    public static class UF
    {
        public const string AC = "AC";
        public const string AL = "AL";
        public const string AP = "AP";
        public const string AM = "AM";
        public const string BA = "BA";
        public const string CE = "CE";
        public const string ES = "ES";
        public const string GO = "GO";
        public const string MA = "MA";
        public const string MT = "MT";
        public const string MS = "MS";
        public const string MG = "MG";
        public const string PA = "PA";
        public const string PB = "PB";
        public const string PR = "PR";
        public const string PE = "PE";
        public const string PI = "PI";
        public const string RJ = "RJ";
        public const string RN = "RN";
        public const string RS = "RS";
        public const string RO = "RO";
        public const string RR = "RR";
        public const string SC = "SC";
        public const string SP = "SP";
        public const string SE = "SE";
        public const string TO = "TO";
        public const string DF = "DF";

        public static bool IsSudeste(string UF)
        {
            switch (UF)
            {
                case MG:
                case SP:
                case ES:
                case RJ:
                    return true;
                default:
                    return false;
            }
        }

        public static object[] GetUfsOrigem() =>
            new object[] { MG, SP };

        public static object[] GetUfsDestino() =>
            new object[] { RJ, PE, MG, PB, PR, PI, RO, SE, TO, SE, PA };
    }
}
