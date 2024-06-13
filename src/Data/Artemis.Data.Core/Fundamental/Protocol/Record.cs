using System.Reflection;

namespace Artemis.Data.Core.Fundamental.Protocol;

#region Interface

/// <summary>
///     协议记录接口
/// </summary>
file interface IRecord
{
    /// <summary>
    ///     记录转字节
    /// </summary>
    byte[]? Bytes { get; }

    /// <summary>
    ///     校验记录是否有效
    /// </summary>
    bool Valid { get; }
}

#endregion

/// <summary>
///     Record抽象实现
/// </summary>
public abstract class Record : IRecord
{
    /// <summary>
    ///     转换器表缓存
    /// </summary>
    private static Dictionary<string, (Func<byte[], object>, Func<object, byte[]?>, long)>? _converterTable;

    /// <summary>
    ///     记录结构缓存
    /// </summary>
    private RecordStructure? _recordStructure;

    /// <summary>
    ///     Record抽象实现
    /// </summary>
    protected Record()
    {
        Valid = false;
    }

    /// <summary>
    ///     Record抽象实现
    /// </summary>
    /// <param name="bytes">字节码</param>
    protected Record(byte[] bytes)
    {
        Bytes = bytes;

        if (RecordStructure.Length != bytes.LongLength)
        {
            Valid = false;
        }
        else
        {
            var properties = RecordStructure.Internal;

            var type = GetType();

            if (properties == null)
                return;

            foreach (var property in properties)
            {
                var propertyBytes = new byte[property.Length];

                Array.Copy(bytes, property.Offset, propertyBytes, 0, property.Length);

                var (map, _, _) = ConverterTable[property.Type];

                var propertyValue = map(propertyBytes);

                type.GetProperty(property.Name)?.SetValue(this, propertyValue);
            }

            Valid = true;
        }
    }

    /// <summary>
    ///     转换器表访问器
    /// </summary>
    private static Dictionary<string, (Func<byte[], object>, Func<object, byte[]?>, long)> ConverterTable =>
        _converterTable ??= new Dictionary<string, (Func<byte[], object>, Func<object, byte[]?>, long)>
        {
            //System
            {
                Constants.Type.Byte,
                (bytes => bytes.ToByte(), code => ((byte)code).ToBytes(), Constants.Length.Byte)
            },
            {
                Constants.Type.Long,
                (bytes => bytes.ToLong(), number => ((long)number).ToBytes(), Constants.Length.Long)
            },
            //Record
            {
                Constants.Type.CommandRecord,
                (bytes => new CommandRecord(bytes), record => ((CommandRecord)record).Bytes, Constants.Length.CommandRecord)
            },
            {
                Constants.Type.StatusRecord,
                (bytes => new StatusRecord(bytes), record => ((StatusRecord)record).Bytes, Constants.Length.StatusRecord)
            },
            {
                Constants.Type.CheckRecord,
                (bytes => new CheckRecord(bytes), record => ((CheckRecord)record).Bytes, Constants.Length.CheckRecord)
            },
            {
                Constants.Type.HeadRecord,
                (bytes => new HeadRecord(bytes), record => ((HeadRecord)record).Bytes, Constants.Length.HeadRecord)
            }
        };

    /// <summary>
    ///     记录结构访问器
    /// </summary>
    private RecordStructure RecordStructure
    {
        get
        {
            if (_recordStructure != null) return _recordStructure;

            _recordStructure = new RecordStructure();

            var type = GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var propertiesStructure = new List<PropertyStructure>();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<PropertyAttribute>();

                if (attribute == null) continue;

                var propertyType = attribute.Type != Constants.Type.Unknown
                    ? attribute.Type
                    : property.PropertyType.Name;

                var (_, _, propertyLength) = ConverterTable[propertyType];

                var length = attribute.Length != Constants.Length.Default ? attribute.Length : propertyLength;

                var propertyStructure = new PropertyStructure
                {
                    Order = attribute.Order,
                    Name = property.Name,
                    Length = length,
                    Type = propertyType
                };

                propertiesStructure.Add(propertyStructure);
            }

            _recordStructure.Properties = propertiesStructure;

            return _recordStructure;
        }
    }

    /// <summary>
    ///     设置字节码
    /// </summary>
    protected void SetBytes()
    {
        var properties = RecordStructure.Internal;

        if (properties == null)
            return;

        var bytes = new byte[RecordStructure.Length];

        var type = GetType();

        foreach (var property in properties)
        {
            var propertyValue = type.GetProperty(property.Name)?.GetValue(this);

            if (propertyValue == null)
                continue;

            var (_, inverseMap, _) = ConverterTable[property.Type];

            var propertyBytes = inverseMap(propertyValue);

            if (propertyBytes != null)
            {
                if (propertyBytes.Length != property.Length) propertyBytes = propertyBytes.FixedBytes(property.Length);

                Array.Copy(propertyBytes, 0, bytes, property.Offset, property.Length);
            }
        }

        Bytes = bytes;

        Valid = true;
    }

    #region Implementation of IRecord

    /// <summary>
    ///     记录转字节
    /// </summary>
    public byte[]? Bytes { get; protected set; }

    /// <summary>
    ///     校验记录是否有效
    /// </summary>
    public bool Valid { get; protected set; }

    #endregion
}