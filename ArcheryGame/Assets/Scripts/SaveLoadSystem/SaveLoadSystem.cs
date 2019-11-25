using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class SaveLoadSystem
{
    public static string KeyMasterVol = "MasterVol";
    public static string KeyMusicVol = "MusicVol";
    public static string KeySFXVol = "SFXVol";
    public static string KeyHiScore = "HiScore";

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void SetInt(string key, int value, bool isEncrypted = false)
    {
        if (isEncrypted)
            SaveLoadSystem.SetString(key, value.ToString(), true);
        else PlayerPrefs.SetInt(key, value);
    }

    public static void SetFloat(string key, float value, bool isEncrypted = false)
    {
        if (isEncrypted)
            SaveLoadSystem.SetString(key, value.ToString(), true);
        else PlayerPrefs.SetFloat(key, value);
    }

    public static void SetString(string key, string value, bool isEncrypted = false)
    {
        if (isEncrypted)
            PlayerPrefs.SetString(key, SaveLoadSystem.Encrypt(value));
        else PlayerPrefs.SetString(key, value);
    }

    public static int GetInt(string key, int defaultValue, bool isEncrypted = false)
    {
        if (!SaveLoadSystem.HasKey(key))
            return defaultValue;
        if (isEncrypted)
        {
            string strEncrypt = PlayerPrefs.GetString(key);
            string strDecrypt = SaveLoadSystem.Decrypt(strEncrypt);
            return int.Parse(strDecrypt);
        }
        else return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static float GetFloat(string key, float defaultValue, bool isEncrypted = false)
    {
        if (!SaveLoadSystem.HasKey(key))
            return defaultValue;
        if (isEncrypted)
        {
            string strEncrypt = PlayerPrefs.GetString(key);
            string strDecrypt = SaveLoadSystem.Decrypt(strEncrypt);
            return float.Parse(strDecrypt);
        }
        else return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static string GetString(string key, string defaultValue, bool isEncrypted = false)
    {
        if (!SaveLoadSystem.HasKey(key))
            return defaultValue;
        if (isEncrypted)
        {
            string strEncrypt = PlayerPrefs.GetString(key);
            return SaveLoadSystem.Decrypt(strEncrypt);
        }
        else return PlayerPrefs.GetString(key, defaultValue);
    }

    static string hash = "123987@!abc";
    public static string Encrypt(string input)
    {
        Debug.Log("xxxxxxxxxx encrypt " + input);
        byte[] data = UTF8Encoding.UTF8.GetBytes(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform transform = trip.CreateEncryptor();
                byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                string res = Convert.ToBase64String(results, 0, results.Length);
                return res;
            }
        }
    }

    public static string Decrypt(string input)
    {
        Debug.Log("xxxxxxxxxx Decrypt " + input);
        byte[] data = Convert.FromBase64String(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform transform = trip.CreateDecryptor();
                byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                string res = UTF8Encoding.UTF8.GetString(results);
                return res;
            }
        }
    }
}
