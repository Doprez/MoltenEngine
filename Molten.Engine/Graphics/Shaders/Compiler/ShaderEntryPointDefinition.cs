﻿using Newtonsoft.Json;

namespace Molten.Graphics;
public class ShaderEntryPointDefinition
{
    string _vs;
    string _ps;
    string _gs;
    string _hs;
    string _ds;
    string _cs;

    public bool Validate(ShaderCompilerContext context)
    {
        if (string.IsNullOrWhiteSpace(_vs))
        {
            if (string.IsNullOrWhiteSpace(_cs))
            {
                context.AddError("Pass must have a vertex or compute shader entry-point.");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(_ps) && string.IsNullOrWhiteSpace(_gs))
            {
                context.AddError("If a vertex entry-point is defined, a geometry or pixel entry-point must also be defined.");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Gets or sets the vertex shader entry point.
    /// </summary>
    [JsonProperty("vertex")]
    public string Vertex
    {
        get => _vs;
        set
        {
            _vs = value;
            Points[ShaderType.Vertex] = value;
        }
    }

    /// <summary>
    /// Gets or sets the vertex shader entry point. This is identical to <see cref="Fragment"/>.
    /// </summary>
    [JsonProperty("pixel")]
    public string Pixel
    {
        get => _ps;
        set
        {
            _ps = value;
            Points[ShaderType.Pixel] = value;
        }
    }

    /// <summary>
    /// Gets or sets the fragment shader entry point. This is identical to <see cref="Pixel"/>.
    /// </summary>
    [JsonProperty("fragment")]
    public string Fragment
    {
        get => _ps;
        set
        {
            _ps = value;
            Points[ShaderType.Pixel] = value;
        }
    }

    /// <summary>
    /// Gets or sets the geometry shader entry point.
    /// </summary>
    [JsonProperty("geometry")]
    public string Geometry
    {
        get => _gs;
        set
        {
            _gs = value;
            Points[ShaderType.Geometry] = value;
        }
    }

    /// <summary>
    /// Gets or sets the hull shader entry point.
    /// </summary>
    [JsonProperty("hull")]
    public string Hull
    {
        get => _hs;
        set
        {
            _hs = value;
            Points[ShaderType.Hull] = value;
        }
    }

    [JsonProperty("domain")]
    public string Domain
    {
        get => _ds;
        set
        {
            _ds = value;
            Points[ShaderType.Domain] = value;
        }
    }

    [JsonProperty("compute")]
    public string Compute
    {
        get => _cs;
        set
        {
            _cs = value;
            Points[ShaderType.Compute] = value;
        }
    }

    [JsonIgnore]
    public Dictionary<ShaderType, string> Points { get; } = new Dictionary<ShaderType, string>();
}