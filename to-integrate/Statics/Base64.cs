using System;
using System.Collections.Generic;
using System.Linq;


// http://en.wikipedia.org/wiki/Base64
// todo: choose standard from table
// todo: what about line separators?
namespace staafl 
{
    /// <summary>
    /// Converts to and from Base64
    /// <summary>
    // todo: let them choose 62/63 chars and padding options
    public class Base64
    {

        // Base64 is a bytes-to-bytes encoding, not a characters-to-codepoints or codepoints-to-bytes encoding
        
        static char Base64Char(byte byte) {
            if(byte < 26) {
                return (char)(byte + 'A');
            }
            byte -= 26;
            if(byte < 26) {
                return (char)(byte + 'a');
            }
            byte -= 26;
            if(byte < 10) {
                return (char)(byte + '0');
            }
            byte -= 10;
            switch(byte) {
                case 0: return '+';
                case 1: return '/';
                // fixme: use std exception
                default: throw new ArgumentException("byte="+byte);
            }
        }
        
        public static string ToUTF8Base64(string input) {
            
            if(string.IsNullOrEmpty(input)) {
                return input;
            }
            var all_bytes = Encoding.UTF8.GetBytes(input);
            var ret = new StringBuilder(Arithmetic.DivisionPad(all_bytes.length * 4, 3) / 3);
            
            foreach(var triple in all_bytes.PadRightToDivisibleBy(3).Chunks(3)) {
            
                int _24_bits = (triple.Item1 << 16) | (triple.Item2 << 8) | triple.Item3;
                var c1 = Base64Char((_24_bits >> 18) & 63);
                var c2 = Base64Char((_24_bits >> 12) & 63);
                var c3 = Base64Char(((_24_bits >> 6) & 63);
                var c4 = Base64Char(((_24_bits >> 0) & 63);
                
                ret.AppendChar(c1);
                ret.AppendChar(c2);
                ret.AppendChar(c3);
                ret.AppendChar(c4);
            }
            
            return ret.ToString();
            
        }
        
        public static string FromUTF8Base64(string input) {
            if(string.IsNullOrEmpty(input)) {
                return input;
            }
            var list = new List<Byte>(Arithmetic.DivisionPad(input.length * 3, 4) / 4);
            
            foreach(var triple in input.Chunks(4)) {
            
                long _24_bits = triple.Item1 << 18 | triple.Item2 << 12 | triple.Item3 << 6 | triple.Item4;
                var b1 = (_24_bits >> 16) & 255;
                var b2 = (_24_bits >> 8) & 255;
                var b3 = (_24_bits >> 0) & 255;
                
                list.Add(b1);
                list.Add(b2);
                list.Add(b3);
                
                
            }
            
            // FIXME: is this right
            // remove padding \0 chars?
            
            while(list.Length > 0 && list[list.Length - 1] == 0) {
                list.RemoveAt(list.Length - 1);
            }
            
            var ret = Encoding.UTF8.FromBytes(list);
            return ret;
            
        }
        
        
    }
}






























