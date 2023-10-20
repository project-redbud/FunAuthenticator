using Milimoe.FunGame.Core.Api.Utility;

namespace Milimoe.FunAuthenticator.Desktop
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
            int now = DateTime.UtcNow.Second;
            now %= 30;
            int remain = 30 - now < 0 ? 30 : 30 - now;
            RemainText.Text = "剩余 " + remain + " 秒";
            Timer.Value = remain * 10 < 0 ? 300 : remain * 10;
            CodeText.Text = TwoFactorAuthenticator.GenerateCode(key);
            while (true)
            {
                now = DateTime.UtcNow.Second;
                now %= 30;
                remain = 30 - now < 0 ? 0 : 30 - now;
                RemainText.Text = "剩余 " + remain + " 秒";
                Timer.Value = remain * 10 < 0 ? 0 : remain * 10;
                for (int i = 0; i < 10; i++)
                {
                    Timer.Value = Timer.Value - 1 < 0 ? 0 : (Timer.Value - 1);
                    await Task.Delay(92);
                }
                now++;
                remain = 30 - now < 0 ? 0 : 30 - now;
                if (remain <= 0)
                {
                    RemainText.Text = "过期";
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
