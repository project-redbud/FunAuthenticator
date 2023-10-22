using Milimoe.FunGame.Core.Api.Utility;

const string keypath = "authenticator.key";
const string publicstring = "----- PUBLIC KEY -----\r\n";

string key = ""; // 验证秘钥

TwoFactorAuthenticator TFA = new();

if (CheckKey())
{
    TaskUtility.NewTask(async () =>
    {
        while (true)
        {
            await NewCode(key);
        }
    }).OnError(Console.WriteLine);
    while (true)
    {
        string str = Console.ReadLine() ?? "";
        if (str == "quit")
        {
            break;
        }
    }
}
else
{
    Console.ReadKey();
}

async Task NewCode(string key)
{
    string code = TwoFactorAuthenticator.GenerateCode(key);
    Console.WriteLine("Current Code: " + code);
    int now = DateTime.UtcNow.Second;
    if (now > 30) now -= 30;
    Console.Write("剩余 " + (30 - now == 0 ? 30 : (30 - now)) + " 秒");
    while (true)
    {
        await Task.Delay(1000);
        now++;
        ClearCurrentConsoleLine();
        Console.Write("剩余 " + (30 - now) + " 秒");
        if (30 - now <= 0)
        {
            Console.Clear();
            break;
        }
    }
}

void ClearCurrentConsoleLine()
{
    int currentLineCursor = Console.CursorTop;
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write(new string(' ', Console.WindowWidth));
    Console.SetCursorPosition(0, currentLineCursor);
}

bool CheckKey()
{
    if (File.Exists(keypath))
    {
        // 读取秘钥
        string encryptedText = File.ReadAllText(keypath);

        // 拆分
        if (TwoFactorAuthenticator.SplitKeyFile(encryptedText, out string[] strs))
        {
            // 解密密文
            string plain = Encryption.RSADecrypt(strs[0].Split(publicstring)[1], strs[1]);

            // 将解密后的秘钥转换为字符串
            key = plain;
            return true;
        }
    }
    else
    {
        Console.WriteLine("缺少认证文件。");
    }
    return false;
}
