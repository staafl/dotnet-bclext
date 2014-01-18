namespace Fairweather.Service
{
    using System;
    using System.Collections.Generic;
    public class Crypt_Clear : ICrypt
    {
        public string Encrypt(string clear) {
            return clear;
        }
        public string Decrypt(string cipher) {
            return cipher;
        }
    }
}
