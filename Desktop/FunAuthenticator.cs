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
                    await Task.Delay(93);
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

        const string keypath = "authenticator.key";
        const string publicstring = "----- PUBLIC KEY -----\r\n";
        private string key = ""; // 验证秘钥

        private bool CheckKey()
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
                MessageBox.Show("缺少认证文件。", "FunAuthenticator");
            }
            return false;
        }
    }
}
