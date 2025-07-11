using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using System.IO;

public static class Helper
{
    public static byte[] Encrypt(byte[] data, string publicKeyPath)
    {
        //Test Pull
        using var outputStream = new MemoryStream();
        using var publicKeyStream = File.OpenRead(publicKeyPath);
        var pubKeyRingBundle = new PgpPublicKeyRingBundle(PgpUtilities.GetDecoderStream(publicKeyStream));
        var pubKey = GetEncryptionKey(pubKeyRingBundle);

        var encryptedDataGenerator = new PgpEncryptedDataGenerator(
            Org.BouncyCastle.Bcpg.SymmetricKeyAlgorithmTag.Cast5, true, new SecureRandom());

        encryptedDataGenerator.AddMethod(pubKey);

        using (var encOut = encryptedDataGenerator.Open(outputStream, data.Length))
        {
            encOut.Write(data, 0, data.Length);
        }

        return outputStream.ToArray();
    }

    private static PgpPublicKey GetEncryptionKey(PgpPublicKeyRingBundle keyRingBundle)
    {
        foreach (PgpPublicKeyRing keyRing in keyRingBundle.GetKeyRings())
        {
            foreach (PgpPublicKey key in keyRing.GetPublicKeys())
            {
                if (key.IsEncryptionKey)
                    return key;
            }
        }
        throw new ArgumentException("No encryption key found in public key file.");
    }
}
