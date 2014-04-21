using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace PcoWeb
{
    public static class ViewHelpers
    {
        public static DateTime? FormtatedDate(string dateString)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");

            try
            {
                return DateTime.Parse(dateString, cultureInfo, DateTimeStyles.NoCurrentDateDefault);
            } 
            catch (FormatException)
            {
                Trace.TraceError("Invalid dateString: " + dateString);

                return null;
            }
        }

        public static DateTime ConvertToTimeZone(DateTime from)
        {
            var zone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

            return TimeZoneInfo.ConvertTimeFromUtc(from, zone);
        }

        public static TProperty Try<TModel, TProperty>(this TModel model, Func<TModel, TProperty> action)
        {
            if (model == null)
                return default(TProperty);

            return action.Invoke(model);
        }

        public static Color ColorBesondere
        {
            get { return Color.FromArgb(0, 255, 255, 107); }
        }

        public static Color ColorAbend
        {
            get { return Color.FromArgb(0, 204, 228, 181); }
        }

        public static Color ColorMorgen
        {
            get { return Color.FromArgb(0, 202, 216, 239); }
        }

        public static IHtmlString ToCssColor(this Color color)
        {
            return MvcHtmlString.Create(string.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, Math.Round((255m - color.A)/255m, 4)));
        }
    }
}