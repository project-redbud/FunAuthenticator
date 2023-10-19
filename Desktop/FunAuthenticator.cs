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
            string code = TwoFactorAuthenticator.GenerateCode(key);
            CodeText.Text = code;
            int now = DateTime.UtcNow.Second;
            if (now > 30) now -= 30;
            int remain = (30 - now == 0 ? 30 : (30 - now));
            RemainText.Text = "ʣ�� " + remain + " ��";
            Timer.Value = remain < 0 ? 30 : remain;
            while (true)
            {
                await Task.Delay(1000);
                now++;
                remain = (30 - now);
                RemainText.Text = "ʣ�� " + remain + " ��";
                Timer.Value = remain < 0 ? 30 : remain;
                if (30 - now <= 0)
                {
                    RemainText.Text = "����";
                    break;
                }
            }
        }

        const string publicpath = "public.key"; // ��Կ�����ģ��ļ�·��
        const string privatepath = "private.key"; // ˽Կ�ļ�·��
        private string key = ""; // ��֤��Կ

        private bool CheckKey()
        {
            if (File.Exists(privatepath) && File.Exists(publicpath))
            {
                // ��ȡ���ܵ���Կ
                string secret = File.ReadAllText(publicpath);

                // ��ȡ���ܵ���Կ
                string privatekey = File.ReadAllText(privatepath);

                // ��������
                string plain = Encryption.RSADecrypt(secret, privatekey);

                // �����ܺ����Կת��Ϊ�ַ���
                key = plain;
                return true;
            }
            else
            {
                MessageBox.Show("ȱ�ٹ�Կ����Կ�ļ���", "FunAuthenticator");
                return false;
            }
        }
    }
}