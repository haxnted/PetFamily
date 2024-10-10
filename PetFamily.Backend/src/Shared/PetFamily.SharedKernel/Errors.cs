namespace PetFamily.SharedKernel;

public static class Errors
{
    public static class General
    {
        public static Error AlreadyUsed(Guid? id = null)
        {
            var Id = id == null ? "Id" : $"{id}";
            return Error.Conflict("value.already.used", $"{Id} is already used. Operation impossible");
        }
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            return Error.NotFound("record.not.found", $"record not found{forId}");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? "" : " " + name + " ";
            return Error.Validation("length.is.invalid", $"invalid{label}length)");
        }

        public static Error InsufficientItems(string? name = null)
        {
            var label = name ?? "items";
            return Error.Validation("insufficient.items", $"Insufficient number of {label} to complete the operation");
        }

    }

    public static class Model
    {
        public static Error AlreadyExist(string? name = null)
        {
            var label = name ?? "entity";
            return Error.Validation($"{label}.already.exist", $"{label} already exist");
        }
    }
}