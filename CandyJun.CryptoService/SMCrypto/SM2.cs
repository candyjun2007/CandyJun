using CandyJun.Common;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CandyJun.CryptoService.SMCrypto
{
    public class SM2
    {
        public static void GenerateKeyPair()
        {
            SM2CryptoServiceProvider sm2 = SM2CryptoServiceProvider.Instance;
            AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
            ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
            ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
            BigInteger privateKey = ecpriv.D;
            ECPoint publicKey = ecpub.Q;

            System.Console.Out.WriteLine("公钥: " + Encoding.UTF8.GetString(Hex.Encode(publicKey.GetEncoded())).ToUpper());
            System.Console.Out.WriteLine("私钥: " + Encoding.UTF8.GetString(Hex.Encode(privateKey.ToByteArray())).ToUpper());
        }
        public static Dictionary<string, string> GetKeyPair()
        {
            SM2CryptoServiceProvider sm2 = SM2CryptoServiceProvider.Instance;
            AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
            ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
            ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
            BigInteger privateKey = ecpriv.D;
            ECPoint publicKey = ecpub.Q;

            var result = new Dictionary<string, string>();
            result.Add("公钥", Encoding.UTF8.GetString(Hex.Encode(publicKey.GetEncoded())).ToUpper());
            result.Add("私钥", Encoding.UTF8.GetString(Hex.Encode(privateKey.ToByteArray())).ToUpper());
            return result;
        }

        //public static X509Certificate MakeRootCert(string filePath, IDictionary subjectNames)
        //{
        //    AsymmetricCipherKeyPair keypair = SM2CryptoServiceProvider.Instance.ecc_key_pair_generator.GenerateKeyPair();
        //    ECPublicKeyParameters pubKey = (ECPublicKeyParameters)keypair.Public; //CA公钥     
        //    ECPrivateKeyParameters priKey = (ECPrivateKeyParameters)keypair.Private;    //CA私钥     



        //    X509Name issuerDN = new X509Name(GetDictionaryKeys(subjectNames), subjectNames);
        //    X509Name subjectDN = issuerDN;  //自签证书，两者一样  

        //    SM2X509V3CertificateGenerator sm2CertGen = new SM2X509V3CertificateGenerator();
        //    //X509V3CertificateGenerator sm2CertGen = new X509V3CertificateGenerator();  
        //    sm2CertGen.SetSerialNumber(new BigInteger(128, new Random()));   //128位     
        //    sm2CertGen.SetIssuerDN(issuerDN);
        //    sm2CertGen.SetNotBefore(DateTime.UtcNow.AddDays(-1));
        //    sm2CertGen.SetNotAfter(DateTime.UtcNow.AddDays(365 * 10));
        //    sm2CertGen.SetSubjectDN(subjectDN);
        //    sm2CertGen.SetPublicKey(pubKey); //公钥  


        //    sm2CertGen.SetSignatureAlgorithm("SM3WITHSM2");

        //    sm2CertGen.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(true));
        //    sm2CertGen.AddExtension(X509Extensions.SubjectKeyIdentifier, false, new SubjectKeyIdentifierStructure(pubKey));
        //    sm2CertGen.AddExtension(X509Extensions.AuthorityKeyIdentifier, false, new AuthorityKeyIdentifierStructure(pubKey));
        //    sm2CertGen.AddExtension(X509Extensions.KeyUsage, true, new KeyUsage(6));


        //    Org.BouncyCastle.X509.X509Certificate sm2Cert = sm2CertGen.Generate(keypair);

        //    sm2Cert.CheckValidity();
        //    sm2Cert.Verify(pubKey);

        //    return sm2Cert;
        //}

        public static String Encrypt(string publicKey, string data)
        {
            return Encrypt(Hex.Decode(publicKey), Encoding.UTF8.GetBytes(data));
        }

        public static String Encrypt(byte[] publicKey, byte[] data)
        {
            if (null == publicKey || publicKey.Length == 0)
            {
                return null;
            }
            if (data == null || data.Length == 0)
            {
                return null;
            }

            byte[] source = new byte[data.Length];
            Array.Copy(data, 0, source, 0, data.Length);

            SM2Cipher cipher = new SM2Cipher();
            SM2CryptoServiceProvider sm2 = SM2CryptoServiceProvider.Instance;

            ECPoint userKey = sm2.ecc_curve.DecodePoint(publicKey);

            ECPoint c1 = cipher.Init_enc(sm2, userKey);
            cipher.Encrypt(source);

            byte[] c3 = new byte[32];
            cipher.Dofinal(c3);

            String sc1 = Encoding.UTF8.GetString(Hex.Encode(c1.GetEncoded()));
            String sc2 = Encoding.UTF8.GetString(Hex.Encode(source));
            String sc3 = Encoding.UTF8.GetString(Hex.Encode(c3));

            return (sc1 + sc2 + sc3).ToUpper();
        }

        public static byte[] Decrypt(string privateKey, string encryptedData)
        {
            return Decrypt(Hex.Decode(privateKey), Hex.Decode(encryptedData));
        }

        public static byte[] Decrypt(byte[] privateKey, byte[] encryptedData)
        {
            if (null == privateKey || privateKey.Length == 0)
            {
                return null;
            }
            if (encryptedData == null || encryptedData.Length == 0)
            {
                return null;
            }

            String data = Encoding.UTF8.GetString(Hex.Encode(encryptedData));

            byte[] c1Bytes = Hex.Decode(Encoding.UTF8.GetBytes(data.Substring(0, 130)));
            int c2Len = encryptedData.Length - 97;
            byte[] c2 = Hex.Decode(Encoding.UTF8.GetBytes(data.Substring(130, 2 * c2Len)));
            byte[] c3 = Hex.Decode(Encoding.UTF8.GetBytes(data.Substring(130 + 2 * c2Len, 64)));

            SM2CryptoServiceProvider sm2 = SM2CryptoServiceProvider.Instance;
            BigInteger userD = new BigInteger(1, privateKey);

            ECPoint c1 = sm2.ecc_curve.DecodePoint(c1Bytes);
            SM2Cipher cipher = new SM2Cipher();
            cipher.Init_dec(userD, c1);
            cipher.Decrypt(c2);
            cipher.Dofinal(c3);

            return c2;
        }

        public static BigInteger[] Sm2Sign(string md, AsymmetricCipherKeyPair keypair)
        {
            return Sm2Sign(Encoding.UTF8.GetBytes(md), keypair);
        }

        public static BigInteger[] Sm2Sign(byte[] md, AsymmetricCipherKeyPair keypair)
        {
            SM3Digest sm3 = new SM3Digest();

            ECPublicKeyParameters ecpub = (ECPublicKeyParameters)keypair.Public;

            byte[] z = SM2CryptoServiceProvider.Instance.Sm2GetZ(Encoding.Default.GetBytes(SM2CryptoServiceProvider.Instance.userId), ecpub.Q);
            sm3.BlockUpdate(z, 0, z.Length);

            byte[] p = md;
            sm3.BlockUpdate(p, 0, p.Length);

            byte[] hashData = new byte[32];
            sm3.DoFinal(hashData, 0);

            // e  
            BigInteger e = new BigInteger(1, hashData);
            // k  
            BigInteger k = null;
            ECPoint kp = null;
            BigInteger r = null;
            BigInteger s = null;
            BigInteger userD = null;

            do
            {
                do
                {

                    ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)keypair.Private;
                    k = ecpriv.D;
                    kp = ecpub.Q;

                    userD = ecpriv.D;

                    // r  
                    r = e.Add(kp.X.ToBigInteger());
                    r = r.Mod(SM2CryptoServiceProvider.Instance.ecc_n);
                }
                while (r.Equals(BigInteger.Zero) || r.Add(k).Equals(SM2CryptoServiceProvider.Instance.ecc_n));

                // (1 + dA)~-1  
                BigInteger da_1 = userD.Add(BigInteger.One);
                da_1 = da_1.ModInverse(SM2CryptoServiceProvider.Instance.ecc_n);
                // s  
                s = r.Multiply(userD);
                s = k.Subtract(s).Mod(SM2CryptoServiceProvider.Instance.ecc_n);
                s = da_1.Multiply(s).Mod(SM2CryptoServiceProvider.Instance.ecc_n);
            }
            while (s.Equals(BigInteger.Zero));

            byte[] btRS = new byte[64];
            byte[] btR = r.ToByteArray();
            byte[] btS = s.ToByteArray();
            Array.Copy(btR, btR.Length - 32, btRS, 0, 32);
            Array.Copy(btS, btS.Length - 32, btRS, 32, 32);

            return new BigInteger[] { r, s };
        }
        public static bool Verify(string msg, string signData, string certData)
        {
            return Verify(Encoding.UTF8.GetBytes(msg), ByteUtils.HexToByteArray(signData), ByteUtils.HexToByteArray(certData));
        }
        public static bool Verify(byte[] msg, byte[] signData, byte[] certData)
        {

            var x5092 = new System.Security.Cryptography.X509Certificates.X509Certificate2(certData);
            byte[] certPK = x5092.GetPublicKey();

            certPK = ByteUtils.SubBytes(certPK, 1, 64);

            byte[] certPKX = ByteUtils.SubBytes(certPK, certPK.Length - 32 - 32, 32);
            byte[] certPKY = ByteUtils.SubBytes(certPK, certPK.Length - 32, 32);


            System.String strcertPKX = ByteUtils.ByteArrayToHex(certPKX);
            System.String strcertPKY = ByteUtils.ByteArrayToHex(certPKY);
            BigInteger biX = new BigInteger(strcertPKX, 16);
            BigInteger biY = new BigInteger(strcertPKY, 16);


            ECFieldElement x = new FpFieldElement(SM2CryptoServiceProvider.Instance.ecc_p, biX);
            ECFieldElement y = new FpFieldElement(SM2CryptoServiceProvider.Instance.ecc_p, biY);

            ECPoint userKey = new FpPoint(SM2CryptoServiceProvider.Instance.ecc_curve, x, y);


            SM3Digest sm3 = new SM3Digest();
            byte[] z = SM2CryptoServiceProvider.Instance.Sm2GetZ(Encoding.Default.GetBytes(SM2CryptoServiceProvider.Instance.userId), userKey);
            sm3.BlockUpdate(z, 0, z.Length);


            byte[] p = msg;
            sm3.BlockUpdate(p, 0, p.Length);

            byte[] md = new byte[32];
            sm3.DoFinal(md, 0);


            byte[] btR = ByteUtils.SubBytes(signData, 0, 32);
            byte[] btS = ByteUtils.SubBytes(signData, 32, 32);


            System.String strR = ByteUtils.ByteArrayToHex(btR);
            System.String strS = ByteUtils.ByteArrayToHex(btS);
            BigInteger r = new BigInteger(strR, 16);
            BigInteger s = new BigInteger(strS, 16);

            // e_  
            BigInteger e = new BigInteger(1, md);
            // t  
            BigInteger t = r.Add(s).Mod(SM2CryptoServiceProvider.Instance.ecc_n);

            if (t.Equals(BigInteger.Zero))
                return false;

            // x1y1  
            ECPoint x1y1 = SM2CryptoServiceProvider.Instance.ecc_point_g.Multiply(s);
            x1y1 = x1y1.Add(userKey.Multiply(t));

            // R  
            BigInteger R = e.Add(x1y1.X.ToBigInteger()).Mod(SM2CryptoServiceProvider.Instance.ecc_n);

            return r.Equals(R);
        }
    }
}
