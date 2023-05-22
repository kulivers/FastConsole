using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using ExCh;

namespace exchRun
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("hello world");
            ServicePointManager.ServerCertificateValidationCallback = ValidateCertificate;

            var check = new Check();
            check.Test();
        }
        static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine(certificate.Subject+$": {sslPolicyErrors}");
            // Perform custom certificate validation
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                // No errors, the certificate is considered valid
                return true;
            }

            if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
            {
                // Handle chain errors, if necessary
                // For example, you can check if any certificates in the chain have expired

                // Loop through the chain status to check for expired certificates
                foreach (X509ChainStatus status in chain.ChainStatus)
                {
                    Console.WriteLine("\\t" + status.Status);
                    if (status.Status == X509ChainStatusFlags.NotTimeValid)
                    {
                        // Certificate is expired, reject it
                        return false;
                    }
                }

                // All certificates are valid, accept the certificate
                return true;
            }

            // Other types of certificate errors occurred, reject the certificate
            return false;
        }

    }
}