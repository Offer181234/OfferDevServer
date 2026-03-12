using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
    public interface IEncryptService
    {
        string EncryptString(string text);
        string DecryptString(string cipherText);
    }
}
