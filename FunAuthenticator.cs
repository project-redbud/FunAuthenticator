using Milimoe.FunGame.Core.Api.Utility;

namespace Milimoe.FunAuthenticator
{
    public partial class FunAuthenticator : Form
    {
        public FunAuthenticator()
        {
            InitializeComponent();
            if (CheckKey())
            {
                TaskUtility.NewTask(async () =>
                {
                    while (true)
                    {
                        await NewCode(key);
                    }
                }).OnError(e =>
                {
                    MessageBox.Show(e.ToString(), "FunAuthenticator");
                });
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public async Task NewCode(string key)
        {
            string code = TwoFactorAuthenticator.GenerateCode(key);
            CodeText.Text = code;
            int now = DateTime.UtcNow.Second;
            if (now > 30) now -= 30;
            RemindText.Text = "剩余 " + (30 - now == 0 ? 30 : (30 - now)) + " 秒";
            while (true)
            {
                await Task.Delay(1000);
                now++;
                RemindText.Text = "剩余 " + (30 - now) + " 秒";
                if (30 - now <= 0)
                {
                    RemindText.Text = "过期";
                    break;
                }
            }
        }

        const string publicpath = "public.key"; // 公钥（密文）文件路径
        const string privatepath = "private.key"; // 私钥文件路径
        private string key = ""; // 验证秘钥

        private bool CheckKey()
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
                MessageBox.Show("缺少公钥和秘钥文件。", "FunAuthenticator");
                return false;
            }
        }
    }
}