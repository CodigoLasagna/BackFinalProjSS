using System.Globalization;
using System.Numerics;
using System.Text;
using Domain;

namespace Helper.Security
{
    public class RsaMath
    {
        private static Random random = new Random();

        // Paso 1: Generar un número primo utilizando Miller-Rabin
        private static BigInteger GeneratePrimeNumber(int bitLength)
        {
            while (true)
            {
                BigInteger candidate = GenerateRandomOddNumber(bitLength);
                if (IsProbablePrime(candidate, 10))
                {
                    return candidate;
                }
            }
        }

        // Generar un número aleatorio de una longitud de bits determinada
        private static BigInteger GenerateRandomOddNumber(int bitLength)
        {
            byte[] bytes = new byte[(bitLength + 7) / 8];
            random.NextBytes(bytes);
            bytes[bytes.Length - 1] &= 0x7F; // El número tiene que ser positivo
            BigInteger result = new BigInteger(bytes);
            return result | 1; // El número tiene que ser impar
        }

        // Comprobar si un número es probablemente primo utilizando Miller-Rabin
        private static bool IsProbablePrime(BigInteger source, int certainty)
        {
            if (source == 2 || source == 3)
                return true;
            if (source < 2 || source % 2 == 0)
                return false;

            BigInteger d = source - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            byte[] bytes = new byte[source.ToByteArray().LongLength];
            BigInteger a;

            for (int i = 0; i < certainty; i++)
            {
                do
                {
                    random.NextBytes(bytes);
                    a = new BigInteger(bytes);
                }
                while (a < 2 || a >= source - 2);

                BigInteger x = BigInteger.ModPow(a, d, source);
                if (x == 1 || x == source - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, source);
                    if (x == 1)
                        return false;
                    if (x == source - 1)
                        break;
                }

                if (x != source - 1)
                    return false;
            }
            return true;
        }

        // Paso 2: Calcular n = p * q y z = (p - 1) * (q - 1)
        private static (BigInteger, BigInteger) CalculateNandZ(BigInteger p, BigInteger q)
        {
            BigInteger n = p * q;
            BigInteger z = (p - 1) * (q - 1);
            return (n, z);
        }

        // Paso 3: Escoger un número 1 < e < z y gcd(e, z) = 1
        private static BigInteger ChooseE(BigInteger z)
        {
            BigInteger e;
            do
            {
                e = GenerateRandomNumber(2, z - 1);
            } while (BigInteger.GreatestCommonDivisor(e, z) != 1);
            return e;
        }

        // Generar un número en un determinado rango
        private static BigInteger GenerateRandomNumber(BigInteger min, BigInteger max)
        {
            byte[] bytes = max.ToByteArray();
            BigInteger result;
            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F; // Asegurarse de que el número es positivo
                result = new BigInteger(bytes);
            } while (result < min || result >= max);
            return result;
        }

        // Paso 4: Calcular d dado que d ≡ e^(-1) mod z
        private static BigInteger CalculateD(BigInteger e, BigInteger z)
        {
            return ModInverse(e, z);
        }

        // Calcular el modulo inverso de e mod z Usando el algoritmo extendido de Euclides
        private static BigInteger ModInverse(BigInteger e, BigInteger z)
        {
            BigInteger t = 0, newT = 1;
            BigInteger r = z, newR = e;

            while (newR != 0)
            {
                BigInteger quotient = r / newR;

                (t, newT) = (newT, t - quotient * newT);
                (r, newR) = (newR, r - quotient * newR);
            }

            if (r > 1)
                throw new ArgumentException("e is not invertible");
            if (t < 0)
                t += z;

            return t;
        }

        // Paso 5-6: Llaves publica key (n, e) y privada (n, d)
        public static (BigInteger, BigInteger) GeneratePublicKey(BigInteger n, BigInteger e)
        {
            return (n, e);
        }

        public static (BigInteger, BigInteger) GeneratePrivateKey(BigInteger n, BigInteger d)
        {
            return (n, d);
        }

        // Encriptar un mensaje con la llave pública
        public static string Encrypt(string message, string n, string e)
        {
            BigInteger real_n = BigInteger.Parse(n);
            BigInteger real_e = BigInteger.Parse(e);
            BigInteger messageInBytes = new BigInteger(Encoding.UTF8.GetBytes(message));
            return BigInteger.ModPow(messageInBytes, real_e, real_n).ToString("X");
        }

        // Desencriptar un mensaje con la llave privada
        public static string Decrypt(string ciphertext, string n, string d)
        {
            BigInteger real_n = BigInteger.Parse(n);
            BigInteger real_d = BigInteger.Parse(d);
            BigInteger cipherBigInteger = BigInteger.Parse(ciphertext, NumberStyles.HexNumber);
            BigInteger messageTextIngeger = BigInteger.ModPow(cipherBigInteger, real_d, real_n);
            return Encoding.UTF8.GetString(messageTextIngeger.ToByteArray());
        }

        // Function to demonstrate the complete RSA key generation
        public static (BigInteger, BigInteger, BigInteger) GenerateRSAKeys(int bitLength)
        {
            // Paso 1
            BigInteger p = GeneratePrimeNumber(bitLength / 2);
            BigInteger q = GeneratePrimeNumber(bitLength / 2);

            // Paso 2
            var (n, z) = CalculateNandZ(p, q);

            // Paso 3
            BigInteger e = ChooseE(z);

            // Paso 4
            BigInteger d = CalculateD(e, z);
            //var publicKey = GeneratePublicKey(n, e);
            //var privateKey = GeneratePrivateKey(n, d);

            //Console.WriteLine($"Public Key: (n = {publicKey.Item1}, e = {publicKey.Item2})");
            //Console.WriteLine($"Private Key: (n = {privateKey.Item1}, d = {privateKey.Item2})");

            //string originalMessage = "holap rsa como va";
            //string encryptedMessage = Encrypt(originalMessage, publicKey.Item1, publicKey.Item2);
            //Console.WriteLine($"OriginalMessage: {originalMessage}");
            //Console.WriteLine($"mensage encriptado: {encryptedMessage}");
            //string decryptedMessage = Decrypt(encryptedMessage, privateKey.Item1, privateKey.Item2);
            //Console.WriteLine($"mensage desencriptado: {decryptedMessage}");
            
            // Paso 5 y 6
            return (n, e, d);
        }
    }
}
