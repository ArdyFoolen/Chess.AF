﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.ChessForm.ChessConstants;
using static AF.Functional.F;
using System.IO;

namespace Chess.AF.ChessForm
{
    public static class ImageHelper
    {
        private static readonly Image ChessPieces;
        private static readonly Image FenImage;
        private static readonly Image MediaPlayerIconSetImage;
        private static readonly Image TurnBoardImage;

        static ImageHelper()
        {
            ChessPieces = ReadEmbeddedRessourceImage("Chess.AF.ChessForm.Images.ChessPieces500x181.png");
            FenImage = ReadEmbeddedRessourceImage("Chess.AF.ChessForm.Images.Fen301x156.png");
            MediaPlayerIconSetImage = ReadEmbeddedRessourceImage("Chess.AF.ChessForm.Images.MediaPlayeIconSet600x620.png");
            TurnBoardImage = ReadEmbeddedRessourceImage("Chess.AF.ChessForm.Images.TurnBoard179x245.png");
        }

        #region public Chess Image Helpers

        public static readonly Dictionary<int, Func<Image>> GetPieceDict = new Dictionary<int, Func<Image>>()
        {
            { 1, BlackPawn },
            { 2, BlackKnight },
            { 3, BlackBishop },
            { 4, BlackRook },
            { 5, BlackQueen },
            { 6, BlackKing },
            { 8, WhitePawn },
            { 9, WhiteKnight },
            { 10, WhiteBishop },
            { 11, WhiteRook },
            { 12, WhiteQueen },
            { 13, WhiteKing }
        };

        public static Image WhiteKing()
            => GetPiece(0, false);
        public static Image BlackKing()
            => GetPiece(0, true);
        public static Image WhiteQueen()
            => GetPiece(1, false, 3);
        public static Image BlackQueen()
            => GetPiece(1, true, 3);
        public static Image WhiteQueenSmall()
            => GetPiece(1, false, 3, true);
        public static Image BlackQueenSmall()
            => GetPiece(1, true, 3, true);
        public static Image WhiteBishop()
            => GetPiece(2, false, 5);
        public static Image BlackBishop()
            => GetPiece(2, true, 5);
        public static Image WhiteBishopSmall()
            => GetPiece(2, false, 5, true);
        public static Image BlackBishopSmall()
            => GetPiece(2, true, 5, true);
        public static Image WhiteKnight()
            => GetPiece(3, false, 6);
        public static Image BlackKnight()
            => GetPiece(3, true, 6);
        public static Image WhiteKnightSmall()
            => GetPiece(3, false, 6, true);
        public static Image BlackKnightSmall()
            => GetPiece(3, true, 6, true);
        public static Image WhiteRook()
            => GetPiece(4, false, 8);
        public static Image BlackRook()
            => GetPiece(4, true, 8);
        public static Image WhiteRookSmall()
            => GetPiece(4, false, 8, true);
        public static Image BlackRookSmall()
            => GetPiece(4, true, 8, true);
        public static Image WhitePawn()
            => GetPiece(5, false, 9);
        public static Image BlackPawn()
            => GetPiece(5, true, 9);

        #endregion

        #region public Fen Image Helper

        public static Image Fen()
            => FenImage;

        #endregion

        #region public Turn Board Image Helper

        public static Image TurnBoard()
            => TurnBoardImage;

        #endregion

        #region public Media Player Helper Methods

        public static Image FirstMove(int? width = null, int? height = null)
            => LoadMediaPlayerImage(1, 1, width, height);

        public static Image PreviousMove(int? width = null, int? height = null)
            => LoadMediaPlayerImage(3, 1, width, height);

        public static Image NextMove(int? width = null, int? height = null)
            => LoadMediaPlayerImage(2, 1, width, height);

        public static Image LastMove(int? width = null, int? height = null)
            => LoadMediaPlayerImage(0, 1, width, height);

        private static Image LoadMediaPlayerImage(int horizontal, int vertical, int? width = null, int? height = null)
        {
            width = width ?? MediaPlayerWidth;
            height = height ?? MediaPlayerHeight;

            int x = MediaPlayerWidth * horizontal;
            int y = MediaPlayerHeight * vertical;

            Rectangle source = new Rectangle(new Point(x, y), new Size(MediaPlayerWidth, MediaPlayerHeight));
            return ResizeImage(CropImage(MediaPlayerIconSetImage, source), width.Value, height.Value);
        }

        #endregion

        #region private Checss Image Helper Methods

        private static Dictionary<int, Image> PiecesDict = new Dictionary<int, Image>();
        /// <summary>
        /// Index
        /// 0 : King
        /// 1 : Queen
        /// 2 : Bishop
        /// 3 : Knight
        /// 4 : Rook
        /// 5 : Pawn
        /// </summary>
        /// <param name="index"></param>
        /// <param name="blackPiece"></param>
        /// <returns></returns>
        private static Image GetPiece(int index, bool blackPiece, int correct = 0, bool halfSize = false)
        {
            int width = PieceWidth;
            int key = index + (blackPiece ? 6 : 0);
            if (halfSize)
            {
                key += 12;
                width /= 2;
            }
            if (!PiecesDict.ContainsKey(key))
                PiecesDict.Add(key, LoadPieceImage(index, blackPiece, correct, width));

            return PiecesDict[key];
        }

        private static Image LoadPieceImage(int index, bool blackPiece, int correct, int? width = null)
        {
            width = width ?? PieceWidth;
            int y = blackPiece ? 90 : 0;
            int x = (500 / 6) * index + correct;
            Rectangle source = new Rectangle(new Point(x, y), new Size(500 / 6, 90));
            return ResizeImage(CropImage(ChessPieces, source), width.Value, width.Value);
        }

        #endregion

        #region private Image Helper Methods

        private static Bitmap CropImage(Image orgImg, Rectangle source)
        {
            Rectangle destination = new Rectangle(Point.Empty, source.Size);
            var cropImage = new Bitmap(source.Width, source.Height);
            Using(Graphics.FromImage(cropImage), g => g.DrawImage(orgImg, destination, source, GraphicsUnit.Pixel));
            return cropImage;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Using(Graphics.FromImage(destImage), g => ResizeImage(image, destRect, g));

            return destImage;
        }

        private static void ResizeImage(Image image, Rectangle destRect, Graphics graphics)
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Using(new ImageAttributes(), w => DrawImage(image, destRect, graphics, w));
        }

        private static void DrawImage(Image image, Rectangle destRect, Graphics graphics, ImageAttributes wrapMode)
        {
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        }

        private static Image ReadEmbeddedRessourceImage(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return Using(assembly.GetManifestResourceStream(resourceName), s => ImageFromStream(s));
        }

        private static Image ImageFromStream(Stream stream)
            => (stream != null) ? Image.FromStream(stream) : null;

        #endregion
    }
}
