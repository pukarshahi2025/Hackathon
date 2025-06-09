using System;
using System.Collections.Generic;
using System.Reflection;

public static class FixedWidthParser
{
    public static T ParseLine<T>(string line) where T : new()
    {
        var result = new T();

        foreach (var prop in typeof(T).GetProperties())
        {
            var attr = prop.GetCustomAttribute<FixedLengthFieldAttribute>();
            if (attr != null)
            {
                //if (line.Length < attr.Start + attr.Length)
                //    throw new FormatException($"Line too short for field '{prop.Name}' (needs {attr.Start + attr.Length} chars).");

                string value = line.Substring(attr.Start, attr.Length);
                prop.SetValue(result, value);
            }
        }

        return result;
    }

    public static List<string> Validate<T>(T model, int lineNumber) where T : class
    {
        var errors = new List<string>();
        var type = typeof(T);

        foreach (var prop in type.GetProperties())
        {
            var attr = prop.GetCustomAttribute<FixedLengthFieldAttribute>();
            if (attr == null) continue;

            string value = ((string)prop.GetValue(model))?.Trim();
            int columnNumber = attr.Start; // zero-based column index

            if (string.IsNullOrWhiteSpace(value))
            {
                errors.Add($"Line {lineNumber}, Column {columnNumber}: {prop.Name} is empty.");
                continue;
            }

            if (value.Length != attr.Length)
            {
                errors.Add($"Line {lineNumber}, Column {columnNumber}: {prop.Name} length must be {attr.Length} but was {value.Length}.");
            }

            // Assuming prefix is the property name like AID, BID, etc.
            var expectedPrefix = prop.Name;
            if (!value.StartsWith(expectedPrefix) && expectedPrefix != "DATE" && expectedPrefix.StartsWith("SMLP"))
            {
                errors.Add($"Line {lineNumber}, Column {columnNumber}: {prop.Name} must start with '{expectedPrefix}' but was '{value}'.");
            }
            if (prop.Name == "DATE")
            {
                if (!DateTime.TryParseExact(value, "yyyyMMdd", null,
                    System.Globalization.DateTimeStyles.None, out _))
                {
                    errors.Add($"Line {lineNumber}, Column {columnNumber}: {prop.Name} must be a valid date in yyyyMMdd format (was '{value}').");
                }
            }
        }

        return errors;
    }
}
