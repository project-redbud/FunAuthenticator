using Milimoe.FunGame.Core.Api.Utility;

const string publicpath = "public.key"; // 公钥（密文）文件路径
const string privatepath = "private.key"; // 私钥文件路径

string key = ""; // 验证秘钥

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
    if (File.Exists(privatepath) && File.Exists(publicpath))
    {
        // 读取加密的秘钥
        string secret = File.ReadAllText(publicpath);

        // 读取加密的秘钥
        string privatekey = File.ReadAllText(privatepath);

        // 解密密文
        string plain = Encryption.RSADecrypt(secret, privatekey);

        // 将解密后的秘钥转换为字符串
        key = plain;
        return true;
    }
    else
    {
        Console.WriteLine("缺少公钥和秘钥文件。");
        return false;
    }
}