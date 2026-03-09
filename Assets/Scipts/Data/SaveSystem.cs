using UnityEngine;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public static class SaveSystem
{
    private static readonly string SavePath =
        Path.Combine(Application.persistentDataPath, "save.dat");

    // Chave simples 
    private const string EncryptionKey = "iAyQ5yE4d8cvqt4Q";

    // PUBLIC API 
    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        // DEBUG apenas no Editor
        #if UNITY_EDITOR
            Debug.Log("SAVE JSON:\n" + json);
        #endif

        byte[] encryptedData = Encrypt(json);
        File.WriteAllBytes(SavePath, encryptedData);

        Debug.Log("Jogo guardado em: " + SavePath);
        // C:\Users\Dinis\AppData\LocalLow\DefaultCompany\2DGame
    }

    public static SaveData Load()
    {
        if (!HasSave()) return null;

        byte[] encryptedData = File.ReadAllBytes(SavePath);
        string json = Decrypt(encryptedData);

        // DEBUG
        #if UNITY_EDITOR
            Debug.Log("LOAD JSON:\n" + json);
        #endif

        Debug.Log("Jogo carregado");
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static bool HasSave()
    {
        return File.Exists(SavePath);
    }

    // ENCRYPTION 
    private static byte[] Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
        aes.IV = new byte[16]; // IV fixo

        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(plainBytes, 0, plainBytes.Length);
            cs.FlushFinalBlock();
        }

        return ms.ToArray();
    }

    private static string Decrypt(byte[] cipherBytes)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
        aes.IV = new byte[16];

        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
        {
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.FlushFinalBlock();
        }

        return Encoding.UTF8.GetString(ms.ToArray());
    }
}
