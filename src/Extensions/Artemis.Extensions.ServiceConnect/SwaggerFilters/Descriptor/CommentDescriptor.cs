using System.Reflection;
using System.Text.RegularExpressions;

namespace Artemis.Extensions.ServiceConnect.SwaggerFilters.Descriptor;

/// <summary>
///     注释描述器
/// </summary>
internal static class CommentDescriptor
{
    /// <summary>
    ///     表达式提取器缓存
    /// </summary>
    private static Regex? _expressionPicker;

    /// <summary>
    ///     表达式提取器
    /// </summary>
    private static Regex ExpressionPicker => _expressionPicker ??= new Regex("@(.*)");

    /// <summary>
    ///     描述
    /// </summary>
    /// <param name="comments"></param>
    /// <param name="memberInfo"></param>
    internal static ISchemaInfo Describe(string[] comments, MemberInfo? memberInfo)
    {
        var descriptions = new List<string>();

        var result = new SchemaInfo();

        foreach (var comment in comments)
        {
            if (comment.StartsWith('@'))
            {
                var expression = ExpressionPicker.Match(comment).Groups[1].Value;

                #region Required

                if (RequiredResolve.IsMatch(expression))
                    if (memberInfo is not null)
                    {
                        var type = memberInfo.DeclaringType?.Name;

                        var member = memberInfo.Name;

                        if (type is not null)
                        {
                            if (!DescriptorCache.RequiredFieldsMap.ContainsKey(type))
                                DescriptorCache.RequiredFieldsMap.Add(type, new HashSet<string>());

                            DescriptorCache.RequiredFieldsMap[type].Add(member);
                        }
                    }

                #endregion

                #region Number

                if (MinResolve.IsMatch(expression))
                {
                    var numberComment = MinResolve.Match(expression).Groups[1].Value;

                    var numberParams = numberComment.Split(',');

                    if (numberParams.Length == 1)
                    {
                        var valid = int.TryParse(numberParams[0], out var number);

                        if (valid)
                            result.Minimum ??= number;
                    }
                }

                if (MaxResolve.IsMatch(expression))
                {
                    var numberComment = MaxResolve.Match(expression).Groups[1].Value;

                    var numberParams = numberComment.Split(',');

                    if (numberParams.Length == 1)
                    {
                        var valid = int.TryParse(numberParams[0], out var number);

                        if (valid)
                            result.Maximum ??= number;
                    }
                }

                if (RangeResolve.IsMatch(expression))
                {
                    var numberComment = RangeResolve.Match(expression).Groups[1].Value;

                    var numberParams = numberComment.Split(',');

                    if (numberParams.Length == 2)
                    {
                        var rightValid = int.TryParse(numberParams[0], out var rightNumber);

                        var leftValid = int.TryParse(numberParams[1], out var leftNumber);

                        if (rightValid && leftValid && rightNumber <= leftNumber)
                        {
                            result.Minimum ??= rightNumber;
                            result.Maximum ??= leftNumber;
                        }
                    }
                }

                #endregion

                #region StringLength

                if (MinLengthResolve.IsMatch(expression))
                {
                    var lengthComment = MinLengthResolve.Match(expression).Groups[1].Value;

                    var lengthParams = lengthComment.Split(',');

                    if (lengthParams.Length == 1)
                    {
                        var valid = int.TryParse(lengthParams[0], out var length);

                        if (valid)
                            result.MinLength ??= length;
                    }
                }

                if (MaxLengthResolve.IsMatch(expression))
                {
                    var lengthComment = MaxLengthResolve.Match(expression).Groups[1].Value;

                    var lengthParams = lengthComment.Split(',');

                    if (lengthParams.Length == 1)
                    {
                        var valid = int.TryParse(lengthParams[0], out var length);

                        if (valid)
                            result.MaxLength ??= length;
                    }
                }

                if (LengthResolve.IsMatch(expression))
                {
                    var lengthComment = LengthResolve.Match(expression).Groups[1].Value;

                    var lengthParams = lengthComment.Split(',');

                    if (lengthParams.Length == 1)
                    {
                        var valid = int.TryParse(lengthParams[0], out var length);

                        if (valid)
                        {
                            result.MinLength ??= length;
                            result.MaxLength ??= length;
                        }
                    }

                    if (lengthParams.Length == 2)
                    {
                        var rightValid = int.TryParse(lengthParams[0], out var rightLength);

                        var leftValid = int.TryParse(lengthParams[1], out var leftLength);

                        if (rightValid && leftValid && rightLength <= leftLength)
                        {
                            result.MinLength ??= rightLength;
                            result.MaxLength ??= leftLength;
                        }
                    }
                }

                #endregion

                #region Format

                if (DescriptorCache.FormatMap.ContainsKey(expression)) DescriptorCache.FormatMap[expression](result);

                #endregion

                #region Example

                if (ExampleResolve.IsMatch(expression))
                {
                    var example = ExampleResolve.Match(expression).Groups[1].Value;

                    result.Example = example;
                }

                #endregion
            }
            else
            {
                descriptions.Add(comment);
            }
        }

        result.Description = string.Join(',', descriptions);

        return result;
    }

