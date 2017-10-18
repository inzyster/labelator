using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace labelassembler
{
    public class Bin
    {

        public int Width { get; set; }
        public int Height { get; set; }
        public List<TileData> Items { get; set; }

        public List<TileData> Pack(List<TileData> items)
        {
            Items = items.ToList();
            var rootNode = new Node { Width = this.Width, Height = this.Height, X = 0, Y = 0 };
            foreach (var item in this.Items)
            {
                var node = FindNodeThatFits(rootNode, item.TargetSizeWithPadding.Width, item.TargetSizeWithPadding.Height);
                if (node != null)
                {
                    item.BinNode = SplitNode(node, item.TargetSizeWithPadding.Width, item.TargetSizeWithPadding.Height);
                }
            }

            return this.Items.ToList();
        }

        private Node FindNodeThatFits(Node rootNode, int itemWidth, int itemHeight)
        {
            if (rootNode.IsOccupied)
            {
                var nextNode = FindNodeThatFits(rootNode.Bottom, itemWidth, itemHeight);
                if (nextNode == null)
                {
                    nextNode = FindNodeThatFits(rootNode.Right, itemWidth, itemHeight);
                }
                return nextNode;
            }
            else if (itemWidth <= rootNode.Width && itemHeight <= rootNode.Height)
            {
                return rootNode;
            }
            else
            {
                return null;
            }
        }

        private Node SplitNode(Node node, int itemWidth, int itemHeight)
        {
            node.IsOccupied = true;
            node.Right = new Node
            {
                X = node.X + itemWidth,
                Y = node.Y,
                Width = node.Width - itemWidth,
                Height = node.Height
            };
            node.Bottom = new Node
            {
                X = node.X,
                Y = node.Y + itemHeight,
                Width = node.Width,
                Height = node.Height - itemHeight
            };
            return node;
        }

    }

    public class Node
    {

        public Node Right { get; set; }
        public Node Bottom { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsOccupied { get; set; }

        public override string ToString()
        {
            return $"x: {X}, y: {Y}, width: {Width}, height: {Height}, is occupied:  {IsOccupied}";
        }

    }

}
