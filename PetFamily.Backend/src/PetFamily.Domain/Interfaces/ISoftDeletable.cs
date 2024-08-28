namespace PetFamily.Domain.Interfaces;

public interface ISoftDeletable
{
    public void Activate();

    public void Deactivate();
}