using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Byz_Chess
{
    public static class Globals
    {
        private static ResourceDictionary _piecesDictionary;
        public static ResourceDictionary PiecesDictionary
        {
            get
            {
                if (_piecesDictionary == null || _piecesDictionary.Source == null)
                {
                    var myResourceDictionary = new ResourceDictionary();
                    myResourceDictionary.Source =
                        new Uri("/Byz Chess;component/Resources/Pieces.xaml",
                            UriKind.RelativeOrAbsolute);
                    PiecesDictionary = myResourceDictionary;
                }

                return _piecesDictionary;
            }
            set => _piecesDictionary = value;
        }

    }
}
