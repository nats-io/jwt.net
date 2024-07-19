// ReSharper disable all
#pragma warning disable

using System;
using System.Text;

namespace NATS.Jwt.Internal;

internal static class Sha512256
{
    private static readonly ulong[] K =
    [
        0x428a2f98d728ae22,
        0x7137449123ef65cd,
        0xb5c0fbcfec4d3b2f,
        0xe9b5dba58189dbbc,
        0x3956c25bf348b538,
        0x59f111f1b605d019,
        0x923f82a4af194f9b,
        0xab1c5ed5da6d8118,
        0xd807aa98a3030242,
        0x12835b0145706fbe,
        0x243185be4ee4b28c,
        0x550c7dc3d5ffb4e2,
        0x72be5d74f27b896f,
        0x80deb1fe3b1696b1,
        0x9bdc06a725c71235,
        0xc19bf174cf692694,
        0xe49b69c19ef14ad2,
        0xefbe4786384f25e3,
        0x0fc19dc68b8cd5b5,
        0x240ca1cc77ac9c65,
        0x2de92c6f592b0275,
        0x4a7484aa6ea6e483,
        0x5cb0a9dcbd41fbd4,
        0x76f988da831153b5,
        0x983e5152ee66dfab,
        0xa831c66d2db43210,
        0xb00327c898fb213f,
        0xbf597fc7beef0ee4,
        0xc6e00bf33da88fc2,
        0xd5a79147930aa725,
        0x06ca6351e003826f,
        0x142929670a0e6e70,
        0x27b70a8546d22ffc,
        0x2e1b21385c26c926,
        0x4d2c6dfc5ac42aed,
        0x53380d139d95b3df,
        0x650a73548baf63de,
        0x766a0abb3c77b2a8,
        0x81c2c92e47edaee6,
        0x92722c851482353b,
        0xa2bfe8a14cf10364,
        0xa81a664bbc423001,
        0xc24b8b70d0f89791,
        0xc76c51a30654be30,
        0xd192e819d6ef5218,
        0xd69906245565a910,
        0xf40e35855771202a,
        0x106aa07032bbd1b8,
        0x19a4c116b8d2d0c8,
        0x1e376c085141ab53,
        0x2748774cdf8eeb99,
        0x34b0bcb5e19b48a8,
        0x391c0cb3c5c95a63,
        0x4ed8aa4ae3418acb,
        0x5b9cca4f7763e373,
        0x682e6ff3d6b2b8a3,
        0x748f82ee5defb2fc,
        0x78a5636f43172f60,
        0x84c87814a1f0ab72,
        0x8cc702081a6439ec,
        0x90befffa23631e28,
        0xa4506cebde82bde9,
        0xbef9a3f7b2c67915,
        0xc67178f2e372532b,
        0xca273eceea26619c,
        0xd186b8c721c0c207,
        0xeada7dd6cde0eb1e,
        0xf57d4f7fee6ed178,
        0x06f067aa72176fba,
        0x0a637dc5a2c898a6,
        0x113f9804bef90dae,
        0x1b710b35131c471b,
        0x28db77f523047d84,
        0x32caab7b40c72493,
        0x3c9ebe0a15c9bebc,
        0x431d67c49c100d4c,
        0x4cc5d4becb3e42b6,
        0x597f299cfc657e2a,
        0x5fcb6fab3ad6faec,
        0x6c44198c4a475817
    ];

    public static byte[] ComputeHash(byte[] message)
    {
        ulong[] H = new ulong[8] {
            0x22312194FC2BF72C, 0x9F555FA3C84C64C2, 0x2393B86B6F53B151, 0x963877195940EABD,
            0x96283EE2A88EFFE3, 0xBE5E1E2553863992, 0x2B0199FC2C85B8AA, 0x0EB72DDC81C52CA2
        };

        int paddedLength = ((message.Length + 17) / 128 + 1) * 128;
        byte[] padded = new byte[paddedLength];
        Array.Copy(message, padded, message.Length);
        padded[message.Length] = 0x80;

        ulong bitLength = (ulong)message.Length * 8;
        for (int i = 0; i < 8; i++)
            padded[paddedLength - 8 + i] = (byte)(bitLength >> (56 - 8 * i));

        for (int i = 0; i < paddedLength; i += 128)
        {
            ulong[] W = new ulong[80];
            for (int t = 0; t < 16; t++)
                // W[t] = BitConverter.Toulong(padded, i + t * 8).ReverseEndianness();
                W[t] = SwapEndianness(BitConverter.ToUInt64(padded, i + t * 8));

            for (int t = 16; t < 80; t++)
                W[t] = sigma1(W[t - 2]) + W[t - 7] + sigma0(W[t - 15]) + W[t - 16];

            ulong a = H[0], b = H[1], c = H[2], d = H[3], e = H[4], f = H[5], g = H[6], h = H[7];

            for (int t = 0; t < 80; t++)
            {
                ulong T1 = h + Sigma1(e) + Ch(e, f, g) + K[t] + W[t];
                ulong T2 = Sigma0(a) + Maj(a, b, c);
                h = g;
                g = f;
                f = e;
                e = d + T1;
                d = c;
                c = b;
                b = a;
                a = T1 + T2;
            }

            H[0] += a;
            H[1] += b;
            H[2] += c;
            H[3] += d;
            H[4] += e;
            H[5] += f;
            H[6] += g;
            H[7] += h;
        }

        byte[] hash = new byte[32];
        for (int i = 0; i < 4; i++)
        {
            ulong value = SwapEndianness(H[i]);
            BitConverter.GetBytes(value).CopyTo(hash, i * 8);
        }

        return hash;
    }

    public static string ComputeHashString(string input)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = ComputeHash(inputBytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    private static ulong Ch(ulong x, ulong y, ulong z) => (x & y) ^ (~x & z);

    private static ulong Maj(ulong x, ulong y, ulong z) => (x & y) ^ (x & z) ^ (y & z);

    private static ulong RotR(ulong x, int n) => (x >> n) | (x << (64 - n));

    private static ulong Sigma0(ulong x) => RotR(x, 28) ^ RotR(x, 34) ^ RotR(x, 39);

    private static ulong Sigma1(ulong x) => RotR(x, 14) ^ RotR(x, 18) ^ RotR(x, 41);

    private static ulong sigma0(ulong x) => RotR(x, 1) ^ RotR(x, 8) ^ (x >> 7);

    private static ulong sigma1(ulong x) => RotR(x, 19) ^ RotR(x, 61) ^ (x >> 6);

    private static ulong SwapEndianness(ulong x) =>
        ((x & 0x00000000000000FF) << 56) |
        ((x & 0x000000000000FF00) << 40) |
        ((x & 0x0000000000FF0000) << 24) |
        ((x & 0x00000000FF000000) << 8) |
        ((x & 0x000000FF00000000) >> 8) |
        ((x & 0x0000FF0000000000) >> 24) |
        ((x & 0x00FF000000000000) >> 40) |
        ((x & 0xFF00000000000000) >> 56);
}
