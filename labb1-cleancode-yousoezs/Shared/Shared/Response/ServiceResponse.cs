using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Shared.Response;

public record ServiceResponse<T>(bool Success, T? Data, string Message);
