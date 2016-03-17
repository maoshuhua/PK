using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：ValidateCode
    /// <summary>
    /// 验证码相关
    /// </summary>
    /// <remarks>
    /// 验证码相关
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        叶斌成           新建
    /// =======================================================================
    public class ValidateCode
    {
        private CaptchaImage ci = null;

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        public string CreateValidateCode(int length)
        {
            ci = new CaptchaImage
            {
                Width = 120,
                Height = 35,
                TextLength = length,
                BackgroundNoise = CaptchaImage.BackgroundNoiseLevel.Extreme,
                FontWarp = CaptchaImage.FontWarpFactor.Medium,
                LineNoise = CaptchaImage.LineNoiseLevel.High
            };

            return ci.Text;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        public byte[] CreateValidateGraphic(string validateCode="")
        {
            if (ci != null)
            {
                using (Bitmap b = ci.RenderImage())
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    //将图像保存到指定的流
                    b.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    return ms.GetBuffer();

                    //ms.Dispose();
                }
            }

            return null;

        }

    }
}