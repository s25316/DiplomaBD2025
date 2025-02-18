using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class Ex
{
    public Guid ExceptionId { get; set; }

    public DateTime Created { get; set; }

    public string ExceptionType { get; set; } = null!;

    public string? Method { get; set; }

    public string StackTrace { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? Other { get; set; }

    public int? Request { get; set; }
}
