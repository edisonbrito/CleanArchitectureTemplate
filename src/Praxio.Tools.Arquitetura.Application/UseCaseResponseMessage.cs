using System.Collections.Generic;
using System.Linq;

namespace Praxio.Tools.Arquitetura.Application
{
    public abstract class UseCaseResponseMessage
    {
        public IEnumerable<string> Errors { get; }

        protected UseCaseResponseMessage(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        protected UseCaseResponseMessage(string error)
        {
            Errors =  new List<string> { error };
        }

        protected UseCaseResponseMessage() { }

        public bool IsValid()
        {
            return Errors == null || !Errors.Any();
        }
    }
}