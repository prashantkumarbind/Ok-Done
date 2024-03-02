using eSanjeevaniIcu.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace eSanjeevaniIcu.Shared.Extension
{
    public static class EnumExtension
    {

        public static T GetEnumByString<T>(this string value)
        {
            return (T)System.Enum.Parse(typeof(T), value);
        }

        public static string GetEnumDescription(this System.Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static RuleOutcome GetRuleoutcomeByEnum(this System.Enum value, RuleOutcomeStatus status)
        {
            var description = GetEnumDescription(value);

            return new RuleOutcome
            {
                Code = Convert.ToString(value),
                Message = description,
                Status = status
            };
        }
    }
}
