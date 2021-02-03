using System.IO;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// 提供热更新所使用的公共方法
/// </summary>
public static class Helps
{
    /// <summary>
    /// 根据目标文件路径生成MD5编码
    /// </summary>
    /// <param name="targetFilePath">目标文件路径</param>
    /// <returns>目标路径的MD5编码</returns>
    public static string GetMD5Values(string targetFilePath)
    {
        StringBuilder sb = new StringBuilder();
        targetFilePath = targetFilePath.Trim();//去除空格
        using (FileStream fs = new FileStream(targetFilePath, FileMode.Open))
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(fs);//根据打开的文件流,计算其hash值,转为二进制文件
            for (int i = 0; i < result.Length; i++)
            {
                //"x2"表示输出按照16进制,且为2位对齐输出
                sb.Append(result[i].ToString("x2"));
            }
        }
        return sb.ToString();
    }
}
