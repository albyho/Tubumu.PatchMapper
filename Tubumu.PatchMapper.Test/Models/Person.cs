using System;

namespace Tubumu.PatchMapper.Test.Models
{
    public class PersonInput : PatchInput
    {
        public string? Name { get; set; }

        public int? Age { get; set; }

        public string? Gender { get; set; }
    }

    public class PersonEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }
    }
}
