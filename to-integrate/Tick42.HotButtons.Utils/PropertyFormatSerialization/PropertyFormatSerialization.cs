using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tick42.HotButtons.Utils.PropertyFormatSerialization
{
    /// <summary>
    /// Contains logic for deserializing entity objects from .property format storage.
    /// </summary>
    public static class PropertyFormatSerialization
    {
        const string NULL = "NULL";

        public static IPropertyFormatEntity DeserializeEntity(
            string rootPattern,
            string name,
            Type entityType,
            IEnumerable<string> propertyFile,
            bool allowMissing = false)
        {
            if (propertyFile == null)
            {
                throw new ArgumentNullException("propertyFile");
            }

            Regex propertyLineRegex = SimpleCache.GetOrCreate(() => new Regex(@"^([^=]+)=(.*)"), "Regex");

            var configuration = new Dictionary<string, string>();
            foreach (string line in propertyFile.Select(s => s.Trim()))
            {
                if (line == "" || line.StartsWith("#"))
                {
                    continue;
                }
                Match match = propertyLineRegex.Match(line);
                if (!match.Success)
                {
                    throw new ArgumentException("invalid property file format: " + line);
                }
                configuration[match.Groups[1].Value] = match.Groups[2].Value;
            }

            IPropertyFormatEntity entity = DeserializeEntity(
                rootPattern,
                name,
                entityType,
                configuration,
                allowMissing: allowMissing);

            return entity;
        }

        public static IPropertyFormatEntity DeserializeEntity<TConfigValue>(
            string rootPattern,
            string name,
            Type entityType,
            IDictionary<string, TConfigValue> configuration,
            IPropertyFormatEntity parent = null,
            bool allowMissing = false)
        {
            // LATER: support only fixed types
            if (rootPattern == null)
            {
                throw new ArgumentNullException("rootPattern");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }
            if (!typeof(IPropertyFormatEntity).IsAssignableFrom(entityType))
            {
                throw new ArgumentException("entityType: " + entityType.Name);
            }
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            var resultEntity = (IPropertyFormatEntity) Activator.CreateInstance(entityType);
            resultEntity.Name = name;
            string pattern = string.Format(rootPattern, name);

            // ex: pattern = HotButtonPanel

            TConfigValue enabledProperty;
            if (configuration.TryGetValue(pattern + ".Enabled", out enabledProperty))
            {
                object enabledBool;
                if (GetFinalValue(enabledProperty, typeof(bool), out enabledBool))
                {
                    if (!(bool) enabledBool)
                    {
                        return null;
                    }
                }
            }

            foreach (PropertyInfo property in entityType.GetPropertiesCache())
            {
                if (property.Name == "Name")
                {
                    continue;
                }

                if (!property.CanWrite)
                {
                    continue;
                }

                object value;

                string propertyPattern = pattern + "." + property.Name;
                // ex: propertyPattern = HotButtonPanel.Background

                if (property.Name == "Parent")
                {
                    if (parent != null && property.PropertyType.IsInstanceOfType(parent))
                    {
                        value = parent;
                    }
                    else
                    {
                        value = null;
                    }
                }
                else if (typeof(IPropertyFormatEntity).IsAssignableFrom(property.PropertyType))
                {
                    value =
                        DeserializeEntity(
                            pattern + ".{0}",
                            property.Name,
                            property.PropertyType,
                            configuration,
                            resultEntity,
                            allowMissing);
                }
                else
                {
                    if (!TryDeserializeEnumerableOrDictionary(
                            configuration,
                            allowMissing,
                            property,
                            propertyPattern,
                            resultEntity,
                            out value))
                    {

                        if (!TryDeserializeScalar(configuration, allowMissing, propertyPattern, property, out value))
                        {
                            continue;
                        }
                    }

                }
                try
                {
                    property.SetValue(resultEntity, value, null);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(
                        string.Format("Unable to set property {0} to {1}", propertyPattern, value),
                        ex);
                }
            }
            return resultEntity;
        }

        // private 

        static bool TryDeserializeScalar<TConfigValue>(
            IDictionary<string, TConfigValue> configuration,
            bool allowMissing,
            string propertyPattern,
            PropertyInfo property,
            out object value)
        {
            value = null;
            TConfigValue temp;
            if (configuration.TryGetValue(propertyPattern, out temp))
            {
                if (!GetFinalValue(temp, property.PropertyType, out value))
                {
                    throw new ApplicationException(
                        string.Format(
                            "CM Property for {0}'s value {1} is not compatible with {2}",
                            propertyPattern,
                            value,
                            property.PropertyType));
                }
            }
            else
            {
                if (allowMissing)
                {
                    object[] defAttribute =
                        property.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                    if (defAttribute.Length > 0)
                    {
                        value = ((DefaultValueAttribute) defAttribute[0]).Value;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    throw new ArgumentException(
                        string.Format("Property pattern {0} not found", propertyPattern));
                }
            }
            if (value as string == NULL)
            {
                value = null;
            }
            return true;
        }

        static bool TryDeserializeEnumerableOrDictionary<TConfigValue>(
            IDictionary<string, TConfigValue> configuration,
            bool allowMissing,
            PropertyInfo property,
            string propertyPattern,
            IPropertyFormatEntity resultEntity,
            out object value)
        {
            value = null;
            // indexed property
            Type[] iEnumerableGenericArguments =
                property.PropertyType.ImplementsGenericInterface(typeof(IEnumerable<>));
            Type[] iDictionaryGenericArguments =
                property.PropertyType.ImplementsGenericInterface(typeof(IDictionary<,>));

            var isIDictionary =
                property.PropertyType.IsInterface &&
                iDictionaryGenericArguments != null &&
                iDictionaryGenericArguments.Length == 2;

            var isIEnumerableAndNotIDictionary =
                !isIDictionary &&
                property.PropertyType.IsInterface &&
                iEnumerableGenericArguments != null &&
                iEnumerableGenericArguments.Length == 1;

            if (!isIEnumerableAndNotIDictionary && !isIDictionary)
            {
                return false;
            }

            Type nestedEntityType =
                isIEnumerableAndNotIDictionary ? iEnumerableGenericArguments[0] : iDictionaryGenericArguments[1];

            if (isIEnumerableAndNotIDictionary &&
                !typeof(IPropertyFormatEntity).IsAssignableFrom(nestedEntityType))
            {
                throw new InvalidOperationException(
                    string.Format("Type {0} not allowed for IEnumerable<> deserialization", iEnumerableGenericArguments[0].Name));
            }
            if (isIDictionary &&
                !typeof(IPropertyFormatEntity).IsAssignableFrom(nestedEntityType))
            {
                throw new InvalidOperationException(
                    string.Format("Type {0} not allowed for IDictionary<> value deserialization",
                        nestedEntityType.Name));
            }
            if (isIDictionary &&
                iDictionaryGenericArguments[0] != typeof(string))
            {
                throw new InvalidOperationException(
                    string.Format("Type {0} not allowed for IDictionary<> value deserialization",
                        iDictionaryGenericArguments[0].Name));
            }

            var indexedPatternRegex =
                SimpleCache.GetOrCreate<Regex>(
                    (propertyPattern + @"[(\w+)]")
                    .Replace("[", @"\[")
                    .Replace("]", @"\]"));

            string indexedPatternFormat = propertyPattern + "[{0}]";

            // ex: indexedPatternFormat = HotButtonPanel.Buttons[{0}]

            string[] indexedPropNames = configuration
                .Select(kvp => indexedPatternRegex.Match(kvp.Key))
                .Where(match => match.Success)
                .Select(match => match.Groups[1].Value)
                .Distinct()
                .ToArray();

            Array array = Array.CreateInstance(
                nestedEntityType,
                new[] { indexedPropNames.Length });

            int ii = 0;
            foreach (string subEntityName in indexedPropNames)
            {
                IPropertyFormatEntity subEntity = DeserializeEntity(
                    indexedPatternFormat,
                    subEntityName,
                    nestedEntityType,
                    configuration,
                    resultEntity,
                    allowMissing);

                if (subEntity == null)
                {
                    continue;
                }

                array.SetValue(subEntity, indices: new[] { ii });

                ii += 1;
            }

            if (isIDictionary)
            {
                var dictType = typeof(Dictionary<,>)
                    .MakeGenericTypeCache(
                        typeof(string),
                        nestedEntityType);
                dynamic dict = Activator.CreateInstance(dictType);
                foreach (IPropertyFormatEntity valueEntity in array)
                    dict.Add(valueEntity.Name, (dynamic) valueEntity);
                value = dict;
            }
            else
            {
                value = array.Resize(ii);
            }
            return true;
        }

        static bool GetFinalValue<TConfigValue>(TConfigValue originalValue, Type targetType,
           out object finalValue)
        {
            finalValue = originalValue;
            while (finalValue != null &&
                   finalValue.GetType().GetPropertyCache("Value") != null)
            {
                finalValue = ((dynamic) finalValue).Value;
            }

            if (finalValue is string)
            {
                if (targetType == typeof(string))
                {
                    return true;
                }

                string asString = finalValue + "";

                if (targetType.IsEnum)
                {
                    return Utils.TryParseEnum(targetType, asString, out finalValue);
                }
                else
                {
                    if (asString == "")
                    {
                        finalValue = Activator.CreateInstance(targetType);
                        {
                            return true;
                        }
                    }

                    if (!asString.TryParse(targetType, out finalValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}