using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using EtnaPOS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace EtnaPOS.Services
{
    public class SolutionNodeImageSelector : TreeListNodeImageSelector
    {
        static readonly Dictionary<TypeNode, ImageSource> ImageCache;
        static SolutionNodeImageSelector()
        {
            ImageCache = new Dictionary<TypeNode, ImageSource>();
        }
        public override ImageSource Select(TreeListRowData rowData)
        {
           if(rowData.Row is Node)
            {
                    Node product = (Node)rowData.Row;
                    if (product == null)
                    {
                        return null;
                    }
                    return GetImageByTypeNode(product.Type);
                
            }
            return null;
            
        }

        private ImageSource GetImageByTypeNode(TypeNode type)
        {
            if (ImageCache.ContainsKey(type))
            {
                return ImageCache[type];
            }
            var extension = new SvgImageSourceExtension()
            {
                Uri = new Uri(GetImagePathByTypeNode(type)),
                Size = new Size(16, 16)
            };
            var image = (ImageSource)extension.ProvideValue(null);
            ImageCache.Add(type, image);
            return image;

        }
        public static string GetImagePathByTypeNode(TypeNode type)
        {
            return Directory.GetCurrentDirectory() + "/Images/TreeViewImages/" + type.ToString() + ".svg";
        }
    }
}
