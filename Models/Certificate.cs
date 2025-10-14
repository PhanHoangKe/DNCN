using System;
using System.Collections.Generic;

namespace EduFlex.Models;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public int EnrollmentId { get; set; }

    public string CertificateCode { get; set; } = null!;

    public DateTime? IssuedAt { get; set; }

    public string? PdfUrl { get; set; }

    public virtual Enrollment Enrollment { get; set; } = null!;
}
