using Org.BouncyCastle.Utilities.Encoders;
using System.Text;

namespace CandyJun.CryptoService.SMCrypto
{
    public class SM3
    {
        public static string Hash(string str)
        {
            SM3Digest sm3 = new SM3Digest();
            byte[] md = new byte[sm3.GetDigestSize()];
            byte[] msg1 = Encoding.UTF8.GetBytes(str);
            sm3.BlockUpdate(msg1, 0, msg1.Length);
            sm3.DoFinal(md, 0);
            string s = Encoding.UTF8.GetString(Hex.Encode(md));
            return s.ToUpper();
        }
    }
}