    #region Required @required

    /// <summary>
    ///     必要性标识解析缓存
    /// </summary>
    private static Regex? _requiredResolve;

    /// <summary>
    ///     必要性标识解析
    /// </summary>
    /// <example>@required</example>
    private static Regex RequiredResolve => _requiredResolve ??= new Regex("required");

    #endregion

    #region Number @mini(x) @maxi(x) @range(x,x)

    /// <summary>
    ///     最小值解析缓存
    /// </summary>
    private static Regex? _minResolve;

    /// <summary>
    ///     最小值解析
    /// </summary>
    /// <example>@min(0)</example>
    private static Regex MinResolve => _minResolve ??= new Regex(@"min\((.*)\)");

    /// <summary>
    ///     最大值解析缓存
    /// </summary>
    private static Regex? _maxResolve;

    /// <summary>
    ///     最大值解析
    /// </summary>
    /// <example>@max(100)</example>
    private static Regex MaxResolve => _maxResolve ??= new Regex(@"max\((.*)\)");

    /// <summary>
    ///     值范围解析缓存
    /// </summary>
    private static Regex? _rangeResolve;

    /// <summary>
    ///     值范围解析
    /// </summary>
    /// <example>@range(0,100)</example>
    private static Regex RangeResolve => _rangeResolve ??= new Regex(@"range\((.*)\)");

    #endregion

    #region StringLength @length(x,x) @minLength(x) @maxLength(x)

    /// <summary>
    ///     最短长度缓存标识解析缓存
    /// </summary>
    private static Regex? _minLengthResolve;

    /// <summary>
    ///     最短长度缓存标识解析
    /// </summary>
    /// <example>@minLength(12)</example>
    private static Regex MinLengthResolve => _minLengthResolve ??= new Regex(@"minLength\((.*)\)");

    /// <summary>
    ///     长度缓存标识解析缓存
    /// </summary>
    private static Regex? _maxLengthResolve;

    /// <summary>
    ///     长度缓存标识解析
    /// </summary>
    /// <example>@maxLength(12)</example>
    private static Regex MaxLengthResolve => _maxLengthResolve ??= new Regex(@"maxLength\((.*)\)");

    /// <summary>
    ///     长度缓存标识解析缓存
    /// </summary>
    private static Regex? _lengthResolve;

    /// <summary>
    ///     长度缓存标识解析
    /// </summary>
    /// <example>@length(12)</example>
    private static Regex LengthResolve => _lengthResolve ??= new Regex(@"length\((.*)\)");

    #endregion

    #region Example @example(x)

    /// <summary>
    ///     示例解析缓存
    /// </summary>
    private static Regex? _exampleResolve;

    /// <summary>
    ///     示例解析
    /// </summary>
    public static Regex ExampleResolve => _exampleResolve ??= new Regex(@"example\((.*)\)");

    #endregion
}