/*自作「ネットワークライブラリ　送信用」*/
using System;
using System.Text;
using System.IO;
using System.Drawing;
using System.Net.Sockets;

namespace SendLib
{
    public class Send
    {
        private TcpClient socket = new TcpClient();
        private NetworkStream netStream = null;

        /// <summary>
        /// 指定した送信先に接続します
        /// </summary>
        public bool Connect(string IPAddress, int PortNumber)
        {
            try
            {
                socket = new TcpClient();
                socket.Connect(IPAddress, PortNumber);
                netStream = socket.GetStream();
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new ArgumentOutOfRangeException(e.Message, e.InnerException);
            }
            catch (SocketException)
            {
                throw new SocketException();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 接続中の送信先との接続を切断します
        /// </summary>
        public void Close()
        {
            netStream.Close();
            socket.Close();
        }

        /// <summary>
        /// メッセージを送信します
        /// </summary>
        public void SendMessage(string Message)
        {
            if (Message == null)
                throw new ArgumentNullException("Messageがnullです\r\n必ず送信するメッセージを指定してください");

            try
            {
                byte[] buf = Encoding.UTF8.GetBytes(Message);
                byte[] tbuf = new byte[buf.Length + 1];
                tbuf[0] = (byte)'m';//メッセージ
                for (int i = 1; i < buf.Length + 1; i++)
                {
                    tbuf[i] = buf[i - 1];
                }
                netStream.Write(tbuf, 0, tbuf.Length);
            }
            catch (IOException e)
            {
                throw new IOException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// イメージ画像を送信します
        /// </summary>
        public void SendImage(Image ImageSource)
        {
            if (ImageSource == null)
                throw new ArgumentNullException("ImageSourceがnullです\r\n必ず送信するイメージ画像を指定してください");

            try
            {
                byte[] buf = ImageToByteArray(ImageSource);
                byte[] tbuf = new byte[buf.Length + 1];
                tbuf[0] = (byte)'i';
                for (int i = 1; i < buf.Length + 1; i++)
                {
                    tbuf[i] = buf[i - 1];
                }
                netStream.Write(tbuf, 0, tbuf.Length);
            }
            catch (IOException e)
            {
                throw new IOException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// イメージを送信用のbyte配列型に変換して取得します
        /// </summary>
        public byte[] ImageToByteArray(Image img)
        {
            ImageConverter imgconv = new ImageConverter();
            byte[] b = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return b;
        }
        public bool ConnectCheck()
        {
            return socket.Connected;
        }
    }
}
