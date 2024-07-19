using System;
using System.Text.Json.Serialization;

namespace NATS.Jwt.Models;

public record NatsImport
{
    /// <summary>
    /// Name of the import.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; set; }

    /// <summary>
    /// Subject field in an import is always from the perspective of the
    /// initial publisher. In the case of a stream it is the account owning
    /// the stream (the exporter), and in the case of a service it is the
    /// account making the request (the importer).
    /// </summary>
    [JsonPropertyName("subject")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Subject { get; set; }

    [JsonPropertyName("account")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Account { get; set; }

    [JsonPropertyName("token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Token { get; set; }

    /// <summary>
    /// <c>To</c> field in an import is always from the perspective of the subscriber.
    /// In the case of a stream it is the client of the stream (the importer),
    /// from the perspective of a service, it is the subscription waiting for
    /// requests (the exporter). If the field is empty, it will default to the
    /// value in the Subject field.
    /// </summary>
    [Obsolete("Use LocalSubject instead")]
    [JsonPropertyName("to")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string To { get; set; }

    /// <summary>
    /// Local subject used to subscribe (for streams) and publish (for services) to.
    /// This value only needs setting if you want to change the value of the Subject.
    /// If the value of Subject ends in > then LocalSubject needs to end in > as well.
    /// LocalSubject can contain <c>number</c> wildcard references where number references the nth wildcard in Subject.
    /// The sum of wildcard reference and * tokens needs to match the number of * tokens in Subject.
    /// </summary>
    [JsonPropertyName("local_subject")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string LocalSubject { get; set; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Type { get; set; }

    [JsonPropertyName("share")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Share { get; set; }

    [JsonPropertyName("allow_trace")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool AllowTrace { get; set; }
}
