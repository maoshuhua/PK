using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：CaptchaImage
    /// <summary>
    /// CAPTCHA图像处理类
    /// </summary>
    /// <remarks>
    /// CAPTCHA图像处理类
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        叶斌成           新建
    /// =======================================================================
    public class CaptchaImage
    {
        #region Public Enums

        #region BackgroundNoiseLevel enum

        /// <summary>
        /// Amount of background noise to add to rendered image
        /// </summary>
        public enum BackgroundNoiseLevel
        {
            /// <summary>
            /// 
            /// </summary>
            None,
            /// <summary>
            /// 
            /// </summary>
            Low,
            /// <summary>
            /// 
            /// </summary>
            Medium,
            /// <summary>
            /// 
            /// </summary>
            High,
            /// <summary>
            /// 
            /// </summary>
            Extreme
        }

        #endregion

        #region FontWarpFactor enum

        /// <summary>
        /// Amount of random font warping to apply to rendered text
        /// </summary>
        public enum FontWarpFactor
        {
            /// <summary>
            /// 
            /// </summary>
            None,
            /// <summary>
            /// 
            /// </summary>
            Low,
            /// <summary>
            /// 
            /// </summary>
            Medium,
            /// <summary>
            /// 
            /// </summary>
            High,
            /// <summary>
            /// 
            /// </summary>
            Extreme
        }

        #endregion

        #region LineNoiseLevel enum

        /// <summary>
        /// Amount of curved line noise to add to rendered image
        /// </summary>
        public enum LineNoiseLevel
        {
            /// <summary>
            /// 
            /// </summary>
            None,
            /// <summary>
            /// 
            /// </summary>
            Low,
            /// <summary>
            /// 
            /// </summary>
            Medium,
            /// <summary>
            /// 
            /// </summary>
            High,
            /// <summary>
            /// 
            /// </summary>
            Extreme
        }

        #endregion

        #endregion

        private static string[] RandomFontFamilyArray;
        private readonly DateTime _GeneratedAt;
        private readonly string _Guid;
        private readonly Random _Rand;
        private BackgroundNoiseLevel _BackgroundNoise;
        private string _FontFamilyName;
        private FontWarpFactor _FontWarp;
        private string _FontWhitelist;
        private int _Height;
        private LineNoiseLevel _LineNoise;
        private string _RandomText;
        private string _RandomTextChars;
        private int _RandomTextLength;
        private int _Width;

        #region Public Properties

        /// <summary>
        /// Returns a GUID that uniquely identifies this Captcha
        /// </summary>
        public string UniqueId {
            get {
                return _Guid;
            }
        }

        /// <summary>
        /// Returns the date and time this image was last rendered
        /// </summary>
        public DateTime RenderedAt {
            get {
                return _GeneratedAt;
            }
        }

        /// <summary>
        /// Font family to use when drawing the Captcha text. If no font is provided, a random font will be chosen from the font whitelist for each character.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
        public string Font {
            get {
                return _FontFamilyName;
            }
            set {
                // HACK: this is ugly, but might be the best way to handle this.
                // there should be some way to say, FontFamily.Exists(familyName);
                try {
                    Font font = new Font( value, 12f );
                    _FontFamilyName = value;
                    font.Dispose();
                }
                catch ( Exception ) {
                    _FontFamilyName = FontFamily.GenericSerif.Name;
                }
            }
        }

        /// <summary>
        /// Amount of random warping to apply to the Captcha text.
        /// </summary>
        public FontWarpFactor FontWarp {
            get {
                return _FontWarp;
            }
            set {
                _FontWarp = value;
            }
        }

        /// <summary>
        /// Amount of background noise to apply to the Captcha image.
        /// </summary>
        public BackgroundNoiseLevel BackgroundNoise {
            get {
                return _BackgroundNoise;
            }
            set {
                _BackgroundNoise = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public LineNoiseLevel LineNoise {
            get {
                return _LineNoise;
            }
            set {
                _LineNoise = value;
            }
        }

        /// <summary>
        /// A string of valid characters to use in the Captcha text.
        /// A random character will be selected from this string for each character.
        /// </summary>
        public string TextChars {
            get {
                return _RandomTextChars;
            }
            set {
                _RandomTextChars = value;
                _RandomText = GenerateRandomText();
            }
        }

        /// <summary>
        /// Number of characters to use in the Captcha text.
        /// </summary>
        public int TextLength {
            get {
                return _RandomTextLength;
            }
            set {
                _RandomTextLength = value;
                _RandomText = GenerateRandomText();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RandomText {
            get { return _RandomText; }
            set { _RandomText = value; }
        }

        /// <summary>
        /// Returns the randomly generated Captcha text.
        /// </summary>
        public string Text {
            get {
                return _RandomText;
            }
        }

        /// <summary>
        /// Width of Captcha image to generate, in pixels
        /// </summary>
        public int Width {
            get {
                return _Width;
            }
            set {
                if ( ( value <= 60 ) ) {
                    throw new ArgumentOutOfRangeException( "value", value, "width must be greater than 60." );
                }
                _Width = value;
            }
        }

        /// <summary>
        /// Height of Captcha image to generate, in pixels
        /// </summary>
        public int Height {
            get {
                return _Height;
            }
            set {
                if ( value < 25 ) {
                    throw new ArgumentOutOfRangeException( "value", value, "height must be greater than 25." );
                }
                _Height = value;
            }
        }

        /// <summary>
        /// A semicolon-delimited list of valid fonts to use when no font is provided.
        /// </summary>
        public string FontWhitelist {
            get {
                return _FontWhitelist;
            }
            set {
                _FontWhitelist = value;
            }
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public CaptchaImage() {
            _Rand = new Random();
            _FontWarp = FontWarpFactor.Low;
            _BackgroundNoise = BackgroundNoiseLevel.Low;
            _LineNoise = LineNoiseLevel.None;
            _Width = 100;
            _Height = 35;
            _RandomTextLength = 5;
            _RandomTextChars = "ACDEFGHJKLNPQRTUVXYZ2346789";
            _FontFamilyName = "";
            // -- a list of known good fonts in on both Windows XP and Windows Server 2003
            _FontWhitelist = "arial;arial black;comic sans ms;courier new;estrangelo edessa;franklin gothic medium;" +
                             "georgia;lucida console;lucida sans unicode;mangal;microsoft sans serif;palatino linotype;" +
                             "sylfaen;tahoma;times new roman;trebuchet ms;verdana";
            //_RandomText = GenerateRandomText();
            _GeneratedAt = DateTime.UtcNow;
            _Guid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Forces a new Captcha image to be generated using current property value settings.
        /// </summary>
        public Bitmap RenderImage() {
            return GenerateImagePrivate();
        }

        /// <summary>
        /// Returns a random font family from the font whitelist
        /// </summary>
        private string RandomFontFamily() {
            //-- small optimization so we don't have to split for each char
            if ( RandomFontFamilyArray == null ) {
                RandomFontFamilyArray = _FontWhitelist.Split( ';' );
            }
            return RandomFontFamilyArray[_Rand.Next( 0, RandomFontFamilyArray.Length )];
        }

        /// <summary>
        /// generate random text for the CAPTCHA
        /// </summary>
        private string GenerateRandomText() {
            StringBuilder sb = new StringBuilder( _RandomTextLength );
            int maxLength = _RandomTextChars.Length;
            for ( int n = 0; n <= _RandomTextLength - 1; n++ ) {
                sb.Append( _RandomTextChars.Substring( _Rand.Next( maxLength ), 1 ) );
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a random point within the specified x and y ranges
        /// </summary>
        private PointF RandomPoint( int xmin, int xmax, int ymin, int ymax ) {
            return new PointF( _Rand.Next( xmin, xmax ), _Rand.Next( ymin, ymax ) );
        }

        /// <summary>
        /// Returns a random point within the specified rectangle
        /// </summary>
        private PointF RandomPoint( Rectangle rect ) {
            return RandomPoint( rect.Left, rect.Width, rect.Top, rect.Bottom );
        }

        /// <summary>
        /// Returns a GraphicsPath containing the specified string and font
        /// </summary>
        private static GraphicsPath TextPath( string s, Font f, Rectangle r ) {
            StringFormat sf = new StringFormat {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            };
            GraphicsPath gp = new GraphicsPath();
            gp.AddString( s, f.FontFamily, (int) f.Style, f.Size, r, sf );
            return gp;
        }

        /// <summary>
        /// Returns the CAPTCHA font in an appropriate size
        /// </summary>
        private Font GetFont() {
            float fsize;
            string fname = _FontFamilyName;
            if ( String.IsNullOrEmpty( fname ) ) {
                fname = RandomFontFamily();
            }
            switch ( FontWarp ) {
                case FontWarpFactor.Low:
                    fsize = Convert.ToInt32( _Height * 0.8 );
                    break;
                case FontWarpFactor.Medium:
                    fsize = Convert.ToInt32( _Height * 0.85 );
                    break;
                case FontWarpFactor.High:
                    fsize = Convert.ToInt32( _Height * 0.9 );
                    break;
                case FontWarpFactor.Extreme:
                    fsize = Convert.ToInt32( _Height * 0.95 );
                    break;
                default:
                    fsize = Convert.ToInt32( _Height * 0.7 );
                    break;
            }
            return new Font( fname, fsize, FontStyle.Bold );
        }

        /// <summary>
        /// Renders the CAPTCHA image
        /// </summary>
        private Bitmap GenerateImagePrivate() {
            Font fnt = null;
            Bitmap bmp = new Bitmap( _Width, _Height, PixelFormat.Format32bppArgb );
            Graphics gr = Graphics.FromImage( bmp );
            gr.SmoothingMode = SmoothingMode.AntiAlias;

            //-- fill an empty white rectangle
            Rectangle rect = new Rectangle( 0, 0, _Width, _Height );
            Brush br = new SolidBrush( Color.White );
            gr.FillRectangle( br, rect );

            int charOffset = 0;
            double charWidth = _Width / _RandomTextLength;

            foreach ( char c in _RandomText ) {
                //-- establish font and draw area
                fnt = GetFont();
                Rectangle rectChar = new Rectangle( Convert.ToInt32( charOffset * charWidth ), 0, Convert.ToInt32( charWidth ),
                                                   _Height );

                //-- warp the character
                GraphicsPath gp = TextPath( c.ToString(), fnt, rectChar );
                WarpText( gp, rectChar );

                //-- draw the character
                br = new SolidBrush( Color.Black );
                gr.FillPath( br, gp );

                charOffset += 1;
            }

            AddNoise( gr, rect );
            AddLine( gr, rect );

            //-- clean up unmanaged resources
            if ( fnt != null )
                fnt.Dispose();
            br.Dispose();
            gr.Dispose();

            return bmp;
        }

        /// <summary>
        /// Warp the provided text GraphicsPath by a variable amount
        /// </summary>
        private void WarpText( GraphicsPath textPath, Rectangle rect ) {
            double WarpDivisor = 0.0;
            double RangeModifier = 0.0;

            switch ( _FontWarp ) {
                case FontWarpFactor.None:
                    return;
                case FontWarpFactor.Low:
                    WarpDivisor = 6;
                    RangeModifier = 1;
                    break;
                case FontWarpFactor.Medium:
                    WarpDivisor = 5;
                    RangeModifier = 1.3;
                    break;
                case FontWarpFactor.High:
                    WarpDivisor = 4.5;
                    RangeModifier = 1.4;
                    break;
                case FontWarpFactor.Extreme:
                    WarpDivisor = 4;
                    RangeModifier = 1.5;
                    break;
            }

            RectangleF rectF = new RectangleF( Convert.ToSingle( rect.Left ), 0, Convert.ToSingle( rect.Width ), rect.Height );

            int hrange = Convert.ToInt32( rect.Height / WarpDivisor );
            int wrange = Convert.ToInt32( rect.Width / WarpDivisor );
            int left = rect.Left - Convert.ToInt32( wrange * RangeModifier );
            int top = rect.Top - Convert.ToInt32( hrange * RangeModifier );
            int width = rect.Left + rect.Width + Convert.ToInt32( wrange * RangeModifier );
            int height = rect.Top + rect.Height + Convert.ToInt32( hrange * RangeModifier );

            if ( left < 0 )
                left = 0;
            if ( top < 0 )
                top = 0;
            if ( width > Width )
                width = Width;
            if ( height > Height )
                height = Height;

            PointF leftTop = RandomPoint( left, left + wrange, top, top + hrange );
            PointF rightTop = RandomPoint( width - wrange, width, top, top + hrange );
            PointF leftBottom = RandomPoint( left, left + wrange, height - hrange, height );
            PointF rightBottom = RandomPoint( width - wrange, width, height - hrange, height );

            PointF[] points = new[] { leftTop, rightTop, leftBottom, rightBottom };
            Matrix m = new Matrix();
            m.Translate( 0, 0 );
            textPath.Warp( points, rectF, m, WarpMode.Perspective, 0 );
        }


        /// <summary>
        /// Add a variable level of graphic noise to the image
        /// </summary>
        private void AddNoise( Graphics graphics1, Rectangle rect ) {
            int density = 0;
            int size = 0;

            switch ( _BackgroundNoise ) {
                case BackgroundNoiseLevel.None:
                    return;
                case BackgroundNoiseLevel.Low:
                    density = 30;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.Medium:
                    density = 18;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.High:
                    density = 16;
                    size = 39;
                    break;
                case BackgroundNoiseLevel.Extreme:
                    density = 12;
                    size = 38;
                    break;
            }

            SolidBrush br = new SolidBrush( Color.Black );
            int max = Convert.ToInt32( Math.Max( rect.Width, rect.Height ) / size );

            for ( int i = 0; i <= Convert.ToInt32( ( rect.Width * rect.Height ) / density ); i++ ) {
                graphics1.FillEllipse( br, _Rand.Next( rect.Width ), _Rand.Next( rect.Height ), _Rand.Next( max ),
                                      _Rand.Next( max ) );
            }
            br.Dispose();
        }

        /// <summary>
        /// Add variable level of curved lines to the image
        /// </summary>
        private void AddLine( Graphics graphics1, Rectangle rect ) {
            int length = 0;
            float width = 0;
            int linecount = 0;

            switch ( _LineNoise ) {
                case LineNoiseLevel.None:
                    return;
                case LineNoiseLevel.Low:
                    length = 4;
                    width = Convert.ToSingle( _Height / 31.25 );
                    // 1.6
                    linecount = 1;
                    break;
                case LineNoiseLevel.Medium:
                    length = 5;
                    width = Convert.ToSingle( _Height / 27.7777 );
                    // 1.8
                    linecount = 1;
                    break;
                case LineNoiseLevel.High:
                    length = 3;
                    width = Convert.ToSingle( _Height / 25 );
                    // 2.0
                    linecount = 2;
                    break;
                case LineNoiseLevel.Extreme:
                    length = 3;
                    width = Convert.ToSingle( _Height / 22.7272 );
                    // 2.2
                    linecount = 3;
                    break;
            }

            PointF[] pf = new PointF[length + 1];
            Pen p = new Pen( Color.Black, width );

            for ( int l = 1; l <= linecount; l++ ) {
                for ( int i = 0; i <= length; i++ ) {
                    pf[i] = RandomPoint( rect );
                }
                graphics1.DrawCurve( p, pf, 1.75f );
            }

            p.Dispose();
        }
    }
}