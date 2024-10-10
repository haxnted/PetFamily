using System.Collections;

namespace PetFamily.SharedKernel;

public class ErrorList : IEnumerable<Error>
{
    private List<Error> _errors { get; }

    public ErrorList(IEnumerable<Error> errors) => _errors = [..errors];

    public IEnumerator<Error> GetEnumerator() =>
        _errors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        _errors.GetEnumerator();

    public static implicit operator ErrorList(List<Error> errors)
        => new(errors);

    public static implicit operator ErrorList(Error error)
        => new([error]);
}