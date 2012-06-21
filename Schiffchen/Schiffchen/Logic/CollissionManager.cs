using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using System.Windows.Media.Animation;
using Schiffchen.GameElemens;
using Schiffchen.Logic;

namespace Schiffchen.Logic
{
    public class CollissionManager
    {
        public static void HandleFieldCheck(Playground p, Ship currentShip)
        {
            int counter = 0;
            List<Field> markedFields = new List<Field>();
            foreach (Field field in p.fields)
            {
                int smaller = Convert.ToInt32(field.Size.Width / 2.1) * -1;
                Rectangle rect = new Rectangle(currentShip.Rectangle.X, currentShip.Rectangle.Y, currentShip.Rectangle.Width, currentShip.Rectangle.Height);
                rect.Inflate(smaller,smaller);
                if ((field.ReferencedShip == null || field.ReferencedShip == currentShip) && field.Rectangle.Intersects(rect))
                {
                    field.SetColor(Enum.FieldColor.Green);
                    counter++;
                    markedFields.Add(field);
                }
                else
                {
                    field.ResetColor();
                }
            }
            if (counter == currentShip.Size)
            {
                currentShip.OverlayColor = Color.Green;
                AppCache.CurrentMatch.FooterMenu.Get("btnPlace").Visible = true;
            }
            else
            {
                currentShip.OverlayColor = Color.Red;
                AppCache.CurrentMatch.FooterMenu.Get("btnPlace").Visible = false;
                foreach (Field f in markedFields)
                {
                    f.SetColor(Enum.FieldColor.Red);
                }
            }
        }

        public static List<Field> GetFields(Playground p, Ship currentShip)
        {
            List<Field> markedFields = new List<Field>();
            foreach (Field field in p.fields)
            {
                int smaller = Convert.ToInt32(field.Size.Width / 2.1) * -1;
                Rectangle rect = new Rectangle(currentShip.Rectangle.X, currentShip.Rectangle.Y, currentShip.Rectangle.Width, currentShip.Rectangle.Height);
                rect.Inflate(smaller, smaller);
                if ((field.ReferencedShip == null || field.ReferencedShip == currentShip) && field.Rectangle.Intersects(rect))
                {                    
                    markedFields.Add(field);
                }
            }
            if (markedFields.Count == currentShip.Size)
            {
                return markedFields;
            }
            else
            {
                return null;
            }
        }
    }
}
