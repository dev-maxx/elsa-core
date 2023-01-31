using System.Reflection;

namespace Elsa.Workflows.Management.Implementations;

/// <summary>
/// Provides options about a given activity property.
/// </summary>
public interface IActivityPropertyOptionsProvider
{
    /// <summary>
    /// Returns options for the specified property.
    /// </summary>
    object? GetOptions(PropertyInfo property);
}