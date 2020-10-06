using UnityEngine.Networking;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

// Based on https://www.owasp.org/index.php/Certificate_and_Public_Key_Pinning#.Net
public class AcceptAllCertificatesSignedWithASpecificPublicKey : CertificateHandler
{
    private string _pubKey;
    public AcceptAllCertificatesSignedWithASpecificPublicKey(string pubKey)
    {
        _pubKey = pubKey;
    }
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();
        
        if (pk.ToLower().Equals(_pubKey.ToLower()))
        {
            return true;
        }
        return false;
    }

}
